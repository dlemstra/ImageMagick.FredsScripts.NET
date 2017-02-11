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
    public void Test_Defaults()
    {
      var script = new TshirtScript();
      Test_Defaults(script);

      script.AntiAlias = 1.0;
      script.Blur = 5;
      script.Displace = 59;
      script.Fit = TshirtFit.Crop;
      script.Gravity = Gravity.North;
      script.Rotation = 0;
      script.Sharpen = 7;
      script.VerticalShift = -10;

      script.Reset();
      Test_Defaults(script);
    }

    [TestMethod]
    public void Test_Execute_tshirt_blue_flowers_none_r()
    {
      Test_Execute("tshirt_blue.jpg", "flowers_van_gogh.jpg", "tshirt_blue_flowers_none_r.jpg", (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
      });
    }

    [TestMethod]
    public void Test_Execute_tshirt_gray_flowers_none_r()
    {
      Test_Execute("tshirt_gray.jpg", "flowers_van_gogh.jpg", "tshirt_gray_flowers_none_r.jpg", (TshirtScript script) =>
      {
        var topLeft = new PointD(275, 175);
        var topRight = new PointD(404, 175);
        var bottomRight = new PointD(404, 304);
        var bottomLeft = new PointD(275, 304);

        script.SetCoordinates(topLeft, topRight, bottomRight, bottomLeft);
      });
    }

    [TestMethod]
    public void Test_Execute_tshirt_gray_flowers_none_r2()
    {
      Test_Execute("tshirt_gray.jpg", "flowers_van_gogh.jpg", "tshirt_gray_flowers_none_r.jpg", (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
      });
    }

    [TestMethod]
    public void Test_Execute_tshirt_gray_flowers_crop_north()
    {
      Test_Execute("tshirt_gray.jpg", "flowers_van_gogh.jpg", "tshirt_gray_flowers_crop_north.jpg", (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Fit = TshirtFit.Crop;
        script.Gravity = Gravity.North;
      });
    }

    [TestMethod]
    public void Test_Execute_tshirt_gray_flowers_crop_center()
    {
      Test_Execute("tshirt_gray.jpg", "flowers_van_gogh.jpg", "tshirt_gray_flowers_crop_center.jpg", (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Fit = TshirtFit.Crop;
        script.Gravity = Gravity.Center;
      });
    }

    [TestMethod]
    public void Test_Execute_tshirt_gray_flowers_distort()
    {
      Test_Execute("tshirt_gray.jpg", "flowers_van_gogh.jpg", "tshirt_gray_flowers_distort.jpg", (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Fit = TshirtFit.Distort;
      });
    }

    [TestMethod]
    public void Test_Execute_tshirt_gray_flowers_none_rm3()
    {
      Test_Execute("tshirt_gray.jpg", "flowers_van_gogh.jpg", "tshirt_gray_flowers_none_rm3.jpg", (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Rotation = -3;
      });
    }

    [TestMethod]
    public void Test_Execute_tshirt_gray_mario_none_rm3()
    {
      Test_Execute("tshirt_gray.jpg", "Super_Mario.png", "tshirt_gray_mario_none_rm3.jpg", (TshirtScript script) =>
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Rotation = -3;
      });
    }

    [TestMethod]
    public void Test_Excecute_Null()
    {
      using (var logo = Images.Logo)
      {
        ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("tshirt", () =>
        {
          TshirtScript script = new TshirtScript();
          script.Execute(null, logo);
        });

        ExceptionAssert.ThrowsArgumentException<ArgumentNullException>("overlay", () =>
        {
          TshirtScript script = new TshirtScript();
          script.Execute(logo, null);
        });
      }
    }

    [TestMethod]
    public void Test_Coordinates()
    {
      ExceptionAssert.Throws<InvalidOperationException>("No coordinates have been set.", () =>
      {
        using (var logo = Images.Logo)
        {
          TshirtScript script = new TshirtScript();
          script.Execute(logo, logo);
        }
      });

      var invalid = new PointD[]
      {
        new PointD(-10, 10), new PointD(10, -10),
        new PointD(650, 10), new PointD(630, 490)
      };

      for (int i = 0; i < invalid.Length; i++)
      {
        Test_SetCoordinates("topLeft", invalid[i], _topRight, _bottomLeft, _bottomRight);
        Test_SetCoordinates("topRight", _topLeft, invalid[i], _bottomLeft, _bottomRight);
        Test_SetCoordinates("bottomLeft", _topLeft, _topRight, invalid[i], _bottomRight);
        Test_SetCoordinates("bottomRight", _topLeft, _topRight, _bottomLeft, invalid[i]);
      }
    }

    [TestMethod]
    public void Test_Settings()
    {
      var script = new TshirtScript();

      using (var logo = Images.Logo)
      {
        Reset(script);
        script.Execute(logo, logo);

        ExceptionAssert.Throws<InvalidOperationException>("Invalid Gravity specified.", () =>
        {
          Reset(script);
          script.Gravity = Gravity.Forget;
          script.Execute(logo, logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>("Invalid Rotation specified.", () =>
        {
          Reset(script);
          script.Rotation = -361;
          script.Execute(logo, logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>("Invalid Rotation specified.", () =>
        {
          Reset(script);
          script.Rotation = 361;
          script.Execute(logo, logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>("Invalid Lightning specified.", () =>
        {
          Reset(script);
          script.Lighting = -1;
          script.Execute(logo, logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>("Invalid Lightning specified.", () =>
        {
          Reset(script);
          script.Lighting = 31;
          script.Execute(logo, logo);
        });

        ExceptionAssert.Throws<InvalidOperationException>("Invalid Blur specified.", () =>
        {
          Reset(script);
          script.Blur = -1;
          script.Execute(logo, logo);
        });
      }
    }

    private static void Test_Defaults(TshirtScript script)
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

    private static void Test_SetCoordinates(string paramName, PointD topLeft, PointD topRight, PointD bottomLeft, PointD bottomRight)
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

    private void Reset(TshirtScript script)
    {
      script.Reset();
      script.SetCoordinates(_topLeft, _topRight, _bottomLeft, _bottomRight);
    }

    private void Test_Execute(string tshirt, string overlay, string output, Action<TshirtScript> action)
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

          var scriptOutput = script.Execute(tshirtImage, overlayImage);
          TestOutput(scriptOutput, output);
        }
      }
    }
  }
}
