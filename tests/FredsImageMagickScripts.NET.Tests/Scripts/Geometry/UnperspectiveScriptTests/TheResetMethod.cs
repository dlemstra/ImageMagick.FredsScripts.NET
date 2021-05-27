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

using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests
{
    public partial class UnperspectiveScriptTests
    {
        [TestClass]
        public class TheResetMethod : UnperspectiveScriptTests
        {
            [TestMethod]
            public void ShouldSetValueToTheDefaultsForPeakMethod()
            {
                var method = UnperspectiveMethod.Peak;
                var script = CreateUnperspectiveScript(method);

                script.Reset();

                AssertDefaults(script, method);
            }

            [TestMethod]
            public void ShouldSetValueToTheDefaultsForDerivativeMethod()
            {
                var method = UnperspectiveMethod.Derivative;
                var script = CreateUnperspectiveScript(method);

                script.Reset();

                AssertDefaults(script, method);
            }

            private static UnperspectiveScript<ushort> CreateUnperspectiveScript(UnperspectiveMethod method)
            {
                var factory = new MagickFactory();
                return new UnperspectiveScript<ushort>(factory, method)
                {
                    AspectRatio = 1.5,
                    BorderColorLocation = new PointD(10, 10),
                    Blur = 15.0,
                    ColorFuzz = (Percentage)5.0,
                    Default = UnperspectiveDefault.BoundingBoxWidth,
                    DisableViewportCrop = true,
                    Height = 140,
                    MaxPeaks = 25,
                    MinLength = 5,
                    Rotation = UnperspectiveRotation.Rotate180,
                    Sharpen = 8.5,
                    Smooth = 2.0,
                    Threshold = 5,
                    Width = 50,
                };
            }
        }
    }
}
