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

using Xunit;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public partial class TextCleanerScriptTests : ScriptTester
    {
        private static void AssertDefaults(TextCleanerScript<ushort> script)
        {
            Assert.Equal(0.0, script.AdaptiveBlur);
            ColorAssert.AreEqual(new MagickColor("white"), script.BackgroundColor);
            Assert.Equal(0, script.CropOffset.Left);
            Assert.Equal(0, script.CropOffset.Top);
            Assert.Equal(0, script.CropOffset.Right);
            Assert.Equal(0, script.CropOffset.Bottom);
            Assert.Equal(TextCleanerEnhance.Stretch, script.Enhance);
            Assert.Equal((Percentage)5, script.FilterOffset);
            Assert.Equal(15, script.FilterSize);
            Assert.Equal(TextCleanerLayout.Portrait, script.Layout);
            Assert.False(script.MakeGray);
            Assert.Equal(0, script.Padding);
            Assert.Equal(TextCleanerRotation.None, script.Rotation);
            Assert.Equal((Percentage)200, script.Saturation);
            Assert.Equal(0.0, script.Sharpen);
            Assert.Null(script.SmoothingThreshold);
            Assert.False(script.Trim);
            Assert.False(script.Unrotate);
        }
    }
}
