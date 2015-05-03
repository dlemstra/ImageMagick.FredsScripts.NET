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
using System.Linq;
using FredsImageMagickScripts;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FredsImageMagickScripts.NET.Tests.Scripts.Effect
{
	//==============================================================================================
	[TestClass]
	public class TextCleanerScriptTests : ScriptTester
	{
		//===========================================================================================
		private const string _Category = "TextCleanerScript";
		//===========================================================================================
		private static void Test_Defaults(TextCleanerScript script)
		{
			Assert.AreEqual(0.0, script.AdaptiveBlur);
			ColorAssert.AreEqual(new MagickColor("white"), script.BackgroundColor);
			Assert.AreEqual(0, script.CropOffset.Left);
			Assert.AreEqual(0, script.CropOffset.Top);
			Assert.AreEqual(0, script.CropOffset.Right);
			Assert.AreEqual(0, script.CropOffset.Bottom);
			Assert.AreEqual(TextCleanerEnhance.Stretch, script.Enhance);
			Assert.AreEqual(5, script.FilterOffset);
			Assert.AreEqual(15, script.FilterSize);
			Assert.AreEqual(TextCleanerLayout.Portrait, script.Layout);
			Assert.AreEqual(false, script.MakeGray);
			Assert.AreEqual(0, script.Padding);
			Assert.AreEqual(TextCleanerRotation.None, script.Rotation);
			Assert.AreEqual(200, script.Saturation);
			Assert.AreEqual(0.0, script.Sharpen);
			Assert.AreEqual(null, script.SmoothingThreshold);
			Assert.AreEqual(false, script.Trim);
			Assert.AreEqual(false, script.Unrotate);
		}
		//===========================================================================================
		private void Test_Execute(string input, Action<TextCleanerScript> action, string output)
		{
			string inputFile = GetInputFile(input);

			using (MagickImage image = new MagickImage(inputFile))
			{
				TextCleanerScript script = new TextCleanerScript();
				action(script);

				MagickImage scriptOutput = script.Execute(image);
				TestOutput(scriptOutput, output);
			}
		}
		//===========================================================================================
		private void Test_Execute_Abbott2()
		{
			Test_Execute("abbott2.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.None;
				script.FilterSize = 15;
				script.FilterOffset = 20;
			}, "abbott2_g_none_f15_o20.jpg");

			Test_Execute("abbott2.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.Stretch;
				script.FilterSize = 15;
				script.FilterOffset = 20;
			}, "abbott2_g_stretch_f15_o20.jpg");

			Test_Execute("abbott2.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.Stretch;
				script.FilterSize = 25;
				script.FilterOffset = 20;
			}, "abbott2_g_stretch_f25_o20.jpg");

			Test_Execute("abbott2.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.Stretch;
				script.FilterSize = 25;
				script.FilterOffset = 20;
				script.Sharpen = 1;
			}, "abbott2_g_stretch_f25_o20_s1.jpg");

			Test_Execute("abbott2.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.Stretch;
				script.FilterSize = 25;
				script.FilterOffset = 20;
				script.Unrotate = true;
				script.Sharpen = 1;
				script.Trim = true;
				script.Padding = 20;
			}, "abbott2_g_stretch_f25_o20_t30_s1_u_T_p20.jpg");
		}
		//===========================================================================================
		private void Test_Execute_BrScan()
		{
			Test_Execute("brscan_original_r90.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.None;
				script.FilterSize = 15;
				script.FilterOffset = 10;
			}, "brscan_original_r90_g_none_f15_o10.jpg");

			Test_Execute("brscan_original_r90.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.Normalize;
				script.FilterSize = 15;
				script.FilterOffset = 10;
			}, "brscan_original_r90_g_normalize_f15_o10.jpg");

			Test_Execute("brscan_original_r90.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.Normalize;
				script.FilterSize = 15;
				script.FilterOffset = 10;
				script.Sharpen = 1;
			}, "brscan_original_r90_g_normalize_f15_o10_s1.jpg");

			Test_Execute("brscan_original_r90.jpg", (TextCleanerScript script) =>
			{
				script.CropOffset.Top = 50;
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.Normalize;
				script.FilterSize = 15;
				script.FilterOffset = 10;
				script.Unrotate = true;
				script.Sharpen = 2;
				script.Trim = true;
				script.Padding = 20;
			}, "brscan_original_r90_c_0x50x0x0_g_normalize_f15_o10_s2_u_T_p20.jpg");
		}
		//===========================================================================================
		private void Test_Execute_Congress()
		{
			Test_Execute("congress.jpg", (TextCleanerScript script) =>
			{
				script.Enhance = TextCleanerEnhance.Normalize;
				script.FilterSize = 15;
				script.FilterOffset = 5;
				script.Saturation = 200;
			}, "congress_norm_f15_o5_S200.jpg");

			Test_Execute("congress.jpg", (TextCleanerScript script) =>
			{
				script.Enhance = TextCleanerEnhance.Normalize;
				script.FilterSize = 15;
				script.FilterOffset = 5;
				script.Saturation = 200;
				script.Sharpen = 1;
			}, "congress_norm_f15_o5_S200_s1.jpg");

			Test_Execute("congress.jpg", (TextCleanerScript script) =>
			{
				script.Enhance = TextCleanerEnhance.Normalize;
				script.FilterSize = 15;
				script.FilterOffset = 5;
				script.Saturation = 400;
				script.Sharpen = 1;
			}, "congress_norm_f15_o5_S400.jpg");
		}
		//===========================================================================================
		private void Test_Execute_Crankshaft()
		{
			Test_Execute("crankshaft.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.Stretch;
				script.FilterSize = 25;
				script.FilterOffset = 10;
				script.Unrotate = true;
				script.Sharpen = 1;
				script.Trim = true;
				script.Padding = 10;
				script.Layout = TextCleanerLayout.Landscape;
			}, "crankshaft_g_stretch_f25_o10_u_s1_T_p10.jpg");
		}
		//===========================================================================================
		private void Test_Execute_Railways()
		{
			Test_Execute("railways.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.Stretch;
				script.FilterSize = 25;
				script.FilterOffset = 5;
				script.Sharpen = 1;
				script.Layout = TextCleanerLayout.Landscape;
			}, "railways_g_stretch_f25_o5_s1.jpg");
		}
		//===========================================================================================
		private void Test_Execute_Rfid()
		{
			Test_Execute("rfid.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.Stretch;
				script.FilterSize = 25;
				script.FilterOffset = 5;
				script.Sharpen = 1;
				script.Layout = TextCleanerLayout.Landscape;
			}, "rfid_g_stretch_f25_o5_s1.jpg");
		}
		//===========================================================================================
		private void Test_Execute_Telegram()
		{
			Test_Execute("telegram.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.Stretch;
				script.FilterSize = 15;
				script.FilterOffset = 5;
				script.Sharpen = 1;
			}, "telegram_g_stretch_f15_o5_s1.jpg");
		}
		//===========================================================================================
		private void Test_Execute_Twinkle()
		{
			Test_Execute("twinkle.jpg", (TextCleanerScript script) =>
			{
				script.MakeGray = true;
				script.Enhance = TextCleanerEnhance.Stretch;
				script.FilterSize = 25;
				script.FilterOffset = 10;
				script.Sharpen = 1;
				script.Layout = TextCleanerLayout.Landscape;
			}, "twinkle_g_stretch_f25_o10_s1.jpg");
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_Defaults()
		{
			TextCleanerScript script = new TextCleanerScript();
			Test_Defaults(script);

			script.AdaptiveBlur = 2;
			script.BackgroundColor = new MagickColor("yellow");
			script.CropOffset.Left = 1;
			script.CropOffset.Top = 1;
			script.CropOffset.Right = 1;
			script.CropOffset.Bottom = 1;
			script.Enhance = TextCleanerEnhance.Normalize;
			script.FilterOffset = 10;
			script.FilterSize = 10;
			script.Layout = TextCleanerLayout.Landscape;
			script.MakeGray = true;
			script.Padding = 15;
			script.Rotation = TextCleanerRotation.Clockwise;
			script.Saturation = 150;
			script.Sharpen = 1;
			script.SmoothingThreshold = 50;
			script.Trim = true;
			script.Unrotate = true;

			script.Reset();
			Test_Defaults(script);
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_Execute_Null()
		{
			ExceptionAssert.Throws<ArgumentNullException>(() =>
			{
				TextCleanerScript script = new TextCleanerScript();
				script.Execute(null);
			});
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_Execute()
		{
			Test_Execute_Abbott2();
			Test_Execute_BrScan();
			Test_Execute_Congress();
			Test_Execute_Crankshaft();
			Test_Execute_Railways();
			Test_Execute_Rfid();
			Test_Execute_Telegram();
			Test_Execute_Twinkle();
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_Settings()
		{
			TextCleanerScript script = new TextCleanerScript();

			using (MagickImage logo = new MagickImage(Images.Logo))
			{
				ExceptionAssert.Throws<InvalidOperationException>(() =>
				{
					script.AdaptiveBlur = -1;
					script.Execute(logo);
				});

				ExceptionAssert.Throws<InvalidOperationException>(() =>
				{
					script.Reset();
					script.CropOffset.Bottom = -1;
					script.Execute(logo);
				});

				ExceptionAssert.Throws<InvalidOperationException>(() =>
				{
					script.Reset();
					script.CropOffset.Left = -1;
					script.Execute(logo);
				});

				ExceptionAssert.Throws<InvalidOperationException>(() =>
				{
					script.Reset();
					script.CropOffset.Right = -1;
					script.Execute(logo);
				});

				ExceptionAssert.Throws<InvalidOperationException>(() =>
				{
					script.Reset();
					script.CropOffset.Top = -1;
					script.Execute(logo);
				});

				ExceptionAssert.Throws<InvalidOperationException>(() =>
				{
					script.Reset();
					script.FilterSize = -1;
					script.Execute(logo);
				});

				ExceptionAssert.Throws<InvalidOperationException>(() =>
				{
					script.Reset();
					script.Padding = -1;
					script.Execute(logo);
				});

				ExceptionAssert.Throws<InvalidOperationException>(() =>
				{
					script.Reset();
					script.Sharpen = -1;
					script.Execute(logo);
				});

				ExceptionAssert.Throws<InvalidOperationException>(() =>
				{
					script.Reset();
					script.Saturation = -1;
					script.Execute(logo);
				});

				ExceptionAssert.Throws<InvalidOperationException>(() =>
				{
					script.Reset();
					script.SmoothingThreshold = 150;
					script.Execute(logo);
				});

				ExceptionAssert.Throws<InvalidOperationException>(() =>
				{
					script.Reset();
					script.SmoothingThreshold = -50;
					script.Execute(logo);
				});
			}
		}
		//===========================================================================================
	}
	//==============================================================================================
}