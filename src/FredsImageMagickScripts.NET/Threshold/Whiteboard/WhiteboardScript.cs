// <copyright file="WhiteboardScript.cs" company="Dirk Lemstra, Fred Weinhaus">
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
using System.Globalization;
using System.Linq;
using ImageMagick;

namespace FredsImageMagickScripts
{
  /// <summary>
  /// Processses a picture of a whiteboard with writing on it to clean up the background and
  /// correct the perspective. The four corners of the actual interior of the whiteboard in the
  /// picture must be supplied in order to correct the perspective.
  /// </summary>
  public sealed class WhiteboardScript
  {
    private PointD[] _coords;
    private double _height;
    private double _width;

    /// <summary>
    /// Initializes a new instance of the <see cref="WhiteboardScript"/> class.
    /// </summary>
    public WhiteboardScript()
    {
      Reset();
    }

    /// <summary>
    /// Gets or sets the width-to-height aspect ratio of actual whiteboard.
    /// Typical values are: (2:1), (3:2) and (4:3). The default is computed automatically.
    /// </summary>
    public PointD? AspectRatio
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the desired background color of the whiteboard after it has been cleaned up.
    /// The default is white.
    /// </summary>
    public MagickColor BackgroundColor
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the desired dimension(s) of the output image. Choices are: WIDTH, xHEIGHT or WIDTHxHEIGHT;
    /// if either of the first two options are selected, then the other dimension will be computed
    /// from the aspect ratio and magnify will be ignored. If the latter option is selected, then
    /// both aspect and magnify will be ignored. If no coordinates are supplied, then the input
    /// image aspect ratio will be use. The default is to ignore dimensions and use the aspect
    /// and magnify.
    /// </summary>
    public MagickGeometry Dimensions
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the enhancement method for the image brightness before cleaning the background.
    /// The default is Stretch.
    /// </summary>
    public WhiteboardEnhancements Enhance
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the size of the filter used to clean up the background. The filtersize needs to be larger
    /// than the thickness of the writing, but the smaller the better beyond this. Making it
    /// larger will increase the processing time and may lose text. The default is 15.
    /// </summary>
    public int FilterSize
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the output image magnification (or minification) factor. Values larger than 1 will
    /// magnify. Values less than 1 will minify. The default is 1 and will produce an output whose
    /// height is the length of the left edge as defined by the supplied coordinates and whose
    /// width=height*aspect. A value of 2 will be twice that size and a value of 0.5 will be half
    /// that size. If no coordinates are supplied, then the width and height will be those of the
    /// input image multiplied by the magnify factor.
    /// </summary>
    public double? Magnification
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the offset threshold in percent used by the filter to eliminate noise. Values too small
    /// will leave much noise and artifacts in the result. Values too large will remove too much
    /// text leaving gaps. The default is 5.
    /// </summary>
    public Percentage FilterOffset
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the desired color saturation of the text expressed as a percentage. A value of 100 means
    /// no change. The default=200. Larger values will make the text colors more saturated.
    /// </summary>
    public Percentage Saturation
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the amount of sharpening to be applied to the resulting image in pixels. If used, it
    /// should be small (suggested about 1).
    /// </summary>
    public int? SharpeningAmount
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the text smoothing threshold. Smaller values smooth/thicken the text more. Larger values
    /// thin, but can result in gaps in the text. Nominal value is in the middle at about 50.
    /// The default is to disable smoothing.
    /// </summary>
    public Percentage? Threshold
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets percent near white to use for white balancing. The default is 0.01.
    /// </summary>
    public Percentage WhiteBalance
    {
      get;
      set;
    }

