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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public partial class WhiteboardScriptTests
    {
        [TestClass]
        public class TheExecuteMethod : WhiteboardScriptTests
        {
            [TestMethod]
            public void ShouldThrowExceptionWhenInputIsNull()
            {
                var factory = new MagickFactory();
                var script = new WhiteboardScript<ushort>(factory);

                ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () => script.Execute(null));
            }

            [TestMethod]
            public void ShouldExecute_coords_a_m2_f25_o3_none_jpg()
            {
                AssertExecute("whiteboard.jpg", nameof(ShouldExecute_coords_a_m2_f25_o3_none_jpg), (WhiteboardScript<ushort> script) =>
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
            public void ShouldExecute_coords_a_m2_f25_o3_both_jpg()
            {
                AssertExecute("whiteboard.jpg", nameof(ShouldExecute_coords_a_m2_f25_o3_both_jpg), (WhiteboardScript<ushort> script) =>
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
            public void ShouldExecute_coords_a_m2_f25_t60_o3_both_jpg()
            {
                AssertExecute("whiteboard.jpg", nameof(ShouldExecute_coords_a_m2_f25_t60_o3_both_jpg), (WhiteboardScript<ushort> script) =>
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
            public void ShouldExecute_f12_o3_none_jpg()
            {
                AssertExecute("whiteboard1_35pct.jpg", nameof(ShouldExecute_f12_o3_none_jpg), (WhiteboardScript<ushort> script) =>
                {
                    script.FilterSize = 12;
                    script.FilterOffset = (Percentage)3;
                    script.Enhance = WhiteboardEnhancements.None;
                });
            }

            [TestMethod]
            public void ShouldExecute_f12_o3_both_jpg()
            {
                AssertExecute("whiteboard1_35pct.jpg", nameof(ShouldExecute_f12_o3_both_jpg), (WhiteboardScript<ushort> script) =>
                {
                    script.FilterSize = 12;
                    script.FilterOffset = (Percentage)3;
                    script.Enhance = WhiteboardEnhancements.Both;
                });
            }

            [TestMethod]
            public void ShouldExecute_f12_o3_t30_both_jpg()
            {
                AssertExecute("whiteboard1_35pct.jpg", nameof(ShouldExecute_f12_o3_t30_both_jpg), (WhiteboardScript<ushort> script) =>
                {
                    script.FilterSize = 12;
                    script.FilterOffset = (Percentage)3;
                    script.Threshold = (Percentage)30;
                    script.Enhance = WhiteboardEnhancements.Both;
                });
            }

            [TestMethod]
            public void ShouldExecute_f12_o3_t30_s1_both_jpg()
            {
                AssertExecute("whiteboard1_35pct.jpg", nameof(ShouldExecute_f12_o3_t30_s1_both_jpg), (WhiteboardScript<ushort> script) =>
                {
                    script.FilterSize = 12;
                    script.FilterOffset = (Percentage)3;
                    script.Threshold = (Percentage)30;
                    script.SharpeningAmount = 1;
                    script.Enhance = WhiteboardEnhancements.Both;
                });
            }

            [TestMethod]
            public void ShouldExecute_coords_a_f12_o7_t30_both_jpg()
            {
                AssertExecute("whiteboard2.gif", nameof(ShouldExecute_coords_a_f12_o7_t30_both_jpg), (WhiteboardScript<ushort> script) =>
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
            public void ShouldExecute_coords_f12_o7_t30_both_jpg()
            {
                AssertExecute("whiteboard2.gif", nameof(ShouldExecute_coords_f12_o7_t30_both_jpg), (WhiteboardScript<ushort> script) =>
                {
                    script.SetCoordinates(new PointD(55, 60), new PointD(420, 76), new PointD(416, 277), new PointD(75, 345));

                    script.FilterSize = 12;
                    script.FilterOffset = (Percentage)7;
                    script.Threshold = (Percentage)30;
                    script.Enhance = WhiteboardEnhancements.Both;
                });
            }

            [TestMethod]
            public void ShouldExecute_coords_a_f12_o3_t40_both_jpg()
            {
                AssertExecute("WhiteboardBlog.jpg", nameof(ShouldExecute_coords_a_f12_o3_t40_both_jpg), (WhiteboardScript<ushort> script) =>
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
            public void ShouldExecute_coords_f12_o3_t40_both_jpg()
            {
                AssertExecute("WhiteboardBlog.jpg", nameof(ShouldExecute_coords_f12_o3_t40_both_jpg), (WhiteboardScript<ushort> script) =>
                {
                    script.SetCoordinates(new PointD(13, 3), new PointD(342, 6), new PointD(331, 467), new PointD(38, 482));

                    script.FilterSize = 12;
                    script.FilterOffset = (Percentage)3;
                    script.Threshold = (Percentage)40;
                    script.Enhance = WhiteboardEnhancements.Both;
                });
            }

            [TestMethod]
            public void ShouldExecute_coords_d300x_jpg()
            {
                AssertExecute("WhiteboardBlog.jpg", nameof(ShouldExecute_coords_d300x_jpg), (WhiteboardScript<ushort> script) =>
                {
                    script.SetCoordinates(new PointD(13, 3), new PointD(342, 6), new PointD(331, 467), new PointD(38, 482));
                    script.Dimensions = new MagickGeometry(300, 0);
                });
            }

            [TestMethod]
            public void ShouldExecute_f12_o4_both_jpg()
            {
                AssertExecute("whiteboardScenario1.jpg", nameof(ShouldExecute_f12_o4_both_jpg), (WhiteboardScript<ushort> script) =>
                {
                    script.FilterSize = 12;
                    script.FilterOffset = (Percentage)4;
                    script.Enhance = WhiteboardEnhancements.Both;
                });
            }

            [TestMethod]
            public void ShouldExecute_d300x_S100_stretch_jpg()
            {
                AssertExecute("whiteboardScenario1.jpg", nameof(ShouldExecute_d300x_S100_stretch_jpg), (WhiteboardScript<ushort> script) =>
                {
                    script.Dimensions = new MagickGeometry("300x");
                    script.Saturation = new Percentage(100);
                    script.Enhance = WhiteboardEnhancements.Stretch;
                });
            }

            [TestMethod]
            public void ShouldExecute_dx300_sm1_whiteBalance_jpg()
            {
                AssertExecute("whiteboardScenario1.jpg", nameof(ShouldExecute_dx300_sm1_whiteBalance_jpg), (WhiteboardScript<ushort> script) =>
                {
                    script.Dimensions = new MagickGeometry("x300");
                    script.SharpeningAmount = -1;
                    script.Enhance = WhiteboardEnhancements.Whitebalance;
                });
            }

            [TestMethod]
            public void ShouldExecute_d300x300_jpg()
            {
                AssertExecute("whiteboardScenario1.jpg", nameof(ShouldExecute_d300x300_jpg), (WhiteboardScript<ushort> script) =>
                {
                    script.Dimensions = new MagickGeometry("300x300");
                });
            }

            [TestMethod]
            public void ShouldExecute_m0_75_jpg()
            {
                AssertExecute("whiteboardScenario1.jpg", nameof(ShouldExecute_m0_75_jpg), (WhiteboardScript<ushort> script) =>
                {
                    script.Magnification = 0.75;
                });
            }

            private void AssertExecute(string input, string methodName, Action<WhiteboardScript<ushort>> action)
            {
                string inputFile = GetInputFile(input);

                using (var image = new MagickImage(inputFile))
                {
                    var factory = new MagickFactory();
                    var script = new WhiteboardScript<ushort>(factory);
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