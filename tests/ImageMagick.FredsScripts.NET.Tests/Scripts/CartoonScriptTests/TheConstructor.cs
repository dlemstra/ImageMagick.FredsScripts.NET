﻿// Copyright Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/ImageMagick.FredsScripts.NET)
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

using System;
using Xunit;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public partial class CartoonScriptTests
    {
        public class TheConstructor : CartoonScriptTests
        {
            [Fact]
            public void ShouldSetTheDefaults()
            {
                var script = new CartoonScript<ushort>();

                AssertDefaults(script);
            }

            [Fact]
            public void ShouldThrowExceptionWhenMethodIsInvalid()
            {
                var exception = Assert.Throws<ArgumentException>("method", () =>
                {
                    new CartoonScript<ushort>((CartoonMethod)42);
                });

                Assert.Contains("Invalid cartoon method specified.", exception.Message);
            }
        }
    }
}
