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
    public partial class UnperspectiveScriptTests : ScriptTester
    {
        private static void AssertDefaults(UnperspectiveScript<ushort> script, UnperspectiveMethod method)
        {
            Assert.Null(script.AspectRatio);
            Assert.Equal(0, script.BorderColorLocation.X);
            Assert.Equal(0, script.BorderColorLocation.Y);
            Assert.Equal(0, script.Blur);
            Assert.Equal(UnperspectiveDefault.EdgeLength, script.Default);
            Assert.False(script.DisableViewportCrop);
            Assert.Null(script.Height);
            Assert.Equal(10, script.MinLength);
            Assert.Equal(40, script.MaxPeaks);
            Assert.Null(script.Rotation);
            Assert.Null(script.Width);

            if (method == UnperspectiveMethod.Peak)
            {
                Assert.Equal(5.0, script.Sharpen);
                Assert.Equal(1.0, script.Smooth);
                Assert.Equal(4, script.Threshold);
            }
            else
            {
                Assert.Equal(0.0, script.Sharpen);
                Assert.Equal(5.0, script.Smooth);
                Assert.Equal(10, script.Threshold);
            }
        }
    }
}
