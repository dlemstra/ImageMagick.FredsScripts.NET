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
  public class DraganEffectScriptTests : ScriptTester
  {
    private const string _Category = "DraganEffectScript";

    private static void Test_Defaults(DraganEffectScript script)
    {
      Assert.AreEqual(1.0, script.Brightness);
      Assert.AreEqual(0.0, script.Contrast);
      Assert.AreEqual(1.0, script.Darkness);
      Assert.AreEqual((Percentage)150, script.Saturation);
    }

    private void Test_Execute(string input, Action<DraganEffectScript> action, string output)
    {
      string inputFile = GetInputFile(input);

      using (MagickImage image = new MagickImage(inputFile))
      {
        DraganEffectScript script = new DraganEffectScript();
        action(script);

        MagickImage scriptOutput = script.Execute(image);
        TestOutput(scriptOutput, output);
      }
    }

    private void Test_Execute_Before1()
    {
      Test_Execute("before1.gif", (DraganEffectScript script) =>
      {
        script.Brightness = 1;
        script.Contrast = 0;
        script.Darkness = 1;
        script.Saturation = (Percentage)150;
      }, "before1_draganeffect_b1_c0_d1_s150_r5.jpg");

      Test_Execute("before1.gif", (DraganEffectScript script) =>
      {
        script.Brightness = 1.5;
        script.Contrast = -5;
        script.Darkness = 1;
        script.Saturation = (Percentage)175;
      }, "before1_draganeffect_b1p5_cm5_d1_s175_r5.jpg");

      Test_Execute("before1.gif", (DraganEffectScript script) =>
      {
        script.Brightness = 1.5;
        script.Contrast = -5;
        script.Darkness = 2;
        script.Saturation = (Percentage)175;
      }, "before1_draganeffect_b1p5_cm5_d2_s175_r5.jpg");
    }

    private void Test_Execute_Bluehat()
    {
      Test_Execute("bluehat.jpg", (DraganEffectScript script) =>
      {
        script.Brightness = 1;
        script.Contrast = 0;
        script.Darkness = 1;
        script.Saturation = (Percentage)150;
      }, "bluehat_draganeffect_b1_c0_d1_s150_r5.jpg");

      Test_Execute("bluehat.jpg", (DraganEffectScript script) =>
      {
        script.Brightness = 1;
        script.Contrast = -7.5;
        script.Darkness = 1;
        script.Saturation = (Percentage)200;
      }, "bluehat_draganeffect_b1_cm7p5_d1_s200_r5.jpg");

      Test_Execute("bluehat.jpg", (DraganEffectScript script) =>
      {
        script.Brightness = 1;
        script.Contrast = -7.5;
        script.Darkness = 1.25;
        script.Saturation = (Percentage)200;
      }, "bluehat_draganeffect_b1_cm7p5_d1p25_s200_r5.jpg");
    }

    private void Test_Execute_China()
    {
      Test_Execute("CHINA-715-4_small.jpg", (DraganEffectScript script) =>
      {
        script.Brightness = 1;
        script.Contrast = -5;
        script.Darkness = 1;
        script.Saturation = (Percentage)150;
      }, "CHINA-715-4_small_draganeffect_b1_cm5_d1_s150_r5.jpg");

      Test_Execute("CHINA-715-4_small.jpg", (DraganEffectScript script) =>
      {
        script.Contrast = -10;
        script.Darkness = 1;
        script.Saturation = (Percentage)200;
      }, "CHINA-715-4_small_draganeffect_b1_cm10_d1_s200_r5.jpg");
    }

    private void Test_Execute_Mustache()
    {
      Test_Execute("mustache.jpg", (DraganEffectScript script) =>
      {
        script.Brightness = 1;
        script.Contrast = -5;
        script.Darkness = 1.75;
        script.Saturation = (Percentage)175;
      }, "mustache_draganeffect_b1_cm5_d1p75_s175_r5.jpg");
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_Defaults()
    {
      DraganEffectScript script = new DraganEffectScript();
      Test_Defaults(script);

      script.Brightness = 0.5;
      script.Contrast = 4;
      script.Darkness = 2;
      script.Saturation = (Percentage)100;

      script.Reset();
      Test_Defaults(script);
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_Execute()
    {
      Test_Execute_Before1();
      Test_Execute_Bluehat();
      Test_Execute_China();
      Test_Execute_Mustache();
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_Execute_Null()
    {
      ExceptionAssert.ThrowsArgumentException<ArgumentNullException>(() =>
      {
        DraganEffectScript script = new DraganEffectScript();
        script.Execute(null);
      }, "input");
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_Settings()
    {
      DraganEffectScript script = new DraganEffectScript();

      using (MagickImage logo = new MagickImage(Images.Logo))
      {
        script.Execute(logo);

        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Brightness = -1.0;
          script.Execute(logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Reset();
          script.Contrast = -11.0;
          script.Execute(logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Reset();
          script.Contrast = 11.0;
          script.Execute(logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Reset();
          script.Darkness = 0.0;
          script.Execute(logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>(() =>
        {
          script.Reset();
          script.Saturation = (Percentage)(-1);
          script.Execute(logo);
        });
      }
    }
  }
}