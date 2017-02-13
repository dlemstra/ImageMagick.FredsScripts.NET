// <copyright file="AutotrimScriptTests.cs" company="Dirk Lemstra, Fred Weinhaus">
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
  public class AutotrimScriptTests : ScriptTester
  {
    [TestMethod]
    public void Constructor_SettingsSetToDefaults()
    {
      var script = new AutotrimScript();

      AssertDefaults(script);
    }

    [TestMethod]
    public void Reset_AllSettingsChanged_RestoredToDefault()
    {
      var script = new AutotrimScript();
      script.BorderColorLocation = new PointD(10, 10);
      script.ColorFuzz = (Percentage)6;
      script.InnerTrim = true;
      script.PixelShift.Left = 5;
      script.PixelShift.Top = 4;
      script.PixelShift.Right = 3;
      script.PixelShift.Bottom = 2;

      script.Reset();

      AssertDefaults(script);
    }

    [TestMethod]
    public void BorderColorLocation_BeforeLeftTop_ThrowsException()
    {
      var script = new AutotrimScript();

      using (var logo = new MagickImage(Images.Logo))
      {
        script.BorderColorLocation = new PointD(-1, -1);

        // TODO: Fix this after the next release of Magick.NET
        // ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>("x", "Invalid X coordinate: -1", () =>
        ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
        {
          script.Execute(logo);
        });
      }
    }

    [TestMethod]
    public void BorderColorLocation_AfterBottomRight_ThrowsException()
    {
      var script = new AutotrimScript();

      using (var logo = new MagickImage(Images.Logo))
      {
        script.BorderColorLocation = new PointD(logo.Width, logo.Height);

        // TODO: Fix this after the next release of Magick.NET
        // ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>("x", "Invalid X coordinate: " + logo.Width, () =>
        ExceptionAssert.Throws<ArgumentOutOfRangeException>(() =>
        {
          script.Execute(logo);
        });
      }
    }

    [TestMethod]
    public void Execute_InputNull_ThrowsException()
    {
      ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("input", () =>
      {
        var script = new AutotrimScript();
        script.Execute(null);
      });
    }

    [TestMethod]
    public void Execute_zelda3_border()
    {
      AssertExecuteWithFilename("zelda3_border2w.png", "zelda3_border.png", (AutotrimScript script) =>
      {
      });

      AssertExecuteWithFilename("zelda3_border2b.png", "zelda3_border.png", (AutotrimScript script) =>
      {
      });

      AssertExecuteWithFilename("zelda3_border2wrt.png", "zelda3_border.png", (AutotrimScript script) =>
      {
        script.BorderColorLocation = new PointD(129, 0);
      });

      AssertExecuteWithFilename("zelda3_border2brt.png", "zelda3_border.png", (AutotrimScript script) =>
      {
        script.BorderColorLocation = new PointD(129, 0);
      });
    }

    [TestMethod]
    public void Execute_zelda3_rot20_border10()
    {
      AssertExecuteWithFilename("zelda3_rot20_border10.png", "zelda3_rot20_border10.png", (AutotrimScript script) =>
      {
      });
    }

    [TestMethod]
    public void Execute_f30_png()
    {
      AssertExecute("zelda3_radborder.png", nameof(Execute_f30_png), (AutotrimScript script) =>
      {
        script.ColorFuzz = (Percentage)30;
      });
    }

    [TestMethod]
    public void Execute_f35_png()
    {
      AssertExecute("zelda3_radborder.png", nameof(Execute_f35_png), (AutotrimScript script) =>
      {
        script.ColorFuzz = (Percentage)35;
      });
    }

    [TestMethod]
    public void Execute_f40_png()
    {
      AssertExecute("zelda3_radborder.png", nameof(Execute_f40_png), (AutotrimScript script) =>
      {
        script.ColorFuzz = (Percentage)40;
      });
    }

    [TestMethod]
    public void Execute_f60_png()
    {
      AssertExecute("zelda3_radborder.png", nameof(Execute_f60_png), (AutotrimScript script) =>
      {
        script.ColorFuzz = (Percentage)60;
      });
    }

    [TestMethod]
    public void Execute_i_png()
    {
      AssertExecute("zelda3_rot10.png", nameof(Execute_i_png), (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      });

      AssertExecute("zelda3_rotm5.png", nameof(Execute_i_png), (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      });

      AssertExecute("zelda3_rot2.png", nameof(Execute_i_png), (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      });

      AssertExecute("zelda3_rot45.png", nameof(Execute_i_png), (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      });

      AssertExecute("image3s_rot45.png", nameof(Execute_i_png), (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      });

      AssertExecute("image3s_rotm20.png", nameof(Execute_i_png), (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      });

      AssertExecute("image3s_rot10.png", nameof(Execute_i_png), (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      });

      AssertExecute("image3s_rotm5.png", nameof(Execute_i_png), (AutotrimScript script) =>
      {
        script.InnerTrim = true;
      });
    }

    [TestMethod]
    public void Execute_i_f1_png()
    {
      AssertExecute("zelda3_rotm20.png", nameof(Execute_i_f1_png), (AutotrimScript script) =>
      {
        script.InnerTrim = true;
        script.ColorFuzz = (Percentage)1;
      });
    }

    [TestMethod]
    public void Execute_logo_png()
    {
      using (var image = new MagickImage(Images.Logo))
      {
        var script = new AutotrimScript();
        script.InnerTrim = true;

        using (var scriptOutput = script.Execute(image))
        {
          AssertOutput(scriptOutput, "logo.png");
        }
      }
    }

    private static void AssertDefaults(AutotrimScript script)
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

    private void AssertExecute(string input, string methodName, Action<AutotrimScript> action)
    {
      string outputFile = GetOutputFile(input, methodName);
      AssertExecuteWithFilename(input, outputFile, action);
    }

    private void AssertExecuteWithFilename(string input, string output, Action<AutotrimScript> action)
    {
      string inputFile = GetInputFile(input);
      /* LosslessCompress(inputFile); */

      using (var image = new MagickImage(inputFile))
      {
        var script = new AutotrimScript();
        action(script);

        using (var scriptOutput = script.Execute(image))
        {
          AssertOutput(scriptOutput, output);
        }
      }
    }
  }
}