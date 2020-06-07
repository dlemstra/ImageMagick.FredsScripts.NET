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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests
{
    public partial class UnperspectiveScriptTests : ScriptTester
    {
        private static void AssertDefaults(UnperspectiveScript<ushort> script, UnperspectiveMethod method)
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
    }
}
