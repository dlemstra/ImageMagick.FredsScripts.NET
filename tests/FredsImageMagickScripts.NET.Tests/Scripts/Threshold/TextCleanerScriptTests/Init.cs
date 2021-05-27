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
    public partial class TextCleanerScriptTests : ScriptTester
    {
        private static void AssertDefaults(TextCleanerScript<ushort> script)
        {
            Assert.AreEqual(0.0, script.AdaptiveBlur);
            ColorAssert.AreEqual(new MagickColor("white"), script.BackgroundColor);
            Assert.AreEqual(0, script.CropOffset.Left);
            Assert.AreEqual(0, script.CropOffset.Top);
            Assert.AreEqual(0, script.CropOffset.Right);
            Assert.AreEqual(0, script.CropOffset.Bottom);
            Assert.AreEqual(TextCleanerEnhance.Stretch, script.Enhance);
            Assert.AreEqual((Percentage)5, script.FilterOffset);
            Assert.AreEqual(15, script.FilterSize);
            Assert.AreEqual(TextCleanerLayout.Portrait, script.Layout);
            Assert.AreEqual(false, script.MakeGray);
            Assert.AreEqual(0, script.Padding);
            Assert.AreEqual(TextCleanerRotation.None, script.Rotation);
            Assert.AreEqual((Percentage)200, script.Saturation);
            Assert.AreEqual(0.0, script.Sharpen);
            Assert.AreEqual(null, script.SmoothingThreshold);
            Assert.AreEqual(false, script.Trim);
            Assert.AreEqual(false, script.Unrotate);
        }
    }
}
