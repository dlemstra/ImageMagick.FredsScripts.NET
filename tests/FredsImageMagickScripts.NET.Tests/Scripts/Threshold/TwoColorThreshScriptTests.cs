//=================================================================================================
// Copyright 2015-2017 Dirk Lemstra, Fred Weinhaus
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

namespace FredsImageMagickScripts.NET.Tests.Scripts.Threshold
{
  [TestClass]
  public class TwoColorThreshScriptTests : ScriptTester
  {
    private void Test_Execute(string input)
    {
      string inputFile = GetInputFile(input);
      //LosslessCompress(inputFile);

      string output = input.Replace(".jpg", ".gif");

      using (var image = new MagickImage(inputFile))
      {
        using (var scriptOutput = TwoColorThreshScript.Execute(image))
        {
          TestOutput(scriptOutput, output);
        }
      }
    }

    [TestMethod]
    public void Test_Execute_blocks()
    {
      Test_Execute("blocks.gif");
    }

    [TestMethod]
    public void Test_Execute_blood()
    {
      Test_Execute("blood.jpg");
    }

    [TestMethod]
    public void Test_Execute_fingerprint()
    {
      Test_Execute("fingerprint.jpg");
    }

    [TestMethod]
    public void Test_Execute_flower()
    {
      Test_Execute("flower.jpg");
    }

    [TestMethod]
    public void Test_Execute_house()
    {
      Test_Execute("house.jpg");
    }

    [TestMethod]
    public void Test_Execute_kanji()
    {
      Test_Execute("kanji.jpg");
    }

    [TestMethod]
    public void Test_Execute_parts()
    {
      Test_Execute("parts.gif");
    }

    [TestMethod]
    public void Test_Execute_rice()
    {
      Test_Execute("rice.jpg");
    }

    [TestMethod]
    public void Test_Execute_tank()
    {
      Test_Execute("tank.jpg");
    }

    [TestMethod]
    public void Test_Execute_textsample()
    {
      Test_Execute("textsample.jpg");
    }

    [TestMethod]
    public void Test_Excecute_Null()
    {
      ExceptionAssert.ThrowsArgumentException<ArgumentNullException>(() =>
      {
        TwoColorThreshScript.Execute(null);
      }, "input");
    }
  }
}