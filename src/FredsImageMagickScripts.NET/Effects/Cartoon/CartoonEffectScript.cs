// <copyright file="CartoonEffectScript.cs" company="Dirk Lemstra, Fred Weinhaus">
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

namespace FredsImageMagickScripts
{
  /// <summary>
  /// Creates a cartoon-like appearance to an image. The image is smoothed and then multiplied by
  /// a grayscale version of the image with the desired number of levels to produce the segmented
  /// appearance. The pattern parameter changes the shape of the segmentation for the given number
  /// of levels. Edges are then superimposed onto the image.
  /// </summary>
  public sealed class CartoonEffectScript
  {
    private static readonly string _edgeWidth = "2";
    private static readonly Percentage _edgeThreshold = new Percentage(90);
    private static readonly string _edgeGain = "4";

    private CartoonMethod _method;

    /// <summary>
    /// Initializes a new instance of the <see cref="CartoonEffectScript"/> class.
    /// </summary>
    public CartoonEffectScript()
      : this(CartoonMethod.Method1)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CartoonEffectScript"/> class.
    /// </summary>
    /// <param name="method">The cartoon method to use.</param>
    public CartoonEffectScript(CartoonMethod method)
    {
      _method = method;

      Reset();
    }

    /// <summary>
    /// Gets or sets the brightness factor. Valid values are zero or higher. The default is 1.
    /// Increase brightness is larger than 1, decrease brightness is less than 1.
    /// </summary>
    public Percentage Brightness
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the edge amount, which must be &gt;= 0
    /// </summary>
    public double EdgeAmount
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the number of levels, which must be &gt;= 2
    /// </summary>
    public int NumberOflevels
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the segmentation pattern.
    /// </summary>
    public Percentage Pattern
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the saturation. Valid values are zero or higher. A value of 100 is no change. The default is 150.
    /// </summary>
    public Percentage Saturation
    {
      get;
      set;
    }

    /// <summary>
    /// Creates a cartoon-like appearance to an image. The image is smoothed and then multiplied by
    /// a grayscale version of the image with the desired number of levels to produce the segmented
    /// appearance. The pattern parameter changes the shape of the segmentation for the given number
    /// of levels. Edges are then superimposed onto the image.
    /// </summary>
    /// <param name="input">The image to execute the script on.</param>
    /// <returns>The resulting image.</returns>
    public MagickImage Execute(MagickImage input)
    {
      if (input == null)
        throw new ArgumentNullException("input");

      CheckSettings();

      using (MagickImage tmpA1 = input.Clone())
      {
        using (var tmpA2 = input.Clone())
        {
          tmpA2.Level(0, (byte)Pattern);
          tmpA2.ColorSpace = ColorSpace.Gray;
          tmpA2.Posterize(NumberOflevels);
          tmpA2.Depth = 8;
          tmpA2.GammaCorrect(2.2);

          switch (_method)
          {
            case CartoonMethod.Method1:
              return ExecuteMethod1(tmpA1, tmpA2);

            case CartoonMethod.Method2:
              return ExecuteMethod2(tmpA1, tmpA2);

            default:
              throw new InvalidOperationException("Invalid cartoon method specified.");
          }
        }
      }
    }

    /// <summary>
    /// Resets the script to the default setttings.
    /// </summary>
    public void Reset()
    {
      Brightness = (Percentage)100;
      EdgeAmount = 4;
      NumberOflevels = 6;
      Pattern = (Percentage)70;
      Saturation = (Percentage)150;
    }

    private void CheckSettings()
    {
      if (NumberOflevels < 2)
        throw new InvalidOperationException("Number of levels must be >= 2.");

      if (EdgeAmount < 0 || double.IsInfinity(EdgeAmount) || double.IsNaN(EdgeAmount))
        throw new InvalidOperationException("Edge amount must be >= 0.");
    }

    private MagickImage ExecuteMethod2(MagickImage tmpA1, MagickImage tmpA2)
    {
      tmpA2.Blur(0, 1);

      using (var second_0 = tmpA1.Clone())
      {
        using (var second_1 = tmpA2.Clone())
        {
          second_0.Composite(second_1, CompositeOperator.Multiply);
          second_0.Modulate(Brightness, Saturation, (Percentage)100);

          using (var third = tmpA1.Clone())
          {
            third.ColorSpace = ColorSpace.Gray;
            third.Negate();
            third.SetArtifact("convolve:scale", _edgeGain);
            third.Morphology(MorphologyMethod.Convolve, Kernel.DoG, "0,0," + _edgeWidth);
            third.Negate();
            third.Evaluate(Channels.All, EvaluateOperator.Pow, EdgeAmount);
            third.WhiteThreshold(_edgeThreshold);

            var result = second_0.Clone();
            result.Composite(third, CompositeOperator.Multiply);
            return result;
          }
        }
      }
    }

    private MagickImage ExecuteMethod1(MagickImage tmpA1, MagickImage tmpA2)
    {
      tmpA2.Blur(0, 1);

      using (var second_0 = tmpA1.Clone())
      {
        using (var second_1 = tmpA2.Clone())
        {
          second_0.Composite(second_1, CompositeOperator.Multiply);
          second_0.Modulate(Brightness, Saturation, (Percentage)100);

          using (var third = tmpA1.Clone())
          {
            third.ColorSpace = ColorSpace.Gray;

            using (var fourth = third.Clone())
            {
              fourth.Negate();
              fourth.Blur(0, 2);

              var fifth_0 = third.Clone();
              using (var fifth_1 = fourth.Clone())
              {
                fifth_0.Composite(fifth_1, CompositeOperator.ColorDodge);
                fifth_0.Evaluate(Channels.All, EvaluateOperator.Pow, EdgeAmount);
                fifth_0.Threshold(_edgeThreshold);
                fifth_0.Statistic(StatisticType.Median, 3, 3);

                fifth_0.Composite(second_0, CompositeOperator.Multiply);

                return fifth_0;
              }
            }
          }
        }
      }
    }
  }
}
