// <copyright file="TshirtScriptTests.cs" company="Dirk Lemstra, Fred Weinhaus">
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
  public class TshirtScriptTests : ScriptTester
  {
    private PointD _topLeft = new PointD(10, 15);
    private PointD _topRight = new PointD(630, 10);
    private PointD _bottomLeft = new PointD(25, 470);
    private PointD _bottomRight = new PointD(630, 470);

    [TestMethod]
    public void Constructor_SettingsSetToDefaults()
    {
      var script = new TshirtScript();

      AssertDefaults(script);
    }

    [TestMethod]
    public void Reset_AllSettingsChanged_RestoredToDefault()
    {
      var script = new TshirtScript();
      script.AntiAlias = 1.0;
      script.Blur = 5;
      script.Displace = 59;
      script.Fit = TshirtFit.Crop;
      script.Gravity = Gravity.North;
      script.Rotation = 0;
      script.Sharpen = 7;
      script.VerticalShift = -10;

      script.Reset();

      AssertDefaults(script);
    }

    [TestMethod]
    public void Blur_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid blur specified, value should be zero or higher.", (TshirtScript script) =>
      {
        script.Blur = -1;
      });
    }

    [TestMethod]
    public void Gravity_InvalidValue_ThrowsException()
    {
      AssertInvalidOperation("Invalid gravity specified.", (TshirtScript script) =>
      {
        script.Gravity = Gravity.Forget;
      });
    }

    [TestMethod]
    public void Lighting_BelowZero_ThrowsException()
    {
      AssertInvalidOperation("Invalid lighting specified, value must be between 0 and 30.", (TshirtScript script) =>
      {
        script.Lighting = -1;
      });
    }

    [TestMethod]
    public void Lighting_Above30_ThrowsException()
    {
      AssertInvalidOperation("Invalid lighting specified, value must be between 0 and 30.", (TshirtScript script) =>
      {
        script.Lighting = 31;
      });
    }

    [TestMethod]
    public void Rotation_BelowMinus360_ThrowsException()
    {
      AssertInvalidOperation("Invalid rotation specified, value must be between -360 and 360.", (TshirtScript script) =>
      {
        script.Rotation = -361;
      });
    }

    [TestMethod]
    public void Rotation_AboveMinus360_ThrowsException()
    {
      AssertInvalidOperation("Invalid rotation specified, value must be between -360 and 360.", (TshirtScript script) =>
      {
        script.Rotation = 361;
      });
    }

    [TestMethod]
    public void SetCoordinates_GeometryNull_ThrowsException()
    {
      ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("geometry", () =>
      {
        TshirtScript script = new TshirtScript();
        script.SetCoordinates(null);
      });
    }

    [TestMethod]
    public void Execute_NoCoordinatesSet_ThrowsException()
    {
      ExceptionAssert.Throws<InvalidOperationException>("No coordinates have been set.", () =>
      {
        using (var logo = Images.Logo)
        {
          TshirtScript script = new TshirtScript();
          script.Execute(logo, logo);
        }
      });
    }

    [TestMethod]
    public void Excecute_OverlayNull_ThrowsException()
    {
      using (var logo = Images.Logo)
      {
        ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("overlay", () =>
        {
          TshirtScript script = new TshirtScript();
          script.Execute(logo, null);
        });
      }
    }

    [TestMethod]
    public void Excecute_TshirtNull_ThrowsException()
    {
      using (var logo = Images.Logo)
      {
        ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("tshirt", () =>
        {
          TshirtScript script = new TshirtScript();
          script.Execute(null, logo);
        });
      }
    }

    [TestMethod]
    public void Execute_InvalidCoordinates_ThrowsException()
    {
      var invalid = new PointD[]
      {
        new PointD(-10, 10), new PointD(10, -10),
        new PointD(650, 10), new PointD(630, 490)
      };

      for (int i = 0; i < invalid.Length; i++)
      {
        AssertCoordinates("topLeft", invalid[i], _topRight, _bottomLeft, _bottomRight);
        AssertCoordinates("topRight", _topLeft, invalid[i], _bottomLeft, _bottomRight);
        AssertCoordinates("bottomRight", _topLeft, _topRight, invalid[i], _bottomRight);
        AssertCoordinates("bottomLeft", _topLeft, _topRight, _bottomLeft, invalid[i]);
      }
    }

    [TestMethod]
    public void Execute_blue_jpg()
    {
      AssertExecute("tshirt_blue.jpg", "flowers_van_gogh.jpg", nameof(Execute_blue_jpg), (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
      });
    }

    [TestMethod]
    public void Execute_gray_jpg()
    {
      AssertExecute("tshirt_gray.jpg", "flowers_van_gogh.jpg", nameof(Execute_gray_jpg), (TshirtScript script) =>
      {
        var topLeft = new PointD(275, 175);
        var topRight = new PointD(404, 175);
        var bottomRight = new PointD(404, 304);
        var bottomLeft = new PointD(275, 304);

        script.SetCoordinates(topLeft, topRight, bottomRight, bottomLeft);
      });

      AssertExecute("tshirt_gray.jpg", "flowers_van_gogh.jpg", nameof(Execute_gray_jpg), (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
      });
    }

    [TestMethod]
    public void Execute_gray_fc_gn_jpg()
    {
      AssertExecute("tshirt_gray.jpg", "flowers_van_gogh.jpg", nameof(Execute_gray_fc_gn_jpg), (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Fit = TshirtFit.Crop;
        script.Gravity = Gravity.North;
      });
    }

    [TestMethod]
    public void Execute_gray_fc_gc_jpg()
    {
      AssertExecute("tshirt_gray.jpg", "flowers_van_gogh.jpg", nameof(Execute_gray_fc_gc_jpg), (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Fit = TshirtFit.Crop;
        script.Gravity = Gravity.Center;
      });
    }

    [TestMethod]
    public void Execute_gray_fd_jpg()
    {
      AssertExecute("tshirt_gray.jpg", "flowers_van_gogh.jpg", nameof(Execute_gray_fd_jpg), (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Fit = TshirtFit.Distort;
      });
    }

    [TestMethod]
    public void Execute_gray_rm3_jpg()
    {
      AssertExecute("tshirt_gray.jpg", "flowers_van_gogh.jpg", nameof(Execute_gray_rm3_jpg), (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Rotation = -3;
      });

      AssertExecute("tshirt_gray.jpg", "Super_Mario.png", nameof(Execute_gray_rm3_jpg), (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Rotation = -3;
      });
    }

    private static void AssertDefaults(TshirtScript script)
    {
      Assert.AreEqual(2.0, script.AntiAlias);
      Assert.AreEqual(1.0, script.Blur);
      Assert.AreEqual(10, script.Displace);
      Assert.AreEqual(TshirtFit.None, script.Fit);
      Assert.AreEqual(Gravity.Center, script.Gravity);
      Assert.AreEqual(20, script.Lighting);
      Assert.AreEqual(0, script.Rotation);
      Assert.AreEqual(1.0, script.Sharpen);
      Assert.AreEqual(0, script.VerticalShift);
    }

    private static void AssertCoordinates(string paramName, PointD topLeft, PointD topRight, PointD bottomLeft, PointD bottomRight)
    {
      ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>(paramName, () =>
      {
        using (MagickImage logo = Images.Logo)
        {
          TshirtScript script = new TshirtScript();
          script.SetCoordinates(topLeft, topRight, bottomLeft, bottomRight);
          script.Execute(logo, logo);
        }
      });
    }

    private void AssertInvalidOperation(string expectedMessage, Action<TshirtScript> initAction)
    {
      var script = new TshirtScript();

      using (var logo = new MagickImage(Images.Logo))
      {
        initAction(script);

        ExceptionAssert.Throws<InvalidOperationException>(expectedMessage, () =>
        {
          script.SetCoordinates(_topLeft, _topRight, _bottomLeft, _bottomRight);
          script.Execute(logo, logo);
        });
      }
    }

    private void AssertExecute(string tshirt, string overlay, string methodName, Action<TshirtScript> action)
    {
      var tshirtFile = GetInputFile(tshirt);
      /* LosslessCompress(tshirtFile); */

      var overlayFile = GetInputFile(overlay);
      /* LosslessCompress(overlayFile); */

      using (var tshirtImage = new MagickImage(tshirtFile))
      {
        using (var overlayImage = new MagickImage(overlayFile))
        {
          var script = new TshirtScript();
          action(script);

          using (var scriptOutput = script.Execute(tshirtImage, overlayImage))
          {
            string outputFile = GetOutputFile(overlay, methodName);
            AssertOutput(scriptOutput, outputFile);
          }
        }
      }
    }
  }
}
