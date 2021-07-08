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
    public partial class TshirtScriptTests
    {
        [TestClass]
        public class TheExecuteMethod : TshirtScriptTests
        {
            [TestMethod]
            public void ShouldThrowExceptionWhenTshirtIsNull()
            {
                var factory = new MagickFactory();
                var script = new TshirtScript<ushort>(factory);

                var overlay = new MagickImage();
                ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("tshirt", () => script.Execute(null, overlay));
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenOverlayIsNull()
            {
                var factory = new MagickFactory();
                var script = new TshirtScript<ushort>(factory);

                var input = new MagickImage();
                ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("overlay", () => script.Execute(input, null));
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenBlurBelowZero()
            {
                AssertInvalidOperation("Invalid blur specified, value should be zero or higher.", (TshirtScript<ushort> script) =>
                {
                    script.Blur = -1;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenGravityInvalidValue()
            {
                AssertInvalidOperation("Invalid gravity specified.", (TshirtScript<ushort> script) =>
                {
                    script.Gravity = Gravity.Forget;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenLightingBelowZero()
            {
                AssertInvalidOperation("Invalid lighting specified, value must be between 0 and 30.", (TshirtScript<ushort> script) =>
                {
                    script.Lighting = -1;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenLightingAbove30()
            {
                AssertInvalidOperation("Invalid lighting specified, value must be between 0 and 30.", (TshirtScript<ushort> script) =>
                {
                    script.Lighting = 31;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenRotationBelowMinus360()
            {
                AssertInvalidOperation("Invalid rotation specified, value must be between -360 and 360.", (TshirtScript<ushort> script) =>
                {
                    script.Rotation = -361;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenRotationAboveMinus360()
            {
                AssertInvalidOperation("Invalid rotation specified, value must be between -360 and 360.", (TshirtScript<ushort> script) =>
                {
                    script.Rotation = 361;
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenSetCoordinatesGeometryNull()
            {
                ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("geometry", () =>
                {
                    var factory = new MagickFactory();
                    var script = new TshirtScript<ushort>(factory);
                    script.SetCoordinates(null);
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenNoCoordinatesSet()
            {
                ExceptionAssert.Throws<InvalidOperationException>("No coordinates have been set.", () =>
                {
                    using (var logo = Images.Logo)
                    {
                        var factory = new MagickFactory();
                        var script = new TshirtScript<ushort>(factory);
                        script.Execute(logo, logo);
                    }
                });
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenInvalidCoordinates()
            {
                var invalid = new PointD[]
                {
                    new PointD(-10, 10), new PointD(10, -10),
                    new PointD(650, 10), new PointD(630, 490),
                };

                for (int i = 0; i < invalid.Length; i++)
                {
                    AssertCoordinates("topLeft", invalid[i], _topRight, _bottomLeft, _bottomRight);
                    AssertCoordinates("topRight", _topLeft, invalid[i], _bottomLeft, _bottomRight);
                    AssertCoordinates("bottomRight", _topLeft, _topRight, invalid[i], _bottomRight);
                    AssertCoordinates("bottomLeft", _topLeft, _topRight, _bottomLeft, invalid[i]);
                }
            }

            [TestMethod]
            public void ShouldRestoreAlphaChannelForImageWithAlpha()
            {
                var tshirtFile = GetInputFile("tshirt_gray.jpg");

                var overlayFile = GetInputFile("flowers_van_gogh.jpg");

                using (var tshirtImage = new MagickImage(tshirtFile))
                {
                    tshirtImage.Opaque(new MagickColor("#FFFFFF"), MagickColors.None);

                    using (var overlayImage = new MagickImage(overlayFile))
                    {
                        var factory = new MagickFactory();
                        var script = new TshirtScript<ushort>(factory)
                        {
                            Fit = TshirtFit.Crop,
                            Blur = 0,
                            Lighting = 0,
                            Sharpen = 0,
                        };

                        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));

                        using (var scriptOutput = script.Execute(tshirtImage, overlayImage))
                        {
                            Assert.IsTrue(scriptOutput.HasAlpha);
                            AssertOutput(scriptOutput, "flowers_van_gogh_alpha.jpg");
                        }
                    }
                }
            }

            [TestMethod]
            public void ShouldExecute_blue_jpg()
            {
                AssertExecute("tshirt_blue.jpg", "flowers_van_gogh.jpg", nameof(ShouldExecute_blue_jpg), (TshirtScript<ushort> script) =>
                {
                    script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
                });
            }

            [TestMethod]
            public void ShouldExecute_gray_jpg()
            {
                AssertExecute("tshirt_gray.jpg", "flowers_van_gogh.jpg", nameof(ShouldExecute_gray_jpg), (TshirtScript<ushort> script) =>
                {
                    var topLeft = new PointD(275, 175);
                    var topRight = new PointD(404, 175);
                    var bottomRight = new PointD(404, 304);
                    var bottomLeft = new PointD(275, 304);

                    script.SetCoordinates(topLeft, topRight, bottomRight, bottomLeft);
                });

                AssertExecute("tshirt_gray.jpg", "flowers_van_gogh.jpg", nameof(ShouldExecute_gray_jpg), (TshirtScript<ushort> script) =>
                {
                    script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
                });
            }

            [TestMethod]
            public void ShouldExecute_gray_fc_gn_jpg()
            {
                AssertExecute("tshirt_gray.jpg", "flowers_van_gogh.jpg", nameof(ShouldExecute_gray_fc_gn_jpg), (TshirtScript<ushort> script) =>
                {
                    script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
                    script.Fit = TshirtFit.Crop;
                    script.Gravity = Gravity.North;
                });
            }

            [TestMethod]
            public void ShouldExecute_gray_fc_gc_jpg()
            {
                AssertExecute("tshirt_gray.jpg", "flowers_van_gogh.jpg", nameof(ShouldExecute_gray_fc_gc_jpg), (TshirtScript<ushort> script) =>
                {
                    script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
                    script.Fit = TshirtFit.Crop;
                    script.Gravity = Gravity.Center;
                });
            }

            [TestMethod]
            public void ShouldExecute_gray_fd_jpg()
            {
                AssertExecute("tshirt_gray.jpg", "flowers_van_gogh.jpg", nameof(ShouldExecute_gray_fd_jpg), (TshirtScript<ushort> script) =>
                {
                    script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
                    script.Fit = TshirtFit.Distort;
                });
            }

            [TestMethod]
            public void ShouldExecute_gray_rm3_jpg()
            {
                AssertExecute("tshirt_gray.jpg", "flowers_van_gogh.jpg", nameof(ShouldExecute_gray_rm3_jpg), (TshirtScript<ushort> script) =>
                {
                    script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
                    script.Rotation = -3;
                });

                AssertExecute("tshirt_gray.jpg", "Super_Mario.png", nameof(ShouldExecute_gray_rm3_jpg), (TshirtScript<ushort> script) =>
                {
                    script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
                    script.Rotation = -3;
                });
            }

            private static void AssertCoordinates(string paramName, PointD topLeft, PointD topRight, PointD bottomLeft, PointD bottomRight)
            {
                ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>(paramName, () =>
                {
                    using (MagickImage logo = Images.Logo)
                    {
                        var factory = new MagickFactory();
                        var script = new TshirtScript<ushort>(factory);
                        script.SetCoordinates(topLeft, topRight, bottomLeft, bottomRight);
                        script.Execute(logo, logo);
                    }
                });
            }

            private void AssertInvalidOperation(string expectedMessage, Action<TshirtScript<ushort>> initAction)
            {
                var factory = new MagickFactory();
                var script = new TshirtScript<ushort>(factory);

                using (var logo = new MagickImage(Images.Logo))
                {
                    initAction(script);

                    ExceptionAssert.Throws<InvalidOperationException>(expectedMessage, () =>
                    {
                        script.SetCoordinates(_topLeft, _topRight, _bottomLeft, _bottomRight);
                        script.Execute(logo, logo);
                    });
                }
            }

            private void AssertExecute(string tshirt, string overlay, string methodName, Action<TshirtScript<ushort>> action)
            {
                var tshirtFile = GetInputFile(tshirt);
                /* LosslessCompress(tshirtFile); */

                var overlayFile = GetInputFile(overlay);
                /* LosslessCompress(overlayFile); */

                using (var tshirtImage = new MagickImage(tshirtFile))
                {
                    using (var overlayImage = new MagickImage(overlayFile))
                    {
                        var factory = new MagickFactory();
                        var script = new TshirtScript<ushort>(factory);
                        action(script);

                        using (var scriptOutput = script.Execute(tshirtImage, overlayImage))
                        {
                            string outputFile = GetOutputFile(overlay, methodName);
                            AssertOutput(scriptOutput, outputFile);
                        }
                    }
                }
            }
        }
    }
}
