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
    public partial class UnrotateScriptTests
    {
        public class TheExecuteMethod : UnrotateScriptTests
        {
            [Fact]
            public void ShouldThrowExceptionWhenInputIsNull()
            {
                var factory = new MagickFactory();
                var script = new UnrotateScript<ushort>(factory);

                Assert.Throws<ArgumentNullException>("input", () => script.Execute(null));
            }

            [Fact]
            public void ShouldExecute_zelda3_m45_f35()
            {
                AssertExecuteWithFilename("zelda3.png", -45, "zelda3_m45_f35.jpg", (UnrotateScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)35;
                });
            }

            [Fact]
            public void ShouldExecute_zelda3_45_f30()
            {
                AssertExecuteWithFilename("zelda3.png", 45, "zelda3_45_f30.jpg", (UnrotateScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)30;
                });
            }

            [Fact]
            public void ShouldExecute_zelda3_45_f20_am45()
            {
                AssertExecuteWithFilename("zelda3.png", 45, "zelda3_45_f20_am45.jpg", (UnrotateScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)20;
                    script.Angle = -45;
                });
            }

            [Fact]
            public void ShouldExecute_zelda3_m20_f30()
            {
                AssertExecuteWithFilename("zelda3.png", -20, "zelda3_m20_f30.jpg", (UnrotateScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)30;
                });
            }

            [Fact]
            public void ShouldExecute_zelda3_m20_f0()
            {
                AssertExecuteWithFilename("zelda3.png", -20, "zelda3_m20_f0.jpg", (UnrotateScript<ushort> script) =>
                {
                });
            }

            [Fact]
            public void ShouldExecute_zelda3_10_f30()
            {
                AssertExecuteWithFilename("zelda3.png", 10, "zelda3_10_f30.jpg", (UnrotateScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)30;
                });
            }

            [Fact]
            public void ShouldExecute_zelda3_m5_f30()
            {
                AssertExecuteWithFilename("zelda3.png", -5, "zelda3_m5_f30.jpg", (UnrotateScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)30;
                });
            }

            [Fact]
            public void ShouldExecute_zelda3_2()
            {
                AssertExecuteWithFilename("zelda3.png", 2, "zelda3_2_f30.jpg", (UnrotateScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)30;
                });
            }

            [Fact]
            public void ShouldExecute_zelda3_rotm20_border10_f30()
            {
                AssertExecuteWithFilename("zelda3_rotm20_border10.png", 10, "zelda3_rotm20_border10_f30.jpg", (UnrotateScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)30;
                });
            }

            [Fact]
            public void ShouldExecute_zelda3_rot10_f30()
            {
                AssertExecuteWithFilename("zelda3_rot10.png", 10, "zelda3_rot10_f30.jpg", (UnrotateScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)30;
                });
            }

            private void AssertExecuteWithFilename(string input, int rotationAngle, string output, Action<UnrotateScript<ushort>> action)
            {
                var inputFile = GetInputFile(input);

                using (var image = new MagickImage(inputFile))
                {
                    image.BackgroundColor = MagickColors.White;
                    image.Rotate(rotationAngle);

                    var factory = new MagickFactory();
                    var script = new UnrotateScript<ushort>(factory);
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
