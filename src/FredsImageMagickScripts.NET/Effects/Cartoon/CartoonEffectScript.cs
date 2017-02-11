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

namespace FredsImageMagickScripts
{
  /// <summary>
  /// creates a cartoon-like appearance to an image. The image is smoothed and then multiplied by
  /// a grayscale version of the image with the desired number of levels to produce the segmented
  /// appearance. The pattern parameter changes the shape of the segmentation for the given number
  /// of levels. Edges are then superimposed onto the image.
  /// </summary>
  public sealed class CartoonEffectScript
  {
    /// <summary>
    /// Constructor only sets default property values so that it is ready for a call to its <see cref="Execute(MagickImage)"/> method.
    /// </summary>
    public CartoonEffectScript()
    {
      Reset();
    }

    /// <summary>
    /// The brightness factor. Valid values are zero or higher. The default is 1. Increase
    /// brightness is larger than 1, decrease brightness is less than 1.
    /// </summary>
    public Percentage Brightness
    {
      get;
      set;
    }

    /// <summary>
    /// Edge amount, which must be &gt;= 0
    /// </summary>
    public float EdgeAmount
    {
      get;
      set;
    }

    /// <summary>
    /// Edge width, which must be &gt;= 0
    /// </summary>
    public int EdgeWidth
    {
      get;
      set;
    }

    /// <summary>
    /// Edge threshold
    /// </summary>
    public Percentage EdgeThreshold
    {
      get;
      set;
    }

    /// <summary>
    /// Either 1 or 2.
    /// </summary>
    public CartoonMethod Method
    {
      get;
      set;
    }

    /// <summary>
    /// Number of levels, which must be &gt;= 2
    /// </summary>
    public int Numlevels
    {
      get;
      set;
    }

    /// <summary>
    /// Segmentation pattern.
    /// </summary>
    public Percentage Pattern
    {
      get;
      set;
    }

    /// <summary>
    /// Saturation. Valid values are zero or higher. A value of 100 is no change. The default is 150.
    /// </summary>
    public Percentage Saturation
    {
      get;
      set;
    }

    /// <summary>
    /// Applies the cartoon effect.
    /// </summary>
    /// <remarks>
    /// What the script does is as follows (from Fred's original script http://www.fmwconcepts.com/imagemagick/cartoon/index.php)
    /// (Optionally) applies a median filter to the image
    /// Reduces the number of colors in the filtered image
    /// Converts the original image to grayscale
    /// (Optionally) applies a median filter to the grayscale image
    /// Applies a gradient edge detector to the grayscale image
    /// Thresholds the edge image to binary
    /// Composites the edge image with the color reduced image
    /// This is equivalent to the following IM commands:
    /// Smooths the image
    /// Converts a copy to grayscale, posterizes the levels and \
    /// applied median filtering and a small amount of blur
    /// Multiples the smoothed and grayscale images to create \
    /// the cartoon appearance
    /// Negates and blurs the previous image and colordodge \
    /// composites to create an edge image
    /// Raises the edge image to a power to amplify the edges, 
    /// thresholds and median filters it to create a binary edge mask image
    /// Multiplies the binary edge mask image with the cartoonish image
    /// </remarks>
    /// <param name="input">The image to execute the script on.</param>
    public MagickImage Execute(MagickImage input)
    {
      if (input == null)
      {
        throw new ArgumentNullException("input");
      }

      CheckSettings(input);

      using (MagickImage tmpA1 = input.Clone())
      {
        using (var tmpA2 = input.Clone())
        {
          //# convert to grayscale and posterize to create mask image.
          // Note that $setcSpace is empty for version >= 6.8.5.4 (which is assumed in this port
          //convert $tmpA1 - level 0x$pattern % $setcspace - colorspace gray - posterize $numlevels - depth 8 $proc $tmpA2
          tmpA2.Level(0, (byte)Pattern);  // 450ms
          tmpA2.ColorSpace = ColorSpace.Gray;
          tmpA2.Posterize(Numlevels);  //260ms
          tmpA2.Depth = 8;
          tmpA2.GammaCorrect(2.2);

          switch (Method)
          {
            case CartoonMethod.Method1:
              return ExecuteMethod1(tmpA1, tmpA2);

            case CartoonMethod.Method2:
              return ExecuteMethod2(tmpA1, tmpA2);

            default:
              throw new InvalidOperationException($"Value of {nameof(Method)} property not supported");
          }
        }
      }
    }

    /// <summary>
    /// Resets the script to the default setttings.
    /// </summary>
    public void Reset()
    {
      Pattern = (Percentage)70;
      Numlevels = 6;
      Method = CartoonMethod.Method1;
      EdgeAmount = 4;
      Brightness = (Percentage)100;
      Saturation = (Percentage)150;
      EdgeWidth = 2;
      EdgeThreshold = (Percentage)90;
    }

