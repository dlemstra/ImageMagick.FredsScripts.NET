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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ImageMagick;

namespace FredsImageMagickScripts
{
  /// <summary>
  /// Applies an embroidery effect to each color in an image. The image must have limited number of
  /// colors or only the top most frequent colors will be used. Each color will get the same
  /// pattern, but at different rotation angles.
  /// </summary>
  public sealed class EmbroideryScript
  {
    /// <summary>
    /// Creates a new instance of the EmbroideryScript class.
    /// </summary>
    public EmbroideryScript()
    {
      Reset();
    }

    /// <summary>
    /// Initial pattern angle for background color.
    /// Default is 90.
    /// </summary>
    public int Angle
    {
      get;
      set;
    }

    /// <summary>
    /// Bevel azimuth angle.
    /// Default is 130.
    /// </summary>
    public double Azimuth
    {
      get;
      set;
    }

    /// <summary>
    /// Actual background color in image.
    /// Default is most the frequent color.
    /// </summary>
    public MagickColor BackgroundColor
    {
      get;
      set;
    }

    /// <summary>
    /// Pattern bevel amount.
    /// Default is 4.
    /// </summary>
    public int Bevel
    {
      get;
      set;
    }

    /// <summary>
    /// Fuzz value for recoloring near black and near white
    /// Default is 20.
    /// </summary>
    public Percentage ColorFuzz
    {
      get;
      set;
    }

    /// <summary>
    /// Bevel sigmoidal-contrast amount.
    /// Default is 0 (no added contrast).
    /// </summary>
    public double Contrast
    {
      get;
      set;
    }

    /// <summary>
    /// Bevel elevation angle.
    /// Default is 30.
    /// </summary>
    public double Elevation
    {
      get;
      set;
    }

    /// <summary>
    /// Shadow extent.
    /// Default is 2.
    /// </summary>
    public double Extent
    {
      get;
      set;
    }

    /// <summary>
    /// Limit colors near black and near white to gray(graylimit%) and gray(100%-graylimit%).
    /// Default is 20.
    /// </summary>
    public int GrayLimit
    {
      get;
      set;
    }

    /// <summary>
    /// Shadow intensity (higher is darker).
    /// Default is 25.
    /// </summary>
    public Percentage Intensity
    {
      get;
      set;
    }

    /// <summary>
    /// Mixing between before and after spread result.
    /// Default is 100.
    /// </summary>
    public int Mix
    {
      get;
      set;
    }

    /// <summary>
    /// Number of desired or actual colors in image.
    /// Default is 8.
    /// </summary>
    public int NumberOfColors
    {
      get;
      set;
    }

    /// <summary>
    /// The Wave pattern.
    /// Default is Linear.
    /// </summary>
    public EmbroideryPattern Pattern
    {
      get;
      set;
    }

    /// <summary>
    /// The range of pattern angles over all the colors.
    /// Default is 90.
    /// </summary>
    public int Range
    {
      get;
      set;
    }

    /// <summary>
    /// Pattern spread (diffusion).
    /// Default is 1
    /// </summary>
    public double Spread
    {
      get;
      set;
    }

    /// <summary>
    /// The weave thickness.
    /// Default is 2.
    /// </summary>
    public int Thickness
    {
      get;
      set;
    }

    /// <summary>
    /// Applies an embroidery effect to each color in an image. The image must have limited number
    /// of colors or only the top most frequent colors will be used. Each color will get the same
    /// pattern, but at different rotation angles.
    /// </summary>
    /// <param name="input">The image to execute the script on.</param>
    public MagickImage Execute(MagickImage input)
    {
      if (input == null)
        throw new ArgumentNullException("input");

      CheckSettings();

      using (var image = input.Clone())
      {
        var colors = image.Histogram().OrderByDescending(kv => kv.Value).Select(kv => kv.Key).Take(NumberOfColors).ToArray();

        RemapColors(image, colors);

        using (var texture = CreateTexture())
        {
          var pattern = CreatePattern(image.Width * 2, image.Height * 2, texture);

          using (MagickImage nearBlackWhite = ToNearBlackWhite(image))
          {
            using (var images = new MagickImageCollection())
            {
              double angle = (Pattern == EmbroideryPattern.Linear ? -45 : -90) + Angle;

              foreach (var color in colors)
              {
                bool useBevel = Bevel != 0 && color != colors.First();

                using (var croppedPattern = CreateCroppedPattern(image, pattern, angle))
                {
                  using (var alpha = ExtractAlpha(image, color))
                  {
                    var colorImage = CreateColor(alpha, croppedPattern, nearBlackWhite, useBevel);
                    images.Add(colorImage);
                  }
                }

                angle += (Range / (double)colors.Length);
              }

              var result = images.Flatten();
              result.Crop(input.Width, input.Height, Gravity.Center);
              return result;
            }
          }
        }
      }
    }

