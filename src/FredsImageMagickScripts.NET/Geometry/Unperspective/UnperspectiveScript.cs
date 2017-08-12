// Copyright 2015-2017 Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/FredsImageMagickScripts.NET)
//
// These scripts are available free of charge for non-commercial use, ONLY.
//
// For use of these scripts in commercial (for-profit) environments or non-free applications,
// please contact Fred Weinhaus (fmw at alink dot net) for licensing arrangements.
//
// If you: 1) redistribute, 2) incorporate any of these scripts into other free applications or
// 3) reprogram them in another scripting language, then you must contact Fred Weinhaus for
// permission, especially if the result might be used in a commercial or for-profit environment.
//
// Usage, whether stated or not in the script, is restricted to the above licensing arrangements.
// It is also subject, in a subordinate manner, to the ImageMagick license, which can be found at:
// http://www.imagemagick.org/script/license.php

using System;
using System.Collections.Generic;
using ImageMagick;

namespace FredsImageMagickScripts
{
    /// <summary>
    /// Attempts to automatically remove pespective distortion from an image without the need to
    /// manually pick control points. This technique is limited and relies upon the ability to
    /// isolate the outline or boundary of the distorted quadrilateral in the input image from its
    /// surrounding background. This technique will not look for internal edges or other details to
    /// assess the distortion. This technique also works to correct affine distortions such as
    /// rotation and/or skew.
    ///
    /// The basic principal is to isolate the quadrilateral of the distorted region from its
    /// background to form a binary mask. The mask is converted from cartesian coordinates to polar
    /// coordinates and averaged down to one row. This row is then processed either to find the
    /// highest peaks or the highest second derivative peaks. The four peaks identified are then
    /// converted back to cartesian coordinates and used with the ouput dimensions determined from
    /// the user specified (or computed) aspect ratio and user specified dimension.
    /// </summary>
    public sealed partial class UnperspectiveScript
    {
        private UnperspectiveMethod _method;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnperspectiveScript"/> class.
        /// </summary>
        public UnperspectiveScript()
          : this(UnperspectiveMethod.Peak)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnperspectiveScript"/> class.
        /// </summary>
        /// <param name="method">The unpersective method</param>
        public UnperspectiveScript(UnperspectiveMethod method)
        {
            if (method != UnperspectiveMethod.Peak && method != UnperspectiveMethod.Derivative)
                throw new ArgumentException("Invalid unperspective method specified.", nameof(method));

            _method = method;

            Reset();
        }

        /// <summary>
        /// Gets or sets the desired output width/height aspect ratio.
        /// Default is computed automatically.
        /// </summary>
        public double? AspectRatio
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets any location within the border area for the algorithm to find the base border color.
        /// </summary>
        public PointD BorderColorLocation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the blurring amount for preprocessing images of text with no quadrilateral outline.
        /// The default is 0.
        /// </summary>
        public double Blur
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fuzz amount specified as a percent 0 to 100. It is used
        /// 1) for trimming the image to bounding box about the quadrilaterls,
        /// 2) for floodfilling the background to convert the quadrilateral into a binary mask and
        /// 3) for trimming the output image.
        /// The default=10. Use a value that will produce a mask that cleanly corresponds to the
        /// distorted quadrilateral area of the image. Note that method=peak is fairly robust to
        /// minor imperfections in the mask, but method=derivative is not.
        /// </summary>
        public Percentage ColorFuzz
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets default output dimension.
        /// Default is EdgeLength.
        /// </summary>
        public UnperspectiveDefault Default
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the viewport crop of the output image should be disabled and allows
        /// distort to compute a larger output image before doing a fuzzy trim.
        /// </summary>
        public bool DisableViewportCrop
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the desired height of output.
        /// </summary>
        public int? Height
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the trap for maximum number of false peaks before filtering to remove false peaks.
        /// Default is 40.
        /// </summary>
        public int MaxPeaks
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the trap for minimum edge length.
        /// Default is 10.
        /// </summary>
        public int MinLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the desired rotation of output image.
        /// </summary>
        public UnperspectiveRotation? Rotation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sharpening amount used to amplify true peaks.This is a filtering step applied after
        /// the smoothing to the 1D polar images.
        /// Default is 5 when method is Peak and 0 when method is Derivative.
        /// </summary>
        public double Sharpen
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the smoothing amount used to help remove false peaks.
        /// Default is 1 when method is Peak and 5 when method is Derivative.
        /// </summary>
        public double Smooth
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the threshold value for removing false peaks.
        /// Default is 4 when method is Peak and 10 when method is Derivative.
        /// </summary>
        public int Threshold
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets desired width of output.
        /// </summary>
        public int? Width
        {
            get;
            set;
        }

