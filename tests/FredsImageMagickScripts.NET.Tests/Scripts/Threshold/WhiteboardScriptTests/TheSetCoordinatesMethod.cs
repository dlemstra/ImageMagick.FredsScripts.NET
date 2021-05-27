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
    public partial class WhiteboardScriptTests
    {
        [TestClass]
        public class TheSetCoordinatesMethod : WhiteboardScriptTests
        {
            [TestMethod]
            public void ShouldThrowExceptionForInvalidCoordinates()
            {
                var topLeft = new PointD(10, 10);
                var topRight = new PointD(630, 10);
                var bottomLeft = new PointD(10, 470);
                var bottomRight = new PointD(630, 470);

                var invalid = new PointD[]
                {
                    new PointD(-10, 10), new PointD(10, -10),
                    new PointD(650, 10), new PointD(630, 490),
                };

                for (int i = 0; i < invalid.Length; i++)
                {
                    AssertSetCoordinates("topLeft", invalid[i], topRight, bottomLeft, bottomRight);
                    AssertSetCoordinates("topRight", topLeft, invalid[i], bottomLeft, bottomRight);
                    AssertSetCoordinates("bottomRight", topLeft, topRight, invalid[i], bottomRight);
                    AssertSetCoordinates("bottomLeft", topLeft, topRight, bottomLeft, invalid[i]);
                }
            }

            private static void AssertSetCoordinates(string paramName, PointD topLeft, PointD topRight, PointD bottomLeft, PointD bottomRight)
            {
                ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>(paramName, () =>
                {
                    using (var logo = Images.Logo)
                    {
                        var factory = new MagickFactory();
                        var script = new WhiteboardScript<ushort>(factory);
                        script.SetCoordinates(topLeft, topRight, bottomLeft, bottomRight);
                        script.Execute(logo);
                    }
                });
            }
        }
    }
}