    private void CheckSettings(MagickImage image)
    {
      if (Numlevels < 2)
        throw new InvalidOperationException($"{nameof(Numlevels)} must be >=2");

      if (EdgeAmount < 0 || float.IsInfinity(EdgeAmount) || float.IsNaN(EdgeAmount))
        throw new InvalidOperationException($"{nameof(EdgeAmount)} must be >= 0");

      if (EdgeWidth < 0)
        throw new InvalidOperationException($"{nameof(EdgeWidth)} must be >= 0");
    }

    private MagickImage ExecuteMethod2(MagickImage tmpA1, MagickImage tmpA2)
    {
      //# process image
      //# multiply the blurred posterized graycale mask with the smoothed input
      //# convert smoothed input to grayscale
      //# apply high pass filter to grayscale, use power to amplify and threshold
      //# multiply composite the edge image with the smoothed color image

      //  convert $tmpA1 \( $tmpA2 - blur 0x1 \) 
      tmpA2.Blur(0, 1); // 0x1 is default anyway but better be specific

      using (var second_0 = tmpA1.Clone())
      {
        using (var second_1 = tmpA2.Clone())
        {
          // \( -clone 0 - clone 1 - compose over - compose multiply - composite - modulate $brightness,$saturation,100 \) \
          second_0.Composite(second_1, CompositeOperator.Multiply);
          second_0.Modulate(Brightness, Saturation, (Percentage)100);

          using (var third = tmpA1.Clone())
          {
            // \(-clone 0 $setcspace - colorspace gray - negate - define convolve: scale =$edgegain \
            //			-morphology Convolve DoG:0,0,${ edgewidth} -negate \
            //			-evaluate pow $edgeamount - white - threshold $edgethresh % \) \
            third.ColorSpace = ColorSpace.Gray;
            third.Negate();
            third.SetArtifact("convolve:scale", "4");
            third.Morphology(MorphologyMethod.Convolve, Kernel.DoG, "0,0," + EdgeWidth);
            third.Negate();
            third.Evaluate(Channels.All, EvaluateOperator.Pow, EdgeAmount);
            third.WhiteThreshold(EdgeThreshold);

            //  -delete 0,1 - compose over - compose multiply - composite "$outfile"
            var result = second_0.Clone();
            result.Composite(third, CompositeOperator.Multiply);
            return result;
          }
        }
      }
    }

    private MagickImage ExecuteMethod1(MagickImage tmpA1, MagickImage tmpA2)
    {
      //# process image
      //# multiply the blurred posterized graycale mask with the smoothed input
      //# convert smoothed input to grayscale
      //# negate and blur
      //# colordodge composite the grayscale and negated/blurred version to make edgewidth image
      //# use power to amplify and then threshold and median filter
      //# multiply composite the edgewidth with the blended image

      //  convert $tmpA1 \( $tmpA2 - blur 0x1 \) 
      tmpA2.Blur(0, 1); // 0x1 is default anyway but better be specific

      using (var second_0 = tmpA1.Clone())
      {
        using (var second_1 = tmpA2.Clone())
        {
          // \( -clone 0 -clone 1 -compose over -compose multiply -composite -modulate $brightness,$saturation,100 \) \
          second_0.Composite(second_1, CompositeOperator.Multiply);
          second_0.Modulate(Brightness, Saturation, (Percentage)100);

          using (var third = tmpA1.Clone())
          {
            // \( -clone 0 $setcspace -colorspace gray \) \
            third.ColorSpace = ColorSpace.Gray;  // -clone 0 $setcspace -colorspace gray

            using (var fourth = third.Clone())
            {
              // \( -clone 3 -negate -blur 0x${edgewidth} \) \
              fourth.Negate();
              fourth.Blur(0, 2);   // very expensive but necessary

              var fifth_0 = third.Clone();    // this will be the result -> do not wrap in using statement as we do not want to dispose it :)
              using (var fifth_1 = fourth.Clone())
              {
                // \(-clone 3 - clone 4 - compose over - compose colordodge - composite \
                // -evaluate pow $edgeamount - threshold $edgethresh % $medproc \) \
                fifth_0.Composite(fifth_1, CompositeOperator.ColorDodge);
                fifth_0.Evaluate(Channels.All, EvaluateOperator.Pow, EdgeAmount);
                fifth_0.Threshold((Percentage)EdgeThreshold);
                fifth_0.Statistic(StatisticType.Median, 3, 3);

                //  -delete 0,1,3,4 -compose over -compose multiply -composite "$outfile"
                fifth_0.Composite(second_0, CompositeOperator.Multiply);

                return fifth_0; // return copy as it will otherwise get disposed
              }
            }
          }
        }
      }
    }
  }
}
