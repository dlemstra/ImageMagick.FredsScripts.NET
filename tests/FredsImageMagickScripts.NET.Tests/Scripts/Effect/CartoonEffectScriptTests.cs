//=================================================================================================
// Copyright 2015-2017 Dirk Lemstra, Fred Weinhaus
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
      script.EdgeAmount = 5;
      script.EdgeThreshold = (Percentage)42;
      script.EdgeWidth = 15;
      script.Method = CartoonMethod.Method2;
      script.Numlevels = 8;
      script.Pattern = (Percentage)42;
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

        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Numlevels = 1;
          script.Execute(logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Reset();
          script.EdgeAmount = -1;
          script.Execute(logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Reset();
          script.EdgeAmount = float.NaN;
          script.Execute(logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Reset();
          script.EdgeAmount = float.NegativeInfinity;
          script.Execute(logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Reset();
          script.EdgeAmount = float.PositiveInfinity;
          script.Execute(logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Reset();
          script.EdgeWidth = -1;
          script.Execute(logo);
        });


        // Test that execution works fine after a reset
        script.Reset();
        var result = script.Execute(logo);
        Assert.IsNotNull(result);

        // Test method 2 as well to execute different code path
        script.Method = CartoonMethod.Method2;
        result = script.Execute(logo);
        Assert.IsNotNull(result);
      }
    }

    private static void Test_Defaults(CartoonEffectScript script)
    {
      Assert.AreEqual((Percentage)70, script.Pattern);
      Assert.AreEqual(6, script.Numlevels);
      Assert.AreEqual(CartoonMethod.Method1, script.Method);
      Assert.AreEqual(4, script.EdgeAmount);
      Assert.AreEqual((Percentage)100, script.Brightness);
      Assert.AreEqual((Percentage)150, script.Saturation);
      Assert.AreEqual(2, script.EdgeWidth);
      Assert.AreEqual((Percentage)90, script.EdgeThreshold);
    }

    private const string _Category = "CartoonEffectScript";
  }
}