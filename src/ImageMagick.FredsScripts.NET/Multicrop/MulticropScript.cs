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
    /// Crop and unrotate multiple images from a scanned image.
    /// </summary>
    /// <typeparam name="TQuantumType">The quantum type.</typeparam>
    public sealed class MulticropScript<TQuantumType>
        where TQuantumType : struct, IConvertible
    {
        private readonly IMagickFactory<TQuantumType> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MulticropScript{TQuantumType}"/> class.
        /// </summary>
        /// <param name="factory">The magick factory.</param>
        public MulticropScript(IMagickFactory<TQuantumType> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            Reset();
        }

        /// <summary>
        /// Gets or sets the background color to use instead of <see cref="Coords"/>.
        /// Default is null.
        /// </summary>
        public IMagickColor<TQuantumType> BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the fuzz amount specified as a percent 0 to 100.
        /// </summary>
        /// Default is 10.
        public Percentage ColorFuzz { get; set; }

        /// <summary>
        /// Gets or sets the pixel coordinate to extract background color from.
        /// Default is (0,0).
        /// </summary>
        public PointD Coords { get; set; }

        /// <summary>
        /// Gets or sets which regions to discard that have a width or height smaller than this size.
        /// Default is 0.
        /// </summary>
        public int Discard { get; set; }

        /// <summary>
        /// Gets or sets the crop extend on each side in pixels.
        /// Default is 0.
        /// </summary>
        public int Extend { get; set; }

        /// <summary>
        /// Gets or sets the grid spacing in both x and y as percent of image width and height; used to locate images.
        /// Default is 10.
        /// </summary>
        public Percentage Grid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cropped area should be trimed to an orthogonal rectangle.
        /// Default is false.
        /// </summary>
        public bool Innertrim { get; set; }

        /// <summary>
        /// Gets or sets which noisy regions in the mask image should be pruned using a morphology disk shape of the specified size.
        /// Default is 0.
        /// </summary>
        public int Prune { get; set; }

        /// <summary>
        /// Gets or sets the unrotate method that should be used. The default is a Deskew of the image but this could also be done together with the Unrotate script.
        /// Default is Deskew.
        /// </summary>
        public Func<IMagickImage<TQuantumType>, IMagickColor<TQuantumType>, IMagickImage<TQuantumType>> UnrotateMethod { get; set; }

        /// <summary>
        /// Crop and unrotate multiple images from a scanned image.
        /// </summary>
        /// <param name="input">The image to execute the script on.</param>
        /// <returns>The resulting image.</returns>
        public IMagickImageCollection<TQuantumType> Execute(IMagickImage<TQuantumType> input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var backgroundColor = GetBackgroundColor(input);
            var maskColor = _factory.Colors.Red;
            var mask = CreateMask(input, backgroundColor, maskColor);

            var collection = _factory.ImageCollection.Create();

            var grid = Grid.ToDouble();
            var gridWidth = (int)Math.Round((grid / 100) * input.Width);
            var gridHeight = (int)Math.Round((grid / 100) * input.Height);
            using (var pixels = mask.GetPixelsUnsafe())
            {
                for (var y = 0; y < mask.Height; y += gridHeight)
                {
                    for (var x = 0; x < mask.Width; x += gridWidth)
                    {
                        if (pixels.GetPixel(x, y).Equals(maskColor))
                        {
                            mask.FloodFill(_factory.Colors.White, x, y);
                            var image = ExtractImage(input, mask, backgroundColor);

                            mask.FloodFill(_factory.Colors.Transparent, x, y);

                            collection.Add(image);
                        }
                    }
                }
            }

            return collection;
        }

        /// <summary>
        /// Resets the script to the default setttings.
        /// </summary>
        public void Reset()
        {
            BackgroundColor = null;
            ColorFuzz = (Percentage)10;
            Coords = new PointD(0);
            Discard = 0;
            Extend = 0;
            Grid = (Percentage)10;
            Innertrim = false;
            Prune = 0;
            UnrotateMethod = DeskewImage;
        }

        private IMagickImage<TQuantumType> CreateMask(IMagickImage<TQuantumType> input, IMagickColor<TQuantumType> backgroundColor, IMagickColor<TQuantumType> maskColor)
        {
            var mask = input.Clone();
            var transparentColor = _factory.Colors.Transparent;

            mask.ColorFuzz = ColorFuzz;
            mask.BorderColor = backgroundColor;
            mask.Border(1);
            mask.FloodFill(transparentColor, (int)Coords.X, (int)Coords.Y);
            mask.Shave(1);
            mask.InverseOpaque(transparentColor, maskColor);
            mask.BackgroundColor = _factory.Colors.Black;
            mask.Alpha(AlphaOption.Background);

            if (Prune != 0)
            {
                mask.Morphology(MorphologyMethod.Open, Kernel.Disk, (Prune / 2).ToString(), Channels.Composite);
            }

            return mask;
        }

        private IMagickImage<TQuantumType> ExtractImage(IMagickImage<TQuantumType> input, IMagickImage<TQuantumType> mask, IMagickColor<TQuantumType> backgroundColor)
        {
            var newMask = mask.Clone();
            newMask.InverseOpaque(_factory.Colors.White, _factory.Colors.Transparent);
            newMask.Trim();

            var x = newMask.Page.X - Extend;
            var y = newMask.Page.Y - Extend;
            var width = newMask.Width + (2 * Extend);
            var height = newMask.Height + (2 * Extend);

            var image = input.Clone();
            image.Crop(_factory.Geometry.Create(x, y, width, height));
            image.RePage();

            if (UnrotateMethod != null)
            {
                var unrotatedImage = UnrotateMethod(image, backgroundColor);
                if (!object.ReferenceEquals(image, unrotatedImage))
                {
                    image.Dispose();
                    image = unrotatedImage;
                }
            }

            return image;
        }

        private IMagickImage<TQuantumType> DeskewImage(IMagickImage<TQuantumType> image, IMagickColor<TQuantumType> backgroundColor)
        {
            image.BackgroundColor = backgroundColor;
            image.Deskew(new Percentage(40));
            image.ColorFuzz = ColorFuzz;
            image.Trim();
            image.RePage();

            return image;
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
