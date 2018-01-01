// Copyright 2015-2018 Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/FredsImageMagickScripts.NET)
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
using System.Globalization;
using ImageMagick;

namespace FredsImageMagickScripts
{
    /// <summary>
    /// Transforms an image to place it in a region of a tshirt image. The transformed image will
    /// display hightlights from the tshirt image and be distorted to match the wrinkles in the
    /// tshirt image.
    /// </summary>
    public sealed class TshirtScript
    {
        private PointD[] _coords;

        /// <summary>
        /// Initializes a new instance of the <see cref="TshirtScript"/> class.
        /// </summary>
        public TshirtScript()
        {
            Reset();
        }

        /// <summary>
        /// Gets or sets the antialias amount to apply to alpha channel of tshirt image.Values should be
        /// higher than or equal to zero. The default is 2.
        /// </summary>
        public double AntiAlias
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the blurring to apply to the displacement map to avoid jagged displacement.
        /// Values should be higherthan zero. The default is 1.
        /// </summary>
        public double Blur
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the amount of displacement for the distortion of the overlay image. Values should be
        /// higher than zero. The default is 10.
        /// </summary>
        public int Displace
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fitting method. If Crop, then the overlay image will be cropped to make its vertical
        /// aspect ratio fit that of coordinate area. This will not distort the image, only make it fit the size of
        /// the coordinate area. The image will not be cropped, if its vertical aspect ratio is smaller than that
        /// of the region. If Distort, the overlay image  will be fit to the coordinate area, if the aspect ratio
        /// of the overlay image  does not match that of the region or coordinate area. The default is None.
        /// </summary>
        public TshirtFit Fit
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the contrast increase for highlights to apply to the overlay image. Valid values are
        /// between 0 and 30. The default is 20.
        /// </summary>
        public int Lighting
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the gravity for selecting the crop location. The choices are: North, South or Center.
        /// The default is Center.
        /// </summary>
        public Gravity Gravity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an additional clockwise positive rotation in order to make orientational adjustments easier.
        /// Values are betweneen -360 and 360. The default is 0.
        /// </summary>
        public double Rotation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sharpening to apply to the overlay image. Values should be higher than or equal to than zero.
        /// The default is 1.
        /// </summary>
        public double Sharpen
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the vertical shift of the crop region with respect to the gravity setting. Negative is upward and
        /// positive is downward. The default is 0 (no shift).
        /// </summary>
        public int VerticalShift
        {
            get;
            set;
        }

        /// <summary>
        /// Transforms an image to place it in a region of a tshirt image. The transformed image will
        /// display hightlights from the tshirt image and be distorted to match the wrinkles in the
        /// tshirt image.
        /// </summary>
        /// <param name="tshirt">The image of the shirt to put the overlay on.</param>
        /// <param name="overlay">The overlay to put on top of the shirt.</param>
        /// <returns>The resulting image.</returns>
        public IMagickImage Execute(IMagickImage tshirt, IMagickImage overlay)
        {
            if (tshirt == null)
                throw new ArgumentNullException("tshirt");

            if (overlay == null)
                throw new ArgumentNullException("overlay");

            CheckSettings(tshirt);

            var x = _coords[1].X - _coords[0].X;
            var y = _coords[1].Y - _coords[0].Y;
            var topWidth = Math.Sqrt((x * x) + (y * y));
            var scale = (overlay.Width - 1) / (topWidth / 1);

            var overlayCoordinates = CreateOverlayCoordinates(overlay, scale);
            var tshirtCoordinates = CreateTshirtCoordinates(overlay, scale, topWidth);

            var alpha = ExtractAlpha(tshirt);
            var gray = ToGrayScale(tshirt);
            var mean = SubtractMean(gray, tshirtCoordinates);

            ApplyLighting(mean);
            var light = mean.Clone();
            mean.Dispose();

            var blur = gray.Clone();
            ApplyBlur(blur);

            var distorted = DistortOverlay(gray, overlay, overlayCoordinates, tshirtCoordinates);

            var displaced = DisplaceOverlay(distorted, light, blur);
            distorted.Dispose();
            light.Dispose();
            blur.Dispose();

            var output = tshirt.Clone();
            output.Composite(displaced, CompositeOperator.Over);
            displaced.Dispose();

            if (alpha != null)
            {
                output.Alpha(AlphaOption.Off);
                output.Composite(alpha, CompositeOperator.CopyAlpha);

                alpha.Dispose();
            }

            return output;
        }

        /// <summary>
        /// Resets the script to the default setttings.
        /// </summary>
        public void Reset()
        {
            _coords = null;
            AntiAlias = 2.0;
            Blur = 1.0;
            Displace = 10;
            Fit = TshirtFit.None;
            Gravity = Gravity.Center;
            Lighting = 20;
            Rotation = 0.0;
            Sharpen = 1.0;
            VerticalShift = 0;
        }

