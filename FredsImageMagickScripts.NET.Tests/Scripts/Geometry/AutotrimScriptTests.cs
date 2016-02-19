//=================================================================================================
// Copyright 2015 Dirk Lemstra, Fred Weinhaus
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
  public class AutotrimScriptTests : ScriptTester
  {
    private const string _Category = "AutotrimScript";

    private static void Test_Defaults(AutotrimScript script)
    {
      Assert.AreEqual(0, script.BorderColorLocation.X);
      Assert.AreEqual(0, script.BorderColorLocation.Y);
      Assert.AreEqual((Percentage)0, script.ColorFuzz);
      Assert.AreEqual(false, script.InnerTrim);
      Assert.AreEqual(0, script.PixelShift.Left);
      Assert.AreEqual(0, script.PixelShift.Top);
      Assert.AreEqual(0, script.PixelShift.Right);
      Assert.AreEqual(0, script.PixelShift.Bottom);
    }

    private void Test_Execute(string input, Action<AutotrimScript> action, string output)
    {
      string inputFile = GetInputFile(input);

      using (var image = new MagickImage(inputFile))
      {
        var script = new AutotrimScript();
        action(script);

        var scriptOutput = script.Execute(image);
        TestOutput(scriptOutput, output);
      }
    }

    private void Test_Execute_Trim_Of_Square_Image_With_Uniform_Border_Color()
    {
      Test_Execute("zelda3_border2w.png", (AutotrimScript script) =>
      {
      }, "zelda3_border_crop.png");

      Test_Execute("zelda3_border2b.png", (AutotrimScript script) =>
      {
      }, "zelda3_border_crop.png");

      Test_Execute("zelda3_border2wrt.png", (AutotrimScript script) =>
      {
        script.BorderColorLocation = new PointD(129, 0);
      }, "zelda3_border_crop.png");

      Test_Execute("zelda3_border2brt.png", (AutotrimScript script) =>
      {
        script.BorderColorLocation = new PointD(129, 0);
      }, "zelda3_border_crop.png");
    }

    private void Test_Execute_Trim_Of_Square_Image_With_NonUniform_Border_Color()
    {
      Test_Execute("zelda3_radborder.png", (AutotrimScript script) =>
      {
        script.ColorFuzz = (Percentage)30;
      }, "zelda3_radborder_crop_fuzz30.png");

      Test_Execute("zelda3_radborder.png", (AutotrimScript script) =>
      {
        script.ColorFuzz = (Percentage)35;
      }, "zelda3_radborder_crop_fuzz35.png");
      ;

      Test_Execute("zelda3_radborder.png", (AutotrimScript script) =>
      {
        script.ColorFuzz = (Percentage)40;
      }, "zelda3_radborder_crop_fuzz40.png");
      ;

      Test_Execute("zelda3_radborder.png", (AutotrimScript script) =>
      {
        script.ColorFuzz = (Percentage)60;
      }, "zelda3_radborder_crop_fuzz60.png");
    }

    private void Test_Execute_Trim_Of_Rotated_Image_To_Bounding_Region()
    {
      Test_Execute("zelda3_rot20_border10.png", (AutotrimScript script) =>
      {
      }, "zelda3_rot20_border10_crop.png");
    }

    private void Test_Execute_Trim_To_Center_Area_Of_Rotated_Square_Image()
    {
      Test_Execute("zelda3_rot45.png", (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      }, "zelda3_rot45_crop.png");

      Test_Execute("zelda3_rotm20.png", (AutotrimScript script) =>
      {
        script.InnerTrim = true;
        script.ColorFuzz = (Percentage)1;
      }, "zelda3_rotm20_crop_fuzz1.png");

      Test_Execute("zelda3_rot10.png", (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      }, "zelda3_rot10_crop.png");

      Test_Execute("zelda3_rotm5.png", (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      }, "zelda3_rotm5_crop.png");

      Test_Execute("zelda3_rot2.png", (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      }, "zelda3_rot2_crop.png");
    }

    private void Test_Trim_To_Center_Area_Of_Rotated_Rectangular_Image()
    {
      Test_Execute("image3s_rot45.png", (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      }, "image3s_rot45_crop.png");

      Test_Execute("image3s_rotm20.png", (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      }, "image3s_rotm20_crop.png");

      Test_Execute("image3s_rot10.png", (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      }, "image3s_rot10_crop.png");

      Test_Execute("image3s_rotm5.png", (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      }, "image3s_rotm5_crop.png");
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_Defaults()
    {
      var script = new AutotrimScript();
      Test_Defaults(script);

      script.BorderColorLocation = new PointD(10, 10);
      script.ColorFuzz = (Percentage)6;
      script.InnerTrim = true;
      script.PixelShift.Left = 5;
      script.PixelShift.Top = 4;
      script.PixelShift.Right = 3;
      script.PixelShift.Bottom = 2;

      script.Reset();
      Test_Defaults(script);
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_Execute()
    {
      Test_Execute_Trim_Of_Square_Image_With_Uniform_Border_Color();
      Test_Execute_Trim_Of_Square_Image_With_NonUniform_Border_Color();
      Test_Execute_Trim_Of_Rotated_Image_To_Bounding_Region();
      Test_Execute_Trim_To_Center_Area_Of_Rotated_Square_Image();
      Test_Trim_To_Center_Area_Of_Rotated_Rectangular_Image();
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_Execute_Null()
    {
      ExceptionAssert.ThrowsArgumentException<ArgumentNullException>(() =>
      {
        var script = new AutotrimScript();
        script.Execute(null);
      }, "input");
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_Settings()
    {
      var script = new AutotrimScript();

      using (var logo = new MagickImage(Images.Logo))
      {
        ExceptionAssert.Throws<ArgumentException>(() =>
        {
          script.BorderColorLocation = new PointD(-1, -1);
          script.Execute(logo);
        });

        ExceptionAssert.Throws<ArgumentException>(() =>
        {
          script.BorderColorLocation = new PointD(logo.Width, logo.Height);
          script.Execute(logo);
        });
      }
    }
  }
}