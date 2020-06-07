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

using System.Reflection;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests
{
    public partial class UnperspectiveScriptTests
    {
        [TestClass]
        public class TheGetRotationMethod
        {
            [TestMethod]
            public void ShouldReturnCorrectRotationForGeometry10x10()
            {
                ShouldReturnCorrectRotationForGeometry10x10(4, 6, UnperspectiveRotation.Rotate270);
                ShouldReturnCorrectRotationForGeometry10x10(6, 6, UnperspectiveRotation.Rotate180);
                ShouldReturnCorrectRotationForGeometry10x10(6, 4, UnperspectiveRotation.Rotate90);
                ShouldReturnCorrectRotationForGeometry10x10(4, 4, UnperspectiveRotation.None);
            }

            private static void ShouldReturnCorrectRotationForGeometry10x10(int x, int y, UnperspectiveRotation expectedRotation)
            {
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory);

                var type = script.GetType();
                var method = type.GetMethod("GetRotation", BindingFlags.Instance | BindingFlags.NonPublic);

                using (MagickImage image = new MagickImage(MagickColors.Fuchsia, 10, 10))
                {
                    var result = method.Invoke(script, new object[] { new PointD[] { new PointD(x, y) }, image });
                    Assert.AreEqual(expectedRotation, result);
                }
            }
        }
    }
}
