// <copyright file="EmbroideryScriptTests.cs" company="Dirk Lemstra, Fred Weinhaus">
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
  public class EmbroideryScriptTests : ScriptTester
  {
    [TestMethod]
    public void Constructor_SettingsSetToDefaults()
    {
      var script = new EmbroideryScript();

      AssertDefaults(script);
    }

    [TestMethod]
    public void Constructor_InvalidCartoonMethod_ThrowsException()
    {
      var script = new EmbroideryScript();
      script.Angle = 10;
      script.Azimuth = 150.3;
      script.BackgroundColor = MagickColors.Pink;
      script.Bevel = 10;
      script.ColorFuzz = (Percentage)30;
      script.Contrast = 2;
      script.Elevation = 10.5;
      script.Extent = 5;
      script.GrayLimit = 60;
      script.Mix = 99;
      script.NumberOfColors = 1;
      script.Pattern = EmbroideryPattern.Crosshatch;
      script.Range = 1;
      script.Spread = 2.5;
      script.Thickness = 1;

      script.Reset();

      AssertDefaults(script);
    }

    [TestMethod]
    public void Angle_BelowMinus360_ThrowsException()
    {
      AssertInvalidOperation("Invalid angle specified, value must be between -360 and 360.", (EmbroideryScript script) =>
      {
        script.Angle = -361;
      });
    }

    [TestMethod]
    public void Angle_Above360_ThrowsException()
    {
      AssertInvalidOperation("Invalid angle specified, value must be between -360 and 360.", (EmbroideryScript script) =>
      {
        script.Angle = 361;
      });
    }

    [TestMethod]
    public void Azimuth_BelowMinus360_ThrowsException()
    {
      AssertInvalidOperation("Invalid azimuth specified, value must be between -360 and 360.", (EmbroideryScript script) =>
      {
        script.Azimuth = -361;
      });
    }

    [TestMethod]
    public void Azimuth_Above360_ThrowsException()
    {
      AssertInvalidOperation("Invalid azimuth specified, value must be between -360 and 360.", (EmbroideryScript script) =>
      {
        script.Azimuth = 361;
      });
    }

    [TestMethod]
    public void ColorFuzz_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid color fuzz specified, value must be between 0 and 100.", (EmbroideryScript script) =>
      {
        script.ColorFuzz = new Percentage(-1);
      });
    }

    [TestMethod]
    public void ColorFuzz_Above100_ThrowsException()
    {
      AssertInvalidOperation("Invalid color fuzz specified, value must be between 0 and 100.", (EmbroideryScript script) =>
      {
        script.ColorFuzz = new Percentage(101);
      });
    }

    [TestMethod]
    public void Contrast_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid contrast specified, value must be zero or higher.", (EmbroideryScript script) =>
      {
        script.Contrast = -0.99;
      });
    }

    [TestMethod]
    public void Elevation_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid elevation specified, value must be between 0 and 90.", (EmbroideryScript script) =>
      {
        script.Elevation = -0.99;
      });
    }

    [TestMethod]
    public void Elevation_Above90_ThrowsException()
    {
      AssertInvalidOperation("Invalid elevation specified, value must be between 0 and 90.", (EmbroideryScript script) =>
      {
        script.Elevation = 90.01;
      });
    }

    [TestMethod]
    public void Extent_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid extent specified, value must be zero or higher.", (EmbroideryScript script) =>
      {
        script.Extent = -1;
      });
    }

    [TestMethod]
    public void GrayLimit_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid gray limit specified, value must be between 0 and 100.", (EmbroideryScript script) =>
      {
        script.GrayLimit = -1;
      });
    }

    [TestMethod]
    public void GrayLimit_Above100_ThrowsException()
    {
      AssertInvalidOperation("Invalid gray limit specified, value must be between 0 and 100.", (EmbroideryScript script) =>
      {
        script.GrayLimit = 101;
      });
    }

    [TestMethod]
    public void Intensity_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid intensity specified, value must be between 0 and 100.", (EmbroideryScript script) =>
      {
        script.Intensity = new Percentage(-1);
      });
    }

    [TestMethod]
    public void Intensity_Above100_ThrowsException()
    {
      AssertInvalidOperation("Invalid intensity specified, value must be between 0 and 100.", (EmbroideryScript script) =>
      {
        script.Intensity = new Percentage(101);
      });
    }

    [TestMethod]
    public void Mix_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid mix specified, value must be between 0 and 100.", (EmbroideryScript script) =>
      {
        script.Mix = -1;
      });
    }

    [TestMethod]
    public void Mix_Above100_ThrowsException()
    {
      AssertInvalidOperation("Invalid mix specified, value must be between 0 and 100.", (EmbroideryScript script) =>
      {
        script.Mix = 101;
      });
    }

    [TestMethod]
    public void NumberOfColors_Zero_ThrowsException()
    {
      AssertInvalidOperation("Invalid number of colors specified, value must be higher than zero.", (EmbroideryScript script) =>
      {
        script.NumberOfColors = 0;
      });
    }

    [TestMethod]
    public void Pattern_InvalidValue_ThrowsException()
    {
      AssertInvalidOperation("Invalid pattern specified.", (EmbroideryScript script) =>
      {
        script.Pattern = (EmbroideryPattern)42;
      });
    }

    [TestMethod]
    public void Range_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid range specified, value must be between 0 and 360.", (EmbroideryScript script) =>
      {
        script.Range = -1;
      });
    }

    [TestMethod]
    public void Range_Above360_ThrowsException()
    {
      AssertInvalidOperation("Invalid range specified, value must be between 0 and 360.", (EmbroideryScript script) =>
      {
        script.Range = 361;
      });
    }

    [TestMethod]
    public void Spread_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid spread specified, value must be zero or higher.", (EmbroideryScript script) =>
      {
        script.Spread = -0.99;
      });
    }

    [TestMethod]
    public void Thickness_Zero_ThrowsException()
    {
      AssertInvalidOperation("Invalid thickness specified, value must be higher than zero.", (EmbroideryScript script) =>
      {
        script.Thickness = 0;
      });
    }

    [TestMethod]
    public void Test_Execute_Null()
    {
      ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () =>
      {
        var script = new EmbroideryScript();
        script.Execute(null);
      });
    }

    [TestMethod]
    public void Execute_default_jpg()
    {
      AssertExecute("cnbc.jpg", nameof(Execute_default_jpg), (EmbroideryScript script) =>
      {
      });
    }

    [TestMethod]
    public void Execute_f0_g0_jpg()
    {
      AssertExecute("cnbc.jpg", nameof(Execute_f0_g0_jpg), (EmbroideryScript script) =>
      {
        script.ColorFuzz = (Percentage)0;
        script.GrayLimit = 0;
      });
    }

    [TestMethod]
    public void Execute_i0_jpg()
    {
      AssertExecute("cnbc.jpg", nameof(Execute_i0_jpg), (EmbroideryScript script) =>
      {
        script.Intensity = (Percentage)0;
      });
    }

    [TestMethod]
    public void Execute_i0_C10_jpg()
    {
      AssertExecute("cnbc.jpg", nameof(Execute_i0_C10_jpg), (EmbroideryScript script) =>
      {
        script.Intensity = (Percentage)0;
        script.Contrast = 10;
      });
    }

    [TestMethod]
    public void Execute_i50_jpg()
    {
      AssertExecute("cnbc.jpg", nameof(Execute_i50_jpg), (EmbroideryScript script) =>
      {
        script.Intensity = (Percentage)50;
      });
    }

    [TestMethod]
    public void Execute_p2_jpg()
    {
      AssertExecute("cnbc.jpg", nameof(Execute_p2_jpg), (EmbroideryScript script) =>
      {
        script.Pattern = EmbroideryPattern.Crosshatch;
      });
    }

    [TestMethod]
    public void Execute_p2_t3_jpg()
    {
      AssertExecute("cnbc.jpg", nameof(Execute_p2_t3_jpg), (EmbroideryScript script) =>
      {
        script.Pattern = EmbroideryPattern.Crosshatch;
        script.Thickness = 3;
      });
    }

    [TestMethod]
    public void Execute_p2_t5_jpg()
    {
      AssertExecute("cnbc.jpg", nameof(Execute_p2_t5_jpg), (EmbroideryScript script) =>
      {
        script.Pattern = EmbroideryPattern.Crosshatch;
        script.Thickness = 5;
      });
    }

    [TestMethod]
    public void Execute_s100_jpg()
    {
      AssertExecute("cnbc.jpg", nameof(Execute_s100_jpg), (EmbroideryScript script) =>
      {
        script.Spread = 100;
      });
    }

    [TestMethod]
    public void Execute_s0_t3_jpg()
    {
      AssertExecute("cnbc.jpg", nameof(Execute_s0_t3_jpg), (EmbroideryScript script) =>
      {
        script.Spread = 0;
        script.Thickness = 3;
      });
    }

    private static void AssertDefaults(EmbroideryScript script)
    {
      Assert.AreEqual(0, script.Angle);
      Assert.AreEqual(130, script.Azimuth);
      Assert.AreEqual(null, script.BackgroundColor);
      Assert.AreEqual(4, script.Bevel);
      Assert.AreEqual((Percentage)20, script.ColorFuzz);
      Assert.AreEqual(0, script.Contrast);
      Assert.AreEqual(30.0, script.Elevation);
      Assert.AreEqual(2, script.Extent);
      Assert.AreEqual(20, script.GrayLimit);
      Assert.AreEqual((Percentage)25, script.Intensity);
      Assert.AreEqual(100, script.Mix);
      Assert.AreEqual(8, script.NumberOfColors);
      Assert.AreEqual(EmbroideryPattern.Linear, script.Pattern);
      Assert.AreEqual(90, script.Range);
      Assert.AreEqual(1.0, script.Spread);
      Assert.AreEqual(2, script.Thickness);
    }

    private static void AssertInvalidOperation(string expectedMessage, Action<EmbroideryScript> initAction)
    {
      var script = new EmbroideryScript();

      using (var logo = new MagickImage(Images.Logo))
      {
        initAction(script);

        ExceptionAssert.Throws<InvalidOperationException>(expectedMessage, () =>
        {
          script.Execute(logo);
        });
      }
    }

    private void AssertExecute(string input, string methodName, Action<EmbroideryScript> action)
    {
      string inputFile = GetInputFile(input);
      /* LosslessCompress(inputFile); */

      using (var image = new MagickImage(inputFile))
      {
        var script = new EmbroideryScript();
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