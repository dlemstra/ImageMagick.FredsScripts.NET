//=================================================================================================
// Copyright 2015-2017 Christoph Bergmeister, Dirk Lemstra, Fred Weinhaus
// <https://github.com/dlemstra/FredsImageMagickScripts.NET>
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
//=================================================================================================

using System;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests.Scripts.Effect
{
    [TestClass]
    public class CartoonEffectScriptTests : ScriptTester
    {
        [TestMethod, TestCategory(_Category)]
        public void Test_Defaults()
        {
            var script = new CartoonEffectScript();
            Test_Defaults(script);

            script.Brightness = (Percentage)42;
            script.Edgeamount = 5;
            script.Edgethresh = (Percentage)42;
            script.Edgewidth  = 15;
            script.Method     = 2;
            script.Numlevels  = 8;
            script.Pattern    = (Percentage)42;
            script.Saturation = (Percentage)42;

            script.Reset();
            Test_Defaults(script);
        }

        [TestMethod, TestCategory(_Category)]
        public void Test_Execute_Null()
        {
            ExceptionAssert.ThrowsArgumentException<ArgumentNullException>(() =>
            {
                var script = new CartoonEffectScript();
                script.Execute(null);
            }, "input");
        }

        [TestMethod, TestCategory(_Category)]
        public void Test_Settings()
        {
            var script = new CartoonEffectScript();

            using (var logo = new MagickImage(Images.Logo))
            {
                script.Execute(logo);

                ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    script.Numlevels = 1;
                });

                ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    script.Method = -1;
                });
                ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    script.Method = 3;
                });

                ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    script.Edgeamount = -1;
                });

                ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    script.Edgeamount = float.NaN;
                });

                ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    script.Edgeamount = float.NegativeInfinity;
                });

                ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    script.Edgeamount = float.PositiveInfinity;
                });

                ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
                {
                    script.Edgewidth = -1;
                });


                // Test that execution works fine after a reset
                script.Reset();
                script.Execute(logo);
            }
        }


        private static void Test_Defaults(CartoonEffectScript script)
        {
            Assert.AreEqual((Percentage)70, script.Pattern);
            Assert.AreEqual(6, script.Numlevels);
            Assert.AreEqual(1, script.Method);
            Assert.AreEqual(4, script.Edgeamount);
            Assert.AreEqual((Percentage)100, script.Brightness);
            Assert.AreEqual((Percentage)150, script.Saturation);
            Assert.AreEqual(2, script.Edgewidth);
            Assert.AreEqual((Percentage)90, script.Edgethresh);
        }


        private void Test_Execute(string input, Action<CartoonEffectScript> action, string output)
        {
            string inputFile = GetInputFile(input);
            //LosslessCompress(inputFile);

            using (var image = new MagickImage(inputFile))
            {
                var script = new CartoonEffectScript();
                action(script);

                var scriptOutput = script.Execute(image);
                TestOutput(scriptOutput, output);
            }
        }

        private const string _Category = "CartoonEffectScript";
    }
}