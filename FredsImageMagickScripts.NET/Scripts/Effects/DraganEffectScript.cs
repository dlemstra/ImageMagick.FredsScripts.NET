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

namespace FredsImageMagickScripts
{
	///=============================================================================================
	/// <summary>
	/// Applies a Dragan-like effect to an image to enhance wrinkles creating a "gritty" effect.
	/// </summary>
	public class DraganEffectScript
	{
		//===========================================================================================
		private void ApplyBrightness(MagickImage image)
		{
			if (Brightness == 1.0)
				return;

			image.Evaluate(Channels.All, EvaluateOperator.Multiply, Brightness);
		}
		//===========================================================================================
		private void ApplyContrast(MagickImage image)
		{
			if (Contrast == 0.0)
				return;

			bool sharpen = Contrast >= 0.0;
			image.SigmoidalContrast(sharpen, Math.Abs(Contrast), Quantum.Max / 2);
		}
		//===========================================================================================
		private void ApplySaturation(MagickImage result)
		{
			if (Saturation == 0.0)
				return;

			result.Modulate(100, Saturation, 100);
		}
		//===========================================================================================
		private void CheckSettings()
		{
			if (Brightness < 0.0)
				throw new InvalidOperationException("Invalid Brightness specified, value must be zero or higher");

			if (Contrast < -10.0 || Contrast > 10.0)
				throw new InvalidOperationException("Invalid Contrast specified, the range is -10 to 10.");

			if (Darkness < 1.0)
				throw new InvalidOperationException("Invalid Brightness specified, value must be 1 or higher");

			if (Saturation < 0.0)
				throw new InvalidOperationException("Invalid Brightness specified, value must be zero or higher");
		}
		///==========================================================================================
		/// <summary>
		/// Creates a new instance of the WhiteboardScript class using the specified image.
		/// </summary>
		public DraganEffectScript()
		{
			Reset();
		}
		///==========================================================================================
		/// <summary>
		/// The brightness factor. Valid values are zero or higher. The default is 1. Increase
		/// brightness is larger than 1, decrease brightness is less than 1.
		/// </summary>
		public double Brightness
		{
			get;
			set;
		}
		///==========================================================================================
		/// <summary>
		/// Sigmoidal contrast. Valid values are nominally in the range of -10 to 10. Positive values
		/// increase contrast and negative values decrease contrast. The default is 0.
		/// </summary>
		public double Contrast
		{
			get;
			set;
		}
		///==========================================================================================
		/// <summary>
		/// The shadow darkening factor. Valid values are 1 or higher. The default is 1. Darker shadows
		/// is larger than 1.
		/// </summary>
		public double Darkness
		{
			get;
			set;
		}
		///==========================================================================================
		/// <summary>
		/// Saturation. Valid values are zero or higher. A value of 100 is no change. The default is 150.
		/// </summary>
		public int Saturation
		{
			get;
			set;
		}
		///==========================================================================================
		/// <summary>
		/// Applies a Dragan-like effect to an image to enhance wrinkles creating a "gritty" effect.
		/// </summary>
		public MagickImage Execute(MagickImage input)
		{
			if (input == null)
				throw new ArgumentNullException("input");

			CheckSettings();

			using (MagickImage first = input.Clone())
			{
				ApplyBrightness(first);
				ApplyContrast(first);
				ApplySaturation(first);

				using (MagickImage second = first.Clone())
				{
					second.Modulate(100, 0, 100);
					double darkness = 3 / Darkness;
					if (darkness != 1)
					{
						second.Evaluate(Channels.All, EvaluateOperator.Multiply, darkness);
						second.Clamp();
					}

					first.Composite(second, CompositeOperator.Multiply);
					first.Clamp();
				}

				MagickImage result = first.Clone();

				using (MagickImage third = first.Clone())
				{
					third.SetArtifact("convolve:bias", "50%");
					third.SetArtifact("convolve:scale", "1");
					third.Morphology(MorphologyMethod.Convolve, Kernel.DoG, "0,0,5");
					third.Clamp();

					result.Composite(third, CompositeOperator.Overlay);

					using (MagickImage fourth = first.Clone())
					{
						fourth.Modulate(100, 0, 100);

						result.Composite(fourth, CompositeOperator.HardLight);
						return result;
					}
				}
			}
		}
		///==========================================================================================
		/// <summary>
		/// Resets the script to the default setttings.
		/// </summary>
		public void Reset()
		{
			Brightness = 1.0;
			Contrast = 0;
			Darkness = 1;
			Saturation = 150;
		}
		//===========================================================================================
	}
	//==============================================================================================
}
