// Copyright 2015-2020 Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/FredsImageMagickScripts.NET)
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
using ImageMagick;

namespace FredsImageMagickScripts
{
    /// <summary>
    /// Automatically trim a (nearly) uniform color border around an image. If the image is rotated,
    /// one can trim to the bounding box around the image area or alternately trim to the maximum
    /// central area that contains no border pixels. The excess border does not have to completely
    /// surround the image. It may be only on one side. However, one must identify a coordinate
    /// within the border area for the algorithm to extract the base border color and also specify
    /// a fuzz value when the border color is not uniform. For simple border trimming of a normally
    /// oriented image or the bounding box of a rotated image, you may err somewhat towards larger
    /// than optimal fuzz values. For images that contain rotated picture data, when you want to
    /// trim to the central area, you should choose the smallest fuzz value that is appropriate.
    /// For images that contain rotated picture data, an estimate of the rotation angle is needed
    /// for the algorithm to work. However, setting the rotation angle to zero will let the
    /// algorithm determine the rotation angle. The resulting trim is usually pretty good for
    /// angles >= 5 degrees. If the result is off a little, you may use the left/right/top/bottom
    /// arguments to adjust the automatically determined trim region.
    /// </summary>
    /// <typeparam name="TQuantumType">The quantum type.</typeparam>
    public sealed partial class AutotrimScript<TQuantumType>
        where TQuantumType : struct
    {
        private readonly IMagickFactory<TQuantumType> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutotrimScript{TQuantumType}"/> class.
        /// </summary>
        /// <param name="factory">The magick factory.</param>
        public AutotrimScript(IMagickFactory<TQuantumType> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            Reset();
        }

        /// <summary>
        /// Gets or sets any location within the border area for the algorithm to find the base border color.
        /// </summary>
        public PointD BorderColorLocation { get; set; }

        /// <summary>
        /// Gets or sets the fuzz amount specified as a percent 0 to 100. The default is zero which indicates
        /// that border is a uniform color. Larger values are needed when the border is not a uniform color
        /// and to trim the border of the rotated area where the image data is a blend with the
        /// border color.
        /// </summary>
        public Percentage ColorFuzz { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether inner trimming is used. Default is outer trim (false).
        /// </summary>
        public bool InnerTrim { get; set; }

        /// <summary>
        /// Gets the number of extra pixels to shift the trim of the image.
        /// </summary>
        public AutotrimPixelShift PixelShift { get; private set; }

        /// <summary>
        /// Automatically unrotates a rotated image and trims the surrounding border.
        /// </summary>
        /// <param name="input">The image to execute the script on.</param>
        /// <returns>The resulting image.</returns>
        public IMagickImage<TQuantumType> Execute(IMagickImage<TQuantumType> input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var output = input.Clone();
            var borderColor = GetBorderColor(output);

            if (InnerTrim)
                ExecuteInnerTrim(output, borderColor);
            else
                ExecuteOuterTrim(output, borderColor);

            return output;
        }

        /// <summary>
        /// Resets the script to the default setttings.
        /// </summary>
        public void Reset()
        {
            BorderColorLocation = new PointD(0, 0);
            ColorFuzz = (Percentage)0;
            InnerTrim = false;
            PixelShift = new AutotrimPixelShift();
        }

        private static void SwapPoints(Line[] points)
        {
            Line swap;
            if (points[0].Y > points[1].Y)
            {
                swap = points[0];
                points[0] = points[1];
                points[1] = swap;
            }

            if (points[3].Y < points[2].Y)
            {
                swap = points[2];
                points[2] = points[3];
                points[3] = swap;
            }
        }

        private IMagickGeometry TestGeometry(IMagickGeometry geometry, Line line1, Line line2)
        {
            int x = Math.Max(line1.X1, line2.X1);
            int y = line1.Y;

            int width = Math.Min(line1.X2, line2.X2) - x;
            int height = line2.Y - line1.Y;

            var newGeometry = _factory.Geometry.Create(x, y, width, height);

            return newGeometry.CompareTo(geometry) == 1 ? newGeometry : geometry;
        }

        private void Crop(IMagickImage<TQuantumType> image, IMagickGeometry area)
        {
            ShiftGeometry(area);
            image.Crop(area);
            image.RePage();
        }

        private IMagickColor<TQuantumType> GetBorderColor(IMagickImage<TQuantumType> image)
        {
            using (var pixels = image.GetPixels())
            {
                return pixels.GetPixel((int)BorderColorLocation.X, (int)BorderColorLocation.Y).ToColor();
            }
        }

        private IMagickGeometry GetLargestArea(IMagickImage<TQuantumType> image, IMagickColor<TQuantumType> borderColor)
        {
            var points = new Line[4];

            using (var pixels = image.GetPixelsUnsafe())
            {
                var line = new Line(0, 0);

                while (IsBorderColor(pixels, line.X1, line.Y, borderColor) && line.Y < image.Height - 1 && line.X1 < image.Width - 1)
                {
                    line.Y++;
                    line.X1++;
                }

                line.X2 = image.Width - 1;
                while (IsBorderColor(pixels, line.X2, line.Y, borderColor) && line.X2 > 0)
                    line.X2--;

                points[0] = line;

                line = new Line(image.Width - 1, 0);

                while (IsBorderColor(pixels, line.X2, line.Y, borderColor) && line.Y < image.Height && line.X2 > 0)
                {
                    line.Y++;
                    line.X2--;
                }

                line.X1 = 0;
                while (IsBorderColor(pixels, line.X1, line.Y, borderColor) && line.X1 < image.Width - 1)
                    line.X1++;

                points[1] = line;

                line = new Line(0, image.Height - 1);

                while (IsBorderColor(pixels, line.X1, line.Y, borderColor) && line.Y > 0 && line.X1 < image.Width - 1)
                {
                    line.Y--;
                    line.X1++;
                }

                line.X2 = image.Width - 1;
                while (IsBorderColor(pixels, line.X2, line.Y, borderColor) && line.X2 > 0)
                    line.X2--;

                points[2] = line;

                line = new Line(image.Width - 1, image.Height - 1);

                while (IsBorderColor(pixels, line.X2, line.Y, borderColor) && line.Y > 0 && line.X2 > 0)
                {
                    line.Y--;
                    line.X2--;
                }

                line.X1 = 0;
                while (IsBorderColor(pixels, line.X1, line.Y, borderColor) && line.X1 < image.Width - 1)
                    line.X1++;

                points[3] = line;
            }

            var geometry = _factory.Geometry.Create(0, 0);

            SwapPoints(points);

            geometry = TestGeometry(geometry, points[0], points[3]);
            geometry = TestGeometry(geometry, points[1], points[2]);
            geometry = TestGeometry(geometry, points[0], points[2]);
            geometry = TestGeometry(geometry, points[1], points[3]);

            return geometry;
        }

        private void ExecuteInnerTrim(IMagickImage<TQuantumType> image, IMagickColor<TQuantumType> borderColor)
        {
            var area = GetLargestArea(image, borderColor);

            image.Rotate(90);
            var rotatedArea = GetLargestArea(image, borderColor);

            if (rotatedArea.CompareTo(area) == 1)
            {
                Crop(image, rotatedArea);
                image.Rotate(-90);
            }
            else
            {
                image.Rotate(-90);
                Crop(image, area);
            }
        }

        private void ExecuteOuterTrim(IMagickImage<TQuantumType> image, IMagickColor<TQuantumType> borderColor)
        {
            image.BackgroundColor = borderColor;
            image.ColorFuzz = ColorFuzz;
            image.Trim();
            image.RePage();

            var geometry = _factory.Geometry.Create(0, 0, image.Width, image.Height);
            ShiftGeometry(geometry);
            Crop(image, geometry);
        }

        private bool IsBorderColor(IUnsafePixelCollection<TQuantumType> pixels, int x, int y, IMagickColor<TQuantumType> borderColor)
        {
            var color = pixels.GetPixel(x, y).ToColor();
            return color.FuzzyEquals(borderColor, ColorFuzz);
        }

        private void ShiftGeometry(IMagickGeometry geometry)
        {
            geometry.X += PixelShift.Left;
            geometry.Y += PixelShift.Top;
            geometry.Width -= PixelShift.Right;
            geometry.Height -= PixelShift.Bottom;
        }
    }
}
