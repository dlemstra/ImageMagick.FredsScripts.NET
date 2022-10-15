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
    public partial class MulticropScriptTests
    {
        public class TheExecuteMethod : MulticropScriptTests
        {
            [Fact]
            public void ShouldThrowExceptionWhenInputIsNull()
            {
                var factory = new MagickFactory();
                var script = new MulticropScript<ushort>(factory);

                Assert.Throws<ArgumentNullException>("input", () => script.Execute(null));
            }

            [Fact]
            public void ShouldExecute_3ladies1_f20_deskew()
            {
                AssertExecuteWithFilename("3ladies1.jpg", 3, "3ladies1_f20_deskew.jpg", (MulticropScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)20;
                });
            }

            [Fact]
            public void ShouldExecute_3ladies3_f10_unrotate()
            {
                AssertExecuteWithFilename("3ladies2.jpg", 3, "3ladies3_f10_unrotate.jpg", (MulticropScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)10;
                    script.UnrotateMethod = (image, backgroundColor) =>
                    {
                        var factory = new MagickFactory();
                        var unrotateScript = new UnrotateScript<ushort>(factory)
                        {
                            BackgroundColor = backgroundColor,
                            ColorFuzz = script.ColorFuzz
                        };

                        return unrotateScript.Execute(image);
                    };
                });
            }

            [Fact]
            public void ShouldExecute_3ladies3_f5_unull()
            {
                AssertExecuteWithFilename("3ladies3.jpg", 3, "3ladies3_f5_unull.jpg", (MulticropScript<ushort> script) =>
                {
                    script.ColorFuzz = (Percentage)5;
                    script.UnrotateMethod = null;
                });
            }

            private void AssertExecuteWithFilename(string input, int count, string output, Action<MulticropScript<ushort>> action)
            {
                var inputFile = GetInputFile(input);

                using (var image = new MagickImage(inputFile))
                {
                    var factory = new MagickFactory();
                    var script = new MulticropScript<ushort>(factory);
                    action(script);

                    using (var scriptOutput = script.Execute(image))
                    {
                        Assert.Equal(count, scriptOutput.Count);

                        for (var i = 0; i < scriptOutput.Count; i++)
                            AssertOutput(scriptOutput[i], $"{i}.{output}");
                    }
                }
            }
        }
    }
}
