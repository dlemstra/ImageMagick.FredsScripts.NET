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
    public partial class TwoColorThreshScriptTests
    {
        [TestClass]
        public class TheExecuteMethod : TwoColorThreshScriptTests
        {
            [TestMethod]
            public void ShouldThrowExceptionWhenInputIsNull()
            {
                var factory = new MagickFactory();
                var script = new TwoColorThreshScript<ushort>(factory);

                var overlay = new MagickImage();
                ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () => script.Execute(null));
            }

            [TestMethod]
            public void ShouldExecute_blocks()
            {
                AssertExecute("blocks.gif");
            }

            [TestMethod]
            public void ShouldExecute_blood()
            {
                AssertExecute("blood.jpg");
            }

            [TestMethod]
            public void ShouldExecute_fingerprint()
            {
                AssertExecute("fingerprint.jpg");
            }

            [TestMethod]
            public void ShouldExecute_flower()
            {
                AssertExecute("flower.jpg");
            }

            [TestMethod]
            public void ShouldExecute_house()
            {
                AssertExecute("house.jpg");
            }

            [TestMethod]
            public void ShouldExecute_kanji()
            {
                AssertExecute("kanji.jpg");
            }

            [TestMethod]
            public void ShouldExecute_parts()
            {
                AssertExecute("parts.gif");
            }

            [TestMethod]
            public void ShouldExecute_rice()
            {
                AssertExecute("rice.jpg");
            }

            [TestMethod]
            public void ShouldExecute_tank()
            {
                AssertExecute("tank.jpg");
            }

            [TestMethod]
            public void ShouldExecute_textsample()
            {
                AssertExecute("textsample.jpg");
            }

            private void AssertExecute(string input)
            {
                string inputFile = GetInputFile(input);
                /* LosslessCompress(inputFile); */

                string output = input.Replace(".jpg", ".gif");

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