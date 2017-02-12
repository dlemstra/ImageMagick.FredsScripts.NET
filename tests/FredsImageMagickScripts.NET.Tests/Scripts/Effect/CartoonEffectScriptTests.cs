// <copyright file="CartoonEffectScriptTests.cs" company="Dirk Lemstra, Fred Weinhaus">
// https://github.com/dlemstra/FredsImageMagickScripts.NET
//
// Copyright 2015-2017 Dirk Lemstra, Fred Weinhaus
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
// </copyright>

using System;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests
{
  [TestClass]
  public class CartoonEffectScriptTests : ScriptTester
  {
    [TestMethod]
    public void Constructor_SettingsSetToDefaults()
    {
      var script = new CartoonEffectScript();

      AssertDefaults(script);
    }

    [TestMethod]
    public void Reset_AllSettingsChanged_RestoredToDefault()
    {
      var script = new CartoonEffectScript();
      script.Brightness = (Percentage)42;
      script.EdgeAmount = 5;
      script.NumberOflevels = 8;
      script.Pattern = (Percentage)42;
      script.Saturation = (Percentage)42;

      script.Reset();

      AssertDefaults(script);
    }

    [TestMethod]
    public void EdgeAmount_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Edge amount must be >= 0.", (CartoonEffectScript script) =>
      {
        script.EdgeAmount = -1;
      });
    }

    [TestMethod]
    public void EdgeAmount_IsNan_ThrowsException()
    {
      AssertInvalidOperation("Edge amount must be >= 0.", (CartoonEffectScript script) =>
      {
        script.EdgeAmount = double.NaN;
      });
    }

    [TestMethod]
    public void EdgeAmount_IsNegativeInfinity_ThrowsException()
    {
      AssertInvalidOperation("Edge amount must be >= 0.", (CartoonEffectScript script) =>
      {
        script.EdgeAmount = double.NegativeInfinity;
      });
    }

    [TestMethod]
    public void EdgeAmount_IsPositiveInfinity_ThrowsException()
    {
      AssertInvalidOperation("Edge amount must be >= 0.", (CartoonEffectScript script) =>
      {
        script.EdgeAmount = double.PositiveInfinity;
      });
    }

    [TestMethod]
    public void NumberOflevels_BelowTwo_ThrowsException()
    {
      AssertInvalidOperation("Number of levels must be >= 2.", (CartoonEffectScript script) =>
      {
        script.NumberOflevels = 1;
      });
    }

    [TestMethod]
    public void Execute_InputNull_ThrowsException()
    {
      ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () =>
      {
        var script = new CartoonEffectScript();
        script.Execute(null);
      });
    }

    private static void AssertDefaults(CartoonEffectScript script)
    {
      Assert.AreEqual((Percentage)70, script.Pattern);
      Assert.AreEqual(6, script.NumberOflevels);
      Assert.AreEqual(4, script.EdgeAmount);
      Assert.AreEqual((Percentage)100, script.Brightness);
      Assert.AreEqual((Percentage)150, script.Saturation);
    }

    private void AssertInvalidOperation(string expectedMessage, Action<CartoonEffectScript> initAction)
    {
      var script = new CartoonEffectScript();

      using (var logo = new MagickImage(Images.Logo))
      {
        initAction(script);

        ExceptionAssert.Throws<InvalidOperationException>(expectedMessage, () =>
        {
          script.Execute(logo);
        });
      }
    }
  }
}