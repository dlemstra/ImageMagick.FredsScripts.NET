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
    public partial class TwoColorThreshScriptTests
    {
        public class TheExecuteMethod : TwoColorThreshScriptTests
        {
            [Fact]
            public void ShouldThrowExceptionWhenInputIsNull()
            {
                var factory = new MagickFactory();
                var script = new TwoColorThreshScript<ushort>(factory);

                var overlay = new MagickImage();
                Assert.Throws<ArgumentNullException>("input", () => script.Execute(null));
            }

            [Fact]
            public void ShouldExecute_blocks()
            {
                AssertExecute("blocks.gif");
            }

            [Fact]
            public void ShouldExecute_blood()
            {
                AssertExecute("blood.jpg");
            }

            [Fact]
            public void ShouldExecute_fingerprint()
            {
                AssertExecute("fingerprint.jpg");
            }

            [Fact]
            public void ShouldExecute_flower()
            {
                AssertExecute("flower.jpg");
            }

            [Fact]
            public void ShouldExecute_house()
            {
                AssertExecute("house.jpg");
            }

            [Fact]
            public void ShouldExecute_kanji()
            {
                AssertExecute("kanji.jpg");
            }

            [Fact]
            public void ShouldExecute_parts()
            {
                AssertExecute("parts.gif");
            }

            [Fact]
            public void ShouldExecute_rice()
            {
                AssertExecute("rice.jpg");
            }

            [Fact]
            public void ShouldExecute_tank()
            {
                AssertExecute("tank.jpg");
            }

            [Fact]
            public void ShouldExecute_textsample()
            {
                AssertExecute("textsample.jpg");
            }

            private void AssertExecute(string input)
            {
                var inputFile = GetInputFile(input);

                var output = input.Replace(".jpg", ".gif");

                using (var image = new MagickImage(inputFile))
                {
                    var factory = new MagickFactory();
                    var script = new TwoColorThreshScript<ushort>(factory);
                    using (var scriptOutput = script.Execute(image))
                    {
                        AssertOutput(scriptOutput, output);
                    }
                }
            }
        }
    }
}
