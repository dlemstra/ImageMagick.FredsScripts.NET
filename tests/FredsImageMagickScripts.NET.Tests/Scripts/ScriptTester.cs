// Copyright 2015-2017 Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/FredsImageMagickScripts.NET)
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
using System.IO;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests
{
    [ExcludeFromCodeCoverage]
    public abstract class ScriptTester
    {
        private static string _root = GetRoot();

        protected string ScriptName
        {
            get
            {
                var scriptName = GetType().Name;
                return scriptName.Substring(0, scriptName.Length - 5); // Remove 'Tests'
            }
        }

        protected static string GetInputFile(string fileName)
        {
            return _root + @"Input\" + fileName;
        }

        protected static string GetOutputFile(string input, string methodName)
        {
            if (methodName == null)
                throw new ArgumentNullException(nameof(methodName));

            string fileName = Path.GetFileNameWithoutExtension(input);

            int startOffset = "Execute_".Length;
            int lastUnderscore = methodName.LastIndexOf('_');
            fileName += "_" + methodName.Substring(startOffset, lastUnderscore - startOffset);
            fileName += "." + methodName.Substring(lastUnderscore + 1);

            return fileName;
        }

        protected static void LosslessCompress(string fileName)
        {
            var optimizer = new ImageOptimizer();
            optimizer.OptimalCompression = true;
            optimizer.LosslessCompress(fileName);
        }

        protected void AssertOutput(IMagickImage image, string expectedOutput)
        {
            if (image == null)
                throw new InvalidOperationException();

            var actualOutputFile = GetActualOutputFile(expectedOutput);

            if (!actualOutputFile.Directory.Exists)
                actualOutputFile.Directory.Create();

            image.Write(actualOutputFile);

            var expectedOutputFile = GetExpectedOutputFile(expectedOutput);

            /* Compress the image that will be used as the expected output after it has been compared
             * to the result from Fred his script. */
            if (!expectedOutputFile.Exists)
                LosslessCompress(actualOutputFile.FullName);

            using (var expectedImage = new MagickImage(expectedOutputFile))
            {
                using (var actualImage = new MagickImage(actualOutputFile))
                {
                    Assert.AreEqual(expectedImage.Width, actualImage.Width, actualImage.FileName);
                    Assert.AreEqual(expectedImage.Height, actualImage.Height, actualImage.FileName);

                    var distortion = actualImage.Compare(expectedImage, ErrorMetric.RootMeanSquared);

                    if (distortion != 0)
                        LosslessCompress(actualOutputFile.FullName);

                    Assert.AreEqual(0.0, distortion, actualImage.FileName);
                }
            }
        }

        private static string GetRoot()
        {
            string[] paths =
            {
                @"..\..\..\", // Code coverage
                @"..\..\..\..\",
            };

            foreach (string path in paths)
            {
                string directory = Path.GetFullPath(path) + @"Images\";
                if (Directory.Exists(directory))
                    return directory;
            }

            throw new InvalidOperationException("Unable to find the images folder.");
        }

        private FileInfo GetActualOutputFile(string fileName)
        {
            int dotIndex = fileName.LastIndexOf('.');
            var name = fileName.Substring(0, dotIndex) + ".actual" + fileName.Substring(dotIndex);

            return new FileInfo(_root + @"Output\" + ScriptName + @"\" + name);
        }

        private FileInfo GetExpectedOutputFile(string fileName)
        {
            return new FileInfo(_root + @"Output\" + ScriptName + @"\" + fileName);
        }
    }
}
