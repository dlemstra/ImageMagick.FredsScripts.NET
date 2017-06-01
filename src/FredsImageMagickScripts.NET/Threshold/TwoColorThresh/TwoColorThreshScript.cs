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
using ImageMagick;

namespace FredsImageMagickScripts
{
    /// <summary>
    /// Automatically thresholds an image to binary (b/w) format using an adaptive spatial subdivision
    /// color reduction technique. This is the -colors IM operator as implemented with slight
    /// modification from Anthony's Examples at http://www.imagemagick.org/Usage/quantize/#two_color.
    /// For algorithm details, see http://www.imagemagick.org/script/quantize.php
    /// </summary>
    public static class TwoColorThreshScript
    {
        /// <summary>
        /// Automatically thresholds an image to binary (b/w) format using an adaptive spatial subdivision
        /// color reduction technique. This is the -colors IM operator as implemented with slight
        /// modification from Anthony's Examples at http://www.imagemagick.org/Usage/quantize/#two_color.
        /// For algorithm details, see http://www.imagemagick.org/script/quantize.php
        /// </summary>
        /// <param name="input">The image to execute the script on.</param>
        /// <returns>The resulting image.</returns>
        public static MagickImage Execute(MagickImage input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            var result = input.Clone();

            var settings = new QuantizeSettings()
            {
                Colors = 2,
                DitherMethod = DitherMethod.No
            };

            result.Quantize(settings);
            result.ColorSpace = ColorSpace.Gray;
            result.ContrastStretch((Percentage)0);

            return result;
        }
    }
}