        /// <summary>
        /// Automatically remove pespective distortion from an image.
        /// </summary>
        /// <param name="input">The image to execute the script on.</param>
        /// <returns>The resulting image.</returns>
        public IMagickImage Execute(IMagickImage input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            CheckSettings();

            MagickGeometry inputDimensions = new MagickGeometry(input.Width, input.Height);
            MagickColor backgroundColor = GetBorderColor(input);
            int borderSize = (int)((3 * Blur) + 5);

            var output = input.Clone();

            MagickGeometry trimmedDimensions = TrimImage(output, backgroundColor, borderSize);

            PadImage(output, borderSize);

            int xOffset;

            using (IMagickImage paddedMask = CreatePaddedMask(output, out xOffset))
            {
                double maxRad = 0.5 * Hypot(output.Width, output.Height);
                IMagickImage depolar = CreateDepolar(paddedMask, maxRad);

                ushort[] pixels = GetGrayChannel(depolar);

                List<PixelValue> maxList = GetPeaks(pixels, depolar);

                PointD[] corners = GetCorners(paddedMask, maxList, maxRad, xOffset);

                UnperspectiveRotation rotation = GetRotation(corners, output);

                Distort(output, corners, inputDimensions, trimmedDimensions, backgroundColor);
                Rotate(output, rotation);

                return output;
            }
        }

        /// <summary>
        /// Resets the script to the default setttings.
        /// </summary>
        public void Reset()
        {
            if (_method == UnperspectiveMethod.Peak)
            {
                Sharpen = 5.0;
                Smooth = 1.0;
                Threshold = 4;
            }
            else
            {
                Sharpen = 0.0;
                Smooth = 5.0;
                Threshold = 10;
            }

            AspectRatio = null;
            BorderColorLocation = new PointD(0, 0);
            Blur = 0.0;
            ColorFuzz = new Percentage(10);
            Default = UnperspectiveDefault.EdgeLength;
            DisableViewportCrop = false;
            Height = null;
            MaxPeaks = 40;
            MinLength = 10;
            Rotation = null;
            Width = null;
        }

        private static IMagickImage CreateDepolar(IMagickImage image, double maxRad)
        {
            var depolar = image.Clone();

            depolar.VirtualPixelMethod = VirtualPixelMethod.Black;
            depolar.Distort(DistortMethod.DePolar, maxRad);
            depolar.Scale(new MagickGeometry(depolar.Width + "x" + 1 + "!"));
            depolar.VirtualPixelMethod = VirtualPixelMethod.Undefined;

            return depolar;
        }

        private static double[] GetCoefficients(double[] arguments)
        {
            double[] terms = new double[8];
            double[] vectors = new double[8];
            double[][] matrix = new double[8][];

            for (int i = 0; i < 8; i++)
                matrix[i] = new double[8];

            for (int i = 0; i < arguments.Length; i += 4)
            {
                terms[0] = arguments[i + 2];
                terms[1] = arguments[i + 3];
                terms[2] = 1.0;
                terms[3] = 0.0;
                terms[4] = 0.0;
                terms[5] = 0.0;
                terms[6] = -terms[0] * arguments[i];
                terms[7] = -terms[1] * arguments[i];
                LeastSquaresAddTerms(matrix, vectors, terms, arguments[i]);

                terms[0] = 0.0;
                terms[1] = 0.0;
                terms[2] = 0.0;
                terms[3] = arguments[i + 2];
                terms[4] = arguments[i + 3];
                terms[5] = 1.0;
                terms[6] = -terms[3] * arguments[i + 1];
                terms[7] = -terms[4] * arguments[i + 1];
                LeastSquaresAddTerms(matrix, vectors, terms, arguments[i + 1]);
            }

            if (!GaussJordanElimination(matrix, vectors))
                throw new InvalidOperationException("Unsolvable matrix detected.");

            return InvertPerspectiveCoefficients(vectors);
        }

