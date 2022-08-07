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
    public partial class EmbroideryScriptTests : ScriptTester
    {
        private static void AssertDefaults(EmbroideryScript<ushort> script)
        {
            Assert.Equal(0, script.Angle);
            Assert.Equal(130, script.Azimuth);
            Assert.Null(script.BackgroundColor);
            Assert.Equal(4, script.Bevel);
            Assert.Equal((Percentage)20, script.ColorFuzz);
            Assert.Equal(0, script.Contrast);
            Assert.Equal(30.0, script.Elevation);
            Assert.Equal(2, script.Extent);
            Assert.Equal(20, script.GrayLimit);
            Assert.Equal((Percentage)25, script.Intensity);
            Assert.Equal(100, script.Mix);
            Assert.Equal(8, script.NumberOfColors);
            Assert.Equal(EmbroideryPattern.Linear, script.Pattern);
            Assert.Equal(90, script.Range);
            Assert.Equal(1.0, script.Spread);
            Assert.Equal(2, script.Thickness);
        }
    }
}