    /// <summary>
    /// Processses a picture of a whiteboard with writing on it to clean up the background and
    /// correct the perspective. The four corners of the actual interior of the whiteboard in the
    /// picture must be supplied in order to correct the perspective.
    /// </summary>
    /// <param name="input">The image to execute the script on.</param>
    /// <returns>The resulting image.</returns>
    public MagickImage Execute(MagickImage input)
    {
      if (input == null)
        throw new ArgumentNullException("input");

      CheckSettings(input);
      CalculateWidthAndHeight(input);

      var output = input.Clone();
      EnhanceImage(output);
      DistortImage(input, output);
      CopyOpacity(output);
      Sharpen(output);
      Modulate(output);

      return output;
    }

    /// <summary>
    /// Resets the script to the default setttings.
    /// </summary>
    public void Reset()
    {
      _coords = null;
      BackgroundColor = new MagickColor("white");
      Enhance = WhiteboardEnhancements.Stretch;
      FilterSize = 15;
      FilterOffset = (Percentage)5;
      Saturation = (Percentage)200;
      WhiteBalance = (Percentage)0.01;
    }

    /// <summary>
    /// Sets the four x,y coordinates of the corner of the whiteboard. The default will be the
    /// four corners of the input image and thus will not trim any existing border or area outside
    /// the whiteboard, nor will it correct any perspective distortion.
    /// </summary>
    /// <param name="topLeft">Top left coordinate</param>
    /// <param name="topRight">Top right coordinate</param>
    /// <param name="bottomRight">Bottom right coordinate</param>
    /// <param name="bottomLeft">Bottom left coordinate</param>
    public void SetCoordinates(PointD topLeft, PointD topRight, PointD bottomRight, PointD bottomLeft)
    {
      _coords = new PointD[] { topLeft, topRight, bottomRight, bottomLeft };
    }

    private static void CheckCoordinate(MagickImage image, string paramName, PointD coord)
    {
      if (coord.X < 0 || coord.X > image.Width)
        throw new ArgumentOutOfRangeException(paramName);

      if (coord.Y < 0 || coord.Y > image.Height)
        throw new ArgumentOutOfRangeException(paramName);
    }

    private static double GetMean(MagickImage image)
    {
      var mean = image.Statistics().GetChannel(PixelChannel.Composite).Mean;
      return mean * 100 / Quantum.Max;
    }

    private static double GetRatio(MagickImage image, Channels channel, MagickImage mask, double maskMean)
    {
      using (MagickImage channelImage = image.Separate(channel).First())
      {
        channelImage.Composite(mask, CompositeOperator.Multiply);
        var channelMean = GetMean(channelImage);
        var average = 100 * channelMean / maskMean;
        return 100 / average;
      }
    }

    private void ApplyWhiteBalance(MagickImage image)
    {
      using (var mask = image.Clone())
      {
        mask.ColorSpace = ColorSpace.HSB;
        mask.Negate(Channels.Green);

        using (var newMask = mask.Separate(Channels.Green).First())
        {
          using (var maskBlue = mask.Separate(Channels.Blue).First())
          {
            newMask.Composite(maskBlue, CompositeOperator.Multiply);
          }

          newMask.ContrastStretch((Percentage)0, WhiteBalance);
          newMask.InverseOpaque(new MagickColor("white"), new MagickColor("black"));

          double maskMean = GetMean(newMask);

          double redRatio = GetRatio(image, Channels.Red, newMask, maskMean);
          double greenRatio = GetRatio(image, Channels.Green, newMask, maskMean);
          double blueRatio = GetRatio(image, Channels.Blue, newMask, maskMean);

          var matrix = new MagickColorMatrix(3, redRatio, 0, 0, 0, greenRatio, 0, 0, 0, blueRatio);

          image.ColorMatrix(matrix);
        }
      }
    }

    private void CalculateWidthAndHeight(MagickImage image)
    {
      if (_coords != null)
        CalculateWidthAndHeightWithCoords();
      else if (Dimensions != null)
        CalculateWidthAndHeightWithDimensions(image);
      else
        CalculateWidthAndHeightWithMagnification(image);
    }

