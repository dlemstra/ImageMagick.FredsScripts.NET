// Copyright 2015-2020 Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/FredsImageMagickScripts.NET)
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
    public partial class UnperspectiveScriptTests
    {
        [TestClass]
        public class TheExecuteMethod : UnperspectiveScriptTests
        {
            [TestMethod]
            public void ShouldThrowExceptionWhenInputIsNull()
            {
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory);

                var overlay = new MagickImage();
                ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () => script.Execute(null));
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenBorderColorLocationBeforeLeftTop()
            {
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory);

                using (var logo = new MagickImage(Images.Logo))
                {
                    script.BorderColorLocation = new PointD(-1, -1);

                    ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>("x", "Invalid X coordinate: -1.", () =>
                    {
                        script.Execute(logo);
                    });
                }
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenBorderColorLocationAfterBottomRight()
            {
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory);

                using (var logo = new MagickImage(Images.Logo))
                {
                    script.BorderColorLocation = new PointD(logo.Width, logo.Height);

                    ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>("x", "Invalid X coordinate: " + logo.Width + ".", () =>
                    {
                        script.Execute(logo);
                    });
                }
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenDefaultInvalidValue()
            {
                AssertInvalidOperation("Invalid default output dimension specified.", (UnperspectiveScript<ushort> script) =>
                {
                    script.Default = (UnperspectiveDefault)42;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenMaxPeaksToLow()
            {
                AssertInvalidOperation("Unable to continue, the number of peaks is higher than 4.", (UnperspectiveScript<ushort> script) =>
                {
                    script.MaxPeaks = 4;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenThresholdZero()
            {
                AssertInvalidOperation("Unable to continue, the number of peaks should be 4.", (UnperspectiveScript<ushort> script) =>
                {
                    script.Threshold = 0;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenThresholdToHigh()
            {
                AssertInvalidOperation("Unable to continue, the number of peaks should be 4.", (UnperspectiveScript<ushort> script) =>
                {
                    script.Threshold = 20000;
                });

                AssertInvalidOperation("Unable to continue, the number of peaks should be 4.", UnperspectiveMethod.Derivative, (UnperspectiveScript<ushort> script) =>
                {
                    script.Threshold = 20000;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenWidthHeightBothSet()
            {
                AssertInvalidOperation("Both width and height cannot be specified at the same time.", (UnperspectiveScript<ushort> script) =>
                {
                    script.Width = 500;
                    script.Height = 500;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenMinLengthValueToHigh()
            {
                AssertMinLength(0, 209);
                AssertMinLength(0, 276);
                AssertMinLength(25, 354);
                AssertMinLength(25, 427);
            }

            [TestMethod]
            public void ShouldExecute_f20_jpg()
            {
                AssertExecute("mandril2_p30_t30_out.jpg", nameof(ShouldExecute_f20_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)20;
                });

                AssertExecute("mandril2_p30_t30_r60_zc.jpg", nameof(ShouldExecute_f20_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)20;
                });

                AssertExecute("mandril2_pm30_t30_r30_zc.jpg", nameof(ShouldExecute_f20_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)20;
                });
            }

            [TestMethod]
            public void ShouldExecute_f20_r270_jpg()
            {
                AssertExecute("mandril2_p30_t30_r60_zc.jpg", nameof(ShouldExecute_f20_r270_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)20;
                    script.Rotation = UnperspectiveRotation.Rotate270;
                });
            }

            [TestMethod]
            public void ShouldExecute_f52_t8_s2_jpg()
            {
                AssertExecute("mandril2_pm30_t30_r30_zc.jpg", nameof(ShouldExecute_f52_t8_s2_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)52;
                    script.Threshold = 8;
                    script.Smooth = 2;
                });
            }

            [TestMethod]
            public void ShouldExecute_f14_bh_jpg()
            {
                AssertExecute("mandril2_pm30_t30_r30_zc.jpg", nameof(ShouldExecute_f14_bh_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)14;
                    script.Default = UnperspectiveDefault.BoundingBoxHeight;
                });
            }

            [TestMethod]
            public void ShouldExecute_f20_h_jpg()
            {
                AssertExecute("mandril2_pm30_t30_r30_zc.jpg", nameof(ShouldExecute_f20_h_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)20;
                    script.Default = UnperspectiveDefault.Height;
                });
            }

            [TestMethod]
            public void ShouldExecute_f20_s2_vc_jpg()
            {
                AssertExecute("mandril2_round30_p30_t30_out.jpg", nameof(ShouldExecute_f20_s2_vc_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)20;
                    script.Smooth = 2;
                    script.DisableViewportCrop = true;
                });
            }

            [TestMethod]
            public void ShouldExecute_f10_jpg()
            {
                AssertExecute("monet2_p30_t30_r30_out.jpg", nameof(ShouldExecute_f10_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)10;
                });

                AssertExecute("redcanoe_p30_t30_out.jpg", nameof(ShouldExecute_f10_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)10;
                });
            }

            [TestMethod]
            public void ShouldExecute_f7_vc_jpg()
            {
                AssertExecute("receipt1.jpg", nameof(ShouldExecute_f7_vc_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)7;
                    script.DisableViewportCrop = true;
                });
            }

            [TestMethod]
            public void ShouldExecute_f20_vc_jpg()
            {
                AssertExecute("receipt1.jpg", nameof(ShouldExecute_f20_vc_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)20;
                    script.DisableViewportCrop = true;
                });
            }

            [TestMethod]
            public void ShouldExecute_f20_vc_w500_jpg()
            {
                AssertExecute("receipt1.jpg", nameof(ShouldExecute_f20_vc_w500_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)20;
                    script.Width = 500;
                    script.DisableViewportCrop = true;
                });
            }

            [TestMethod]
            public void ShouldExecute_f50_jpg()
            {
                AssertExecute("receipt2.jpg", nameof(ShouldExecute_f50_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)50;
                });
            }

            [TestMethod]
            public void ShouldExecute_f50_w200_jpg()
            {
                AssertExecute("receipt2.jpg", nameof(ShouldExecute_f50_w200_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)50;
                    script.Width = 200;
                });
            }

            [TestMethod]
            public void ShouldExecute_f11_t10_s2_S0_B3_jpg()
            {
                AssertExecute("textsample_localthresh_m1_r25_b5_white_b20_p30_t30_out.png", nameof(ShouldExecute_f11_t10_s2_S0_B3_jpg), (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)11;
                    script.Threshold = 10;
                    script.Smooth = 2;
                    script.Sharpen = 0;
                    script.Blur = 3;
                });
            }

            [TestMethod]
            public void ShouldExecute_f10_derivative_jpg()
            {
                AssertExecute("redcanoe_p30_t30_out.jpg", nameof(ShouldExecute_f10_derivative_jpg), UnperspectiveMethod.Derivative, (UnperspectiveScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)10;
                });
            }

            [TestMethod]
            public void ShouldExecute_a1_b3_h500_S0_t175_m150_jpg()
            {
                AssertExecute("redcanoe_p30_t30_out.jpg", nameof(ShouldExecute_a1_b3_h500_S0_t175_m150_jpg), UnperspectiveMethod.Derivative, (UnperspectiveScript<ushort> script) =>
                {
                    script.AspectRatio = 1;
                    script.Blur = 3;
                    script.Height = 500;
                    script.Smooth = 0;
                    script.Threshold = 175;
                    script.MaxPeaks = 150;
                });
            }

            private static void AssertMinLength(int rotation, int minLength)
            {
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory);
                script.MinLength = minLength;

                var inputFile = GetInputFile("monet2_p30_t30_r30_out.jpg");
                using (var image = new MagickImage(inputFile))
                {
                    image.Rotate(rotation);

                    ExceptionAssert.Throws<InvalidOperationException>("Unable to continue, the edge length is less than 40.", () =>
                    {
                        script.Execute(image);
                    });
                }
            }

            private static void AssertInvalidOperation(string expectedMessage, Action<UnperspectiveScript<ushort>> initAction)
            {
                AssertInvalidOperation(expectedMessage, UnperspectiveMethod.Peak, initAction);
            }

            private static void AssertInvalidOperation(string expectedMessage, UnperspectiveMethod method, Action<UnperspectiveScript<ushort>> initAction)
            {
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory, method);

                using (var logo = new MagickImage(Images.Logo))
                {
                    initAction(script);

                    ExceptionAssert.Throws<InvalidOperationException>(expectedMessage, () =>
                    {
                        script.Execute(logo);
                    });
                }
            }

            private void AssertExecute(string input, string methodName, Action<UnperspectiveScript<ushort>> action)
            {
                AssertExecute(input, methodName, UnperspectiveMethod.Peak, action);
            }

            private void AssertExecute(string input, string methodName, UnperspectiveMethod method, Action<UnperspectiveScript<ushort>> action)
            {
                var inputFile = GetInputFile(input);
                /* LosslessCompress(input); */

                using (var image = new MagickImage(inputFile))
                {
                    var factory = new MagickFactory();
                    var script = new UnperspectiveScript<ushort>(factory, method);
                    action(script);

                    using (var scriptOutput = script.Execute(image))
                    {
                        string outputFile = GetOutputFile(inputFile, methodName);
                        AssertOutput(scriptOutput, outputFile);
                    }
                }
            }
        }
    }
}
