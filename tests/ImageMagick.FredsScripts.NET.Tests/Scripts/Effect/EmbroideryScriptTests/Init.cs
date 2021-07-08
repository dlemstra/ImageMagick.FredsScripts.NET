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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public partial class EmbroideryScriptTests : ScriptTester
    {
        private static void AssertDefaults(EmbroideryScript<ushort> script)
        {
            Assert.AreEqual(0, script.Angle);
            Assert.AreEqual(130, script.Azimuth);
            Assert.AreEqual(null, script.BackgroundColor);
            Assert.AreEqual(4, script.Bevel);
            Assert.AreEqual((Percentage)20, script.ColorFuzz);
            Assert.AreEqual(0, script.Contrast);
            Assert.AreEqual(30.0, script.Elevation);
            Assert.AreEqual(2, script.Extent);
            Assert.AreEqual(20, script.GrayLimit);
            Assert.AreEqual((Percentage)25, script.Intensity);
            Assert.AreEqual(100, script.Mix);
            Assert.AreEqual(8, script.NumberOfColors);
            Assert.AreEqual(EmbroideryPattern.Linear, script.Pattern);
            Assert.AreEqual(90, script.Range);
            Assert.AreEqual(1.0, script.Spread);
            Assert.AreEqual(2, script.Thickness);
        }
    }
}