    private void CheckSettings(MagickImage image)
    {
      if (_coords == null)
        return;

      CheckCoordinate(image, "topLeft", _coords[0]);
      CheckCoordinate(image, "topRight", _coords[1]);
      CheckCoordinate(image, "bottomRight", _coords[2]);
      CheckCoordinate(image, "bottomLeft", _coords[3]);
    }

    private void CalculateWidthAndHeightWithCoords()
    {
      var aspect = CalculateAspectRatio();

      if (Dimensions != null)
      {
        CalculateWidthAndHeightWithDimensions(aspect);
      }
      else
      {
        _height = GetMagnification() * Math.Sqrt(Math.Pow(_coords[0].X - _coords[3].X, 2) + Math.Pow(_coords[0].Y - _coords[3].Y, 2));
        _width = _height * aspect;
      }
    }

    private double CalculateAspectRatio()
    {
      if (AspectRatio.HasValue)
        return AspectRatio.Value.X / AspectRatio.Value.Y;

      // get centroid of quadrilateral
      var centroidX = (_coords[3].X + _coords[2].X + _coords[0].X + _coords[1].X) / 4;
      var centroidY = (_coords[3].Y + _coords[2].Y + _coords[0].Y + _coords[1].Y) / 4;

      // convert to proper x,y coordinates relative to center
      var m1x = _coords[3].X - centroidX;
      var m1y = centroidY - _coords[3].Y;
      var m2x = _coords[2].X - centroidX;
      var m2y = centroidY - _coords[2].Y;
      var m3x = _coords[0].X - centroidX;
      var m3y = centroidY - _coords[0].Y;
      var m4x = _coords[1].X - centroidX;
      var m4y = centroidY - _coords[1].Y;

      // simplified equations, assuming u0=0, v0=0, s=1
      var k2 = (((m1y - m4y) * m3x) - ((m1x - m4x) * m3y) + (m1x * m4y) - (m1y * m4x)) / (((m2y - m4y) * m3x) - ((m2x - m4x) * m3y) + (m2x * m4y) - (m2y * m4x));
      var k3 = (((m1y - m4y) * m2x) - ((m1x - m4x) * m2y) + (m1x * m4y) - (m1y * m4x)) / (((m3y - m4y) * m2x) - ((m3x - m4x) * m2y) + (m3x * m4y) - (m3y * m4x));
      var ff = ((((k3 * m3y) - m1y) * ((k2 * m2y) - m1y)) + (((k3 * m3x) - m1x) * ((k2 * m2x) - m1x))) / ((k3 - 1) * (k2 - 1));
      var f = Math.Sqrt(Math.Sqrt(ff * ff));
      var aspect = Math.Sqrt((Math.Pow(k2 - 1, 2) + (Math.Pow((k2 * m2y) - m1y, 2) / Math.Pow(f, 2)) + (Math.Pow((k2 * m2x) - m1x, 2) / Math.Pow(f, 2))) / (Math.Pow(k3 - 1, 2) + (Math.Pow((k3 * m3y) - m1y, 2) / Math.Pow(f, 2)) + (Math.Pow((k3 * m3x) - m1x, 2) / Math.Pow(f, 2))));

      return aspect;
    }

    private void CalculateWidthAndHeightWithDimensions(MagickImage image)
    {
      var aspect = image.Width / image.Height;

      CalculateWidthAndHeightWithDimensions(aspect);
    }

    private void CalculateWidthAndHeightWithDimensions(double aspect)
    {
      if (Dimensions.Width > 0 && Dimensions.Height > 0)
      {
        _width = Dimensions.Width;
        _height = Dimensions.Height;
      }
      else
      {
        if (Dimensions.Width > 0)
        {
          _width = Dimensions.Width;
          _height = _width * aspect;
        }
        else if (Dimensions.Height > 0)
        {
          _height = Dimensions.Height;
          _width = _height * aspect;
        }
        else
        {
          throw new InvalidOperationException("Invalid dimensions specified.");
        }
      }
    }

