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
    [TestInitialize]
    public void Initialize()
    {
      MagickNET.SetRandomSeed(100);
    }

    [TestMethod]
    public void Test_Defaults()
    {
      var script = new EmbroideryScript();
      Test_Defaults(script);

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
      Test_Defaults(script);
    }

    [TestMethod]
    public void Test_Execute_cnbc_embroidery_n8_p1_t2_g20_f20_a0_r90_i25_e2_B4_A130_E30_C0_S1_N100_M100()
    {
      Test_Execute("cnbc.jpg", "cnbc_embroidery_n8_p1_t2_g20_f20_a0_r90_i25_e2_B4_A130_E30_C0_S1_N100_M100.jpg", (EmbroideryScript script) =>
      {
      });
    }

    [TestMethod]
    public void Test_Execute_cnbc_embroidery_n8_p1_t3_g20_f20_a0_r90_i25_e2_B4_A130_E30_C0_S1_N100_M100()
    {
      Test_Execute("cnbc.jpg", "cnbc_embroidery_n8_p1_t3_g20_f20_a0_r90_i25_e2_B4_A130_E30_C0_S1_N100_M100.jpg", (EmbroideryScript script) =>
      {
        script.Thickness = 3;
      });
    }

    [TestMethod]
    public void Test_Execute_cnbc_embroidery_n8_p2_t2_g20_f20_a0_r90_i25_e2_B4_A130_E30_C0_S1_N100_M100()
    {
      Test_Execute("cnbc.jpg", "cnbc_embroidery_n8_p2_t2_g20_f20_a0_r90_i25_e2_B4_A130_E30_C0_S1_N100_M100.jpg", (EmbroideryScript script) =>
      {
        script.Pattern = EmbroideryPattern.Crosshatch;
      });
    }

    [TestMethod]
    public void Test_Execute_cnbc_embroidery_n8_p2_t3_g20_f20_a0_r90_i25_e2_B4_A130_E30_C0_S1_N100_M100()
    {
      Test_Execute("cnbc.jpg", "cnbc_embroidery_n8_p2_t3_g20_f20_a0_r90_i25_e2_B4_A130_E30_C0_S1_N100_M100.jpg", (EmbroideryScript script) =>
      {
        script.Pattern = EmbroideryPattern.Crosshatch;
        script.Thickness = 3;
      });
    }

    [TestMethod]
    public void Test_Execute_cnbc_embroidery_n8_p2_t5_g20_f20_a0_r90_i25_e2_B4_A130_E30_C0_S1_N100_M100()
    {
      Test_Execute("cnbc.jpg", "cnbc_embroidery_n8_p2_t5_g20_f20_a0_r90_i25_e2_B4_A130_E30_C0_S1_N100_M100.jpg", (EmbroideryScript script) =>
      {
        script.Pattern = EmbroideryPattern.Crosshatch;
        script.Thickness = 5;
      });
    }

    [TestMethod]
    public void Test_Execute_cnbc_embroidery_n8_p1_t2_g20_f20_a0_r90_i0_e2_B4_A130_E30_C0_S1_N100_M100()
    {
      Test_Execute("cnbc.jpg", "cnbc_embroidery_n8_p1_t2_g20_f20_a0_r90_i0_e2_B4_A130_E30_C0_S1_N100_M100.jpg", (EmbroideryScript script) =>
      {
        script.Intensity = (Percentage)0;
      });
    }

    [TestMethod]
    public void Test_Execute_cnbc_embroidery_n8_p1_t2_g20_f20_a0_r90_i50_e2_B4_A130_E30_C0_S1_N100_M100()
    {
      Test_Execute("cnbc.jpg", "cnbc_embroidery_n8_p1_t2_g20_f20_a0_r90_i50_e2_B4_A130_E30_C0_S1_N100_M100.jpg", (EmbroideryScript script) =>
      {
        script.Intensity = (Percentage)50;
      });
    }

    [TestMethod]
    public void Test_Execute_cnbc_embroidery_n8_p1_t2_g20_f20_a0_r90_i0_e2_B4_A130_E30_C10_S1_N100_M100()
    {
      Test_Execute("cnbc.jpg", "cnbc_embroidery_n8_p1_t2_g20_f20_a0_r90_i0_e2_B4_A130_E30_C10_S1_N100_M100.jpg", (EmbroideryScript script) =>
      {
        script.Intensity = (Percentage)0;
        script.Contrast = 10;
      });
    }

    [TestMethod]
    public void Test_Execute_cnbc_embroidery_n8_p1_t2_g0_f0_a0_r90_i25_e2_B4_A130_E30_C0_S1_N100_M100()
    {
      Test_Execute("cnbc.jpg", "cnbc_embroidery_n8_p1_t2_g0_f0_a0_r90_i25_e2_B4_A130_E30_C0_S1_N100_M100.jpg", (EmbroideryScript script) =>
      {
        script.ColorFuzz = (Percentage)0;
        script.GrayLimit = 0;
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
    public void Test_Settings()
    {
      var script = new EmbroideryScript();

      using (var logo = new MagickImage(Images.Logo))
      {
        script.Angle = -361;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Angle = 361;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Azimuth = -361.0;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Azimuth = 361.0;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.ColorFuzz = new Percentage(-1);
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.ColorFuzz = new Percentage(101);
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Contrast = -0.99;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Elevation = -0.99;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Elevation = 90.01;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Extent = -1;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.GrayLimit = -1;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.GrayLimit = 101;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Intensity = new Percentage(-1);
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Intensity = new Percentage(101);
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Mix = -1;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Mix = 101;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.NumberOfColors = 0;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Range = -1;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Range = 361;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Spread = -0.99;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });

        script.Reset();
        script.Thickness = 0;
        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Execute(logo);
        });
      }
    }

    private static void Test_Defaults(EmbroideryScript script)
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

    private void Test_Execute(string input, string output, Action<EmbroideryScript> action)
    {
      string inputFile = GetInputFile(input);
      /* LosslessCompress(inputFile); */

      using (var image = new MagickImage(inputFile))
      {
        var script = new EmbroideryScript();
        action(script);

        var scriptOutput = script.Execute(image);
        AssertOutput(scriptOutput, output);
      }
    }
  }
}