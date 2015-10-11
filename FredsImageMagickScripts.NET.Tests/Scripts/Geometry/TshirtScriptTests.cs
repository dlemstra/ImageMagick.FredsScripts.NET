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

namespace FredsImageMagickScripts.NET.Tests.Scripts.Geometry
{
  [TestClass]
  public class TshirtScriptTests : ScriptTester
  {
    private const string _Category = "TShirtScriptTests";

    private Coordinate _TopLeft = new Coordinate(10, 15);
    private Coordinate _TopRight = new Coordinate(630, 10);
    private Coordinate _BottomLeft = new Coordinate(25, 470);
    private Coordinate _BottomRight = new Coordinate(630, 470);

    private void Reset(TshirtScript script)
    {
      script.Reset();
      script.SetCoordinates(_TopLeft, _TopRight, _BottomLeft, _BottomRight);
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

    private void Test_SetCoordinates(string paramName, Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight)
    {
      ExceptionAssert.ThrowsArgumentException<ArgumentOutOfRangeException>(delegate ()
      {
        using (MagickImage logo = Images.Logo)
        {
          TshirtScript script = new TshirtScript();
          script.SetCoordinates(topLeft, topRight, bottomLeft, bottomRight);
          script.Execute(logo, logo);
        }
      }, paramName);
    }

    private void Test_Execute(string tshirt, string overlay, Action<TshirtScript> action, string output)
    {
      string tshirtFile = GetInputFile(tshirt);
      string overlayFile = GetInputFile(overlay);

      using (MagickImage tshirtImage = new MagickImage(tshirtFile))
      {
        using (MagickImage overlayImage = new MagickImage(overlayFile))
        {
          TshirtScript script = new TshirtScript();
          action(script);

          MagickImage scriptOutput = script.Execute(tshirtImage, overlayImage);
          TestOutput(scriptOutput, output);
        }
      }
    }

    private void Test_Execute_Blue()
    {
      Test_Execute("tshirt_blue.jpg", "flowers_van_gogh.jpg", delegate (TshirtScript script)
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
      }, "tshirt_blue_flowers_none_r.jpg");
    }

    private void Test_Execute_Gray()
    {
      Test_Execute("tshirt_gray.jpg", "flowers_van_gogh.jpg", delegate (TshirtScript script)
      {
        Coordinate topLeft = new Coordinate(275, 175);
        Coordinate topRight = new Coordinate(404, 175);
        Coordinate bottomRight = new Coordinate(404, 304);
        Coordinate bottomLeft = new Coordinate(275, 304);

        script.SetCoordinates(topLeft, topRight, bottomRight, bottomLeft);
      }, "tshirt_gray_flowers_none_r.jpg");

      Test_Execute("tshirt_gray.jpg", "flowers_van_gogh.jpg", delegate (TshirtScript script)
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
      }, "tshirt_gray_flowers_none_r.jpg");

      Test_Execute("tshirt_gray.jpg", "flowers_van_gogh.jpg", delegate (TshirtScript script)
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Fit = TshirtFit.Crop;
        script.Gravity = Gravity.North;
      }, "tshirt_gray_flowers_crop_north.jpg");

      Test_Execute("tshirt_gray.jpg", "flowers_van_gogh.jpg", delegate (TshirtScript script)
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Fit = TshirtFit.Crop;
        script.Gravity = Gravity.Center;
      }, "tshirt_gray_flowers_crop_center.jpg");

      Test_Execute("tshirt_gray.jpg", "flowers_van_gogh.jpg", delegate (TshirtScript script)
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Fit = TshirtFit.Distort;
      }, "tshirt_gray_flowers_distort.jpg");

      Test_Execute("tshirt_gray.jpg", "flowers_van_gogh.jpg", delegate (TshirtScript script)
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
        script.Rotation = -3;
      }, "tshirt_gray_flowers_none_rm3.jpg");
    }

    private void Test_Execute_Gray_Transparent()
    {
      Test_Execute("tshirt_gray.jpg", "Super_Mario.png", delegate (TshirtScript script)
      {
        script.SetCoordinates(new MagickGeometry(275, 175, 130, 130));
      }, "tshirt_gray_mario_none_Rm3_o5x0.jpg");
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_Defaults()
    {
      TshirtScript script = new TshirtScript();
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

    [TestMethod, TestCategory(_Category)]
    public void Test_Execute()
    {
      Test_Execute_Gray();
      Test_Execute_Blue();
      Test_Execute_Gray_Transparent();
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_Excecute_Null()
    {
      using (MagickImage logo = Images.Logo)
      {
        ExceptionAssert.ThrowsArgumentException<ArgumentNullException>(delegate ()
        {
          TshirtScript script = new TshirtScript();
          script.Execute(null, logo);
        }, "tshirt");

        ExceptionAssert.ThrowsArgumentException<ArgumentNullException>(delegate ()
        {
          TshirtScript script = new TshirtScript();
          script.Execute(logo, null);
        }, "overlay");
      }
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_Coordinates()
    {
      ExceptionAssert.Throws<InvalidOperationException>(delegate ()
      {
        using (MagickImage logo = Images.Logo)
        {
          TshirtScript script = new TshirtScript();
          script.Execute(logo, logo);
        }
      }, "No coordinates have been set.");

      Coordinate[] invalid = new Coordinate[]
      {
        new Coordinate(-10, 10), new Coordinate(10, -10),
        new Coordinate(650, 10), new Coordinate(630, 490)
      };

      for (int i = 0; i < invalid.Length; i++)
      {
        Test_SetCoordinates("topLeft", invalid[i], _TopRight, _BottomLeft, _BottomRight);
        Test_SetCoordinates("topRight", _TopLeft, invalid[i], _BottomLeft, _BottomRight);
        Test_SetCoordinates("bottomLeft", _TopLeft, _TopRight, invalid[i], _BottomRight);
        Test_SetCoordinates("bottomRight", _TopLeft, _TopRight, _BottomLeft, invalid[i]);
      }
    }

    [TestMethod, TestCategory(_Category)]
    public void Test_Settings()
    {
      TshirtScript script = new TshirtScript();

      using (MagickImage logo = Images.Logo)
      {
        Reset(script);
        script.Execute(logo, logo);

        ExceptionAssert.Throws<InvalidOperationException>(delegate ()
        {
          Reset(script);
          script.Gravity = Gravity.Forget;
          script.Execute(logo, logo);
        }, "Invalid Gravity specified.");

        ExceptionAssert.Throws<InvalidOperationException>(delegate ()
        {
          Reset(script);
          script.Rotation = -361;
          script.Execute(logo, logo);
        }, "Invalid Rotation specified.");

        ExceptionAssert.Throws<InvalidOperationException>(delegate ()
        {
          Reset(script);
          script.Rotation = 361;
          script.Execute(logo, logo);
        }, "Invalid Rotation specified.");

        ExceptionAssert.Throws<InvalidOperationException>(delegate ()
        {
          Reset(script);
          script.Lighting = -1;
          script.Execute(logo, logo);
        }, "Invalid Lightning specified.");

        ExceptionAssert.Throws<InvalidOperationException>(delegate ()
        {
          Reset(script);
          script.Lighting = 31;
          script.Execute(logo, logo);
        }, "Invalid Lightning specified.");

        ExceptionAssert.Throws<InvalidOperationException>(delegate ()
        {
          Reset(script);
          script.Blur = -1;
          script.Execute(logo, logo);
        }, "Invalid Blur specified.");
      }
    }
  }
}
