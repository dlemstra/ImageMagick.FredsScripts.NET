// Copyright Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/ImageMagick.FredsScripts.NET)
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

namespace ImageMagick.FredsScripts
{
    /// <summary>
    /// Unrotate a rotated image and trim the surrounding border.
    /// </summary>
    /// <typeparam name="TQuantumType">The quantum type.</typeparam>
    public class UnrotateScript<TQuantumType>
        where TQuantumType : struct, IConvertible
    {
        private readonly IMagickFactory<TQuantumType> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnrotateScript{TQuantumType}"/> class.
        /// </summary>
        /// <param name="factory">The magick factory.</param>
        public UnrotateScript(IMagickFactory<TQuantumType> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            Reset();
        }

        /// <summary>
        /// Gets or sets the rotation angle needed to unrotate the picture data within the image. The default (0) tells
        /// the algorithm to automatically estimate the rotation angle. One may override the automatic determination and
        /// specify their own value. Values are positive floats between -45 and 45 degrees. Note that the algorithm cannot
        /// correct beyond 45 degrees and cannot distinguish between exactly +45 degrees and exactly -45. Therefore you may
        /// need to do a 90, 180, or 270 degree rotation after using this script.
        /// Default is (0).
        /// </summary>
        public double Angle { get; set; }

        /// <summary>
        /// Gets or sets the background color to use instead of <see cref="Coords"/>.
        /// Default is null.
        /// </summary>
        public IMagickColor<TQuantumType> BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the fuzz amount specified as a percent 0 to 100.
        /// </summary>
        /// Default is 0.
        public Percentage ColorFuzz { get; set; }

        /// <summary>
        /// Gets or sets the pixel coordinate to extract background color from.
        /// Default is (0,0).
        /// </summary>
        public PointD Coords { get; set; }

        /// <summary>
        /// Unrotate a rotated image and trim the surrounding border.
        /// </summary>
        /// <param name="input">The image to execute the script on.</param>
        /// <returns>The resulting image.</returns>
        public IMagickImage<TQuantumType> Execute(IMagickImage<TQuantumType> input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var backgroundColor = GetBackgroundColor(input);
            var angle = ComputeRotation(input, backgroundColor);

            var result = input.Clone();
            result.BackgroundColor = backgroundColor;
            result.Rotate(angle);
            result.ColorFuzz = ColorFuzz;
            result.Trim();

            return result;
        }

        /// <summary>
        /// Resets the script to the default setttings.
        /// </summary>
        public void Reset()
        {
            Angle = 0;
            BackgroundColor = null;
            ColorFuzz = (Percentage)0;
            Coords = new PointD(0);
        }

        private double ComputeRotation(IMagickImage<TQuantumType> input, IMagickColor<TQuantumType> backgroundColor)
        {
            if (Angle != 0)
                return Angle;

            var white = _factory.Colors.White;

            using (var mask = CreateMask(input, backgroundColor))
            {
                using (var pixels = mask.GetPixelsUnsafe())
                {
                    var p1x = 1;
                    var p1y = 1;
                    var p2x = 1;
                    var p2y = 1;

                    for (var y = 0; y < mask.Height; y++)
                    {
                        var pixel = pixels.GetPixel(0, y);
                        if (pixel.Equals(white))
                        {
                            p1y = y;
                            break;
                        }
                    }

                    for (var x = 0; x < mask.Width; x++)
                    {
                        var pixel = pixels.GetPixel(x, 0);
                        if (pixel.Equals(white))
                        {
                            p2x = x;
                            break;
                        }
                    }

                    var deltaX = p2x - p1x;
                    var deltaY = p1y - p2y;

                    if (deltaX == 0)
                        return 0;

                    var angle = (180 / Math.PI) * Math.Atan2(deltaY, deltaX);
                    if (angle > 45)
                        angle = (angle - 90.005) / 1;
                    else
                        angle = (angle + 0.005) / 1;

                    return angle;
                }
            }
        }

        private IMagickImage<TQuantumType> CreateMask(IMagickImage<TQuantumType> input, IMagickColor<TQuantumType> backgroundColor)
        {
            var mask = input.Clone();
            var transparentColor = _factory.Colors.Transparent;
            var maskColor = _factory.Colors.White;

            mask.ColorFuzz = ColorFuzz;
            mask.BorderColor = backgroundColor;
            mask.Border(1);
            mask.FloodFill(transparentColor, (int)Coords.X, (int)Coords.Y);
            mask.Shave(1);
            mask.InverseOpaque(transparentColor, maskColor);
            mask.BackgroundColor = _factory.Colors.Black;
            mask.Alpha(AlphaOption.Background);
            mask.Trim();

            return mask;
        }

        private IMagickColor<TQuantumType> GetBackgroundColor(IMagickImage<TQuantumType> image)
        {
            if (BackgroundColor != null)
                return BackgroundColor;

            using (var pixels = image.GetPixels())
            {
                return pixels.GetPixel((int)Coords.X, (int)Coords.Y).ToColor();
            }
        }
    }
}
