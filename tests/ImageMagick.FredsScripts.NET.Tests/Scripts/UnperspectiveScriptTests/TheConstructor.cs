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

using System;
using Xunit;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public partial class UnperspectiveScriptTests
    {
        public class TheConstructor : UnperspectiveScriptTests
        {
            [Fact]
            public void ShouldThrowExceptionWhenFactoryIsNull()
            {
                Assert.Throws<ArgumentNullException>("factory", () => new UnperspectiveScript<ushort>(null));
            }

            [Fact]
            public void ShouldThrowExceptionForInvalidUnperspectiveMethod()
            {
                var exception = Assert.Throws<ArgumentException>("method", () =>
                {
                    var factory = new MagickFactory();
                    var script = new UnperspectiveScript<ushort>(factory, (UnperspectiveMethod)42);
                });

                Assert.Contains("Invalid unperspective method specified.", exception.Message);
            }

            [Fact]
            public void ShouldSetTheDefaults()
            {
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory);

                AssertDefaults(script, UnperspectiveMethod.Peak);
            }

            [Fact]
            public void ShouldSetTheDefaultsForPeakMethod()
            {
                var method = UnperspectiveMethod.Peak;
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory, method);

                AssertDefaults(script, method);
            }

            [Fact]
            public void ShouldSetTheDefaultsForDerivativeMethod()
            {
                var method = UnperspectiveMethod.Derivative;
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory, method);

                AssertDefaults(script, method);
            }
        }
    }
}