    /// <summary>
    /// Resets the script to the default setttings.
    /// </summary>
    public void Reset()
    {
      Angle = 0;
      Azimuth = 130;
      BackgroundColor = null;
      Bevel = 4;
      ColorFuzz = (Percentage)20;
      Contrast = 0;
      Elevation = 30;
      Extent = 2;
      GrayLimit = 20;
      Intensity = (Percentage)25;
      Mix = 100;
      NumberOfColors = 8;
      Pattern = EmbroideryPattern.Linear;
      Range = 90;
      Spread = 1.0;
      Thickness = 2;
    }
    private void AddBevel(MagickImage image)
    {
      using (var alphaTexture = image.Clone())
      {
        alphaTexture.Alpha(AlphaOption.Extract);
        alphaTexture.Blur(0, Bevel);
        alphaTexture.Shade(Azimuth, Elevation);
        alphaTexture.Composite(image, CompositeOperator.CopyAlpha);
        alphaTexture.Alpha(AlphaOption.On);
        alphaTexture.Alpha(AlphaOption.Background);
        alphaTexture.Alpha(AlphaOption.Deactivate);
        alphaTexture.AutoLevel(Channels.Composite);
        alphaTexture.Evaluate(Channels.Composite, EvaluateFunction.Polynomial, new double[] { 3.5, -5.05, 2.05, 0.25 });
        alphaTexture.SigmoidalContrast(Contrast);
        alphaTexture.Alpha(AlphaOption.On);

        image.Composite(alphaTexture, CompositeOperator.HardLight);
      }
    }

    private void CheckSettings()
    {
      if (Angle < -360 || Angle > 360)
        throw new InvalidOperationException("Invalid angle specified, value must be between -360 and 360.");

      if (Azimuth < -360.0 || Azimuth > 360.0)
        throw new InvalidOperationException("Invalid azimuth specified, value must be between -360 and 360.");

      if (ColorFuzz < (Percentage)0 || ColorFuzz > (Percentage)100)
        throw new InvalidOperationException("Invalid color fuzz specified, value must be between 0 and 100.");

      if (Contrast < 0.0)
        throw new InvalidOperationException("Invalid contrast specified, value must be higher than zero.");

      if (Elevation < 0.0 || Elevation > 90.0)
        throw new InvalidOperationException("Invalid range specified, value must be between 0 and 90.");

      if (Extent < 0.0)
        throw new InvalidOperationException("Invalid spread specified, value must be higher than zero.");

      if (GrayLimit < 0 || GrayLimit > 100)
        throw new InvalidOperationException("Invalid gray limit specified, value must be between 0 and 100.");

      if (Intensity < (Percentage)0 || Intensity > (Percentage)100)
        throw new InvalidOperationException("Invalid intensity specified, value must be between 0 and 100.");

      if (Mix < 0 || Mix > 100)
        throw new InvalidOperationException("Invalid mix specified, value must be between 0 and 100.");

      if (NumberOfColors <= 0)
        throw new InvalidOperationException("Invalid number of colors specified, value must be higher than zero.");

      if (Range < 0 || Range > 360)
        throw new InvalidOperationException("Invalid range specified, value must be between 0 and 360.");

      if (Spread < 0.0)
        throw new InvalidOperationException("Invalid spread specified, value must be higher than zero.");

      if (Thickness <= 0)
        throw new InvalidOperationException("Invalid thickness specified, value must be higher than zero.");
    }

    private static MagickImage CreateCroppedPattern(MagickImage image, MagickImage pattern, double angle)
    {
      var croppedPattern = pattern.Clone();
      croppedPattern.Rotate(angle);
      croppedPattern.RePage();
      croppedPattern.Crop(image.Width, image.Height, Gravity.Center);
      croppedPattern.RePage();
      return croppedPattern;
    }