        /// <summary>
        /// Sets the four x,y corners of the region in the tshirt where the overlay image will be placed.
        /// Coordinates are not restricted to a rectangle and the region defined by the coordinates can
        /// have rotation.
        /// </summary>
        /// <param name="topLeft">Top left coordinate</param>
        /// <param name="topRight">Top right coordinate</param>
        /// <param name="bottomRight">Bottom right coordinate</param>
        /// <param name="bottomLeft">Bottom left coordinate</param>
        public void SetCoordinates(PointD topLeft, PointD topRight, PointD bottomRight, PointD bottomLeft)
        {
            _coords = new PointD[] { topLeft, topRight, bottomRight, bottomLeft };
        }

        /// <summary>
        /// Sets the four x,y corners of the region in the tshirt where the overlay image will be placed.
        /// </summary>
        /// <param name="geometry">The geometry</param>
        public void SetCoordinates(MagickGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentNullException("geometry");

            var topLeft = new PointD(geometry.X, geometry.Y);
            var topRight = new PointD(geometry.X + geometry.Width - 1, geometry.Y);
            var bottomRight = new PointD(geometry.X + geometry.Width - 1, geometry.Y + geometry.Height - 1);
            var bottomLeft = new PointD(geometry.X, geometry.Y + geometry.Height - 1);
            SetCoordinates(topLeft, topRight, bottomRight, bottomLeft);
        }

        private static void CheckCoordinate(IMagickImage image, string paramName, PointD coord)
        {
            if (coord.X < 0 || coord.X > image.Width)
                throw new ArgumentOutOfRangeException(paramName);

            if (coord.Y < 0 || coord.Y > image.Height)
                throw new ArgumentOutOfRangeException(paramName);
        }

        private static double[] CreateArguments(PointD[] overlayCoordinates, PointD[] tshirtCoordinates)
        {
            var result = new double[16];

            int i = 0;
            for (int j = 0; j < 4; j++)
            {
                result[i++] = overlayCoordinates[j].X;
                result[i++] = overlayCoordinates[j].Y;

                result[i++] = tshirtCoordinates[j].X;
                result[i++] = tshirtCoordinates[j].Y;
            }

            return result;
        }

        private static IMagickImage SubtractMean(IMagickImage image, PointD[] coords)
        {
            using (var img = image.Clone())
            {
                int minX = (int)Math.Min(Math.Min(coords[0].X, coords[1].X), Math.Min(coords[2].X, coords[3].X));
                int minY = (int)Math.Min(Math.Min(coords[0].Y, coords[1].Y), Math.Min(coords[2].Y, coords[3].Y));
                int maxX = (int)Math.Max(Math.Max(coords[0].X, coords[1].X), Math.Max(coords[2].X, coords[3].X));
                int maxY = (int)Math.Max(Math.Max(coords[0].Y, coords[1].Y), Math.Max(coords[2].Y, coords[3].Y));

                int width = maxX - minX + 1;
                int height = maxY - minY + 1;

                img.Crop(minX, minY, width, height);
                img.RePage();

                var statistics = img.Statistics();
                double mean = (statistics.Composite().Mean / Quantum.Max) - 0.5;

                var result = image.Clone();
                result.Evaluate(Channels.All, EvaluateOperator.Subtract, mean * Quantum.Max);
                return result;
            }
        }

        private static IMagickImage ToGrayScale(IMagickImage image)
        {
            var gray = image.Clone();
            gray.Alpha(AlphaOption.Off);
            gray.ColorSpace = ColorSpace.Gray;

            return gray;
        }

        private void ApplyBlur(IMagickImage image)
        {
            if (Blur != 0)
                image.Blur(0, Blur);
        }

        private void ApplyLighting(IMagickImage image)
        {
            if (Lighting != 0)
                image.SigmoidalContrast(true, Lighting / 3.0);
        }

        private void ApplySharpen(IMagickImage image)
        {
            if (Sharpen != 0)
                image.UnsharpMask(0, Sharpen);
        }

        private void CheckSettings(IMagickImage image)
        {
            if (_coords == null)
                throw new InvalidOperationException("No coordinates have been set.");

            CheckCoordinate(image, "topLeft", _coords[0]);
            CheckCoordinate(image, "topRight", _coords[1]);
            CheckCoordinate(image, "bottomRight", _coords[2]);
            CheckCoordinate(image, "bottomLeft", _coords[3]);

            if (Gravity != Gravity.North && Gravity != Gravity.Center && Gravity != Gravity.South)
                throw new InvalidOperationException("Invalid gravity specified.");

            if (Rotation < -360 || Rotation > 360)
                throw new InvalidOperationException("Invalid rotation specified, value must be between -360 and 360.");

            if (Lighting < 0 || Lighting > 30)
                throw new InvalidOperationException("Invalid lighting specified, value must be between 0 and 30.");

            if (Blur < 0)
                throw new InvalidOperationException("Invalid blur specified, value should be zero or higher.");
        }