        private static double[] GetCoefficients(IMagickImage image, double maxRad)
        {
            double[] coeff = new double[8];
            coeff[0] = maxRad;
            coeff[1] = 0.0;
            coeff[2] = image.Width / 2.0;
            coeff[3] = image.Height / 2.0;
            coeff[4] = -Math.PI;
            coeff[5] = Math.PI;
            coeff[6] = (coeff[5] - coeff[4]) / image.Width;
            coeff[7] = (coeff[0] - coeff[1]) / image.Height;
            return coeff;
        }

        private static PointD[] GetCorners(IMagickImage paddedMask, List<PixelValue> maxList, double maxRad, int xOffset)
        {
            double[] coeff = GetCoefficients(paddedMask, maxRad);

            PointD[] corners = new PointD[4];

            for (int i = 0; i < 4; i++)
                corners[i] = GetCorner(maxList, coeff, xOffset, paddedMask.Height, i);

            return corners;
        }

        private static PointD GetCorner(List<PixelValue> maxList, double[] coeff, int xOffset, int height, int index)
        {
            double aa = ((maxList[index].Position + 0.5) * coeff[6]) + coeff[5];
            double rr = ((((maxList[index].Value * height) / 65535.0) + 0.5) * coeff[7]) + coeff[1];

            return new PointD((rr * Math.Sin(aa)) + coeff[2] - 0.5 - xOffset, (rr * Math.Cos(aa)) + coeff[3] - 0.5);
        }

        private static PointD GetPoint(double[] coeff, double x, double y)
        {
            return new PointD(
              Math.Floor(((coeff[0] * x) + (coeff[1] * y) + coeff[2]) / ((coeff[6] * x) + (coeff[7] * y) + 1)),
              Math.Floor(((coeff[3] * x) + (coeff[4] * y) + coeff[5]) / ((coeff[6] * x) + (coeff[7] * y) + 1)));
        }

        private static MagickGeometry GetViewport(double[] arguments, PointD[] corners)
        {
            double[] coeff = GetCoefficients(arguments);

            PointD i1 = GetPoint(coeff, corners[0].X, corners[0].Y);
            PointD i2 = GetPoint(coeff, corners[1].X, corners[1].Y);
            PointD i3 = GetPoint(coeff, corners[2].X, corners[2].Y);
            PointD i4 = GetPoint(coeff, corners[3].X, corners[3].Y);

            double xmax = Math.Max(Math.Max(Math.Max(i1.X, i2.X), i3.X), i4.X);
            double ymax = Math.Max(Math.Max(Math.Max(i1.Y, i2.Y), i3.Y), i4.Y);
            double xmin = Math.Min(Math.Min(Math.Min(i1.X, i2.X), i3.X), i4.X);
            double ymin = Math.Min(Math.Min(Math.Min(i1.Y, i2.Y), i3.Y), i4.Y);

            double iw = Math.Abs(xmax - xmin);
            double ih = Math.Abs(ymax - ymin);

            return new MagickGeometry((int)xmin, (int)ymin, (int)iw, (int)ih);
        }