    private MagickImage CreateColor(MagickImage alpha, MagickImage croppedPattern, MagickImage nearBlackWhite, bool useBevel)
    {
      using (var alphaCopy = nearBlackWhite.Clone())
      {
        alphaCopy.Composite(croppedPattern, CompositeOperator.SoftLight);
        alphaCopy.Alpha(AlphaOption.Off);
        alphaCopy.Composite(alpha, CompositeOperator.CopyAlpha);

        if (useBevel)
          AddBevel(alphaCopy);

        var result = alphaCopy.Clone();
        result.BackgroundColor = MagickColors.Black;
        result.Shadow(0, 0, Extent, Intensity);
        result.RePage();
        result.Level((Percentage)0, (Percentage)50, Channels.Alpha);
        result.Composite(alphaCopy, Gravity.Center, CompositeOperator.Over);

        return result;
      }
    }

    private MagickImage CreateCrosshatchTexture()
    {
      MagickImage gradient = new MagickImage("gradient:", Thickness + 3, Thickness + 3);
      gradient.Rotate(270);

      MagickImage flopped = gradient.Clone();
      flopped.Flop();

      using (MagickImageCollection images = new MagickImageCollection())
      {
        images.Add(gradient);
        images.Add(flopped);

        return images.AppendVertically();
      }
    }

    private MagickImage CreateLinearTexture()
    {
      MagickImage gradient = new MagickImage("gradient:", Thickness, Thickness * 4);
      gradient.Rotate(270);

      MagickImage thick1 = CreateRolled(gradient, Thickness);
      MagickImage thick2 = CreateRolled(gradient, Thickness * 2);
      MagickImage thick3 = CreateRolled(gradient, Thickness * 3);

      using (MagickImageCollection images = new MagickImageCollection())
      {
        images.Add(gradient);
        images.Add(thick1);
        images.Add(thick2);
        images.Add(thick3);

        return images.AppendVertically();
      }
    }

    private MagickImage CreatePattern(int width, int height, MagickImage texture)
    {
      MagickImage pattern = new MagickImage(MagickColors.None, width, height);
      pattern.Texture(texture);

      if (Spread == 0.0)
        return pattern;

      if (Spread == 100.0)
      {
        pattern.Spread(Spread);
        return pattern;
      }

      using (MagickImage mix = pattern.Clone())
      {
        mix.Spread(Spread);

        pattern.Composite(mix, CompositeOperator.Blend, Mix.ToString(CultureInfo.InvariantCulture));
        return pattern;
      }
    }

    private static MagickImage CreateRolled(MagickImage image, int thickness)
    {
      MagickImage rolled = image.Clone();
      rolled.Roll(thickness, 0);
      return rolled;
    }

    private MagickImage CreateTexture()
    {
      if (Pattern == EmbroideryPattern.Linear)
        return CreateLinearTexture();

      if (Pattern == EmbroideryPattern.Crosshatch)
        return CreateCrosshatchTexture();

      throw new NotImplementedException();
    }

    private static MagickImage ExtractAlpha(MagickImage image, MagickColor color)
    {
      var alpha = image.Clone();
      alpha.InverseTransparent(color);
      alpha.Alpha(AlphaOption.Extract);
      return alpha;
    }

    private static void RemapColors(MagickImage image, IEnumerable<MagickColor> colors)
    {
      using (var images = new MagickImageCollection())
      {
        foreach (var color in colors)
          images.Add(new MagickImage(color, 1, 1));

        using (MagickImage colorMap = images.AppendHorizontally())
        {
          image.Map(colorMap, new QuantizeSettings()
          {
            DitherMethod = DitherMethod.No
          });
        }
      }
    }

    private MagickImage ToNearBlackWhite(MagickImage image)
    {
      MagickImage result = image.Clone();
      if (GrayLimit == 0 && ColorFuzz == (Percentage)0)
        return result;

      result.ColorFuzz = ColorFuzz;
      result.Opaque(MagickColors.White, new MagickColor("gray(" + (100 - GrayLimit) + "%)"));
      result.Opaque(MagickColors.Black, new MagickColor("gray(" + GrayLimit + "%)"));
      result.ColorFuzz = (Percentage)0;

      return result;
    }

  }
}
