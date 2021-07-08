// Copyright Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/FredsImageMagickScripts.NET)
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
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageMagick.FredsScripts.NET.Tests
{
    public static class ExceptionAssert
    {
        public static TException Throws<TException>(Action action)
           where TException : Exception
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            try
            {
                action();
                Assert.Fail("Exception of type {0} was not thrown.", typeof(TException).Name);
            }
            catch (TException exception)
            {
                var type = exception.GetType();
                if (type != typeof(TException))
                    Assert.Fail("Exception of type {0} was not thrown an exception of type {1} was thrown.", typeof(TException).Name, type.Name);

                return exception;
            }
            catch (Exception)
            {
                throw;
            }

            return null;
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "False positive.")]
        public static void Throws<TException>(string expectedMessage, Action action)
           where TException : Exception
        {
            var exception = Throws<TException>(action);
            Assert.AreEqual(expectedMessage, exception.Message);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "False positive.")]
        public static void ThrowsArgumentException<TException>(string paramName, Action action)
          where TException : ArgumentException
        {
            var exception = Throws<TException>(action);
            Assert.AreEqual(paramName, exception.ParamName);
        }

        [SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "False positive.")]
        public static void ThrowsArgumentException<TException>(string paramName, string expectedMessage, Action action)
          where TException : ArgumentException
        {
            var exception = Throws<TException>(action);
            Assert.AreEqual(expectedMessage, exception.Message.Split(Environment.NewLine.ToCharArray())[0]);
            Assert.AreEqual(paramName, exception.ParamName);
        }
    }
}