        private static bool GaussJordanElimination(double[][] matrix, double[] vectors)
        {
            double[] columns = new double[8];
            double[] rows = new double[8];
            double[] pivots = new double[8];
            int column = 0;
            int row = 0;
            for (int i = 0; i < 8; i++)
            {
                double max = 0.0;
                for (int j = 0; j < 8; j++)
                {
                    if (pivots[j] == 1)
                        continue;

                    for (int k = 0; k < 8; k++)
                    {
                        if (pivots[k] != 0)
                        {
                            if (pivots[k] > 1)
                                return false;
                        }
                        else if (Math.Abs(matrix[j][k]) >= max)
                        {
                            max = Math.Abs(matrix[j][k]);
                            row = j;
                            column = k;
                        }
                    }
                }

                pivots[column]++;
                if (row != column)
                {
                    for (int k = 0; k < 8; k++)
                        GaussJordanSwap(ref matrix[row][k], ref matrix[column][k]);
                    GaussJordanSwap(ref vectors[row], ref vectors[column]);
                }

                rows[i] = row;
                columns[i] = column;
                if (matrix[column][column] == 0.0)
                    return false;  /* sigularity */
                double scale = PerceptibleReciprocal(matrix[column][column]);
                matrix[column][column] = 1.0;
                for (int j = 0; j < 8; j++)
                    matrix[column][j] *= scale;
                vectors[column] *= scale;
                for (int j = 0; j < 8; j++)
                {
                    if (j == column)
                        continue;
                    scale = matrix[j][column];
                    matrix[j][column] = 0.0;
                    for (int k = 0; k < 8; k++)
                        matrix[j][k] -= scale * matrix[column][k];
                    vectors[j] -= scale * vectors[column];
                }
            }

            return true;
        }

        private static void GaussJordanSwap(ref double x, ref double y)
        {
            if (x == y)
                return;

            x += y;
            y = x - y;
            x = x - y;
        }

        private static double Hypot(double x, double y)
        {
            return Math.Sqrt((x * x) + (y * y));
        }

        private static double[] InvertPerspectiveCoefficients(double[] coeff)
        {
            double[] inverse = new double[8];

            double determinant = PerceptibleReciprocal((coeff[0] * coeff[4]) - (coeff[3] * coeff[1]));
            inverse[0] = determinant * (coeff[4] - (coeff[7] * coeff[5]));
            inverse[1] = determinant * ((coeff[7] * coeff[2]) - coeff[1]);
            inverse[2] = determinant * ((coeff[1] * coeff[5]) - (coeff[4] * coeff[2]));
            inverse[3] = determinant * ((coeff[6] * coeff[5]) - coeff[3]);
            inverse[4] = determinant * (coeff[0] - (coeff[6] * coeff[2]));
            inverse[5] = determinant * ((coeff[3] * coeff[2]) - (coeff[0] * coeff[5]));
            inverse[6] = determinant * ((coeff[3] * coeff[7]) - (coeff[6] * coeff[4]));
            inverse[7] = determinant * ((coeff[6] * coeff[1]) - (coeff[0] * coeff[7]));

            return inverse;
        }

        private static void LeastSquaresAddTerms(double[][] matrix, double[] vectors, double[] terms, double result)
        {
            for (int j = 0; j < 8; j++)
            {
                for (int i = 0; i < 8; i++)
                    matrix[i][j] += terms[i] * terms[j];
                vectors[j] += result * terms[j];
            }
        }

        private static void PadImage(IMagickImage image, int borderSize)
        {
            image.Border(borderSize);
        }

        private static double PerceptibleReciprocal(double value)
        {
            double
              sign;

            sign = value < 0.0 ? -1.0 : 1.0;
            if ((sign * value) >= double.Epsilon)
                return 1.0 / value;
            return sign / double.Epsilon;
        }

