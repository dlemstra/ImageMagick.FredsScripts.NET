﻿// Copyright 2015-2020 Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/FredsImageMagickScripts.NET)
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
    public partial class CartoonScriptTests
    {
        [TestClass]
        public class TheResetMethod : CartoonScriptTests
        {
            [TestMethod]
            public void ShouldSetValueToTheDefaults()
            {
                var script = new CartoonScript<ushort>
                {
                    Brightness = (Percentage)42,
                    EdgeAmount = 5,
                    NumberOflevels = 8,
                    Pattern = (Percentage)42,
                    Saturation = (Percentage)42,
                };

                script.Reset();

                AssertDefaults(script);
            }
        }
    }
}