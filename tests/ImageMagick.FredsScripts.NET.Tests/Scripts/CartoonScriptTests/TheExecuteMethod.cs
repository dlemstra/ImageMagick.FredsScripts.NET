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
    public partial class CartoonScriptTests
    {
        public class TheExecuteMethod : CartoonScriptTests
        {
            [Fact]
            public void ShouldThrowExceptionWhenInputIsNull()
            {
                Assert.Throws<ArgumentNullException>("input", () =>
                {
                    var script = new CartoonScript<ushort>();
                    script.Execute(null);
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenBrightnessBelowZero()
            {
                AssertInvalidOperation("Invalid brightness specified, value must be zero or higher.", (CartoonScript<ushort> script) =>
                {
                    script.Brightness = (Percentage)(-1);
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenEdgeAmountBelowZero()
            {
                AssertInvalidOperation("Invalid edge amount specified, value must be zero or higher.", (CartoonScript<ushort> script) =>
                {
                    script.EdgeAmount = -1;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenEdgeAmountIsNan()
            {
                AssertInvalidOperation("Invalid edge amount specified, value must be zero or higher.", (CartoonScript<ushort> script) =>
                {
                    script.EdgeAmount = double.NaN;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenEdgeAmountIsNegativeInfinity()
            {
                AssertInvalidOperation("Invalid edge amount specified, value must be zero or higher.", (CartoonScript<ushort> script) =>
                {
                    script.EdgeAmount = double.NegativeInfinity;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenEdgeAmountIsPositiveInfinity()
            {
                AssertInvalidOperation("Invalid edge amount specified, value must be zero or higher.", (CartoonScript<ushort> script) =>
                {
                    script.EdgeAmount = double.PositiveInfinity;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenNumberOflevelsBelowTwo()
            {
                AssertInvalidOperation("Invalid number of levels specified, value must be two or higher.", (CartoonScript<ushort> script) =>
                {
                    script.NumberOflevels = 1;
                });
            }

            [Fact]
            public void ShouldThrowExceptionWhenSaturationBelowZero()
            {
                AssertInvalidOperation("Invalid saturation specified, value must be zero or higher.", (CartoonScript<ushort> script) =>
                {
                    script.Saturation = (Percentage)(-1);
                });
            }

            [Fact]
            public void ShouldExecute_p60_jpg()
            {
                AssertExecute("photo-1h.jpg", nameof(ShouldExecute_p60_jpg), (CartoonScript<ushort> script) =>
                {
                    script.Pattern = new Percentage(60);
                });
            }

            [Fact]
            public void ShouldExecute_p70_jpg()
            {
                AssertExecute("photo-1h.jpg", nameof(ShouldExecute_p70_jpg), (CartoonScript<ushort> script) =>
                {
                    script.Pattern = new Percentage(70);
                });
            }

            [Fact]
            public void ShouldExecute_p80_jpg()
            {
                AssertExecute("photo-1h.jpg", nameof(ShouldExecute_p80_jpg), (CartoonScript<ushort> script) =>
                {
                    script.Pattern = new Percentage(80);
                });
            }

            [Fact]
            public void ShouldExecute_n4_p70_jpg()
            {
                AssertExecute("photo-1h.jpg", nameof(ShouldExecute_n4_p70_jpg), (CartoonScript<ushort> script) =>
                {
                    script.NumberOflevels = 4;
                    script.Pattern = new Percentage(70);
                });
            }

            [Fact]
            public void ShouldExecute_n5_p70_jpg()
            {
                AssertExecute("photo-1h.jpg", nameof(ShouldExecute_n5_p70_jpg), (CartoonScript<ushort> script) =>
                {
                    script.NumberOflevels = 5;
                    script.Pattern = new Percentage(70);
                });
            }

            [Fact]
            public void ShouldExecute_e3_p70_jpg()
            {
                AssertExecute("photo-1h.jpg", nameof(ShouldExecute_e3_p70_jpg), (CartoonScript<ushort> script) =>
                {
                    script.EdgeAmount = 3;
                    script.Pattern = new Percentage(70);
                });
            }

            [Fact]
            public void ShouldExecute_e5_p70_jpg()
            {
                AssertExecute("photo-1h.jpg", nameof(ShouldExecute_e5_p70_jpg), (CartoonScript<ushort> script) =>
                {
                    script.EdgeAmount = 5;
                    script.Pattern = new Percentage(70);
                });
            }

            [Fact]
            public void ShouldExecute_m2_jpg()
            {
                AssertExecute("photo-1h.jpg", nameof(ShouldExecute_m2_jpg), CartoonMethod.Method2, (CartoonScript<ushort> script) =>
                {
                });
            }

            [Fact]
            public void ShouldExecute_m3_jpg()
            {
                AssertExecute("photo-1h.jpg", nameof(ShouldExecute_m3_jpg), CartoonMethod.Method3, (CartoonScript<ushort> script) =>
                {
                });
            }

            [Fact]
            public void ShouldExecute_m4_jpg()
            {
                AssertExecute("photo-1h.jpg", nameof(ShouldExecute_m4_jpg), CartoonMethod.Method4, (CartoonScript<ushort> script) =>
                {
                });
            }

            [Fact]
            public void ShouldExecute_e3_n6_p70_jpg()
            {
                AssertExecute("oriental_lady.jpg", nameof(ShouldExecute_e3_n6_p70_jpg), (CartoonScript<ushort> script) =>
                {
                    script.EdgeAmount = 3;
                    script.NumberOflevels = 6;
                    script.Pattern = new Percentage(70);
                });
            }

            [Fact]
            public void ShouldExecute_b120_e3_p80_jpg()
            {
                AssertExecute("redhat.jpg", nameof(ShouldExecute_b120_e3_p80_jpg), (CartoonScript<ushort> script) =>
                {
                    script.Brightness = new Percentage(120);
                    script.EdgeAmount = 3;
                    script.Pattern = new Percentage(80);
                });
            }

            [Fact]
            public void ShouldExecute_m1_jpg()
            {
                AssertExecute("obama.jpg", nameof(ShouldExecute_m1_jpg), (CartoonScript<ushort> script) =>
                {
                });
            }

            private static void AssertInvalidOperation(string expectedMessage, Action<CartoonScript<ushort>> initAction)
            {
                var script = new CartoonScript<ushort>();

                using (var logo = new MagickImage(Images.Logo))
                {
                    initAction(script);

                    var exception = Assert.Throws<InvalidOperationException>(() =>
                    {
                        script.Execute(logo);
                    });

                    Assert.Contains(expectedMessage, exception.Message);
                }
            }

            private void AssertExecute(string input, string methodName, Action<CartoonScript<ushort>> action)
                => AssertExecute(input, methodName, CartoonMethod.Method1, action);

            private void AssertExecute(string input, string methodName, CartoonMethod method, Action<CartoonScript<ushort>> action)
            {
                var inputFile = GetInputFile(input);

                using (var image = new MagickImage(inputFile))
                {
                    var script = new CartoonScript<ushort>(method);
                    action(script);

                    using (var scriptOutput = script.Execute(image))
                    {
                        var outputFile = GetOutputFile(input, methodName);
                        AssertOutput(scriptOutput, outputFile);
                    }
                }
            }
        }
    }
}
