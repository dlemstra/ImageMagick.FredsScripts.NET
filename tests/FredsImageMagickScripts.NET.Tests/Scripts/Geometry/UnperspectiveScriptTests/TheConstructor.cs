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

using System;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests
{
    public partial class UnperspectiveScriptTests
    {
        [TestClass]
        public class TheConstructor : UnperspectiveScriptTests
        {
            [TestMethod]
            public void ShouldThrowExceptionWhenFactoryIsNull()
            {
                ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("factory", () => new UnperspectiveScript<ushort>(null));
            }

            [TestMethod]
            public void ShouldThrowExceptionForInvalidUnperspectiveMethod()
            {
                ExceptionAssert.ThrowsArgumentException<ArgumentException>("method", "Invalid unperspective method specified.", () =>
                {
                    var factory = new MagickFactory();
                    var script = new UnperspectiveScript<ushort>(factory, (UnperspectiveMethod)42);
                });
            }

            [TestMethod]
            public void ShouldSetTheDefaults()
            {
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory);

                AssertDefaults(script, UnperspectiveMethod.Peak);
            }

            [TestMethod]
            public void ShouldSetTheDefaultsForPeakMethod()
            {
                var method = UnperspectiveMethod.Peak;
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory, method);

                AssertDefaults(script, method);
            }

            [TestMethod]
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