    private void CalculateWidthAndHeightWithMagnification(MagickImage image)
    {
      var magnification = GetMagnification();
      _width = image.Width * magnification;
      _height = image.Height * magnification;
    }

    private void CopyOpacity(MagickImage image)
    {
      image.Alpha(AlphaOption.Off);

      using (var gray = image.Clone())
      {
        gray.ColorSpace = ColorSpace.Gray;
        gray.Negate();
        gray.AdaptiveThreshold(FilterSize, FilterSize, FilterOffset);
        gray.ContrastStretch((Percentage)0);
        if (Threshold.HasValue)
        {
          gray.Blur((double)Threshold.Value / 100.0, Quantum.Max);
          gray.Level(Threshold.Value, new Percentage(100));
        }

        image.Composite(gray, CompositeOperator.CopyAlpha);
        image.Opaque(MagickColors.Transparent, BackgroundColor);
        image.Alpha(AlphaOption.Off);
      }
    }

    private void DistortImage(MagickImage input, MagickImage image)
    {
      if (_coords != null)
        DistortImageWithCoords(image);
      else if (Dimensions != null)
        DistortImageWithDimensions(input, image);
      else if (Magnification.HasValue)
        DistortImageWithMagnification(input, image);
    }

    private void DistortImageWithCoords(MagickImage image)
    {
      SetDistortViewport(image, 0, 0);

      var arguments = new double[16]
      {
        _coords[0].X, _coords[0].Y, 0, 0, _coords[1].X, _coords[1].Y, _width, 0,
        _coords[2].X, _coords[2].Y, _width, _height, _coords[3].X, _coords[3].Y, 0, _height
      };

      image.Distort(DistortMethod.Perspective, arguments);
    }

    private void DistortImageWithDimensions(MagickImage input, MagickImage image)
    {
      var delX = (input.Width - _width) / 2;
      var delY = (input.Height - _height) / 2;
      SetDistortViewport(image, (int)delX, (int)delY);

      var cx = input.Width / 2;
      var cy = input.Height / 2;
      var magX = input.Width / _width;
      var magy = input.Height / _height;

      image.Distort(DistortMethod.ScaleRotateTranslate, cx, cy, magX, magy, 0);
    }

    private void DistortImageWithMagnification(MagickImage input, MagickImage image)
    {
      var delX = (input.Width - _width) / 2;
      var delY = (input.Height - _height) / 2;
      SetDistortViewport(image, (int)delX, (int)delY);

      image.Distort(DistortMethod.ScaleRotateTranslate, Magnification.Value, 0);
    }

    private void EnhanceImage(MagickImage image)
    {
      if (Enhance == WhiteboardEnhancements.None)
        return;

      if (Enhance.HasFlag(WhiteboardEnhancements.Stretch))
        image.ContrastStretch((Percentage)0, (Percentage)0);

      if (Enhance.HasFlag(WhiteboardEnhancements.Whitebalance))
        ApplyWhiteBalance(image);
    }

    private double GetMagnification()
    {
      return Magnification.HasValue ? Magnification.Value : 1;
    }

    private void Modulate(MagickImage image)
    {
      if (Saturation == (Percentage)100)
        return;

      image.Modulate((Percentage)100, Saturation);
    }

    private void SetDistortViewport(MagickImage image, int x, int y)
    {
      image.VirtualPixelMethod = VirtualPixelMethod.White;

      var viewport = string.Format(CultureInfo.InvariantCulture, "{0}x{1}+{2}+{3}", (int)_width, (int)_height, x, y);
      image.SetArtifact("distort:viewport", viewport);
    }

    private void Sharpen(MagickImage image)
    {
      if (!SharpeningAmount.HasValue || SharpeningAmount <= 0)
        return;

      image.Sharpen(0, SharpeningAmount.Value);
    }
  }
}
