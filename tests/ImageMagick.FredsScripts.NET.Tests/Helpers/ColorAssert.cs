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
using Xunit.Sdk;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public static class ColorAssert
    {
        public static void AreEqual(IMagickColor<ushort> expected, IMagickColor<ushort> actual)
        {
            if (expected == null)
                throw new InvalidOperationException();

            if (actual == null)
                throw new InvalidOperationException();

            Equal(expected.R, actual.R, expected, actual, "R");
            Equal(expected.G, actual.G, expected, actual, "G");
            Equal(expected.B, actual.B, expected, actual, "B");
            Equal(expected.A, actual.A, expected, actual, "A");
        }

        private static void Equal(ushort expected, ushort actual, IMagickColor<ushort> expectedColor, IMagickColor<ushort> actualColor, string channel)
        {
            if (actual < expected || actual > expected)
                throw new XunitException(channel + " is not equal (" + expectedColor.ToString() + " != " + actualColor.ToString() + ") (" + expected + " != " + actual + ")");
        }
    }
}
