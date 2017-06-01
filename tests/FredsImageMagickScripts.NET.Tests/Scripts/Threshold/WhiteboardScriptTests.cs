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
    public class WhiteboardScriptTests : ScriptTester
    {
        [TestMethod]
        public void Constructor_SettingsSetToDefaults()
        {
            var script = new WhiteboardScript();

            AssertDefaults(script);
        }

        [TestMethod]
        public void Reset_AllSettingsChanged_RestoredToDefault()
        {
            var script = new WhiteboardScript();
            script.BackgroundColor = new MagickColor("purple");
            script.Enhance = WhiteboardEnhancements.None;
            script.FilterOffset = (Percentage)5;
            script.FilterSize = 10;
            script.Saturation = (Percentage)100;
            script.WhiteBalance = (Percentage)0.1;

            script.Reset();

            AssertDefaults(script);
        }

        [TestMethod]
        public void Execute_InvalidCoordinates_ThrowsException()
        {
            var topLeft = new PointD(10, 10);
            var topRight = new PointD(630, 10);
            var bottomLeft = new PointD(10, 470);
            var bottomRight = new PointD(630, 470);

            var invalid = new PointD[]
            {
        new PointD(-10, 10), new PointD(10, -10),
        new PointD(650, 10), new PointD(630, 490)
            };

            for (int i = 0; i < invalid.Length; i++)
            {
                AssertSetCoordinates("topLeft", invalid[i], topRight, bottomLeft, bottomRight);
                AssertSetCoordinates("topRight", topLeft, invalid[i], bottomLeft, bottomRight);
                AssertSetCoordinates("bottomRight", topLeft, topRight, invalid[i], bottomRight);
                AssertSetCoordinates("bottomLeft", topLeft, topRight, bottomLeft, invalid[i]);
            }
        }

        [TestMethod]
        public void Execute_InvalidDimensions_ThrowsException()
        {
            AssertDimensions(0, 0);
            AssertDimensions(-1, -1);
            AssertDimensions(-1, 0);
            AssertDimensions(0, -1);
        }

        [TestMethod]
        public void Execute_InputNull_ThrowsException()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                var script = new WhiteboardScript();
                script.Execute(null);
            });
        }

        [TestMethod]
        public void Execute_coords_a_m2_f25_o3_none_jpg()
        {
            AssertExecute("whiteboard.jpg", nameof(Execute_coords_a_m2_f25_o3_none_jpg), (WhiteboardScript script) =>
            {
                script.SetCoordinates(new PointD(101, 53), new PointD(313, 31), new PointD(313, 218), new PointD(101, 200));
                script.AspectRatio = new PointD(4, 3);

                script.Magnification = 2;
                script.FilterSize = 25;
                script.FilterOffset = (Percentage)3;
                script.Enhance = WhiteboardEnhancements.None;
            });
        }

        [TestMethod]
        public void Execute_coords_a_m2_f25_o3_both_jpg()
        {
            AssertExecute("whiteboard.jpg", nameof(Execute_coords_a_m2_f25_o3_both_jpg), (WhiteboardScript script) =>
            {
                script.SetCoordinates(new PointD(101, 53), new PointD(313, 31), new PointD(313, 218), new PointD(101, 200));
                script.AspectRatio = new PointD(4, 3);

                script.Magnification = 2;
                script.FilterSize = 25;
                script.FilterOffset = (Percentage)3;
                script.Enhance = WhiteboardEnhancements.Both;
            });
        }

        [TestMethod]
        public void Execute_coords_a_m2_f25_t60_o3_both_jpg()
        {
            AssertExecute("whiteboard.jpg", nameof(Execute_coords_a_m2_f25_t60_o3_both_jpg), (WhiteboardScript script) =>
            {
                script.SetCoordinates(new PointD(101, 53), new PointD(313, 31), new PointD(313, 218), new PointD(101, 200));
                script.AspectRatio = new PointD(4, 3);

                script.Magnification = 2;
                script.FilterSize = 25;
                script.FilterOffset = (Percentage)3;
                script.Threshold = (Percentage)60;
                script.Enhance = WhiteboardEnhancements.Both;
            });
        }

        [TestMethod]
        public void Execute_f12_o3_none_jpg()
        {
            AssertExecute("whiteboard1_35pct.jpg", nameof(Execute_f12_o3_none_jpg), (WhiteboardScript script) =>
            {
                script.FilterSize = 12;
                script.FilterOffset = (Percentage)3;
                script.Enhance = WhiteboardEnhancements.None;
            });
        }

        [TestMethod]
        public void Execute_f12_o3_both_jpg()
        {
            AssertExecute("whiteboard1_35pct.jpg", nameof(Execute_f12_o3_both_jpg), (WhiteboardScript script) =>
            {
                script.FilterSize = 12;
                script.FilterOffset = (Percentage)3;
                script.Enhance = WhiteboardEnhancements.Both;
            });
        }

        [TestMethod]
        public void Execute_f12_o3_t30_both_jpg()
        {
            AssertExecute("whiteboard1_35pct.jpg", nameof(Execute_f12_o3_t30_both_jpg), (WhiteboardScript script) =>
            {
                script.FilterSize = 12;
                script.FilterOffset = (Percentage)3;
                script.Threshold = (Percentage)30;
                script.Enhance = WhiteboardEnhancements.Both;
            });
        }

        [TestMethod]
        public void Execute_f12_o3_t30_s1_both_jpg()
        {
            AssertExecute("whiteboard1_35pct.jpg", nameof(Execute_f12_o3_t30_s1_both_jpg), (WhiteboardScript script) =>
            {
                script.FilterSize = 12;
                script.FilterOffset = (Percentage)3;
                script.Threshold = (Percentage)30;
                script.SharpeningAmount = 1;
                script.Enhance = WhiteboardEnhancements.Both;
            });
        }

        [TestMethod]
        public void Execute_coords_a_f12_o7_t30_both_jpg()
        {
            AssertExecute("whiteboard2.gif", nameof(Execute_coords_a_f12_o7_t30_both_jpg), (WhiteboardScript script) =>
            {
                script.SetCoordinates(new PointD(55, 60), new PointD(420, 76), new PointD(416, 277), new PointD(75, 345));
                script.AspectRatio = new PointD(4, 3);

                script.FilterSize = 12;
                script.FilterOffset = (Percentage)7;
                script.Threshold = (Percentage)30;
                script.Enhance = WhiteboardEnhancements.Both;
            });
        }

        [TestMethod]
        public void Execute_coords_f12_o7_t30_both_jpg()
        {
            AssertExecute("whiteboard2.gif", nameof(Execute_coords_f12_o7_t30_both_jpg), (WhiteboardScript script) =>
            {
                script.SetCoordinates(new PointD(55, 60), new PointD(420, 76), new PointD(416, 277), new PointD(75, 345));

                script.FilterSize = 12;
                script.FilterOffset = (Percentage)7;
                script.Threshold = (Percentage)30;
                script.Enhance = WhiteboardEnhancements.Both;
            });
        }

        [TestMethod]
        public void Execute_coords_a_f12_o3_t40_both_jpg()
        {
            AssertExecute("WhiteboardBlog.jpg", nameof(Execute_coords_a_f12_o3_t40_both_jpg), (WhiteboardScript script) =>
            {
                script.SetCoordinates(new PointD(13, 3), new PointD(342, 6), new PointD(331, 467), new PointD(38, 482));
                script.AspectRatio = new PointD(3, 4);

                script.FilterSize = 12;
                script.FilterOffset = (Percentage)3;
                script.Threshold = (Percentage)40;
                script.Enhance = WhiteboardEnhancements.Both;
            });
        }

        [TestMethod]
        public void Execute_coords_f12_o3_t40_both_jpg()
        {
            AssertExecute("WhiteboardBlog.jpg", nameof(Execute_coords_f12_o3_t40_both_jpg), (WhiteboardScript script) =>
            {
                script.SetCoordinates(new PointD(13, 3), new PointD(342, 6), new PointD(331, 467), new PointD(38, 482));

                script.FilterSize = 12;
                script.FilterOffset = (Percentage)3;
                script.Threshold = (Percentage)40;
                script.Enhance = WhiteboardEnhancements.Both;
            });
        }

        [TestMethod]
        public void Execute_coords_d300x_jpg()
        {
            AssertExecute("WhiteboardBlog.jpg", nameof(Execute_coords_d300x_jpg), (WhiteboardScript script) =>
            {
                script.SetCoordinates(new PointD(13, 3), new PointD(342, 6), new PointD(331, 467), new PointD(38, 482));
                script.Dimensions = new MagickGeometry(300, 0);
            });
        }

        [TestMethod]
        public void Execute_f12_o4_both_jpg()
        {
            AssertExecute("whiteboardScenario1.jpg", nameof(Execute_f12_o4_both_jpg), (WhiteboardScript script) =>
            {
                script.FilterSize = 12;
                script.FilterOffset = (Percentage)4;
                script.Enhance = WhiteboardEnhancements.Both;
            });
        }

        [TestMethod]
        public void Execute_d300x_S100_stretch_jpg()
        {
            AssertExecute("whiteboardScenario1.jpg", nameof(Execute_d300x_S100_stretch_jpg), (WhiteboardScript script) =>
            {
                script.Dimensions = new MagickGeometry("300x");
                script.Saturation = new Percentage(100);
                script.Enhance = WhiteboardEnhancements.Stretch;
            });
        }

        [TestMethod]
        public void Execute_dx300_sm1_whiteBalance_jpg()
        {
            AssertExecute("whiteboardScenario1.jpg", nameof(Execute_dx300_sm1_whiteBalance_jpg), (WhiteboardScript script) =>
            {
                script.Dimensions = new MagickGeometry("x300");
                script.SharpeningAmount = -1;
                script.Enhance = WhiteboardEnhancements.Whitebalance;
            });
        }

        [TestMethod]
        public void Execute_d300x300_jpg()
        {
            AssertExecute("whiteboardScenario1.jpg", nameof(Execute_d300x300_jpg), (WhiteboardScript script) =>
            {
                script.Dimensions = new MagickGeometry("300x300");
            });
        }

        [TestMethod]
        public void Execute_m0_75_jpg()
        {
            AssertExecute("whiteboardScenario1.jpg", nameof(Execute_m0_75_jpg), (WhiteboardScript script) =>
            {
                script.Magnification = 0.75;
            });
        }

        private static void AssertDefaults(WhiteboardScript script)
        {
            ColorAssert.AreEqual(new MagickColor("white"), script.BackgroundColor);
            Assert.AreEqual(WhiteboardEnhancements.Stretch, script.Enhance);
            Assert.AreEqual((Percentage)5, script.FilterOffset);
            Assert.AreEqual(15, script.FilterSize);
            Assert.AreEqual((Percentage)200, script.Saturation);
            Assert.AreEqual((Percentage)0.01, script.WhiteBalance);
        }

        private static void AssertDimensions(int width, int height)
        {
            ExceptionAssert.Throws<InvalidOperationException>(() =>
            {
                using (var logo = Images.Logo)
                {
                    var script = new WhiteboardScript();
                    script.Dimensions = new MagickGeometry(width, height);
                    script.Execute(logo);
                }
            });
        }

        private static void AssertSetCoordinates(string paramName, PointD topLeft, PointD topRight, PointD bottomLeft, PointD bottomRight)
        {
            ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>(paramName, () =>
            {
                using (var logo = Images.Logo)
                {
                    var script = new WhiteboardScript();
                    script.SetCoordinates(topLeft, topRight, bottomLeft, bottomRight);
                    script.Execute(logo);
                }
            });
        }

        private void AssertExecute(string input, string methodName, Action<WhiteboardScript> action)
        {
            string inputFile = GetInputFile(input);

            using (var image = new MagickImage(inputFile))
            {
                var script = new WhiteboardScript();
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
