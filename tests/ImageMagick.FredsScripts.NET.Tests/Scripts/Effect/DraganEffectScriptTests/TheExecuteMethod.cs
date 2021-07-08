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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public partial class DraganEffectScriptTests
    {
        [TestClass]
        public class TheExecuteMethod : DraganEffectScriptTests
        {
            [TestMethod]
            public void ShouldThrowExceptionWhenInputIsNull()
            {
                var factory = new MagickFactory();
                var script = new DraganEffectScript<ushort>(factory);

                ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () => script.Execute(null));
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenBrightnessBelowZero()
            {
                AssertInvalidOperation("Invalid brightness specified, value must be zero or higher.", (DraganEffectScript<ushort> script) =>
                {
                    script.Brightness = -1.0;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenContrastBelowMinusTen()
            {
                AssertInvalidOperation("Invalid contrast specified, the range is -10 to 10.", (DraganEffectScript<ushort> script) =>
                {
                    script.Contrast = -11.0;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenContrastAboveTen()
            {
                AssertInvalidOperation("Invalid contrast specified, the range is -10 to 10.", (DraganEffectScript<ushort> script) =>
                {
                    script.Contrast = 11.0;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenDarknessIsZero()
            {
                AssertInvalidOperation("Invalid darkness specified, value must be 1 or higher.", (DraganEffectScript<ushort> script) =>
                {
                    script.Darkness = 0.0;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenSaturationBelowZero()
            {
                AssertInvalidOperation("Invalid saturation specified, value must be zero or higher.", (DraganEffectScript<ushort> script) =>
                {
                    script.Saturation = (Percentage)(-1);
                });
            }

            [TestMethod]
            public void ShouldExecute_b1_c0_d1_s150_r5_jpg()
            {
                AssertExecute("before1.gif", nameof(ShouldExecute_b1_c0_d1_s150_r5_jpg), (DraganEffectScript<ushort> script) =>
                {
                    script.Brightness = 1;
                    script.Contrast = 0;
                    script.Darkness = 1;
                    script.Saturation = (Percentage)150;
                });

                AssertExecute("bluehat.jpg", nameof(ShouldExecute_b1_c0_d1_s150_r5_jpg), (DraganEffectScript<ushort> script) =>
                {
                    script.Brightness = 1;
                    script.Contrast = 0;
                    script.Darkness = 1;
                    script.Saturation = (Percentage)150;
                });
            }

            [TestMethod]
            public void ShouldExecute_b1p5_cm5_d1_s175_r5_jpg()
            {
                AssertExecute("before1.gif", nameof(ShouldExecute_b1p5_cm5_d1_s175_r5_jpg), (DraganEffectScript<ushort> script) =>
                {
                    script.Brightness = 1.5;
                    script.Contrast = -5;
                    script.Darkness = 1;
                    script.Saturation = (Percentage)175;
                });
            }

            [TestMethod]
            public void ShouldExecute_b1p5_cm5_d2_s175_r5_jpg()
            {
                AssertExecute("before1.gif", nameof(ShouldExecute_b1p5_cm5_d2_s175_r5_jpg), (DraganEffectScript<ushort> script) =>
                {
                    script.Brightness = 1.5;
                    script.Contrast = -5;
                    script.Darkness = 2;
                    script.Saturation = (Percentage)175;
                });
            }

            [TestMethod]
            public void ShouldExecute_b1_cm7p5_d1_s200_r5_jpg()
            {
                AssertExecute("bluehat.jpg", nameof(ShouldExecute_b1_cm7p5_d1_s200_r5_jpg), (DraganEffectScript<ushort> script) =>
                {
                    script.Brightness = 1;
                    script.Contrast = -7.5;
                    script.Darkness = 1;
                    script.Saturation = (Percentage)200;
                });
            }

            [TestMethod]
            public void ShouldExecute_b1_cm7p5_d1p25_s200_r5_jpg()
            {
                AssertExecute("bluehat.jpg", nameof(ShouldExecute_b1_cm7p5_d1p25_s200_r5_jpg), (DraganEffectScript<ushort> script) =>
                {
                    script.Brightness = 1;
                    script.Contrast = -7.5;
                    script.Darkness = 1.25;
                    script.Saturation = (Percentage)200;
                });
            }

            [TestMethod]
            public void ShouldExecute_b1_cm5_d1_s150_r5_jpg()
            {
                AssertExecute("CHINA-715-4_small.jpg", nameof(ShouldExecute_b1_cm5_d1_s150_r5_jpg), (DraganEffectScript<ushort> script) =>
                {
                    script.Brightness = 1;
                    script.Contrast = -5;
                    script.Darkness = 1;
                    script.Saturation = (Percentage)150;
                });
            }

            [TestMethod]
            public void ShouldExecute_b1_cm10_d1_s200_r5_jpg()
            {
                AssertExecute("CHINA-715-4_small.jpg", nameof(ShouldExecute_b1_cm10_d1_s200_r5_jpg), (DraganEffectScript<ushort> script) =>
                {
                    script.Contrast = -10;
                    script.Darkness = 1;
                    script.Saturation = (Percentage)200;
                });
            }

            [TestMethod]
            public void ShouldExecute_b1_cm5_d1p75_s175_r5_jpg()
            {
                AssertExecute("mustache.jpg", nameof(ShouldExecute_b1_cm5_d1p75_s175_r5_jpg), (DraganEffectScript<ushort> script) =>
                {
                    script.Brightness = 1;
                    script.Contrast = -5;
                    script.Darkness = 1.75;
                    script.Saturation = (Percentage)175;
                });
            }

            [TestMethod]
            public void ShouldExecute_d3_s100_jpg()
            {
                AssertExecute("mustache.jpg", nameof(ShouldExecute_d3_s100_jpg), (DraganEffectScript<ushort> script) =>
                {
                    script.Darkness = 3;
                    script.Saturation = (Percentage)100;
                });
            }

            private static void AssertInvalidOperation(string expectedMessage, Action<DraganEffectScript<ushort>> initAction)
            {
                var factory = new MagickFactory();
                var script = new DraganEffectScript<ushort>(factory);

                using (var logo = new MagickImage(Images.Logo))
                {
                    initAction(script);

                    ExceptionAssert.Throws<InvalidOperationException>(expectedMessage, () =>
                    {
                        script.Execute(logo);
                    });
                }
            }

            private void AssertExecute(string input, string methodName, Action<DraganEffectScript<ushort>> action)
            {
                string inputFile = GetInputFile(input);
                /* LosslessCompress(inputFile); */

                using (var image = new MagickImage(inputFile))
                {
                    var factory = new MagickFactory();
                    var script = new DraganEffectScript<ushort>(factory);
                    action(script);

                    using (var scriptOutput = script.Execute(image))
                    {
                        string outputFile = GetOutputFile(input, methodName);
                        AssertOutput(scriptOutput, outputFile);
                    }
                }
            }
        }
    }
}
