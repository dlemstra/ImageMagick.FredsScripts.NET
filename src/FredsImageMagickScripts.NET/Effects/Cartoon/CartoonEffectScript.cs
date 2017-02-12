﻿// <copyright file="CartoonEffectScript.cs" company="Dirk Lemstra, Fred Weinhaus">
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
    private static readonly int _edgeWidth = 2;
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
      if (method != CartoonMethod.Method1 && method != CartoonMethod.Method2)
        throw new ArgumentException("Invalid cartoon method specified.", nameof(method));

      _method = method;

      Reset();
    }

    /// <summary>
    /// Gets or sets the brightness factor. Valid values are zero or higher. The default is 100.
    /// Increase brightness is larger than 1, decrease brightness is less than 1.
    /// </summary>
    public Percentage Brightness
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the edge amount, which must be &gt;= 0. The default is 4.
    /// </summary>
    public double EdgeAmount
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the number of levels, which must be &gt;= 2 The default is 6.
    /// </summary>
    public int NumberOflevels
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the segmentation pattern. The default is 70.
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

      using (MagickImage first = SelectiveBlur(input))
      {
        using (var second = first.Clone())
        {
          second.Level((Percentage)0, Pattern);
          second.ColorSpace = ColorSpace.Gray;
          second.Posterize(NumberOflevels);
          second.GammaCorrect(2.2);
          second.Blur(0, 1);

          if (_method == CartoonMethod.Method1)
            return ExecuteMethod1(first, second);

          return ExecuteMethod2(first, second);
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

    private static MagickImage SelectiveBlur(MagickImage image)
    {
      MagickImage result = image.Clone();
      result.SelectiveBlur(0, 5, new Percentage(10));

      return result;
    }

    private void CheckSettings()
    {
      if (Brightness.ToDouble() < 0.0)
        throw new InvalidOperationException("Invalid brightness specified, value must be zero or higher.");

      if (EdgeAmount < 0 || double.IsInfinity(EdgeAmount) || double.IsNaN(EdgeAmount))
        throw new InvalidOperationException("Invalid edge amount specified, value must be zero or higher.");

      if (NumberOflevels < 2)
        throw new InvalidOperationException("Invalid number of levels specified, value must be two or higher.");

      if (Saturation.ToDouble() < 0.0)
        throw new InvalidOperationException("Invalid saturation specified, value must be zero or higher.");
    }

    private MagickImage ExecuteMethod1(MagickImage first, MagickImage second)
    {
      using (var first_0 = first.Clone())
      {
        first_0.Composite(second, CompositeOperator.Multiply);
        first_0.Modulate(Brightness, Saturation, (Percentage)100);

        using (var third = first.Clone())
        {
          third.ColorSpace = ColorSpace.Gray;

          using (var fourth = third.Clone())
          {
            fourth.Negate();
            fourth.Blur(0, _edgeWidth);

            var result = third.Clone();
            result.Composite(fourth, CompositeOperator.ColorDodge);
            result.Evaluate(Channels.All, EvaluateOperator.Pow, EdgeAmount);
            result.Threshold(_edgeThreshold);
            result.Statistic(StatisticType.Median, 3, 3);

            result.Composite(first_0, CompositeOperator.Multiply);

            return result;
          }
        }
      }
    }

    private MagickImage ExecuteMethod2(MagickImage first, MagickImage second)
    {
      var result = first.Clone();
      result.Composite(second, CompositeOperator.Multiply);
      result.Modulate(Brightness, Saturation, (Percentage)100);

      using (var third = first.Clone())
      {
        third.ColorSpace = ColorSpace.Gray;
        third.Negate();
        third.SetArtifact("convolve:scale", _edgeGain);
        third.Morphology(MorphologyMethod.Convolve, Kernel.DoG, "0,0," + _edgeWidth);
        third.Negate();
        third.Evaluate(Channels.All, EvaluateOperator.Pow, EdgeAmount);
        third.WhiteThreshold(_edgeThreshold);

        result.Composite(third, CompositeOperator.Multiply);
        return result;
      }
    }
  }
}
