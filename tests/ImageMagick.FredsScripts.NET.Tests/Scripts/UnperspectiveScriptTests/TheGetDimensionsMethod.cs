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

using System.Reflection;
using Xunit;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public partial class UnperspectiveScriptTests
    {
        public class TheGetDimensionsMethod
        {
            [Fact]
            public void ShouldReturnCorrectGeometryForUnperspectiveEdgeLength()
            {
                var geometry = GetDimensions(UnperspectiveDefault.EdgeLength);

                Assert.Equal(new MagickGeometry(1006, 670), geometry);
            }

            [Fact]
            public void ShouldReturnCorrectGeometryForUnperspectiveBoundingBoxHeight()
            {
                var geometry = GetDimensions(UnperspectiveDefault.BoundingBoxHeight);

                Assert.Equal(new MagickGeometry(1200, 800), geometry);
            }

            [Fact]
            public void ShouldReturnCorrectGeometryForUnperspectiveBoundingBoxWidth()
            {
                var geometry = GetDimensions(UnperspectiveDefault.BoundingBoxWidth);

                Assert.Equal(new MagickGeometry(800, 533), geometry);
            }

            [Fact]
            public void ShouldReturnCorrectGeometryForUnperspectiveHeight()
            {
                var geometry = GetDimensions(UnperspectiveDefault.Height);

                Assert.Equal(new MagickGeometry(1500, 1000), geometry);
            }

            [Fact]
            public void ShouldReturnCorrectGeometryForUnperspectiveWidth()
            {
                var geometry = GetDimensions(UnperspectiveDefault.Width);

                Assert.Equal(new MagickGeometry(1000, 666), geometry);
            }

            private static MagickGeometry GetDimensions(UnperspectiveDefault value)
            {
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory)
                {
                    AspectRatio = 1.5,
                    Default = value,
                };

                var type = script.GetType();
                var method = type.GetMethod("GetDimensions", BindingFlags.Instance | BindingFlags.NonPublic);

                var corners = new PointD[4]
                {
                    new PointD(0, 0),
                    new PointD(300, 600),
                    new PointD(10, 10),
                    new PointD(290, 490),
                };
                var inputDimensions = new MagickGeometry(1000, 1000);
                var trimmedDimensions = new MagickGeometry(800, 800);

                return (MagickGeometry)method.Invoke(script, new object[] { null, corners, inputDimensions, trimmedDimensions });
            }
        }
    }
}
