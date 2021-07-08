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
    public partial class WhiteboardScriptTests
    {
        [TestClass]
        public class TheResetMethod : WhiteboardScriptTests
        {
            [TestMethod]
            public void ShouldSetValueToTheDefaults()
            {
                var factory = new MagickFactory();
                var script = new WhiteboardScript<ushort>(factory)
                {
                    BackgroundColor = new MagickColor("purple"),
                    Enhance = WhiteboardEnhancements.None,
                    FilterOffset = (Percentage)5,
                    FilterSize = 10,
                    Saturation = (Percentage)100,
                    WhiteBalance = (Percentage)0.1,
                };

                script.Reset();

                AssertDefaults(script);
            }
        }
    }
}
