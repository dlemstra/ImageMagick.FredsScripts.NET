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
    /// Applies a Dragan-like effect to an image to enhance wrinkles creating a "gritty" effect.
    /// </summary>
    /// <typeparam name="TQuantumType">The quantum type.</typeparam>
    public sealed class DraganEffectScript<TQuantumType>
        where TQuantumType : struct
    {
        private readonly IMagickFactory<TQuantumType> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DraganEffectScript{TQuantumType}"/> class.
        /// </summary>
        /// <param name="factory">The magick factory.</param>
        public DraganEffectScript(IMagickFactory<TQuantumType> factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            Reset();
        }

        /// <summary>
        /// Gets or sets the brightness factor. Valid values are zero or higher. The default is 1.
        /// Increase brightness is larger than 1, decrease brightness is less than 1.
        /// </summary>
        public double Brightness { get; set; }

        /// <summary>
        /// Gets or sets the sigmoidal contrast. Valid values are nominally in the range of -10 to 10.
        /// Positive values increase contrast and negative values decrease contrast. The default is 0.
        /// </summary>
        public double Contrast { get; set; }

        /// <summary>
        /// Gets or sets the shadow darkening factor. Valid values are 1 or higher. The default is 1.
        /// Darker shadows is larger than 1.
        /// </summary>
        public double Darkness { get; set; }

        /// <summary>
        /// Gets or sets saturation. Valid values are zero or higher. A value of 100 is no change.
        /// The default is 150.
        /// </summary>
        public Percentage Saturation { get; set; }

        /// <summary>
        /// Applies a Dragan-like effect to an image to enhance wrinkles creating a "gritty" effect.
        /// </summary>
        /// <param name="input">The image to execute the script on.</param>
        /// <returns>The resulting image.</returns>
        public IMagickImage<TQuantumType> Execute(IMagickImage<TQuantumType> input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            CheckSettings();

            using (var first = input.Clone())
            {
                ApplyBrightness(first);
                ApplyContrast(first);
                ApplySaturation(first);

                using (var second = first.Clone())
                {
                    second.Modulate((Percentage)100, (Percentage)0, (Percentage)100);
                    double darkness = 3 / Darkness;
                    if (darkness != 1)
                    {
                        second.Evaluate(Channels.All, EvaluateOperator.Multiply, darkness);
                        second.Clamp();
                    }

                    first.Composite(second, CompositeOperator.Multiply);
                    first.Clamp();
                }

                var output = first.Clone();

                using (var third = first.Clone())
                {
                    third.SetArtifact("convolve:bias", "50%");
                    third.SetArtifact("convolve:scale", "1");
                    third.Morphology(MorphologyMethod.Convolve, Kernel.DoG, "0,0,5");
                    third.Clamp();

                    output.Composite(third, CompositeOperator.Overlay);

                    using (var fourth = first.Clone())
                    {
                        fourth.Modulate((Percentage)100, (Percentage)0, (Percentage)100);

                        output.Composite(fourth, CompositeOperator.HardLight);
                        return output;
                    }
                }
            }
        }

        /// <summary>
        /// Resets the script to the default setttings.
        /// </summary>
        public void Reset()
        {
            Brightness = 1.0;
            Contrast = 0;
            Darkness = 1;
            Saturation = (Percentage)150;
        }

        private void ApplyBrightness(IMagickImage<TQuantumType> image)
        {
            if (Brightness == 1.0)
                return;

            image.Evaluate(Channels.All, EvaluateOperator.Multiply, Brightness);
        }

        private void ApplyContrast(IMagickImage<TQuantumType> image)
        {
            if (Contrast == 0.0)
                return;

            var contrast = Math.Abs(Contrast);
            var midpoint = ((IConvertible)_factory.QuantumInfo.Max).ToDouble(null) / 2;

            image.SigmoidalContrast(Contrast >= 0.0, contrast, midpoint);
        }

        private void ApplySaturation(IMagickImage<TQuantumType> result)
        {
            if (Saturation == (Percentage)100)
                return;

            result.Modulate((Percentage)100, Saturation, (Percentage)100);
        }

        private void CheckSettings()
        {
            if (Brightness < 0.0)
                throw new InvalidOperationException("Invalid brightness specified, value must be zero or higher.");

            if (Contrast < -10.0 || Contrast > 10.0)
                throw new InvalidOperationException("Invalid contrast specified, the range is -10 to 10.");

            if (Darkness < 1.0)
                throw new InvalidOperationException("Invalid darkness specified, value must be 1 or higher.");

            if (Saturation.ToDouble() < 0.0)
                throw new InvalidOperationException("Invalid saturation specified, value must be zero or higher.");
        }
    }
}
