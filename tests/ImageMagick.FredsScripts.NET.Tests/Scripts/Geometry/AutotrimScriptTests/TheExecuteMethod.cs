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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public partial class AutotrimScriptTests
    {
        [TestClass]
        public class TheExecuteMethod : AutotrimScriptTests
        {
            [TestMethod]
            public void ShouldThrowExceptionWhenInputIsNull()
            {
                var factory = new MagickFactory();
                var script = new AutotrimScript<ushort>(factory);

                ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () => script.Execute(null));
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenBorderColorLocationBeforeLeftTop()
            {
                var factory = new MagickFactory();
                var script = new AutotrimScript<ushort>(factory);

                using (var logo = new MagickImage(Images.Logo))
                {
                    script.BorderColorLocation = new PointD(-1, -1);

                    ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>("x", "Invalid X coordinate: -1.", () =>
                    {
                        script.Execute(logo);
                    });
                }
            }

            [TestMethod]
            public void ShouldThrowExceptionWhenBorderColorLocationAfterBottomRight()
            {
                var factory = new MagickFactory();
                var script = new AutotrimScript<ushort>(factory);

                using (var logo = new MagickImage(Images.Logo))
                {
                    script.BorderColorLocation = new PointD(logo.Width, logo.Height);

                    ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>("x", $"Invalid X coordinate: {logo.Width}.", () =>
                    {
                        script.Execute(logo);
                    });
                }
            }

            [TestMethod]
            public void ShouldExecute_zelda3_border()
            {
                AssertExecuteWithFilename("zelda3_border2w.png", "zelda3_border.png", (AutotrimScript<ushort> script) =>
                {
                });

                AssertExecuteWithFilename("zelda3_border2b.png", "zelda3_border.png", (AutotrimScript<ushort> script) =>
                {
                });

                AssertExecuteWithFilename("zelda3_border2wrt.png", "zelda3_border.png", (AutotrimScript<ushort> script) =>
                {
                    script.BorderColorLocation = new PointD(129, 0);
                });

                AssertExecuteWithFilename("zelda3_border2brt.png", "zelda3_border.png", (AutotrimScript<ushort> script) =>
                {
                    script.BorderColorLocation = new PointD(129, 0);
                });
            }

            [TestMethod]
            public void ShouldExecute_zelda3_rot20_border10()
            {
                AssertExecuteWithFilename("zelda3_rot20_border10.png", "zelda3_rot20_border10.png", (AutotrimScript<ushort> script) =>
                {
                });
            }

            [TestMethod]
            public void ShouldExecute_f30_png()
            {
                AssertExecute("zelda3_radborder.png", nameof(ShouldExecute_f30_png), (AutotrimScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)30;
                });
            }

            [TestMethod]
            public void ShouldExecute_f35_png()
            {
                AssertExecute("zelda3_radborder.png", nameof(ShouldExecute_f35_png), (AutotrimScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)35;
                });
            }

            [TestMethod]
            public void ShouldExecute_f40_png()
            {
                AssertExecute("zelda3_radborder.png", nameof(ShouldExecute_f40_png), (AutotrimScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)40;
                });
            }

            [TestMethod]
            public void ShouldExecute_f60_png()
            {
                AssertExecute("zelda3_radborder.png", nameof(ShouldExecute_f60_png), (AutotrimScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)60;
                });
            }

            [TestMethod]
            public void ShouldExecute_i_png()
            {
                AssertExecute("zelda3_rot10.png", nameof(ShouldExecute_i_png), (AutotrimScript<ushort> script) =>
                {
                    script.InnerTrim = true;
                });

                AssertExecute("zelda3_rotm5.png", nameof(ShouldExecute_i_png), (AutotrimScript<ushort> script) =>
                {
                    script.InnerTrim = true;
                });

                AssertExecute("zelda3_rot2.png", nameof(ShouldExecute_i_png), (AutotrimScript<ushort> script) =>
                {
                    script.InnerTrim = true;
                });

                AssertExecute("zelda3_rot45.png", nameof(ShouldExecute_i_png), (AutotrimScript<ushort> script) =>
                {
                    script.InnerTrim = true;
                });

                AssertExecute("image3s_rot45.png", nameof(ShouldExecute_i_png), (AutotrimScript<ushort> script) =>
                {
                    script.InnerTrim = true;
                });

                AssertExecute("image3s_rotm20.png", nameof(ShouldExecute_i_png), (AutotrimScript<ushort> script) =>
                {
                    script.InnerTrim = true;
                });

                AssertExecute("image3s_rot10.png", nameof(ShouldExecute_i_png), (AutotrimScript<ushort> script) =>
                {
                    script.InnerTrim = true;
                });

                AssertExecute("image3s_rotm5.png", nameof(ShouldExecute_i_png), (AutotrimScript<ushort> script) =>
                {
                    script.InnerTrim = true;
                });
            }

            [TestMethod]
            public void ShouldExecute_i_f1_png()
            {
                AssertExecute("zelda3_rotm20.png", nameof(ShouldExecute_i_f1_png), (AutotrimScript<ushort> script) =>
                {
                    script.InnerTrim = true;
                    script.ColorFuzz = (Percentage)1;
                });
            }

            [TestMethod]
            public void ShouldExecute_logo_png()
            {
                using (var image = new MagickImage(Images.Logo))
                {
                    var factory = new MagickFactory();
                    var script = new AutotrimScript<ushort>(factory);
                    script.InnerTrim = true;

                    using (var scriptOutput = script.Execute(image))
                    {
                        AssertOutput(scriptOutput, "logo.png");
                    }
                }
            }

            private void AssertExecute(string input, string methodName, Action<AutotrimScript<ushort>> action)
            {
                string outputFile = GetOutputFile(input, methodName);
                AssertExecuteWithFilename(input, outputFile, action);
            }

            private void AssertExecuteWithFilename(string input, string output, Action<AutotrimScript<ushort>> action)
            {
                string inputFile = GetInputFile(input);
                /* LosslessCompress(inputFile); */

                using (var image = new MagickImage(inputFile))
                {
                    var factory = new MagickFactory();
                    var script = new AutotrimScript<ushort>(factory);
                    action(script);

                    using (var scriptOutput = script.Execute(image))
                    {
                        AssertOutput(scriptOutput, output);
                    }
                }
            }
        }
    }
}
