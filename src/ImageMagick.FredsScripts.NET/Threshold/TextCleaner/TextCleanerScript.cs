﻿// Copyright Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/ImageMagick.FredsScripts.NET)
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
    /// Processses a scanned document of text to clean the text background and enhance the text.
    /// </summary>
    /// <typeparam name="TQuantumType">The quantum type.</typeparam>
    public sealed class TextCleanerScript<TQuantumType>
        where TQuantumType : struct
    {
        private readonly IMagickFactory<TQuantumType> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextCleanerScript{TQuantumType}"/> class.
        /// </summary>
        /// <param name="factory">The magick factory.</param>
        public TextCleanerScript(IMagickFactory<TQuantumType> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            Reset();
        }

        /// <summary>
        ///  Gets or sets the value to apply for an alternate text smoothing using and adaptive blur.
        ///  Valid values are zero or higher. The default value is zero.
        /// </summary>
        public double AdaptiveBlur { get; set; }

        /// <summary>
        /// Gets or sets the desired background color after it has been cleaned up. The default is white.
        /// </summary>
        public IMagickColor<TQuantumType> BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the image cropping offsets.
        /// </summary>
        public TextCleanerCropOffset CropOffset { get; set; }

        /// <summary>
        /// Gets or sets the enhance image brightness bearing cleaning, the default value is Stretch.
        /// </summary>
        public TextCleanerEnhance Enhance { get; set; }

        /// <summary>
        /// Gets or sets the offset threshold in percent used by the filter to eliminate noise. Valid
        /// values are zero or higher. Values too small will leave much noise and artifacts in the result.
        /// Values too large will remove too much text leaving gaps. The default value is 5.
        /// </summary>
        public Percentage FilterOffset { get; set; }

        /// <summary>
        /// Gets or sets the size of the filter used to clean up the background. Valid values are zero or
        /// higher. The filtersize needs to be larger than the thickness of the writing, but the smaller the
        /// better beyond this. Making it larger will increase the processing time and may lose text.
        /// The default value is 15.
        /// </summary>
        public int FilterSize { get; set; }

        /// <summary>
        /// Gets or sets desired layout, the default is Portrait.
        /// </summary>
        public TextCleanerLayout Layout { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to convert the document to grayscale before enhancing.
        /// </summary>
        public bool MakeGray { get; set; }

        /// <summary>
        /// Gets or sets the border pad amount around outer part of image. Valid values are zero or higher.
        /// The default value is 0.
        /// </summary>
        public int Padding { get; set; }

        /// <summary>
        /// Gets or sets the image rotation. Rotate image 90 degrees in direction specified if spect ratio does
        /// not match layout. The default value is no rotation.
        /// </summary>
        public TextCleanerRotation Rotation { get; set; }

        /// <summary>
        /// Gets or sets the color saturation. Only applicable if MakeGray is false. The default value is 200.
        /// </summary>
        public Percentage Saturation { get; set; }

        /// <summary>
        /// Gets or sets the amount of pixel sharpening to be applied to the resulting text. Valid values are zero
        /// or higher. If used, it should be small (suggested about 1). The default value is zero.
        /// </summary>
        public double Sharpen { get; set; }

        /// <summary>
        /// Gets or sets the text smoothing threshold. Valid values are between 0 and 100. Smaller values
        /// smooth/thicken the text more. Larger values thin, but can result in gaps in the text. Nominal
        /// value is in the middle at about 50. The default value is to disable smoothing.
        /// </summary>
        public Percentage? SmoothingThreshold { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the border around the image should be trimmed.
        /// Effective only if background well-cleaned.
        /// </summary>
        public bool Trim { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the image will be unrotated, the default value is false.
        /// </summary>
        public bool Unrotate { get; set; }

        /// <summary>
        /// Processses a scanned document of text to clean the text background and enhance the text.
        /// </summary>
        /// <param name="input">The image to execute the script on.</param>
        /// <returns>The resulting image.</returns>
        public IMagickImage<TQuantumType> Execute(IMagickImage<TQuantumType> input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            CheckSettings();

            var output = input.Clone();
            RotateImage(output);
            CropImage(output);
            ConvertToGrayscale(output);
            EnhanceImage(output);
            RemoveNoise(output);
            UnrotateImage(output);
            SharpenImage(output);
            SaturateImage(output);
            AdaptiveBlurImage(output);
            TrimImage(output);
            PadImage(output);

            return output;
        }

        /// <summary>
        /// Resets the script to the default setttings.
        /// </summary>
        public void Reset()
        {
            AdaptiveBlur = 0.0;
            BackgroundColor = _factory.Color.Create("white");
            CropOffset = new TextCleanerCropOffset();
            Enhance = TextCleanerEnhance.Stretch;
            FilterOffset = (Percentage)5;
            FilterSize = 15;
            Layout = TextCleanerLayout.Portrait;
            MakeGray = false;
            Padding = 0;
            Rotation = TextCleanerRotation.None;
            Saturation = (Percentage)200;
            Sharpen = 0.0;
            SmoothingThreshold = null;
            Trim = false;
            Unrotate = false;
        }

        private void AdaptiveBlurImage(IMagickImage<TQuantumType> image)
        {
            if (AdaptiveBlur == 0.0)
                return;

            image.AdaptiveBlur(AdaptiveBlur, 1.0);
        }

        private void CheckSettings()
        {
            if (AdaptiveBlur < 0.0)
                throw new InvalidOperationException("Invalid adaptive blur specified, value must be zero or higher.");

            if (!CropOffset.IsValid)
                throw new InvalidOperationException("Invalid crop offset specified, values must be zero or higher.");

            if (FilterSize < 0)
                throw new InvalidOperationException("Invalid filter size specified, value must be zero or higher.");

            if (Padding < 0)
                throw new InvalidOperationException("Invalid padding specified, value must be zero or higher.");

            if (Sharpen < 0.0)
                throw new InvalidOperationException("Invalid sharpen specified, value must be zero or higher.");

            if (Saturation.ToDouble() < 0.0)
                throw new InvalidOperationException("Invalid saturation specified, value must be zero or higher.");

            if (SmoothingThreshold != null)
            {
                var textSmoothingThreshold = SmoothingThreshold.Value.ToDouble();
                if (textSmoothingThreshold < 0 || textSmoothingThreshold > 100)
                    throw new InvalidOperationException("Invalid smoothing threshold specified, value must be between zero and 100.");
            }
        }

        private void ConvertToGrayscale(IMagickImage<TQuantumType> image)
        {
            if (!MakeGray)
                return;

            image.ColorSpace = ColorSpace.Gray;
            image.ColorType = ColorType.Grayscale;
        }

        private void CropImage(IMagickImage<TQuantumType> image)
        {
            if (!CropOffset.IsSet)
                return;

            var width = image.Width - (CropOffset.Left + CropOffset.Right);
            var height = image.Height - (CropOffset.Top + CropOffset.Bottom);

            image.Crop(_factory.Geometry.Create(CropOffset.Left, CropOffset.Top, width, height));
        }

        private void EnhanceImage(IMagickImage<TQuantumType> image)
        {
            if (Enhance == TextCleanerEnhance.Stretch)
                image.ContrastStretch((Percentage)0);
            else if (Enhance == TextCleanerEnhance.Normalize)
                image.Normalize();
        }

        private void PadImage(IMagickImage<TQuantumType> image)
        {
            if (Padding == 0)
                return;

            image.Compose = CompositeOperator.Over;
            image.BorderColor = BackgroundColor;
            image.Border(Padding);
        }

        private void RemoveNoise(IMagickImage<TQuantumType> image)
        {
            using (var second = image.Clone())
            {
                second.ColorSpace = ColorSpace.Gray;
                second.Negate();
                second.AdaptiveThreshold(FilterSize, FilterSize, FilterOffset);
                second.ContrastStretch((Percentage)0);

                if (SmoothingThreshold != null)
                {
                    var max = ((IConvertible)_factory.QuantumInfo.Max).ToDouble(null);
                    second.Blur(SmoothingThreshold.Value.ToDouble() / 100, max);
                    second.Level(SmoothingThreshold.Value, new Percentage(100));
                }

                image.Composite(second, CompositeOperator.CopyAlpha);
            }

            image.Opaque(_factory.Color.Create("transparent"), BackgroundColor);
            image.Alpha(AlphaOption.Off);
        }

        private void RotateImage(IMagickImage<TQuantumType> image)
        {
            if (Rotation == TextCleanerRotation.None)
                return;
            if ((Layout == TextCleanerLayout.Portrait && image.Height < image.Width) ||
              (Layout == TextCleanerLayout.Landscape && image.Height > image.Width))
            {
                if (Rotation == TextCleanerRotation.Counterclockwise)
                    image.Rotate(90);
                else
                    image.Rotate(-90);
            }
        }

        private void SaturateImage(IMagickImage<TQuantumType> image)
        {
            if (Saturation == (Percentage)100)
                return;

            image.Modulate((Percentage)100, Saturation, (Percentage)100);
        }

        private void SharpenImage(IMagickImage<TQuantumType> image)
        {
            if (Sharpen == 0.0)
                return;

            image.Sharpen(0.0, Sharpen);
        }

        private void TrimImage(IMagickImage<TQuantumType> result)
        {
            if (!Trim)
                return;

            result.Trim();
            result.RePage();
        }

        private void UnrotateImage(IMagickImage<TQuantumType> image)
        {
            if (!Unrotate)
                return;

            image.BackgroundColor = BackgroundColor;
            image.Deskew((Percentage)40);
        }
    }
}