        private static void RemoveInvalidValues(List<PixelValue> list, int width)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i].Position >= width && list[i].Position < 2 * width)
                {
                    list[i].Position -= width;
                    continue;
                }

                list.RemoveAt(i);
            }
        }

        private static void ResetMaxList(List<PixelValue> maxList, IMagickImage image)
        {
            using (var pixels = image.GetPixels())
            {
                var values = pixels.ToShortArray(0, 0, image.Width, 1, "R");
                for (int i = 0; i < maxList.Count; i++)
                {
                    maxList[i].Value = values[maxList[i].Position];
                }
            }
        }

        private static void Rotate(IMagickImage output, UnperspectiveRotation rotation)
        {
            if (rotation != UnperspectiveRotation.None)
                output.Rotate((int)rotation);
        }

        private double CalculateAspectRation(IMagickImage image, PointD[] corners)
        {
            if (AspectRatio.HasValue)
                return AspectRatio.Value;

            double centroidX = image.Width / 2.0;
            double centroidY = image.Height / 2.0;

            // convert to proper x,y coordinates relative to center
            var m1x = corners[1].X - centroidX;
            var m1y = centroidY - corners[1].Y;
            var m2x = corners[2].X - centroidX;
            var m2y = centroidY - corners[2].Y;
            var m3x = corners[0].X - centroidX;
            var m3y = centroidY - corners[0].Y;
            var m4x = corners[3].X - centroidX;
            var m4y = centroidY - corners[3].Y;

            // simplified equations, assuming u0=0, v0=0, s=1
            var k2 = (((m1y - m4y) * m3x) - ((m1x - m4x) * m3y) + (m1x * m4y) - (m1y * m4x)) / (((m2y - m4y) * m3x) - ((m2x - m4x) * m3y) + (m2x * m4y) - (m2y * m4x));
            var k3 = (((m1y - m4y) * m2x) - ((m1x - m4x) * m2y) + (m1x * m4y) - (m1y * m4x)) / (((m3y - m4y) * m2x) - ((m3x - m4x) * m2y) + (m3x * m4y) - (m3y * m4x));
            var ff = ((((k3 * m3y) - m1y) * ((k2 * m2y) - m1y)) + (((k3 * m3x) - m1x) * ((k2 * m2x) - m1x))) / ((k3 - 1) * (k2 - 1));
            var f = Math.Sqrt(Math.Sqrt(ff * ff));
            var aspect = Math.Sqrt((Math.Pow(k2 - 1, 2) + (Math.Pow((k2 * m2y) - m1y, 2) / Math.Pow(f, 2)) + (Math.Pow((k2 * m2x) - m1x, 2) / Math.Pow(f, 2))) / (Math.Pow(k3 - 1, 2) + (Math.Pow((k3 * m3y) - m1y, 2) / Math.Pow(f, 2)) + (Math.Pow((k3 * m3x) - m1x, 2) / Math.Pow(f, 2))));

            return aspect;
        }

        private void CheckSettings()
        {
            if (Width != null && Height != null)
                throw new InvalidOperationException("Both width and height cannot be specified at the same time.");

            if (Default != UnperspectiveDefault.BoundingBoxHeight && Default != UnperspectiveDefault.BoundingBoxWidth &&
                Default != UnperspectiveDefault.EdgeLength && Default != UnperspectiveDefault.Height && Default != UnperspectiveDefault.Width)
                throw new InvalidOperationException("Invalid default output dimension specified.");
        }

        private IMagickImage CreateMask(IMagickImage image)
        {
            var mask = image.Clone();
            mask.Alpha(AlphaOption.Off);
            mask.Alpha(AlphaOption.Set);
            mask.ColorFuzz = ColorFuzz;
            mask.Settings.FillColor = MagickColors.Transparent;
            mask.Draw(new DrawableAlpha(0, 0, PaintMethod.Floodfill));
            mask.InverseOpaque(MagickColors.Transparent, MagickColors.White);
            mask.Opaque(MagickColors.Transparent, MagickColors.Black);
            mask.Alpha(AlphaOption.Off);

            if (Blur != 0.0)
            {
                mask.Blur(0, Blur);
                mask.Threshold(new Percentage(0));
            }

            return mask;
        }

        private IMagickImage CreatePaddedMask(IMagickImage image, out int xOffset)
        {
            xOffset = 0;
            int minWidth = 500;

            var paddedMask = CreateMask(image);
            if (paddedMask.Width < minWidth)
            {
                paddedMask.BackgroundColor = MagickColors.Black;
                paddedMask.Extent(minWidth, paddedMask.Height, Gravity.Center);
                xOffset = (minWidth - image.Width) / 2;
            }

            return paddedMask;
        }

        private void Distort(IMagickImage output, PointD[] corners, MagickGeometry inputDimensions, MagickGeometry trimmedDimensions, MagickColor backgroundColor)
        {
            MagickGeometry outputDimensions = GetDimensions(output, corners, inputDimensions, trimmedDimensions);

            double[] arguments = new double[16]
            {
        corners[0].X, corners[0].Y, 0, 0,
        corners[1].X, corners[1].Y, 0, outputDimensions.Height,
        corners[2].X, corners[2].Y, outputDimensions.Width, outputDimensions.Height,
        corners[3].X, corners[3].Y, outputDimensions.Width, 0
            };

            output.VirtualPixelMethod = VirtualPixelMethod.Background;
            output.BackgroundColor = backgroundColor;
            if (!DisableViewportCrop)
                output.SetArtifact("distort:viewport", GetViewport(arguments, corners).ToString());
            output.Distort(DistortMethod.Perspective, true, arguments);
            output.BorderColor = backgroundColor;
            output.Border(2);
            output.ColorFuzz = ColorFuzz;
            output.Trim();
            output.RePage();
        }

        private MagickColor GetBorderColor(IMagickImage image)
        {
            using (var pixels = image.GetPixels())
            {
                return pixels.GetPixel((int)BorderColorLocation.X, (int)BorderColorLocation.Y).ToColor();
            }
        }

        private MagickGeometry GetDimensions(IMagickImage image, PointD[] corners, MagickGeometry inputDimensions, MagickGeometry trimmedDimensions)
        {
            double left = Hypot(corners[0].X - corners[1].X, corners[0].Y - corners[1].Y);
            double bottom = Hypot(corners[1].X - corners[2].X, corners[1].Y - corners[2].Y);
            double right = Hypot(corners[2].X - corners[3].X, corners[2].Y - corners[3].Y);
            double top = Hypot(corners[3].X - corners[0].X, corners[3].Y - corners[0].Y);

            if (left < MinLength || bottom < MinLength || right < MinLength || top < MinLength)
                throw new InvalidOperationException("Unable to continue, the edge length is less than " + MaxPeaks + ".");

            double aspectRatio = CalculateAspectRation(image, corners);

            if (Height != null)
                return new MagickGeometry((int)Math.Floor(aspectRatio * Height.Value), Height.Value);

            if (Width != null)
                return new MagickGeometry(Width.Value, (int)Math.Floor(Width.Value / aspectRatio));

            if (Default == UnperspectiveDefault.EdgeLength)
                return new MagickGeometry((int)Math.Floor(left * aspectRatio), (int)left);

            if (Default == UnperspectiveDefault.BoundingBoxHeight)
                return new MagickGeometry((int)Math.Floor(trimmedDimensions.Height * aspectRatio), trimmedDimensions.Height);

            if (Default == UnperspectiveDefault.BoundingBoxWidth)
                return new MagickGeometry(trimmedDimensions.Width, (int)Math.Floor(trimmedDimensions.Width / aspectRatio));

            if (Default == UnperspectiveDefault.Height)
                return new MagickGeometry((int)Math.Floor(inputDimensions.Height * aspectRatio), inputDimensions.Height);

            // Default == UnperspectiveDefault.Width
            return new MagickGeometry(inputDimensions.Width, (int)Math.Floor(inputDimensions.Width / aspectRatio));
        }

        private ushort[] GetGrayChannel(IMagickImage image)
        {
            using (var images = new MagickImageCollection())
            {
                images.Add(image.Clone());
                images.Add(image.Clone());
                images.Add(image.Clone());

                using (var depolar = images.AppendHorizontally())
                {
                    if (Smooth != 0.0)
                        depolar.Blur(0, Smooth);

                    if (Sharpen != 0.0)
                        depolar.Sharpen(0, Sharpen);

                    if (_method == UnperspectiveMethod.Derivative)
                    {
                        depolar.VirtualPixelMethod = VirtualPixelMethod.Tile;
                        depolar.SetArtifact("convolve:scale", "50%!");
                        depolar.SetArtifact("convolve:bias", "50%");
                        depolar.Morphology(MorphologyMethod.Convolve, "3x1: -1,0,1");
                        depolar.AutoLevel();

                        depolar.SetArtifact("convolve:bias", "0");
                        depolar.Morphology(MorphologyMethod.Convolve, "3x1: 1,0,-1");
                        depolar.AutoLevel();
                        depolar.Crop(image.Width, 1, Gravity.Center);
                    }

                    depolar.Depth = 16;

                    using (var pixels = depolar.GetPixels())
                    {
                        return pixels.ToShortArray(0, 0, depolar.Width, 1, "R");
                    }
                }
            }
        }

        private List<PixelValue> GetPeaks(ushort[] pixels, IMagickImage image)
        {
            ushort min = ushort.MaxValue;
            ushort max = ushort.MinValue;

            List<PixelValue> minList = new List<PixelValue>();
            List<PixelValue> maxList = new List<PixelValue>();
            bool lookingForMax = true;

            for (int i = 0; i < pixels.Length; i++)
            {
                ushort pixel = pixels[i];
                if (pixel > max)
                    max = pixel;

                if (pixel < min)
                    min = pixel;

                if (lookingForMax)
                {
                    if (pixel < max)
                    {
                        int j = i - 1;
                        maxList.Add(new PixelValue(j, pixels[j]));
                        min = pixel;
                        lookingForMax = false;
                    }
                }
                else
                {
                    if (pixel > min)
                    {
                        int j = i - 1;
                        minList.Add(new PixelValue(j, pixels[j]));
                        max = pixel;
                        lookingForMax = true;
                    }
                }
            }

            if (_method == UnperspectiveMethod.Peak)
            {
                RemoveInvalidValues(maxList, image.Width);
                RemoveInvalidValues(minList, image.Width);
            }

            if (maxList.Count > MaxPeaks)
                throw new InvalidOperationException("Unable to continue, the number of peaks is higher than " + MaxPeaks + ".");

            RemoveFalsePeaks(maxList, minList);

            if (maxList.Count != 4)
                throw new InvalidOperationException("Unable to continue, the number of peaks should be 4.");

            ResetMaxList(maxList, image);

            return maxList;
        }

        private UnperspectiveRotation GetRotation(PointD[] corners, IMagickImage image)
        {
            if (Rotation != null)
                return Rotation.Value;

            var m3x = corners[0].X - (image.Width / 2.0);
            var m3y = (image.Height / 2.0) - corners[0].Y;

            if (m3x < 0 && m3y < 0)
                return UnperspectiveRotation.Rotate270;
            else if (m3x > 0 && m3y < 0)
                return UnperspectiveRotation.Rotate180;
            else if (m3x > 0 && m3y > 0)
                return UnperspectiveRotation.Rotate90;
            else
                return UnperspectiveRotation.None;
        }

        private void RemoveFalsePeaks(List<PixelValue> maxList, List<PixelValue> minList)
        {
            if (Threshold == 0)
                return;

            if (_method == UnperspectiveMethod.Peak)
            {
                double posThreshold = Math.Pow(Threshold, 2);
                double threshold = Math.Pow(Threshold * 255, 2);

                for (int i = maxList.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < minList.Count; j++)
                    {
                        if (Math.Pow(maxList[i].Position - minList[j].Position, 2) <= posThreshold && Math.Pow(maxList[i].Value - minList[j].Value, 2) <= threshold)
                        {
                            maxList.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
            else
            {
                double threshold = Threshold * 255;
                for (int i = maxList.Count - 1; i >= 0; i--)
                {
                    if (maxList[i].Value <= threshold)
                        maxList.RemoveAt(i);
                }
            }
        }

        private MagickGeometry TrimImage(IMagickImage image, MagickColor backgroundColor, int borderSize)
        {
            image.BorderColor = backgroundColor;
            image.Border(borderSize);
            image.ColorFuzz = ColorFuzz;
            image.Trim();
            image.RePage();

            return new MagickGeometry(image.Width, image.Height);
        }
    }
}
