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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests.Scripts
{
	//==============================================================================================
	public abstract class ScriptTester
	{
		//===========================================================================================
		private static string _ImagesRoot = @"..\..\..\Images\";
		//===========================================================================================
		private string GetExpectedOutputFile(string fileName)
		{
			return _ImagesRoot + @"Output\" + ScriptName + @"\" + fileName;
		}
		//===========================================================================================
		private string GetActualOutputFile(string fileName)
		{
			int dotIndex = fileName.LastIndexOf('.');
			string name = fileName.Substring(0, dotIndex) + ".actual" + fileName.Substring(dotIndex);

			return _ImagesRoot + @"Output\" + ScriptName + @"\" + name;
		}
		//===========================================================================================
		protected string ScriptName
		{
			get
			{
				string scriptName = GetType().Name;
				return scriptName.Substring(0, scriptName.Length - 5); // Remove 'Tests'
			}
		}
		//===========================================================================================
		protected string GetInputFile(string fileName)
		{
			return _ImagesRoot + @"Input\" + fileName;
		}
		//===========================================================================================
		protected void TestOutput(MagickImage image, string expectedOutput)
		{
			string actualOutputFile = GetActualOutputFile(expectedOutput);
			image.Write(actualOutputFile);

			string expectedOutputFile = GetExpectedOutputFile(expectedOutput);
			using (MagickImage expectedImage = new MagickImage(expectedOutputFile))
			{
				using (MagickImage actualImage = new MagickImage(actualOutputFile))
				{
					double distortion = actualImage.Compare(expectedImage, ErrorMetric.RootMeanSquared);

					Assert.AreEqual(0.0, distortion, "Distortion is too high.");
				}
			}
		}
		//===========================================================================================
	}
	//==============================================================================================
}
