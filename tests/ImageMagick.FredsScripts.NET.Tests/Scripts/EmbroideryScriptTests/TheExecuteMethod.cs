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
using Xunit;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public partial class EmbroideryScriptTests
    {
        public class TheExecuteMethod : EmbroideryScriptTests
        {
            [Fact]
            public void ShouldThrowExceptionWhenInputIsNull()
            {
                var factory = new MagickFactory();
                var script = new EmbroideryScript<ushort>(factory);

                Assert.Throws<ArgumentNullException>("input", () => script.Execute(null));
            }

            [Fact]
            public void ShouldThrowExceptionWhenAngleBelowMinus360()
            {
                AssertInvalidOperation("Invalid angle specified, value must be between -360 and 360.", (EmbroideryScript<ushort> script) =>
                {
                    script.Angle = -361;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenAngleAbove360()
            {
                AssertInvalidOperation("Invalid angle specified, value must be between -360 and 360.", (EmbroideryScript<ushort> script) =>
                {
                    script.Angle = 361;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenAzimuthBelowMinus360()
            {
                AssertInvalidOperation("Invalid azimuth specified, value must be between -360 and 360.", (EmbroideryScript<ushort> script) =>
                {
                    script.Azimuth = -361;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenAzimuthAbove360()
            {
                AssertInvalidOperation("Invalid azimuth specified, value must be between -360 and 360.", (EmbroideryScript<ushort> script) =>
                {
                    script.Azimuth = 361;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenColorFuzzBelowZero()
            {
                AssertInvalidOperation("Invalid color fuzz specified, value must be between 0 and 100.", (EmbroideryScript<ushort> script) =>
                {
                    script.ColorFuzz = new Percentage(-1);
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenColorFuzzAbove100()
            {
                AssertInvalidOperation("Invalid color fuzz specified, value must be between 0 and 100.", (EmbroideryScript<ushort> script) =>
                {
                    script.ColorFuzz = new Percentage(101);
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenContrastBelowZero()
            {
                AssertInvalidOperation("Invalid contrast specified, value must be zero or higher.", (EmbroideryScript<ushort> script) =>
                {
                    script.Contrast = -0.99;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenElevationBelowZero()
            {
                AssertInvalidOperation("Invalid elevation specified, value must be between 0 and 90.", (EmbroideryScript<ushort> script) =>
                {
                    script.Elevation = -0.99;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenElevationAbove90()
            {
                AssertInvalidOperation("Invalid elevation specified, value must be between 0 and 90.", (EmbroideryScript<ushort> script) =>
                {
                    script.Elevation = 90.01;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenExtentBelowZero()
            {
                AssertInvalidOperation("Invalid extent specified, value must be zero or higher.", (EmbroideryScript<ushort> script) =>
                {
                    script.Extent = -1;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenGrayLimitBelowZero()
            {
                AssertInvalidOperation("Invalid gray limit specified, value must be between 0 and 100.", (EmbroideryScript<ushort> script) =>
                {
                    script.GrayLimit = -1;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenGrayLimitAbove100()
            {
                AssertInvalidOperation("Invalid gray limit specified, value must be between 0 and 100.", (EmbroideryScript<ushort> script) =>
                {
                    script.GrayLimit = 101;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenIntensityBelowZero()
            {
                AssertInvalidOperation("Invalid intensity specified, value must be between 0 and 100.", (EmbroideryScript<ushort> script) =>
                {
                    script.Intensity = new Percentage(-1);
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenIntensityAbove100()
            {
                AssertInvalidOperation("Invalid intensity specified, value must be between 0 and 100.", (EmbroideryScript<ushort> script) =>
                {
                    script.Intensity = new Percentage(101);
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenMixBelowZero()
            {
                AssertInvalidOperation("Invalid mix specified, value must be between 0 and 100.", (EmbroideryScript<ushort> script) =>
                {
                    script.Mix = -1;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenMixAbove100()
            {
                AssertInvalidOperation("Invalid mix specified, value must be between 0 and 100.", (EmbroideryScript<ushort> script) =>
                {
                    script.Mix = 101;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenumberOfColorsZero()
            {
                AssertInvalidOperation("Invalid number of colors specified, value must be higher than zero.", (EmbroideryScript<ushort> script) =>
                {
                    script.NumberOfColors = 0;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenPatternInvalidValue()
            {
                AssertInvalidOperation("Invalid pattern specified.", (EmbroideryScript<ushort> script) =>
                {
                    script.Pattern = (EmbroideryPattern)42;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenRangeBelowZero()
            {
                AssertInvalidOperation("Invalid range specified, value must be between 0 and 360.", (EmbroideryScript<ushort> script) =>
                {
                    script.Range = -1;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenRangeAbove360()
            {
                AssertInvalidOperation("Invalid range specified, value must be between 0 and 360.", (EmbroideryScript<ushort> script) =>
                {
                    script.Range = 361;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenSpreadBelowZero()
            {
                AssertInvalidOperation("Invalid spread specified, value must be zero or higher.", (EmbroideryScript<ushort> script) =>
                {
                    script.Spread = -0.99;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenThicknessZero()
            {
                AssertInvalidOperation("Invalid thickness specified, value must be higher than zero.", (EmbroideryScript<ushort> script) =>
                {
                    script.Thickness = 0;
                });
            }

            [Fact]
            public void ShouldExecute_default_jpg()
            {
                AssertExecute("cnbc.jpg", nameof(ShouldExecute_default_jpg), (EmbroideryScript<ushort> script) =>
                {
                });
            }

            [Fact]
            public void ShouldExecute_f0_g0_jpg()
            {
                AssertExecute("cnbc.jpg", nameof(ShouldExecute_f0_g0_jpg), (EmbroideryScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)0;
                    script.GrayLimit = 0;
                });
            }

            [Fact]
            public void ShouldExecute_i0_jpg()
            {
                AssertExecute("cnbc.jpg", nameof(ShouldExecute_i0_jpg), (EmbroideryScript<ushort> script) =>
                {
                    script.Intensity = (Percentage)0;
                });
            }

            [Fact]
            public void ShouldExecute_i0_C10_jpg()
            {
                AssertExecute("cnbc.jpg", nameof(ShouldExecute_i0_C10_jpg), (EmbroideryScript<ushort> script) =>
                {
                    script.Intensity = (Percentage)0;
                    script.Contrast = 10;
                });
            }

            [Fact]
            public void ShouldExecute_i50_jpg()
            {
                AssertExecute("cnbc.jpg", nameof(ShouldExecute_i50_jpg), (EmbroideryScript<ushort> script) =>
                {
                    script.Intensity = (Percentage)50;
                });
            }

            [Fact]
            public void ShouldExecute_p2_jpg()
            {
                AssertExecute("cnbc.jpg", nameof(ShouldExecute_p2_jpg), (EmbroideryScript<ushort> script) =>
                {
                    script.Pattern = EmbroideryPattern.Crosshatch;
                });
            }

            [Fact]
            public void ShouldExecute_p2_t3_jpg()
            {
                AssertExecute("cnbc.jpg", nameof(ShouldExecute_p2_t3_jpg), (EmbroideryScript<ushort> script) =>
                {
                    script.Pattern = EmbroideryPattern.Crosshatch;
                    script.Thickness = 3;
                });
            }

            [Fact]
            public void ShouldExecute_p2_t5_jpg()
            {
                AssertExecute("cnbc.jpg", nameof(ShouldExecute_p2_t5_jpg), (EmbroideryScript<ushort> script) =>
                {
                    script.Pattern = EmbroideryPattern.Crosshatch;
                    script.Thickness = 5;
                });
            }

            [Fact]
            public void ShouldExecute_s100_jpg()
            {
                AssertExecute("cnbc.jpg", nameof(ShouldExecute_s100_jpg), (EmbroideryScript<ushort> script) =>
                {
                    script.Spread = 100;
                });
            }

            [Fact]
            public void ShouldExecute_s0_t3_jpg()
            {
                AssertExecute("cnbc.jpg", nameof(ShouldExecute_s0_t3_jpg), (EmbroideryScript<ushort> script) =>
                {
                    script.Spread = 0;
                    script.Thickness = 3;
                });
            }

            private static void AssertInvalidOperation(string expectedMessage, Action<EmbroideryScript<ushort>> initAction)
            {
                var factory = new MagickFactory();
                var script = new EmbroideryScript<ushort>(factory);

                using (var logo = new MagickImage(Images.Logo))
                {
                    initAction(script);

                    var exception = Assert.Throws<InvalidOperationException>(() =>
                    {
                        script.Execute(logo);
                    });

                    Assert.Contains(expectedMessage, exception.Message);
                }
            }

            private void AssertExecute(string input, string methodName, Action<EmbroideryScript<ushort>> action)
            {
                var inputFile = GetInputFile(input);

                using (var image = new MagickImage(inputFile))
                {
                    var factory = new MagickFactory();
                    var script = new EmbroideryScript<ushort>(factory);
                    action(script);

                    using (var scriptOutput = script.Execute(image))
                    {
                        var outputFile = GetOutputFile(input, methodName);
                        AssertOutput(scriptOutput, outputFile);
                    }
                }
            }
        }
    }
}
