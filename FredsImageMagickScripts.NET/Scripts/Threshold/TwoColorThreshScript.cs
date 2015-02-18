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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageMagick;

namespace FredsImageMagickScripts
{
	///=============================================================================================
	/// <summary>
	/// Automatically thresholds an image to binary (b/w) format using an adaptive spatial subdivision
	/// color reduction technique. This is the -colors IM operator as implemented with slight
	/// modification from Anthony's Examples at http://www.imagemagick.org/Usage/quantize/#two_color.
	/// For algorithm details, see http://www.imagemagick.org/script/quantize.php
	/// </summary>
	public static class TwoColorThreshScript
	{
		///==========================================================================================
		/// <summary>
		/// Automatically thresholds an image to binary (b/w) format using an adaptive spatial subdivision
		/// color reduction technique. This is the -colors IM operator as implemented with slight
		/// modification from Anthony's Examples at http://www.imagemagick.org/Usage/quantize/#two_color.
		/// For algorithm details, see http://www.imagemagick.org/script/quantize.php
		/// </summary>
		public static MagickImage Execute(MagickImage input)
		{
			MagickImage result = input.Clone();

			QuantizeSettings settings = new QuantizeSettings()
			{
				Colors = 2,
				DitherMethod = DitherMethod.No
			};

			result.Quantize(settings);
			result.ColorSpace =  ColorSpace.GRAY;
			result.ContrastStretch(0);

			return result;
		}
		//===========================================================================================
	}
	//==============================================================================================
}
