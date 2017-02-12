// <copyright file="UnperspectiveScriptTests.cs" company="Dirk Lemstra, Fred Weinhaus">
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
  public class UnperspectiveScriptTests : ScriptTester
  {
    [TestMethod]
    public void Test_Defaults()
    {
      Test_Defaults(UnperspectiveMethod.Peak);
      Test_Defaults(UnperspectiveMethod.Derivative);
    }

    [TestMethod]
    public void Test_Execute_mandril2_p30_t30_out_unperspect_f20_a_t4_s1_S5_B0_r0_el_peak()
    {
      Test_Execute("mandril2_p30_t30_out.jpg", "mandril2_p30_t30_out_unperspect_f20_a_t4_s1_S5_B0_r0_el_peak.jpg", (UnperspectiveScript script) =>
       {
         script.ColorFuzz = (Percentage)20;
       });
    }

    [TestMethod]
    public void Test_Execute_mandril2_p30_t30_r60_zc_unperspect_f20_a_t4_s1_S5_B0_r0_el_peak()
    {
      Test_Execute("mandril2_p30_t30_r60_zc.jpg", "mandril2_p30_t30_r60_zc_unperspect_f20_a_t4_s1_S5_B0_r0_el_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)20;
      });
    }

    [TestMethod]
    public void Test_Execute_mandril2_p30_t30_r60_zc_unperspect_f20_a_t4_s1_S5_B0_r270_el_peak()
    {
      Test_Execute("mandril2_p30_t30_r60_zc.jpg", "mandril2_p30_t30_r60_zc_unperspect_f20_a_t4_s1_S5_B0_r270_el_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)20;
        script.Rotation = UnperspectiveRotation.Rotate270;
      });
    }

    [TestMethod]
    public void Test_Execute_mandril2_pm30_t30_r30_zc_unperspect_f20_a_t4_s1_S5_B0_r0_el_peak()
    {
      Test_Execute("mandril2_pm30_t30_r30_zc.jpg", "mandril2_pm30_t30_r30_zc_unperspect_f20_a_t4_s1_S5_B0_r0_el_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)20;
      });
    }

    [TestMethod]
    public void Test_Execute_mandril2_pm30_t30_r30_zc_unperspect_f52_a_t8_s2_S5_B0_r0_el_peak()
    {
      Test_Execute("mandril2_pm30_t30_r30_zc.jpg", "mandril2_pm30_t30_r30_zc_unperspect_f52_a_t8_s2_S5_B0_r0_el_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)52;
        script.Threshold = 8;
        script.Smooth = 2;
      });
    }

    [TestMethod]
    public void Test_Execute_mandril2_pm30_t30_r30_zc_unperspect_f20_a_t4_s1_S5_B0_r0_bh_peak()
    {
      Test_Execute("mandril2_pm30_t30_r30_zc.jpg", "mandril2_pm30_t30_r30_zc_unperspect_f20_a_t4_s1_S5_B0_r0_bh_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)15;
        script.Default = UnperspectiveDefault.BoundingBoxHeight;
      });
    }

    [TestMethod]
    public void Test_Execute_mandril2_pm30_t30_r30_zc_unperspect_f20_a_t4_s1_S5_B0_r0_h_peak()
    {
      Test_Execute("mandril2_pm30_t30_r30_zc.jpg", "mandril2_pm30_t30_r30_zc_unperspect_f20_a_t4_s1_S5_B0_r0_h_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)20;
        script.Default = UnperspectiveDefault.Height;
      });
    }

    [TestMethod]
    public void Test_Execute_mandril2_round30_p30_t30_out_unperspect_f20_a_t4_s2_S5_B0_r0_el_peak()
    {
      Test_Execute("mandril2_round30_p30_t30_out.jpg", "mandril2_round30_p30_t30_out_unperspect_f20_a_t4_s2_S5_B0_r0_el_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)20;
        script.Smooth = 2;
        script.DisableViewportCrop = true;
      });
    }

    [TestMethod]
    public void Test_Execute_monet2_p30_t30_r30_out_unperspect_f10_a_t4_s1_S5_B0_r0_el_peak()
    {
      Test_Execute("monet2_p30_t30_r30_out.jpg", "monet2_p30_t30_r30_out_unperspect_f10_a_t4_s1_S5_B0_r0_el_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)10;
      });
    }

    [TestMethod]
    public void Test_Execute_redcanoe_p30_t30_out_unperspect_f10_a_t4_s1_S5_B0_r0_el_peak()
    {
      Test_Execute("redcanoe_p30_t30_out.jpg", "redcanoe_p30_t30_out_unperspect_f10_a_t4_s1_S5_B0_r0_el_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)10;
      });
    }

    [TestMethod]
    public void Test_Execute_receipt1_unperspect_f7_a_t4_s1_S5_B0_r0_el_peak()
    {
      Test_Execute("receipt1.jpg", "receipt1_unperspect_f7_a_t4_s1_S5_B0_r0_el_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)7;
        script.DisableViewportCrop = true;
      });
    }

    [TestMethod]
    public void Test_Execute_receipt1_unperspect_f20_a_t4_s1_S5_B0_r0_el_peak()
    {
      Test_Execute("receipt1.jpg", "receipt1_unperspect_f20_a_t4_s1_S5_B0_r0_el_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)20;
        script.DisableViewportCrop = true;
      });
    }

    [TestMethod]
    public void Test_Execute_receipt1_unperspect_f20_a_t4_s1_S5_B0_r0_el_peak_w500()
    {
      Test_Execute("receipt1.jpg", "receipt1_unperspect_f20_a_t4_s1_S5_B0_r0_el_peak_w500.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)20;
        script.Width = 500;
        script.DisableViewportCrop = true;
      });
    }

    [TestMethod]
    public void Test_Execute_receipt2_unperspect_f50_a_t4_s1_S5_B0_r0_el_peak()
    {
      Test_Execute("receipt2.jpg", "receipt2_unperspect_f50_a_t4_s1_S5_B0_r0_el_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)50;
      });
    }

    [TestMethod]
    public void Test_Execute_receipt2_unperspect_f50_a_t4_s1_S5_B0_r0_el_peak_w200()
    {
      Test_Execute("receipt2.jpg", "receipt2_unperspect_f50_a_t4_s1_S5_B0_r0_el_peak_w200.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)50;
        script.Width = 200;
      });
    }

    [TestMethod]
    public void Test_Execute_textsample_localthresh_m1_r25_b5_white_b20_p30_t30_out_unperspect27_f11_a_t10_s2_S0_B3_r0_el_peak()
    {
      Test_Execute("textsample_localthresh_m1_r25_b5_white_b20_p30_t30_out.png", "textsample_localthresh_m1_r25_b5_white_b20_p30_t30_out_unperspect27_f11_a_t10_s2_S0_B3_r0_el_peak.jpg", (UnperspectiveScript script) =>
      {
        script.ColorFuzz = (Percentage)11;
        script.Threshold = 10;
        script.Smooth = 2;
        script.Sharpen = 0;
        script.Blur = 3;
      });
    }

    [TestMethod]
    public void Test_Execute_Null()
    {
      ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () =>
      {
        var script = new UnperspectiveScript();
        script.Execute(null);
      });
    }

    [TestMethod]
    public void Test_Settings()
    {
      var script = new UnperspectiveScript();

      using (var logo = new MagickImage(Images.Logo))
      {
        ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
        {
          script.BorderColorLocation = new PointD(-1, -1);
          script.Execute(logo);
        });

        ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
        {
          script.BorderColorLocation = new PointD(logo.Width, logo.Height);
          script.Execute(logo);
        });
      }
    }

    private static void Test_Defaults(UnperspectiveScript script, UnperspectiveMethod method)
    {
      Assert.AreEqual(null, script.AspectRatio);
      Assert.AreEqual(0, script.BorderColorLocation.X);
      Assert.AreEqual(0, script.BorderColorLocation.Y);
      Assert.AreEqual(0, script.Blur);
      Assert.AreEqual(UnperspectiveDefault.EdgeLength, script.Default);
      Assert.AreEqual(false, script.DisableViewportCrop);
      Assert.AreEqual(null, script.Height);
      Assert.AreEqual(10, script.MinLength);
      Assert.AreEqual(40, script.MaxPeaks);
      Assert.AreEqual(null, script.Rotation);
      Assert.AreEqual(null, script.Width);

      if (method == UnperspectiveMethod.Peak)
      {
        Assert.AreEqual(5.0, script.Sharpen);
        Assert.AreEqual(1.0, script.Smooth);
        Assert.AreEqual(4, script.Threshold);
      }
      else
      {
        Assert.AreEqual(0.0, script.Sharpen);
        Assert.AreEqual(5.0, script.Smooth);
        Assert.AreEqual(10, script.Threshold);
      }
    }

    private static void Test_Defaults(UnperspectiveMethod method)
    {
      var script = new UnperspectiveScript(method);
      Test_Defaults(script, method);

      script.AspectRatio = 1.5;
      script.BorderColorLocation = new PointD(10, 10);
      script.Blur = 15.0;
      script.ColorFuzz = (Percentage)5.0;
      script.Default = UnperspectiveDefault.BoundingBoxWidth;
      script.DisableViewportCrop = true;
      script.Height = 140;
      script.MaxPeaks = 25;
      script.MinLength = 5;
      script.Rotation = UnperspectiveRotation.Rotate180;
      script.Sharpen = 8.5;
      script.Smooth = 2.0;
      script.Threshold = 5;
      script.Width = 50;

      script.Reset();
      Test_Defaults(script, method);
    }

    private void Test_Execute(string fileName, string output, Action<UnperspectiveScript> action)
    {
      var input = GetInputFile(fileName);
      /* LosslessCompress(input); */

      using (var inputImage = new MagickImage(input))
      {
        var script = new UnperspectiveScript();
        action(script);

        var scriptOutput = script.Execute(inputImage);
        AssertOutput(scriptOutput, output);
      }
    }
  }
}