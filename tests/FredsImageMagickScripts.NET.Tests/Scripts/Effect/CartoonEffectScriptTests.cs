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
    public void Constructor_InvalidCartoonMethod_ThrowsException()
    {
      ExceptionAssert.ThrowsArgumentException<ArgumentException>("method", "Invalid cartoon method specified.", () =>
      {
        new CartoonEffectScript((CartoonMethod)42);
      });
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
    public void Brightness_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid brightness specified, value must be zero or higher.", (CartoonEffectScript script) =>
      {
        script.Brightness = (Percentage)(-1);
      });
    }

    [TestMethod]
    public void EdgeAmount_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid edge amount specified, value must be zero or higher.", (CartoonEffectScript script) =>
      {
        script.EdgeAmount = -1;
      });
    }

    [TestMethod]
    public void EdgeAmount_IsNan_ThrowsException()
    {
      AssertInvalidOperation("Invalid edge amount specified, value must be zero or higher.", (CartoonEffectScript script) =>
      {
        script.EdgeAmount = double.NaN;
      });
    }

    [TestMethod]
    public void EdgeAmount_IsNegativeInfinity_ThrowsException()
    {
      AssertInvalidOperation("Invalid edge amount specified, value must be zero or higher.", (CartoonEffectScript script) =>
      {
        script.EdgeAmount = double.NegativeInfinity;
      });
    }

    [TestMethod]
    public void EdgeAmount_IsPositiveInfinity_ThrowsException()
    {
      AssertInvalidOperation("Invalid edge amount specified, value must be zero or higher.", (CartoonEffectScript script) =>
      {
        script.EdgeAmount = double.PositiveInfinity;
      });
    }

    [TestMethod]
    public void NumberOflevels_BelowTwo_ThrowsException()
    {
      AssertInvalidOperation("Invalid number of levels specified, value must be two or higher.", (CartoonEffectScript script) =>
      {
        script.NumberOflevels = 1;
      });
    }

    [TestMethod]
    public void Saturation_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid saturation specified, value must be zero or higher.", (CartoonEffectScript script) =>
      {
        script.Saturation = (Percentage)(-1);
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

    [TestMethod]
    public void Execute_p60_jpg()
    {
      AssertExecute("photo-1h.jpg", nameof(Execute_p60_jpg), (CartoonEffectScript script) =>
      {
        script.Pattern = new Percentage(60);
      });
    }

    [TestMethod]
    public void Execute_p70_jpg()
    {
      AssertExecute("photo-1h.jpg", nameof(Execute_p70_jpg), (CartoonEffectScript script) =>
      {
        script.Pattern = new Percentage(70);
      });
    }

    [TestMethod]
    public void Execute_p80_jpg()
    {
      AssertExecute("photo-1h.jpg", nameof(Execute_p80_jpg), (CartoonEffectScript script) =>
      {
        script.Pattern = new Percentage(80);
      });
    }

    [TestMethod]
    public void Execute_n4_p70_jpg()
    {
      AssertExecute("photo-1h.jpg", nameof(Execute_n4_p70_jpg), (CartoonEffectScript script) =>
      {
        script.NumberOflevels = 4;
        script.Pattern = new Percentage(70);
      });
    }

    [TestMethod]
    public void Execute_n5_p70_jpg()
    {
      AssertExecute("photo-1h.jpg", nameof(Execute_n5_p70_jpg), (CartoonEffectScript script) =>
      {
        script.NumberOflevels = 5;
        script.Pattern = new Percentage(70);
      });
    }

    [TestMethod]
    public void Execute_e3_p70_jpg()
    {
      AssertExecute("photo-1h.jpg", nameof(Execute_e3_p70_jpg), (CartoonEffectScript script) =>
      {
        script.EdgeAmount = 3;
        script.Pattern = new Percentage(70);
      });
    }

    [TestMethod]
    public void Execute_e5_p70_jpg()
    {
      AssertExecute("photo-1h.jpg", nameof(Execute_e5_p70_jpg), (CartoonEffectScript script) =>
      {
        script.EdgeAmount = 5;
        script.Pattern = new Percentage(70);
      });
    }

    [TestMethod]
    public void Execute_m2_jpg()
    {
      AssertExecute("photo-1h.jpg", nameof(Execute_m2_jpg), CartoonMethod.Method2, (CartoonEffectScript script) =>
      {
      });
    }

    [TestMethod]
    public void Execute_e3_n6_p70_jpg()
    {
      AssertExecute("oriental_lady.jpg", nameof(Execute_e3_n6_p70_jpg), (CartoonEffectScript script) =>
      {
        script.EdgeAmount = 3;
        script.NumberOflevels = 6;
        script.Pattern = new Percentage(70);
      });
    }

    [TestMethod]
    public void Execute_b120_e3_p80_jpg()
    {
      AssertExecute("redhat.jpg", nameof(Execute_b120_e3_p80_jpg), (CartoonEffectScript script) =>
      {
        script.Brightness = new Percentage(120);
        script.EdgeAmount = 3;
        script.Pattern = new Percentage(80);
      });
    }

    [TestMethod]
    public void Execute_m1_jpg()
    {
      AssertExecute("obama.jpg", nameof(Execute_m1_jpg), (CartoonEffectScript script) =>
      {
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

    private static void AssertInvalidOperation(string expectedMessage, Action<CartoonEffectScript> initAction)
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

    private void AssertExecute(string input, string methodName, Action<CartoonEffectScript> action)
    {
      AssertExecute(input, methodName, CartoonMethod.Method1, action);
    }

    private void AssertExecute(string input, string methodName, CartoonMethod method, Action<CartoonEffectScript> action)
    {
      string inputFile = GetInputFile(input);
      /* LosslessCompress(inputFile); */

      using (var image = new MagickImage(inputFile))
      {
        var script = new CartoonEffectScript(method);
        action(script);

        using (var scriptOutput = script.Execute(image))
        {
          string outputFile = GetOutputFile(input, methodName);
          AssertOutput(scriptOutput, outputFile);
        }
      }
    }
  }
}