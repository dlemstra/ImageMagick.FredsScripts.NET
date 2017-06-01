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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests
{
    [TestClass]
    public class DraganEffectScriptTests : ScriptTester
    {
        [TestMethod]
        public void Constructor_SettingsSetToDefaults()
        {
            var script = new DraganEffectScript();

            AssertDefaults(script);
        }

        [TestMethod]
        public void Reset_AllSettingsChanged_RestoredToDefault()
        {
            var script = new DraganEffectScript();
            script.Brightness = 0.5;
            script.Contrast = 4;
            script.Darkness = 2;
            script.Saturation = (Percentage)100;

            script.Reset();

            AssertDefaults(script);
        }

        [TestMethod]
        public void Test_Execute_Null()
        {
            ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () =>
            {
                var script = new DraganEffectScript();
                script.Execute(null);
            });
        }

        [TestMethod]
        public void Brightness_BelowZero_ThrowsException()
        {
            AssertInvalidOperation("Invalid brightness specified, value must be zero or higher.", (DraganEffectScript script) =>
            {
                script.Brightness = -1.0;
            });
        }

        [TestMethod]
        public void Contrast_BelowMinusTen_ThrowsException()
        {
            AssertInvalidOperation("Invalid contrast specified, the range is -10 to 10.", (DraganEffectScript script) =>
            {
                script.Contrast = -11.0;
            });
        }

        [TestMethod]
        public void Contrast_AboveTen_ThrowsException()
        {
            AssertInvalidOperation("Invalid contrast specified, the range is -10 to 10.", (DraganEffectScript script) =>
            {
                script.Contrast = 11.0;
            });
        }

        [TestMethod]
        public void Darkness_Zero_ThrowsException()
        {
            AssertInvalidOperation("Invalid darkness specified, value must be 1 or higher.", (DraganEffectScript script) =>
            {
                script.Darkness = 0.0;
            });
        }

        [TestMethod]
        public void Saturation_BelowZero_ThrowsException()
        {
            AssertInvalidOperation("Invalid saturation specified, value must be zero or higher.", (DraganEffectScript script) =>
            {
                script.Saturation = (Percentage)(-1);
            });
        }

        [TestMethod]
        public void Execute_b1_c0_d1_s150_r5_jpg()
        {
            AssertExecute("before1.gif", nameof(Execute_b1_c0_d1_s150_r5_jpg), (DraganEffectScript script) =>
            {
                script.Brightness = 1;
                script.Contrast = 0;
                script.Darkness = 1;
                script.Saturation = (Percentage)150;
            });

            AssertExecute("bluehat.jpg", nameof(Execute_b1_c0_d1_s150_r5_jpg), (DraganEffectScript script) =>
            {
                script.Brightness = 1;
                script.Contrast = 0;
                script.Darkness = 1;
                script.Saturation = (Percentage)150;
            });
        }

        [TestMethod]
        public void Execute_b1p5_cm5_d1_s175_r5_jpg()
        {
            AssertExecute("before1.gif", nameof(Execute_b1p5_cm5_d1_s175_r5_jpg), (DraganEffectScript script) =>
            {
                script.Brightness = 1.5;
                script.Contrast = -5;
                script.Darkness = 1;
                script.Saturation = (Percentage)175;
            });
        }

        [TestMethod]
        public void Execute_b1p5_cm5_d2_s175_r5_jpg()
        {
            AssertExecute("before1.gif", nameof(Execute_b1p5_cm5_d2_s175_r5_jpg), (DraganEffectScript script) =>
            {
                script.Brightness = 1.5;
                script.Contrast = -5;
                script.Darkness = 2;
                script.Saturation = (Percentage)175;
            });
        }

        [TestMethod]
        public void Execute_b1_cm7p5_d1_s200_r5_jpg()
        {
            AssertExecute("bluehat.jpg", nameof(Execute_b1_cm7p5_d1_s200_r5_jpg), (DraganEffectScript script) =>
            {
                script.Brightness = 1;
                script.Contrast = -7.5;
                script.Darkness = 1;
                script.Saturation = (Percentage)200;
            });
        }

        [TestMethod]
        public void Execute_b1_cm7p5_d1p25_s200_r5_jpg()
        {
            AssertExecute("bluehat.jpg", nameof(Execute_b1_cm7p5_d1p25_s200_r5_jpg), (DraganEffectScript script) =>
            {
                script.Brightness = 1;
                script.Contrast = -7.5;
                script.Darkness = 1.25;
                script.Saturation = (Percentage)200;
            });
        }

        [TestMethod]
        public void Execute_b1_cm5_d1_s150_r5_jpg()
        {
            AssertExecute("CHINA-715-4_small.jpg", nameof(Execute_b1_cm5_d1_s150_r5_jpg), (DraganEffectScript script) =>
            {
                script.Brightness = 1;
                script.Contrast = -5;
                script.Darkness = 1;
                script.Saturation = (Percentage)150;
            });
        }

        [TestMethod]
        public void Execute_b1_cm10_d1_s200_r5_jpg()
        {
            AssertExecute("CHINA-715-4_small.jpg", nameof(Execute_b1_cm10_d1_s200_r5_jpg), (DraganEffectScript script) =>
            {
                script.Contrast = -10;
                script.Darkness = 1;
                script.Saturation = (Percentage)200;
            });
        }

        [TestMethod]
        public void Execute_b1_cm5_d1p75_s175_r5_jpg()
        {
            AssertExecute("mustache.jpg", nameof(Execute_b1_cm5_d1p75_s175_r5_jpg), (DraganEffectScript script) =>
            {
                script.Brightness = 1;
                script.Contrast = -5;
                script.Darkness = 1.75;
                script.Saturation = (Percentage)175;
            });
        }

        [TestMethod]
        public void Execute_d3_s100_jpg()
        {
            AssertExecute("mustache.jpg", nameof(Execute_d3_s100_jpg), (DraganEffectScript script) =>
            {
                script.Darkness = 3;
                script.Saturation = (Percentage)100;
            });
        }

        private static void AssertDefaults(DraganEffectScript script)
        {
            Assert.AreEqual(1.0, script.Brightness);
            Assert.AreEqual(0.0, script.Contrast);
            Assert.AreEqual(1.0, script.Darkness);
            Assert.AreEqual((Percentage)150, script.Saturation);
        }

        private static void AssertInvalidOperation(string expectedMessage, Action<DraganEffectScript> initAction)
        {
            var script = new DraganEffectScript();

            using (var logo = new MagickImage(Images.Logo))
            {
                initAction(script);

                ExceptionAssert.Throws<InvalidOperationException>(expectedMessage, () =>
                {
                    script.Execute(logo);
                });
            }
        }

        private void AssertExecute(string input, string methodName, Action<DraganEffectScript> action)
        {
            string inputFile = GetInputFile(input);
            /* LosslessCompress(inputFile); */

            using (var image = new MagickImage(inputFile))
            {
                var script = new DraganEffectScript();
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