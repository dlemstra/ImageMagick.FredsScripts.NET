//=================================================================================================
// Copyright 2015 Dirk Lemstra, Fred Weinhaus
// <https://github.com/dlemstra/FredsImageMagickScripts.NET>
//
// These scripts are available free of charge for non-commercial use, ONLY.
//
// For use of scripts in commercial (for-profit) environments or non-free applications, please
// contact Fred Weinhaus for licensing arrangements. His email address is fmw at alink dot net.
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
using System.Linq;
using FredsImageMagickScripts;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests.Scripts.Threshold
{
	//==============================================================================================
	[TestClass]
	public class TwoColorThreshScriptTests : ScriptTester
	{
		//===========================================================================================
		private const string _Category = "TwoColorThreshScriptTests";
		//===========================================================================================
		private void Test_Execute(string input)
		{
			string inputFile = GetInputFile(input);
			string output = input.Replace(".jpg", ".gif");

			using (MagickImage image = new MagickImage(inputFile))
			{
				MagickImage scriptOutput = TwoColorThreshScript.Execute(image);
				TestOutput(scriptOutput, output);
			}
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_Execute()
		{
			Test_Execute("blocks.gif");
			Test_Execute("blood.jpg");
			Test_Execute("fingerprint.jpg");
			Test_Execute("flower.jpg");
			Test_Execute("house.jpg");
			Test_Execute("kanji.jpg");
			Test_Execute("parts.gif");
			Test_Execute("rice.jpg");
			Test_Execute("tank.jpg");
			Test_Execute("textsample.jpg");
		}
		//===========================================================================================
	}
	//==============================================================================================
}