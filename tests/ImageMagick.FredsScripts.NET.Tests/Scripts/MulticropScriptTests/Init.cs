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
    public partial class MulticropScriptTests : ScriptTester
    {
        private static void AssertDefaults(MulticropScript<ushort> script)
        {
            Assert.Null(script.BackgroundColor);
            Assert.Equal((Percentage)10, script.ColorFuzz);
            Assert.Equal(new PointD(0), script.Coords);
            Assert.Equal(0, script.Discard);
            Assert.Equal(0, script.Extend);
            Assert.Equal((Percentage)10, script.Grid);
            Assert.False(script.Innertrim);
            Assert.Equal(0, script.Prune);
            Assert.Equal("DeskewImage", script.UnrotateMethod.Method.Name);
            Assert.Equal("MulticropScript`1", script.UnrotateMethod.Method.DeclaringType.Name);
        }
    }
}
