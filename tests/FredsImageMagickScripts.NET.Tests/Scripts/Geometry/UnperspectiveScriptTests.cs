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
using System.Reflection;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests
{
    [TestClass]
    public class UnperspectiveScriptTests : ScriptTester
    {
        [TestMethod]
        public void Constructor_Peak_SettingsSetToDefaults()
        {
            Constructor_SettingsSetToDefaults(UnperspectiveMethod.Peak);
        }

        [TestMethod]
        public void Constructor_Derivative_SettingsSetToDefaults()
        {
            Constructor_SettingsSetToDefaults(UnperspectiveMethod.Derivative);
        }

        [TestMethod]
        public void Constructor_InvalidUnperspectiveMethod_ThrowsException()
        {
            ExceptionAssert.ThrowsArgumentException<ArgumentException>("method", "Invalid unperspective method specified.", () =>
            {
                new UnperspectiveScript((UnperspectiveMethod)42);
            });
        }

        [TestMethod]
        public void Reset_ConstructedWithPeakAllSettingsChanged_RestoredToDefault()
        {
            Reset_AllSettingsChanged_RestoredToDefault(UnperspectiveMethod.Peak);
        }

        [TestMethod]
        public void Reset_ConstructedWithDerivativeAllSettingsChanged_RestoredToDefault()
        {
            Reset_AllSettingsChanged_RestoredToDefault(UnperspectiveMethod.Derivative);
        }

        [TestMethod]
        public void BorderColorLocation_BeforeLeftTop_ThrowsException()
        {
            var script = new UnperspectiveScript();

            using (var logo = new MagickImage(Images.Logo))
            {
                script.BorderColorLocation = new PointD(-1, -1);

                // TODO: Fix this after the next release of Magick.NET
                // ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>("x", "Invalid X coordinate: -1", () =>
                ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    script.Execute(logo);
                });
            }
        }

        [TestMethod]
        public void BorderColorLocation_AfterBottomRight_ThrowsException()
        {
            var script = new UnperspectiveScript();

            using (var logo = new MagickImage(Images.Logo))
            {
                script.BorderColorLocation = new PointD(logo.Width, logo.Height);

                // TODO: Fix this after the next release of Magick.NET
                // ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>("x", "Invalid X coordinate: " + logo.Width, () =>
                ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    script.Execute(logo);
                });
            }
        }

        [TestMethod]
        public void Default_InvalidValue_ThrowsException()
        {
            AssertInvalidOperation("Invalid default output dimension specified.", (UnperspectiveScript script) =>
            {
                script.Default = (UnperspectiveDefault)42;
            });
        }

        [TestMethod]
        public void MaxPeaks_ToLow_ThrowsException()
        {
            AssertInvalidOperation("Unable to continue, the number of peaks is higher than 4.", (UnperspectiveScript script) =>
            {
                script.MaxPeaks = 4;
            });
        }

        [TestMethod]
        public void Threshold_Zero_ThrowsException()
        {
            AssertInvalidOperation("Unable to continue, the number of peaks should be 4.", (UnperspectiveScript script) =>
            {
                script.Threshold = 0;
            });
        }

        [TestMethod]
        public void Threshold_ToHigh_ThrowsException()
        {
            AssertInvalidOperation("Unable to continue, the number of peaks should be 4.", (UnperspectiveScript script) =>
            {
                script.Threshold = 20000;
            });

            AssertInvalidOperation("Unable to continue, the number of peaks should be 4.", UnperspectiveMethod.Derivative, (UnperspectiveScript script) =>
            {
                script.Threshold = 20000;
            });
        }

        [TestMethod]
        public void WidthHeight_BothSet_ThrowsException()
        {
            AssertInvalidOperation("Both width and height cannot be specified at the same time.", (UnperspectiveScript script) =>
            {
                script.Width = 500;
                script.Height = 500;
            });
        }

        [TestMethod]
        public void MinLength_ValueToHigh_ThrowsException()
        {
            AssertMinLength(0, 209);
            AssertMinLength(0, 276);
            AssertMinLength(25, 354);
            AssertMinLength(25, 427);
        }

        [TestMethod]
        public void GetCoefficients_UnsolvableMatrix_ThrowsException()
        {
            GetCoefficients_UnsolvableMatrix_ThrowsException(new double[] { 1, 2, 3, 4 });
            GetCoefficients_UnsolvableMatrix_ThrowsException(new double[] { -4, double.NaN, double.NaN, double.NaN });
        }

        [TestMethod]
        public void GetDimensions_UnperspectiveEdgeLength_ReturnsCorrectGeometry()
        {
            MagickGeometry geometry = GetDimensions(UnperspectiveDefault.EdgeLength);

            Assert.AreEqual(new MagickGeometry(1006, 670), geometry);
        }

        [TestMethod]
        public void GetDimensions_UnperspectiveBoundingBoxHeight_ReturnsCorrectGeometry()
        {
            MagickGeometry geometry = GetDimensions(UnperspectiveDefault.BoundingBoxHeight);

            Assert.AreEqual(new MagickGeometry(1200, 800), geometry);
        }

        [TestMethod]
        public void GetDimensions_UnperspectiveBoundingBoxWidth_ReturnsCorrectGeometry()
        {
            MagickGeometry geometry = GetDimensions(UnperspectiveDefault.BoundingBoxWidth);

            Assert.AreEqual(new MagickGeometry(800, 533), geometry);
        }

        [TestMethod]
        public void GetDimensions_UnperspectiveHeight_ReturnsCorrectGeometry()
        {
            MagickGeometry geometry = GetDimensions(UnperspectiveDefault.Height);

            Assert.AreEqual(new MagickGeometry(1500, 1000), geometry);
        }

        [TestMethod]
        public void GetDimensions_UnperspectiveWidth_ReturnsCorrectGeometry()
        {
            MagickGeometry geometry = GetDimensions(UnperspectiveDefault.Width);

            Assert.AreEqual(new MagickGeometry(1000, 666), geometry);
        }

        [TestMethod]
        public void GetRotation_Geometry10x10_ReturnsCorrectRotation()
        {
            GetRotation_Geometry10x10_ReturnsCorrectRotation(4, 6, UnperspectiveRotation.Rotate270);
            GetRotation_Geometry10x10_ReturnsCorrectRotation(6, 6, UnperspectiveRotation.Rotate180);
            GetRotation_Geometry10x10_ReturnsCorrectRotation(6, 4, UnperspectiveRotation.Rotate90);
            GetRotation_Geometry10x10_ReturnsCorrectRotation(4, 4, UnperspectiveRotation.None);
        }

        [TestMethod]
        public void PerceptibleReciprocal_NegativeNan_ReturnsPositiveInfinity()
        {
            var script = new UnperspectiveScript();

            Type type = script.GetType();
            MethodInfo method = type.GetMethod("PerceptibleReciprocal", BindingFlags.Static | BindingFlags.NonPublic);

            var result = method.Invoke(null, new object[] { -double.NaN });
            Assert.AreEqual(result, double.PositiveInfinity);
        }

        [TestMethod]
        public void Execute_InputNull_ThrowsException()
        {
            ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () =>
            {
                var script = new UnperspectiveScript();
                script.Execute(null);
            });
        }

        [TestMethod]
        public void Execute_f20_jpg()
        {
            AssertExecute("mandril2_p30_t30_out.jpg", nameof(Execute_f20_jpg), (UnperspectiveScript script) =>
             {
                 script.ColorFuzz = (Percentage)20;
             });

            AssertExecute("mandril2_p30_t30_r60_zc.jpg", nameof(Execute_f20_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)20;
            });

            AssertExecute("mandril2_pm30_t30_r30_zc.jpg", nameof(Execute_f20_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)20;
            });
        }

        [TestMethod]
        public void Execute_f20_r270_jpg()
        {
            AssertExecute("mandril2_p30_t30_r60_zc.jpg", nameof(Execute_f20_r270_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)20;
                script.Rotation = UnperspectiveRotation.Rotate270;
            });
        }

        [TestMethod]
        public void Execute_f52_t8_s2_jpg()
        {
            AssertExecute("mandril2_pm30_t30_r30_zc.jpg", nameof(Execute_f52_t8_s2_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)52;
                script.Threshold = 8;
                script.Smooth = 2;
            });
        }

        [TestMethod]
        public void Execute_f15_bh_jpg()
        {
            AssertExecute("mandril2_pm30_t30_r30_zc.jpg", nameof(Execute_f15_bh_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)15;
                script.Default = UnperspectiveDefault.BoundingBoxHeight;
            });
        }

        [TestMethod]
        public void Execute_f20_h_jpg()
        {
            AssertExecute("mandril2_pm30_t30_r30_zc.jpg", nameof(Execute_f20_h_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)20;
                script.Default = UnperspectiveDefault.Height;
            });
        }

        [TestMethod]
        public void Execute_f20_s2_vc_jpg()
        {
            AssertExecute("mandril2_round30_p30_t30_out.jpg", nameof(Execute_f20_s2_vc_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)20;
                script.Smooth = 2;
                script.DisableViewportCrop = true;
            });
        }

        [TestMethod]
        public void Execute_f10_jpg()
        {
            AssertExecute("monet2_p30_t30_r30_out.jpg", nameof(Execute_f10_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)10;
            });

            AssertExecute("redcanoe_p30_t30_out.jpg", nameof(Execute_f10_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)10;
            });
        }

        [TestMethod]
        public void Execute_f7_vc_jpg()
        {
            AssertExecute("receipt1.jpg", nameof(Execute_f7_vc_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)7;
                script.DisableViewportCrop = true;
            });
        }

        [TestMethod]
        public void Execute_f20_vc_jpg()
        {
            AssertExecute("receipt1.jpg", nameof(Execute_f20_vc_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)20;
                script.DisableViewportCrop = true;
            });
        }

        [TestMethod]
        public void Execute_f20_vc_w500_jpg()
        {
            AssertExecute("receipt1.jpg", nameof(Execute_f20_vc_w500_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)20;
                script.Width = 500;
                script.DisableViewportCrop = true;
            });
        }

        [TestMethod]
        public void Execute_f50_jpg()
        {
            AssertExecute("receipt2.jpg", nameof(Execute_f50_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)50;
            });
        }

        [TestMethod]
        public void Execute_f50_w200_jpg()
        {
            AssertExecute("receipt2.jpg", nameof(Execute_f50_w200_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)50;
                script.Width = 200;
            });
        }

        [TestMethod]
        public void Execute_f11_t10_s2_S0_B3_jpg()
        {
            AssertExecute("textsample_localthresh_m1_r25_b5_white_b20_p30_t30_out.png", nameof(Execute_f11_t10_s2_S0_B3_jpg), (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)11;
                script.Threshold = 10;
                script.Smooth = 2;
                script.Sharpen = 0;
                script.Blur = 3;
            });
        }

        [TestMethod]
        public void Execute_f10_derivative_jpg()
        {
            AssertExecute("redcanoe_p30_t30_out.jpg", nameof(Execute_f10_derivative_jpg), UnperspectiveMethod.Derivative, (UnperspectiveScript script) =>
            {
                script.ColorFuzz = (Percentage)10;
            });
        }

        [TestMethod]
        public void Execute_a1_b3_h500_S0_t175_m150_jpg()
        {
            AssertExecute("redcanoe_p30_t30_out.jpg", nameof(Execute_a1_b3_h500_S0_t175_m150_jpg), UnperspectiveMethod.Derivative, (UnperspectiveScript script) =>
            {
                script.AspectRatio = 1;
                script.Blur = 3;
                script.Height = 500;
                script.Smooth = 0;
                script.Threshold = 175;
                script.MaxPeaks = 150;
            });
        }

        private static void AssertDefaults(UnperspectiveScript script, UnperspectiveMethod method)
        {
            Assert.AreEqual(null, script.AspectRatio);
            Assert.AreEqual(0, script.BorderColorLocation.X);
            Assert.AreEqual(0, script.BorderColorLocation.Y);
            Assert.AreEqual(0, script.Blur);
            Assert.AreEqual(UnperspectiveDefault.EdgeLength, script.Default);
            Assert.AreEqual(false, script.DisableViewportCrop);
            Assert.AreEqual(null, script.Height);
            Assert.AreEqual(10, script.MinLength);
            Assert.AreEqual(40, script.MaxPeaks);
            Assert.AreEqual(null, script.Rotation);
            Assert.AreEqual(null, script.Width);

            if (method == UnperspectiveMethod.Peak)
            {
                Assert.AreEqual(5.0, script.Sharpen);
                Assert.AreEqual(1.0, script.Smooth);
                Assert.AreEqual(4, script.Threshold);
            }
            else
            {
                Assert.AreEqual(0.0, script.Sharpen);
                Assert.AreEqual(5.0, script.Smooth);
                Assert.AreEqual(10, script.Threshold);
            }
        }

        private static void AssertInvalidOperation(string expectedMessage, Action<UnperspectiveScript> initAction)
        {
            AssertInvalidOperation(expectedMessage, UnperspectiveMethod.Peak, initAction);
        }

        private static void AssertInvalidOperation(string expectedMessage, UnperspectiveMethod method, Action<UnperspectiveScript> initAction)
        {
            var script = new UnperspectiveScript(method);

            using (var logo = new MagickImage(Images.Logo))
            {
                initAction(script);

                ExceptionAssert.Throws<InvalidOperationException>(expectedMessage, () =>
                {
                    script.Execute(logo);
                });
            }
        }

        private static void AssertMinLength(int rotation, int minLength)
        {
            var script = new UnperspectiveScript();
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

        private static void Constructor_SettingsSetToDefaults(UnperspectiveMethod method)
        {
            var script = new UnperspectiveScript(method);

            AssertDefaults(script, method);
        }

        private static void Reset_AllSettingsChanged_RestoredToDefault(UnperspectiveMethod method)
        {
            var script = new UnperspectiveScript(method);
            script.AspectRatio = 1.5;
            script.BorderColorLocation = new PointD(10, 10);
            script.Blur = 15.0;
            script.ColorFuzz = (Percentage)5.0;
            script.Default = UnperspectiveDefault.BoundingBoxWidth;
            script.DisableViewportCrop = true;
            script.Height = 140;
            script.MaxPeaks = 25;
            script.MinLength = 5;
            script.Rotation = UnperspectiveRotation.Rotate180;
            script.Sharpen = 8.5;
            script.Smooth = 2.0;
            script.Threshold = 5;
            script.Width = 50;

            script.Reset();

            AssertDefaults(script, method);
        }

        private static void GetCoefficients_UnsolvableMatrix_ThrowsException(double[] arguments)
        {
            var script = new UnperspectiveScript();

            Type type = script.GetType();
            MethodInfo method = type.GetMethod("GetCoefficients", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(double[]) }, null);

            ExceptionAssert.Throws<InvalidOperationException>("Unsolvable matrix detected.", () =>
            {
                try
                {
                    method.Invoke(null, new object[] { arguments });
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            });
        }

        private static MagickGeometry GetDimensions(UnperspectiveDefault value)
        {
            var script = new UnperspectiveScript();
            script.AspectRatio = 1.5;
            script.Default = value;

            Type type = script.GetType();
            MethodInfo method = type.GetMethod("GetDimensions", BindingFlags.Instance | BindingFlags.NonPublic);

            PointD[] corners = new PointD[4]
            {
        new PointD(0, 0),
        new PointD(300, 600),
        new PointD(10, 10),
        new PointD(290, 490)
            };
            MagickGeometry inputDimensions = new MagickGeometry(1000, 1000);
            MagickGeometry trimmedDimensions = new MagickGeometry(800, 800);

            return (MagickGeometry)method.Invoke(script, new object[] { null, corners, inputDimensions, trimmedDimensions });
        }

        private static void GetRotation_Geometry10x10_ReturnsCorrectRotation(int x, int y, UnperspectiveRotation expectedRotation)
        {
            var script = new UnperspectiveScript();

            Type type = script.GetType();
            MethodInfo method = type.GetMethod("GetRotation", BindingFlags.Instance | BindingFlags.NonPublic);

            using (MagickImage image = new MagickImage(MagickColors.Fuchsia, 10, 10))
            {
                var result = method.Invoke(script, new object[] { new PointD[] { new PointD(x, y) }, image });
                Assert.AreEqual(expectedRotation, result);
            }
        }

        private void AssertExecute(string input, string methodName, Action<UnperspectiveScript> action)
        {
            AssertExecute(input, methodName, UnperspectiveMethod.Peak, action);
        }

        private void AssertExecute(string input, string methodName, UnperspectiveMethod method, Action<UnperspectiveScript> action)
        {
            var inputFile = GetInputFile(input);
            /* LosslessCompress(input); */

            using (var image = new MagickImage(inputFile))
            {
                var script = new UnperspectiveScript(method);
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