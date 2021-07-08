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
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public partial class UnperspectiveScriptTests
    {
        [TestClass]
        public class TheGetCoefficientsMethod
        {
            [TestMethod]
            public void ShouldThrowExceptionForUnsolvableMatrix()
            {
                ShouldThrowExceptionForUnsolvableMatrix(new double[] { 1, 2, 3, 4 });
                ShouldThrowExceptionForUnsolvableMatrix(new double[] { -4, double.NaN, double.NaN, double.NaN });
            }

            private static void ShouldThrowExceptionForUnsolvableMatrix(double[] arguments)
            {
                var factory = new MagickFactory();
                var script = new UnperspectiveScript<ushort>(factory);

                var type = script.GetType();
                var method = type.GetMethod("GetCoefficients", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(double[]) }, null);

                ExceptionAssert.Throws<InvalidOperationException>("Unsolvable matrix detected.", () =>
                {
                    try
                    {
                        method.Invoke(null, new object[] { arguments });
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }
                });
            }
        }
    }
}
