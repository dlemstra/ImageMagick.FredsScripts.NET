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
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests
{
    [TestClass]
    public class TextCleanerScriptTests : ScriptTester
    {
        [TestMethod]
        public void Constructor_SettingsSetToDefaults()
        {
            var script = new TextCleanerScript();

            AssertDefaults(script);
        }

        [TestMethod]
        public void Reset_AllSettingsChanged_RestoredToDefault()
        {
            var script = new TextCleanerScript();
            script.AdaptiveBlur = 2;
            script.BackgroundColor = new MagickColor("yellow");
            script.CropOffset.Left = 1;
            script.CropOffset.Top = 1;
            script.CropOffset.Right = 1;
            script.CropOffset.Bottom = 1;
            script.Enhance = TextCleanerEnhance.Normalize;
            script.FilterOffset = (Percentage)10;
            script.FilterSize = 10;
            script.Layout = TextCleanerLayout.Landscape;
            script.MakeGray = true;
            script.Padding = 15;
            script.Rotation = TextCleanerRotation.Clockwise;
            script.Saturation = (Percentage)150;
            script.Sharpen = 1;
            script.SmoothingThreshold = (Percentage)50;
            script.Trim = true;
            script.Unrotate = true;

            script.Reset();

            AssertDefaults(script);
        }

        [TestMethod]
        public void AdaptiveBlur_BelowZero_ThrowsException()
        {
            AssertInvalidOperation("Invalid adaptive blur specified, value must be zero or higher.", (TextCleanerScript script) =>
            {
                script.AdaptiveBlur = -1;
            });
        }

        [TestMethod]
        public void CropOffset_BottomBelowZero_ThrowsException()
        {
            AssertInvalidOperation("Invalid crop offset specified, values must be zero or higher.", (TextCleanerScript script) =>
            {
                script.CropOffset.Bottom = -1;
            });
        }

        [TestMethod]
        public void CropOffset_LeftBelowZero_ThrowsException()
        {
            AssertInvalidOperation("Invalid crop offset specified, values must be zero or higher.", (TextCleanerScript script) =>
            {
                script.CropOffset.Left = -1;
            });
        }

        [TestMethod]
        public void CropOffset_RightBelowZero_ThrowsException()
        {
            AssertInvalidOperation("Invalid crop offset specified, values must be zero or higher.", (TextCleanerScript script) =>
            {
                script.CropOffset.Right = -1;
            });
        }

        [TestMethod]
        public void CropOffset_TopBelowZero_ThrowsException()
        {
            AssertInvalidOperation("Invalid crop offset specified, values must be zero or higher.", (TextCleanerScript script) =>
            {
                script.CropOffset.Top = -1;
            });
        }

        [TestMethod]
        public void FilterSize_BelowZero_ThrowsException()
        {
            AssertInvalidOperation("Invalid filter size specified, value must be zero or higher.", (TextCleanerScript script) =>
            {
                script.FilterSize = -1;
            });
        }

        [TestMethod]
        public void Padding_BelowZero_ThrowsException()
        {
            AssertInvalidOperation("Invalid padding specified, value must be zero or higher.", (TextCleanerScript script) =>
            {
                script.Padding = -1;
            });
        }

        [TestMethod]
        public void Sharpen_BelowZero_ThrowsException()
        {
            AssertInvalidOperation("Invalid sharpen specified, value must be zero or higher.", (TextCleanerScript script) =>
            {
                script.Sharpen = -1;
            });
        }

        [TestMethod]
        public void Saturation_BelowZero_ThrowsException()
        {
            AssertInvalidOperation("Invalid saturation specified, value must be zero or higher.", (TextCleanerScript script) =>
            {
                script.Saturation = (Percentage)(-1);
            });
        }

        [TestMethod]
        public void SmoothingThreshold_Above100_ThrowsException()
        {
            AssertInvalidOperation("Invalid smoothing threshold specified, value must be between zero and 100.", (TextCleanerScript script) =>
            {
                script.SmoothingThreshold = (Percentage)101;
            });
        }

        [TestMethod]
        public void SmoothingThreshold_MinusOneOrLower_ThrowsException()
        {
            AssertInvalidOperation("Invalid smoothing threshold specified, value must be between zero and 100.", (TextCleanerScript script) =>
            {
                script.SmoothingThreshold = (Percentage)(-1);
            });
        }

        [TestMethod]
        public void Execute_InputNull_ThrowsException()
        {
            ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () =>
            {
                var script = new TextCleanerScript();
                script.Execute(null);
            });
        }

        [TestMethod]
        public void Execute_g_none_f15_o20_jpg()
        {
            AssertExecute("abbott2.jpg", nameof(Execute_g_none_f15_o20_jpg), (TextCleanerScript script) =>
            {
                script.MakeGray = true;
                script.Enhance = TextCleanerEnhance.None;
                script.FilterSize = 15;
                script.FilterOffset = (Percentage)20;
            });
        }

        [TestMethod]
        public void Execute_g_stretch_f15_o20_jpg()
        {
            AssertExecute("abbott2.jpg", nameof(Execute_g_stretch_f15_o20_jpg), (TextCleanerScript script) =>
            {
                script.MakeGray = true;
                script.Enhance = TextCleanerEnhance.Stretch;
                script.FilterSize = 15;
                script.FilterOffset = (Percentage)20;
            });
        }

        [TestMethod]
        public void Execute_g_stretch_f25_o20_jpg()
        {
            AssertExecute("abbott2.jpg", nameof(Execute_g_stretch_f25_o20_jpg), (TextCleanerScript script) =>
            {
                script.MakeGray = true;
                script.Enhance = TextCleanerEnhance.Stretch;
                script.FilterSize = 25;
                script.FilterOffset = (Percentage)20;
            });
        }

        [TestMethod]
        public void Execute_g_stretch_f25_o20_s1_jpg()
        {
            AssertExecute("abbott2.jpg", nameof(Execute_g_stretch_f25_o20_s1_jpg), (TextCleanerScript script) =>
            {
                script.MakeGray = true;
                script.Enhance = TextCleanerEnhance.Stretch;
                script.FilterSize = 25;
                script.FilterOffset = (Percentage)20;
                script.Sharpen = 1;
            });
        }

        [TestMethod]
        public void Execute_g_stretch_f25_o20_t30_s1_u_T_p20_jpg()
        {
            AssertExecute("abbott2.jpg", nameof(Execute_g_stretch_f25_o20_t30_s1_u_T_p20_jpg), (TextCleanerScript script) =>
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
        public void Execute_c_0x50x0x0_g_normalize_f15_o10_s2_u_T_p20_jpg()
        {
            AssertExecute("brscan_original_r90.jpg", nameof(Execute_c_0x50x0x0_g_normalize_f15_o10_s2_u_T_p20_jpg), (TextCleanerScript script) =>
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
        public void Execute_g_none_f15_o10_jpg()
        {
            AssertExecute("brscan_original_r90.jpg", nameof(Execute_g_none_f15_o10_jpg), (TextCleanerScript script) =>
            {
                script.MakeGray = true;
                script.Enhance = TextCleanerEnhance.None;
                script.FilterSize = 15;
                script.FilterOffset = (Percentage)10;
            });
        }

        [TestMethod]
        public void Execute_g_normalize_f15_o10_jpg()
        {
            AssertExecute("brscan_original_r90.jpg", nameof(Execute_g_normalize_f15_o10_jpg), (TextCleanerScript script) =>
            {
                script.MakeGray = true;
                script.Enhance = TextCleanerEnhance.Normalize;
                script.FilterSize = 15;
                script.FilterOffset = (Percentage)10;
            });
        }

        [TestMethod]
        public void Execute_g_normalize_f15_o10_s1_jpg()
        {
            AssertExecute("brscan_original_r90.jpg", nameof(Execute_g_normalize_f15_o10_s1_jpg), (TextCleanerScript script) =>
            {
                script.MakeGray = true;
                script.Enhance = TextCleanerEnhance.Normalize;
                script.FilterSize = 15;
                script.FilterOffset = (Percentage)10;
                script.Sharpen = 1;
            });
        }

        [TestMethod]
        public void Execute_norm_f15_o5_S200_jpg()
        {
            AssertExecute("congress.jpg", nameof(Execute_norm_f15_o5_S200_jpg), (TextCleanerScript script) =>
            {
                script.Enhance = TextCleanerEnhance.Normalize;
                script.FilterSize = 15;
                script.FilterOffset = (Percentage)5;
                script.Saturation = (Percentage)200;
            });
        }

        [TestMethod]
        public void Execute_norm_f15_o5_S200_s1_jpg()
        {
            AssertExecute("congress.jpg", nameof(Execute_norm_f15_o5_S200_s1_jpg), (TextCleanerScript script) =>
            {
                script.Enhance = TextCleanerEnhance.Normalize;
                script.FilterSize = 15;
                script.FilterOffset = (Percentage)5;
                script.Saturation = (Percentage)200;
                script.Sharpen = 1;
            });
        }

        [TestMethod]
        public void Execute_norm_f15_o5_S400_jpg()
        {
            AssertExecute("congress.jpg", nameof(Execute_norm_f15_o5_S400_jpg), (TextCleanerScript script) =>
            {
                script.Enhance = TextCleanerEnhance.Normalize;
                script.FilterSize = 15;
                script.FilterOffset = (Percentage)5;
                script.Saturation = (Percentage)400;
                script.Sharpen = 1;
            });
        }

        [TestMethod]
        public void Execute_g_stretch_f25_o10_u_s1_T_p10_jpg()
        {
            AssertExecute("crankshaft.jpg", nameof(Execute_g_stretch_f25_o10_u_s1_T_p10_jpg), (TextCleanerScript script) =>
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
        public void Execute_g_stretch_f25_o5_s1_jpg()
        {
            AssertExecute("railways.jpg", nameof(Execute_g_stretch_f25_o5_s1_jpg), (TextCleanerScript script) =>
            {
                script.MakeGray = true;
                script.Enhance = TextCleanerEnhance.Stretch;
                script.FilterSize = 25;
                script.FilterOffset = (Percentage)5;
                script.Sharpen = 1;
                script.Layout = TextCleanerLayout.Landscape;
            });

            AssertExecute("rfid.jpg", nameof(Execute_g_stretch_f25_o5_s1_jpg), (TextCleanerScript script) =>
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
        public void Execute_a2_S100_st50_rc_jpg()
        {
            AssertExecute("rfid.jpg", nameof(Execute_a2_S100_st50_rc_jpg), (TextCleanerScript script) =>
            {
                script.AdaptiveBlur = 2;
                script.Rotation = TextCleanerRotation.Clockwise;
                script.Saturation = new Percentage(100);
                script.SmoothingThreshold = new Percentage(50);
            });
        }

        [TestMethod]
        public void Execute_g_f25_o5_s1_rcc_jpg()
        {
            AssertExecute("rfid.jpg", nameof(Execute_g_f25_o5_s1_rcc_jpg), (TextCleanerScript script) =>
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
        public void Execute_g_stretch_f15_o5_s1_jpg()
        {
            AssertExecute("telegram.jpg", nameof(Execute_g_stretch_f15_o5_s1_jpg), (TextCleanerScript script) =>
            {
                script.MakeGray = true;
                script.Enhance = TextCleanerEnhance.Stretch;
                script.FilterSize = 15;
                script.FilterOffset = (Percentage)5;
                script.Sharpen = 1;
            });
        }

        [TestMethod]
        public void Execute_g_stretch_f25_o10_s1_jpg()
        {
            AssertExecute("twinkle.jpg", nameof(Execute_g_stretch_f25_o10_s1_jpg), (TextCleanerScript script) =>
            {
                script.MakeGray = true;
                script.Enhance = TextCleanerEnhance.Stretch;
                script.FilterSize = 25;
                script.FilterOffset = (Percentage)10;
                script.Sharpen = 1;
                script.Layout = TextCleanerLayout.Landscape;
            });
        }

        private static void AssertDefaults(TextCleanerScript script)
        {
            Assert.AreEqual(0.0, script.AdaptiveBlur);
            ColorAssert.AreEqual(new MagickColor("white"), script.BackgroundColor);
            Assert.AreEqual(0, script.CropOffset.Left);
            Assert.AreEqual(0, script.CropOffset.Top);
            Assert.AreEqual(0, script.CropOffset.Right);
            Assert.AreEqual(0, script.CropOffset.Bottom);
            Assert.AreEqual(TextCleanerEnhance.Stretch, script.Enhance);
            Assert.AreEqual((Percentage)5, script.FilterOffset);
            Assert.AreEqual(15, script.FilterSize);
            Assert.AreEqual(TextCleanerLayout.Portrait, script.Layout);
            Assert.AreEqual(false, script.MakeGray);
            Assert.AreEqual(0, script.Padding);
            Assert.AreEqual(TextCleanerRotation.None, script.Rotation);
            Assert.AreEqual((Percentage)200, script.Saturation);
            Assert.AreEqual(0.0, script.Sharpen);
            Assert.AreEqual(null, script.SmoothingThreshold);
            Assert.AreEqual(false, script.Trim);
            Assert.AreEqual(false, script.Unrotate);
        }

        private static void AssertInvalidOperation(string expectedMessage, Action<TextCleanerScript> initAction)
        {
            var script = new TextCleanerScript();

            using (var logo = new MagickImage(Images.Logo))
            {
                initAction(script);

                ExceptionAssert.Throws<InvalidOperationException>(expectedMessage, () =>
                {
                    script.Execute(logo);
                });
            }
        }

        private void AssertExecute(string input, string methodName, Action<TextCleanerScript> action)
        {
            string inputFile = GetInputFile(input);
            /* LosslessCompress(inputFile); */

            using (var image = new MagickImage(inputFile))
            {
                TextCleanerScript script = new TextCleanerScript();
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