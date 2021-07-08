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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public partial class TshirtScriptTests : ScriptTester
    {
        private readonly PointD _topLeft = new PointD(10, 15);
        private readonly PointD _topRight = new PointD(630, 10);
        private readonly PointD _bottomLeft = new PointD(25, 470);
        private readonly PointD _bottomRight = new PointD(630, 470);

        private static void AssertDefaults(TshirtScript<ushort> script)
        {
            Assert.AreEqual(2.0, script.AntiAlias);
            Assert.AreEqual(1.0, script.Blur);
            Assert.AreEqual(10, script.Displace);
            Assert.AreEqual(TshirtFit.None, script.Fit);
            Assert.AreEqual(Gravity.Center, script.Gravity);
            Assert.AreEqual(20, script.Lighting);
            Assert.AreEqual(0, script.Rotation);
            Assert.AreEqual(1.0, script.Sharpen);
            Assert.AreEqual(0, script.VerticalShift);
        }
    }
}
