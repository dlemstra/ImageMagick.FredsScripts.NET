// Copyright Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/FredsImageMagickScripts.NET)
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests
{
    public partial class TextCleanerScriptTests
    {
        [TestClass]
        public class TheExecuteMethod : TextCleanerScriptTests
        {
            [TestMethod]
            public void ShouldThrowExceptionWhenInputIsNull()
            {
                var factory = new MagickFactory();
                var script = new TextCleanerScript<ushort>(factory);

                var overlay = new MagickImage();
                ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () => script.Execute(null));
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenAdaptiveBlurBelowZero()
            {
                AssertInvalidOperation("Invalid adaptive blur specified, value must be zero or higher.", (TextCleanerScript<ushort> script) =>
                {
                    script.AdaptiveBlur = -1;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenCropOffsetBottomBelowZero()
            {
                AssertInvalidOperation("Invalid crop offset specified, values must be zero or higher.", (TextCleanerScript<ushort> script) =>
                {
                    script.CropOffset.Bottom = -1;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenCropOffsetLeftBelowZero()
            {
                AssertInvalidOperation("Invalid crop offset specified, values must be zero or higher.", (TextCleanerScript<ushort> script) =>
                {
                    script.CropOffset.Left = -1;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenCropOffsetRightBelowZero()
            {
                AssertInvalidOperation("Invalid crop offset specified, values must be zero or higher.", (TextCleanerScript<ushort> script) =>
                {
                    script.CropOffset.Right = -1;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenCropOffsetTopBelowZero()
            {
                AssertInvalidOperation("Invalid crop offset specified, values must be zero or higher.", (TextCleanerScript<ushort> script) =>
                {
                    script.CropOffset.Top = -1;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenFilterSizeBelowZero()
            {
                AssertInvalidOperation("Invalid filter size specified, value must be zero or higher.", (TextCleanerScript<ushort> script) =>
                {
                    script.FilterSize = -1;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenPaddingBelowZero()
            {
                AssertInvalidOperation("Invalid padding specified, value must be zero or higher.", (TextCleanerScript<ushort> script) =>
                {
                    script.Padding = -1;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenSharpenBelowZero()
            {
                AssertInvalidOperation("Invalid sharpen specified, value must be zero or higher.", (TextCleanerScript<ushort> script) =>
                {
                    script.Sharpen = -1;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenSaturationBelowZero()
            {
                AssertInvalidOperation("Invalid saturation specified, value must be zero or higher.", (TextCleanerScript<ushort> script) =>
                {
                    script.Saturation = (Percentage)(-1);
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenSmoothingThresholdAbove100()
            {
                AssertInvalidOperation("Invalid smoothing threshold specified, value must be between zero and 100.", (TextCleanerScript<ushort> script) =>
                {
                    script.SmoothingThreshold = (Percentage)101;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenSmoothingThresholdMinusOneOrLower()
            {
                AssertInvalidOperation("Invalid smoothing threshold specified, value must be between zero and 100.", (TextCleanerScript<ushort> script) =>
                {
                    script.SmoothingThreshold = (Percentage)(-1);
                });
            }

            [TestMethod]
            public void ShouldExecute_g_none_f15_o20_jpg()
            {
                AssertExecute("abbott2.jpg", nameof(ShouldExecute_g_none_f15_o20_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.None;
                    script.FilterSize = 15;
                    script.FilterOffset = (Percentage)20;
                });
            }

            [TestMethod]
            public void ShouldExecute_g_stretch_f15_o20_jpg()
            {
                AssertExecute("abbott2.jpg", nameof(ShouldExecute_g_stretch_f15_o20_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Stretch;
                    script.FilterSize = 15;
                    script.FilterOffset = (Percentage)20;
                });
            }

            [TestMethod]
            public void ShouldExecute_g_stretch_f25_o20_jpg()
            {
                AssertExecute("abbott2.jpg", nameof(ShouldExecute_g_stretch_f25_o20_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Stretch;
                    script.FilterSize = 25;
                    script.FilterOffset = (Percentage)20;
                });
            }

            [TestMethod]
            public void ShouldExecute_g_stretch_f25_o20_s1_jpg()
            {
                AssertExecute("abbott2.jpg", nameof(ShouldExecute_g_stretch_f25_o20_s1_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Stretch;
                    script.FilterSize = 25;
                    script.FilterOffset = (Percentage)20;
                    script.Sharpen = 1;
                });
            }

            [TestMethod]
            public void ShouldExecute_g_stretch_f25_o20_t30_s1_u_T_p20_jpg()
            {
                AssertExecute("abbott2.jpg", nameof(ShouldExecute_g_stretch_f25_o20_t30_s1_u_T_p20_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Stretch;
                    script.FilterSize = 25;
                    script.FilterOffset = (Percentage)20;
                    script.Unrotate = true;
                    script.Sharpen = 1;
                    script.Trim = true;
                    script.Padding = 20;
                });
            }

            [TestMethod]
            public void ShouldExecute_c_0x50x0x0_g_normalize_f15_o10_s2_u_T_p20_jpg()
            {
                AssertExecute("brscan_original_r90.jpg", nameof(ShouldExecute_c_0x50x0x0_g_normalize_f15_o10_s2_u_T_p20_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.CropOffset.Top = 50;
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Normalize;
                    script.FilterSize = 15;
                    script.FilterOffset = (Percentage)10;
                    script.Unrotate = true;
                    script.Sharpen = 2;
                    script.Trim = true;
                    script.Padding = 20;
                });
            }

            [TestMethod]
            public void ShouldExecute_g_none_f15_o10_jpg()
            {
                AssertExecute("brscan_original_r90.jpg", nameof(ShouldExecute_g_none_f15_o10_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.None;
                    script.FilterSize = 15;
                    script.FilterOffset = (Percentage)10;
                });
            }

            [TestMethod]
            public void ShouldExecute_g_normalize_f15_o10_jpg()
            {
                AssertExecute("brscan_original_r90.jpg", nameof(ShouldExecute_g_normalize_f15_o10_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Normalize;
                    script.FilterSize = 15;
                    script.FilterOffset = (Percentage)10;
                });
            }

            [TestMethod]
            public void ShouldExecute_g_normalize_f15_o10_s1_jpg()
            {
                AssertExecute("brscan_original_r90.jpg", nameof(ShouldExecute_g_normalize_f15_o10_s1_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Normalize;
                    script.FilterSize = 15;
                    script.FilterOffset = (Percentage)10;
                    script.Sharpen = 1;
                });
            }

            [TestMethod]
            public void ShouldExecute_norm_f15_o5_S200_jpg()
            {
                AssertExecute("congress.jpg", nameof(ShouldExecute_norm_f15_o5_S200_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.Enhance = TextCleanerEnhance.Normalize;
                    script.FilterSize = 15;
                    script.FilterOffset = (Percentage)5;
                    script.Saturation = (Percentage)200;
                });
            }

            [TestMethod]
            public void ShouldExecute_norm_f15_o5_S200_s1_jpg()
            {
                AssertExecute("congress.jpg", nameof(ShouldExecute_norm_f15_o5_S200_s1_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.Enhance = TextCleanerEnhance.Normalize;
                    script.FilterSize = 15;
                    script.FilterOffset = (Percentage)5;
                    script.Saturation = (Percentage)200;
                    script.Sharpen = 1;
                });
            }

            [TestMethod]
            public void ShouldExecute_norm_f15_o5_S400_jpg()
            {
                AssertExecute("congress.jpg", nameof(ShouldExecute_norm_f15_o5_S400_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.Enhance = TextCleanerEnhance.Normalize;
                    script.FilterSize = 15;
                    script.FilterOffset = (Percentage)5;
                    script.Saturation = (Percentage)400;
                    script.Sharpen = 1;
                });
            }

            [TestMethod]
            public void ShouldExecute_g_stretch_f25_o10_u_s1_T_p10_jpg()
            {
                AssertExecute("crankshaft.jpg", nameof(ShouldExecute_g_stretch_f25_o10_u_s1_T_p10_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Stretch;
                    script.FilterSize = 25;
                    script.FilterOffset = (Percentage)10;
                    script.Unrotate = true;
                    script.Sharpen = 1;
                    script.Trim = true;
                    script.Padding = 10;
                    script.Layout = TextCleanerLayout.Landscape;
                });
            }

            [TestMethod]
            public void ShouldExecute_g_stretch_f25_o5_s1_jpg()
            {
                AssertExecute("railways.jpg", nameof(ShouldExecute_g_stretch_f25_o5_s1_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Stretch;
                    script.FilterSize = 25;
                    script.FilterOffset = (Percentage)5;
                    script.Sharpen = 1;
                    script.Layout = TextCleanerLayout.Landscape;
                });

                AssertExecute("rfid.jpg", nameof(ShouldExecute_g_stretch_f25_o5_s1_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Stretch;
                    script.FilterSize = 25;
                    script.FilterOffset = (Percentage)5;
                    script.Sharpen = 1;
                    script.Layout = TextCleanerLayout.Landscape;
                });
            }

            [TestMethod]
            public void ShouldExecute_a2_S100_st50_rc_jpg()
            {
                AssertExecute("rfid.jpg", nameof(ShouldExecute_a2_S100_st50_rc_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.AdaptiveBlur = 2;
                    script.Rotation = TextCleanerRotation.Clockwise;
                    script.Saturation = new Percentage(100);
                    script.SmoothingThreshold = new Percentage(50);
                });
            }

            [TestMethod]
            public void ShouldExecute_g_f25_o5_s1_rcc_jpg()
            {
                AssertExecute("rfid.jpg", nameof(ShouldExecute_g_f25_o5_s1_rcc_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Stretch;
                    script.FilterSize = 25;
                    script.FilterOffset = (Percentage)5;
                    script.Rotation = TextCleanerRotation.Counterclockwise;
                    script.Sharpen = 1;
                });
            }

            [TestMethod]
            public void ShouldExecute_g_stretch_f15_o5_s1_jpg()
            {
                AssertExecute("telegram.jpg", nameof(ShouldExecute_g_stretch_f15_o5_s1_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Stretch;
                    script.FilterSize = 15;
                    script.FilterOffset = (Percentage)5;
                    script.Sharpen = 1;
                });
            }

            [TestMethod]
            public void ShouldExecute_g_stretch_f25_o10_s1_jpg()
            {
                AssertExecute("twinkle.jpg", nameof(ShouldExecute_g_stretch_f25_o10_s1_jpg), (TextCleanerScript<ushort> script) =>
                {
                    script.MakeGray = true;
                    script.Enhance = TextCleanerEnhance.Stretch;
                    script.FilterSize = 25;
                    script.FilterOffset = (Percentage)10;
                    script.Sharpen = 1;
                    script.Layout = TextCleanerLayout.Landscape;
                });
            }

            private static void AssertInvalidOperation(string expectedMessage, Action<TextCleanerScript<ushort>> initAction)
            {
                var factory = new MagickFactory();
                var script = new TextCleanerScript<ushort>(factory);

                using (var logo = new MagickImage(Images.Logo))
                {
                    initAction(script);

                    ExceptionAssert.Throws<InvalidOperationException>(expectedMessage, () =>
                    {
                        script.Execute(logo);
                    });
                }
            }

            private void AssertExecute(string input, string methodName, Action<TextCleanerScript<ushort>> action)
            {
                string inputFile = GetInputFile(input);
                /* LosslessCompress(inputFile); */

                using (var image = new MagickImage(inputFile))
                {
                    var factory = new MagickFactory();
                    var script = new TextCleanerScript<ushort>(factory);
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