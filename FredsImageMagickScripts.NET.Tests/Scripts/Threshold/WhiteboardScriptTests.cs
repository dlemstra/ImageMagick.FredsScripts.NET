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

namespace FredsImageMagickScripts.NET.Tests.Scripts.Threshold
{
	//==============================================================================================
	[TestClass]
	public class WhiteboardScriptTests : ScriptTester
	{
		//===========================================================================================
		private const string _Category = "WhiteboardScriptTests";
		//===========================================================================================
		private static void Test_Defaults(WhiteboardScript script)
		{
			ColorAssert.AreEqual(new MagickColor("white"), script.BackgroundColor);
			Assert.AreEqual(WhiteboardEnhancements.Stretch, script.Enhance);
			Assert.AreEqual((Percentage)5, script.FilterOffset);
			Assert.AreEqual(15, script.FilterSize);
			Assert.AreEqual((Percentage)200, script.Saturation);
			Assert.AreEqual((Percentage)0.01, script.WhiteBalance);
		}
		//===========================================================================================
		private static void TestDimensions(int width, int height)
		{
			ExceptionAssert.Throws<InvalidOperationException>(delegate()
			{
				using (MagickImage logo = Images.Logo)
				{
					WhiteboardScript script = new WhiteboardScript();
					script.Dimensions = new MagickGeometry(width, height);
					script.Execute(logo);
				}
			});
		}
		//===========================================================================================
		private void Test_Execute(string input, Action<WhiteboardScript> action, string output)
		{
			string inputFile = GetInputFile(input);

			using (MagickImage image = new MagickImage(inputFile))
			{
				WhiteboardScript script = new WhiteboardScript();
				action(script);

				MagickImage scriptOutput = script.Execute(image);
				TestOutput(scriptOutput, output);
			}
		}
		//===========================================================================================
		private void Test_Execute_Whiteboard()
		{
			Test_Execute("whiteboard.jpg", delegate(WhiteboardScript script)
			{
				script.SetCoordinates(new Coordinate(101, 53), new Coordinate(313, 31),
					new Coordinate(313, 218), new Coordinate(101, 200));
				script.Enhance = WhiteboardEnhancements.None;
				script.AspectRatio = new PointD(4, 3);
				script.Magnification = 2;
				script.FilterSize = 25;
				script.FilterOffset = (Percentage)3;
			}, "whiteboard_a1p33_m2_S200_f25_o3_none.jpg");

			Test_Execute("whiteboard.jpg", delegate(WhiteboardScript script)
			{
				script.SetCoordinates(new Coordinate(101, 53), new Coordinate(313, 31),
					new Coordinate(313, 218), new Coordinate(101, 200));
				script.Enhance = WhiteboardEnhancements.Both;
				script.AspectRatio = new PointD(4, 3);
				script.Magnification = 2;
				script.FilterSize = 25;
				script.FilterOffset = (Percentage)3;
			}, "whiteboard_a1p33_m2_S200_f25_o3_both.jpg");

			Test_Execute("whiteboard.jpg", delegate(WhiteboardScript script)
			{
				script.SetCoordinates(new Coordinate(101, 53), new Coordinate(313, 31),
					new Coordinate(313, 218), new Coordinate(101, 200));
				script.Enhance = WhiteboardEnhancements.Both;
				script.AspectRatio = new PointD(4, 3);
				script.Magnification = 2;
				script.FilterSize = 25;
				script.FilterOffset = (Percentage)3;
				script.Threshold = (Percentage)60;
			}, "whiteboard_a1p33_m2_S200_t60_f25_o3_both.jpg");
		}
		//===========================================================================================
		private void Test_Execute_Whiteboard1_35pct()
		{
			Test_Execute("whiteboard1_35pct.jpg", delegate(WhiteboardScript script)
			{
				script.Enhance = WhiteboardEnhancements.None;
				script.FilterSize = 12;
				script.FilterOffset = (Percentage)3;
			}, "whiteboard1_35pct_m1_S200_f12_o3_none.jpg");

			Test_Execute("whiteboard1_35pct.jpg", delegate(WhiteboardScript script)
			{
				script.Enhance = WhiteboardEnhancements.Both;
				script.FilterSize = 12;
				script.FilterOffset = (Percentage)3;
			}, "whiteboard1_35pct_m1_S200_f12_o3_both.jpg");

			Test_Execute("whiteboard1_35pct.jpg", delegate(WhiteboardScript script)
			{
				script.Enhance = WhiteboardEnhancements.Both;
				script.FilterSize = 12;
				script.FilterOffset = (Percentage)3;
				script.Threshold = (Percentage)30;
			}, "whiteboard1_35pct_m1_S200_t30_f12_o3_both.jpg");

			Test_Execute("whiteboard1_35pct.jpg", delegate(WhiteboardScript script)
			{
				script.Enhance = WhiteboardEnhancements.Both;
				script.FilterSize = 12;
				script.FilterOffset = (Percentage)3;
				script.Threshold = (Percentage)30;
				script.SharpeningAmount = 1;
			}, "whiteboard1_35pct_m1_S200_s1_t30_f12_o3_both.jpg");
		}
		//===========================================================================================
		private void Test_Execute_Whiteboard2()
		{
			Test_Execute("whiteboard2.gif", delegate(WhiteboardScript script)
			{
				script.SetCoordinates(new Coordinate(55, 60), new Coordinate(420, 76),
					new Coordinate(416, 277), new Coordinate(75, 345));
				script.Enhance = WhiteboardEnhancements.Both;
				script.AspectRatio = new PointD(4, 3);
				script.FilterSize = 12;
				script.FilterOffset = (Percentage)7;
				script.Threshold = (Percentage)30;
			}, "whiteboard2_a1p5_m1_S200_t30_f12_o7_both.jpg");

			Test_Execute("whiteboard2.gif", delegate(WhiteboardScript script)
			{
				script.SetCoordinates(new Coordinate(55, 60), new Coordinate(420, 76),
					new Coordinate(416, 277), new Coordinate(75, 345));
				script.Enhance = WhiteboardEnhancements.Both;
				script.FilterSize = 12;
				script.FilterOffset = (Percentage)7;
				script.Threshold = (Percentage)30;
			}, "whiteboard2_a_m1_S200_t30_f12_o7_both.jpg");
		}
		//===========================================================================================
		private void Test_Execute_WhiteboardBlog()
		{
			Test_Execute("WhiteboardBlog.jpg", delegate(WhiteboardScript script)
			{
				script.SetCoordinates(new Coordinate(13, 3), new Coordinate(342, 6),
					new Coordinate(331, 467), new Coordinate(38, 482));
				script.Enhance = WhiteboardEnhancements.Both;
				script.AspectRatio = new PointD(3, 4);
				script.FilterSize = 12;
				script.FilterOffset = (Percentage)3;
				script.Threshold = (Percentage)40;
			}, "WhiteboardBlog_a0p75_m1_S200_t40_f12_o3_both.jpg");

			Test_Execute("WhiteboardBlog.jpg", delegate(WhiteboardScript script)
			{
				script.SetCoordinates(new Coordinate(13, 3), new Coordinate(342, 6),
					new Coordinate(331, 467), new Coordinate(38, 482));
				script.Enhance = WhiteboardEnhancements.Both;
				script.FilterSize = 12;
				script.FilterOffset = (Percentage)3;
				script.Threshold = (Percentage)40;
			}, "WhiteboardBlog_a_m1_S200_t40_f12_o3_both.jpg");
		}
		//===========================================================================================
		private void Test_Execute_WhiteboardScenario1()
		{
			Test_Execute("whiteboardScenario1.jpg", delegate(WhiteboardScript script)
			{
				script.Enhance = WhiteboardEnhancements.Both;
				script.FilterSize = 12;
				script.FilterOffset = (Percentage)3;
			}, "whiteboardScenario1_m1_S200_f12_o3_both.jpg");
		}
		//===========================================================================================
		private void Test_SetCoordinates(Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight)
		{
			ExceptionAssert.Throws<ArgumentOutOfRangeException>(delegate()
			{
				using (MagickImage logo = Images.Logo)
				{
					WhiteboardScript script = new WhiteboardScript();
					script.SetCoordinates(topLeft, topRight, bottomLeft, bottomRight);
					script.Execute(logo);
				}
			});
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_Defaults()
		{
			WhiteboardScript script = new WhiteboardScript();
			Test_Defaults(script);

			script.BackgroundColor = new MagickColor("purple");
			script.Enhance = WhiteboardEnhancements.None;
			script.FilterOffset = (Percentage)5;
			script.FilterSize = 10;
			script.Saturation = (Percentage)100;
			script.WhiteBalance = (Percentage)0.1;

			script.Reset();
			Test_Defaults(script);
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_Execute()
		{
			Test_Execute_Whiteboard();
			Test_Execute_Whiteboard1_35pct();
			Test_Execute_Whiteboard2();
			Test_Execute_WhiteboardBlog();
			Test_Execute_WhiteboardScenario1();
		}
		//===========================================================================================
		[TestMethod, TestCategory(_Category)]
		public void Test_Exceptions()
		{
			ExceptionAssert.Throws<ArgumentNullException>(delegate()
			{
				WhiteboardScript script = new WhiteboardScript();
				script.Execute(null);
			});

			Coordinate topLeft = new Coordinate(10, 10);
			Coordinate topRight = new Coordinate(630, 10);
			Coordinate bottomLeft = new Coordinate(10, 470);
			Coordinate bottomRight = new Coordinate(630, 470);

			Coordinate[] invalid = new Coordinate[]
			{
				new Coordinate(-10, 10), new Coordinate(10, -10),
				new Coordinate(650, 10), new Coordinate(630, 490)
			};

			for (int i = 0; i < invalid.Length; i++)
			{
				Test_SetCoordinates(invalid[i], topRight, bottomLeft, bottomRight);
				Test_SetCoordinates(topLeft, invalid[i], bottomLeft, bottomRight);
				Test_SetCoordinates(topLeft, topRight, invalid[i], bottomRight);
				Test_SetCoordinates(topLeft, topRight, bottomLeft, invalid[i]);
			}

			TestDimensions(0, 0);
			TestDimensions(-1, -1);
			TestDimensions(-1, 0);
			TestDimensions(0, -1);
		}
		//===========================================================================================
	}
	//==============================================================================================
}