        private PointD[] CreateOverlayCoordinates(IMagickImage overlay, double scale)
        {
            var angle = -Math.Atan2(_coords[1].Y - _coords[0].Y, _coords[1].X - _coords[0].X);
            var xOffset = _coords[0].X;
            var yOffset = _coords[0].Y;

            PointD[] coords = new PointD[4];
            for (int i = 0; i < 4; i++)
            {
                coords[i] = new PointD(
                  (int)Math.Round(((_coords[i].X - xOffset) * Math.Cos(angle)) + ((_coords[i].Y - yOffset) * Math.Sin(angle))),
                  (int)Math.Round(((_coords[i].X - xOffset) * Math.Sin(angle)) + ((_coords[i].Y - yOffset) * Math.Cos(angle))));
            }

            double ho = Math.Max(coords[3].Y - coords[0].Y, coords[2].Y - coords[1].Y);

            coords[0] = new PointD(0, 0);
            coords[1] = new PointD(overlay.Width - 1, 0);
            if (Fit == TshirtFit.Distort)
                coords[2] = new PointD(overlay.Width - 1, overlay.Height - 1);
            else
                coords[2] = new PointD(overlay.Width - 1, scale * ho);
            coords[3] = new PointD(0, coords[2].Y);

            return coords;
        }

        private PointD[] CreateTshirtCoordinates(IMagickImage overlay, double scale, double topWidth)
        {
            if (Rotation == 0)
                return _coords;

            var rotate = (Math.PI / 180) * Rotation;
            var xcent = Math.Round(0.5 * topWidth) + _coords[0].X;
            var ycent = Math.Round((0.5 * (overlay.Height / scale)) + _coords[0].Y);

            var coords = new PointD[4];
            for (int i = 0; i < 4; i++)
            {
                coords[i] = new PointD(
                  (int)Math.Round(xcent + ((_coords[i].X - xcent) * Math.Cos(rotate)) - ((_coords[i].Y - ycent) * Math.Sin(rotate))),
                  (int)Math.Round(ycent + ((_coords[i].X - xcent) * Math.Sin(rotate)) + ((_coords[i].Y - ycent) * Math.Cos(rotate))));
            }

            return coords;
        }

        private IMagickImage CropOverlay(IMagickImage image, PointD[] coords)
        {
            var result = image.Clone();
            if (Fit == TshirtFit.Crop)
            {
                int height = (int)coords[2].Y + 1;
                if (image.Height > height)
                    result.Crop(image.Width, height, Gravity);
            }

            return result;
        }

        private IMagickImage DisplaceOverlay(IMagickImage overlay, IMagickImage light, IMagickImage blur)
        {
            var mergedAlpha = overlay.Clone();
            mergedAlpha.Alpha(AlphaOption.Extract);

            var output = overlay.Clone();
            output.Composite(light, CompositeOperator.HardLight);

            output.Alpha(AlphaOption.Off);
            output.Composite(mergedAlpha, CompositeOperator.CopyAlpha);
            mergedAlpha.Dispose();

            var args = string.Format(CultureInfo.InvariantCulture, "{0},{0}", -Displace);
            output.Composite(blur, 0, 0, CompositeOperator.Displace, args);

            return output;
        }

        private IMagickImage DistortOverlay(IMagickImage grayShirt, IMagickImage overlay, PointD[] overlayCoordinates, PointD[] tshirtCoordinates)
        {
            using (var images = new MagickImageCollection())
            {
                grayShirt.Alpha(AlphaOption.Transparent);
                grayShirt.BackgroundColor = MagickColors.Transparent;
                images.Add(grayShirt);

                var croppedOverlay = CropOverlay(overlay, overlayCoordinates);
                croppedOverlay.VirtualPixelMethod = VirtualPixelMethod.Transparent;

                var arguments = CreateArguments(overlayCoordinates, tshirtCoordinates);
                var distortSettings = new DistortSettings()
                {
                    Bestfit = true
                };
                croppedOverlay.Distort(DistortMethod.Perspective, distortSettings, arguments);
                ApplySharpen(croppedOverlay);

                images.Add(croppedOverlay);

                return images.Merge();
            }
        }

        private IMagickImage ExtractAlpha(IMagickImage image)
        {
            if (!image.HasAlpha)
                return null;

            var alpha = image.Clone();
            alpha.Alpha(AlphaOption.Extract);
            alpha.Blur(0, AntiAlias);
            alpha.Level((Percentage)50, (Percentage)100);

            return alpha;
        }
    }
}
