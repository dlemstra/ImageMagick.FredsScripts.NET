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
using System.Globalization;
using ImageMagick;

namespace FredsImageMagickScripts
{
  /// <summary>
  /// Transforms an image to place it in a region of a tshirt image. The transformed image will
  /// display hightlights from the tshirt image and be distorted to match the wrinkles in the
  /// tshirt image.
  /// </summary>
  public sealed class TshirtScript
  {
    private PointD[] _Coords;

    /// <summary>
    /// Creates a new instance of the TshirtScript class.
    /// </summary>
    public TshirtScript()
    {
      Reset();
    }

    /// <summary>
    /// AntiAlias amount to apply to alpha channel of tshirt image.Values should be  higher than or equal to zero.
    /// The default is 2.
    /// </summary>
    public double AntiAlias
    {
      get;
      set;
    }

    /// <summary>
    /// The blurring to apply to the displacement map to avoid jagged displacement. Values should be higher
    /// than zero. The default is 1.
    /// </summary>
    public double Blur
    {
      get;
      set;
    }

    /// <summary>
    /// The amount of displacement for the distortion of the overlay image. Values should be higher than
    /// zero. The default is 10.
    /// </summary>
    public int Displace
    {
      get;
      set;
    }

    /// <summary>
    /// If Crop, then the overlay image will be cropped to make its vertical aspect ratio fit that
    /// of coordinate area. This will not distort the image, only make it fit the size of the coordinate
    /// area. The image will not be cropped, if its vertical aspect ratio is smaller than that of the
    /// region. If Distort, the overlay image  will be fit to the coordinate area, if the aspect ratio
    /// of the overlay image  does not match that of the region or coordinate area. The default is None.
    /// </summary>
    public TshirtFit Fit
    {
      get;
      set;
    }

    /// <summary>
    /// The contrast increase for highlights to apply to the overlay image. Valid values are between 0 and 30. The
    /// default is 20.
    /// </summary>
    public int Lighting
    {
      get;
      set;
    }

    /// <summary>
    /// Gravity for selecting the crop location. The choices are: North, South or Center. The default is Center.
    /// </summary>
    public Gravity Gravity
    {
      get;
      set;
    }

    /// <summary>
    /// An additional clockwise positive rotation in order to make orientational adjustments easier.
    /// Values are betweneen -360 and 360. The default is 0.
    /// </summary>
    public double Rotation
    {
      get;
      set;
    }

    /// <summary>
    /// Sharpening to apply to the overlay image. Values should be higher than or equal to than zero.
    /// The default is 1.
    /// </summary>
    public double Sharpen
    {
      get;
      set;
    }

    /// <summary>
    /// The vertical shift of the crop region with respect to the gravity setting. Negative is upward and
    /// positive is downward. The default is 0 (no shift).
    /// </summary>
    public int VerticalShift
    {
      get;
      set;
    }

    /// <summary>
    /// Transforms an image to place it in a region of a tshirt image. The transformed image will
    /// display hightlights from the tshirt image and be distorted to match the wrinkles in the
    /// tshirt image.
    /// </summary>
    /// <param name="tshirt">The image of the shirt to put the overlay on.</param>
    /// <param name="overlay">The overlay to put on top of the shirt.</param>
    public MagickImage Execute(MagickImage tshirt, MagickImage overlay)
    {
      if (tshirt == null)
        throw new ArgumentNullException("tshirt");

      if (overlay == null)
        throw new ArgumentNullException("overlay");

      CheckSettings(tshirt);

      var x = _Coords[1].X - _Coords[0].X;
      var y = _Coords[1].Y - _Coords[0].Y;
      var topWidth = Math.Sqrt(x * x + y * y);
      var scale = (overlay.Width - 1) / (topWidth / 1);

      var overlayCoordinates = CreateOverlayCoordinates(overlay, scale);
      var tshirtCoordinates = CreateTshirtCoordinates(overlay, scale, topWidth);

      var alpha = ExtractAlpha(tshirt);
      var gray = ToGrayScale(tshirt);
      var mean = SubtractMean(gray, tshirtCoordinates);

      ApplyLighting(mean);
      var light = mean.Clone();
      mean.Dispose();

      var blur = gray.Clone();
      ApplyBlur(blur);

      var distorted = DistortOverlay(gray, overlay, overlayCoordinates, tshirtCoordinates);

      var displaced = DisplaceOverlay(distorted, light, blur);
      distorted.Dispose();
      light.Dispose();
      blur.Dispose();

      var output = tshirt.Clone();
      output.Composite(displaced, CompositeOperator.Over);
      displaced.Dispose();

      if (alpha != null)
      {
        output.Alpha(AlphaOption.Off);
        output.Composite(alpha, CompositeOperator.CopyAlpha);

        alpha.Dispose();
      }

      return output;
    }

    /// <summary>
    /// Resets the script to the default setttings.
    /// </summary>
    public void Reset()
    {
      _Coords = null;
      AntiAlias = 2.0;
      Blur = 1.0;
      Displace = 10;
      Fit = TshirtFit.None;
      Gravity = Gravity.Center;
      Lighting = 20;
      Rotation = 0.0;
      Sharpen = 1.0;
      VerticalShift = 0;
    }

    /// <summary>
    /// Sets the four x,y corners of the region in the tshirt where the overlay image will be placed.
    /// Coordinates are not restricted to a rectangle and the region defined by the coordinates can
    /// have rotation.
    /// </summary>
    /// <param name="topLeft">Top left coordinate</param>
    /// <param name="topRight">Top right coordinate</param>
    /// <param name="bottomLeft">Bottom left coordinate</param>
    /// <param name="bottomRight">Bottom right coordinate</param>
    public void SetCoordinates(PointD topLeft, PointD topRight, PointD bottomRight, PointD bottomLeft)
    {
      _Coords = new PointD[] { topLeft, topRight, bottomRight, bottomLeft };
    }

    /// <summary>
    /// Sets the four x,y corners of the region in the tshirt where the overlay image will be placed.
    /// </summary>
    /// <param name="geometry">The geometry</param>
    public void SetCoordinates(MagickGeometry geometry)
    {
      if (geometry == null)
        throw new ArgumentNullException("geometry");

      var topLeft = new PointD(geometry.X, geometry.Y);
      var topRight = new PointD(geometry.X + geometry.Width - 1, geometry.Y);
      var bottomRight = new PointD(geometry.X + geometry.Width - 1, geometry.Y + geometry.Height - 1);
      var bottomLeft = new PointD(geometry.X, geometry.Y + geometry.Height - 1);
      SetCoordinates(topLeft, topRight, bottomRight, bottomLeft);
    }

    private void ApplyBlur(MagickImage image)
    {
      if (Blur != 0)
        image.Blur(0, Blur);
    }

    private void ApplyLighting(MagickImage image)
    {
      if (Lighting != 0)
        image.SigmoidalContrast(true, Lighting / 3.0);
    }

    private void ApplySharpen(MagickImage image)
    {
      if (Sharpen != 0)
        image.UnsharpMask(0, Sharpen);
    }

    private static void CheckCoordinate(MagickImage image, string paramName, PointD coord)
    {
      if (coord.X < 0 || coord.X > image.Width)
        throw new ArgumentOutOfRangeException(paramName);

      if (coord.Y < 0 || coord.Y > image.Height)
        throw new ArgumentOutOfRangeException(paramName);
    }

    private static double[] CreateArguments(PointD[] overlayCoordinates, PointD[] tshirtCoordinates)
    {
      var result = new double[16];

      int i = 0;
      for (int j = 0; j < 4; j++)
      {
        result[i++] = overlayCoordinates[j].X;
        result[i++] = overlayCoordinates[j].Y;

        result[i++] = tshirtCoordinates[j].X;
        result[i++] = tshirtCoordinates[j].Y;
      }

      return result;
    }

    private static MagickImage SubtractMean(MagickImage image, PointD[] coords)
    {
      using (var img = image.Clone())
      {
        int minX = (int)Math.Min(Math.Min(coords[0].X, coords[1].X), Math.Min(coords[2].X, coords[3].X));
        int minY = (int)Math.Min(Math.Min(coords[0].Y, coords[1].Y), Math.Min(coords[2].Y, coords[3].Y));
        int maxX = (int)Math.Max(Math.Max(coords[0].X, coords[1].X), Math.Max(coords[2].X, coords[3].X));
        int maxY = (int)Math.Max(Math.Max(coords[0].Y, coords[1].Y), Math.Max(coords[2].Y, coords[3].Y));

        int width = maxX - minX + 1;
        int height = maxY - minY + 1;

        img.Crop(minX, minY, width, height);
        img.RePage();

        var statistics = img.Statistics();
        double mean = (statistics.Composite().Mean / Quantum.Max) - 0.5;

        var result = image.Clone();
        result.Evaluate(Channels.All, EvaluateOperator.Subtract, mean * Quantum.Max);
        return result;
      }
    }

    private static MagickImage ToGrayScale(MagickImage image)
    {
      var gray = image.Clone();
      gray.Alpha(AlphaOption.Off);
      gray.ColorSpace = ColorSpace.Gray;

      return gray;
    }

    private void CheckSettings(MagickImage image)
    {
      if (_Coords == null)
        throw new InvalidOperationException("No coordinates have been set.");

      CheckCoordinate(image, "topLeft", _Coords[0]);
      CheckCoordinate(image, "topRight", _Coords[1]);
      CheckCoordinate(image, "bottomRight", _Coords[2]);
      CheckCoordinate(image, "bottomLeft", _Coords[3]);

      if (Gravity != Gravity.North && Gravity != Gravity.Center && Gravity != Gravity.South)
        throw new InvalidOperationException("Invalid Gravity specified.");

      if (Rotation < -360 || Rotation > 360)
        throw new InvalidOperationException("Invalid Rotation specified.");

      if (Lighting < 0 || Lighting > 30)
        throw new InvalidOperationException("Invalid Lightning specified.");

      if (Blur < 0)
        throw new InvalidOperationException("Invalid Blur specified.");
    }

    private PointD[] CreateOverlayCoordinates(MagickImage overlay, double scale)
    {
      var angle = -Math.Atan2(_Coords[1].Y - _Coords[0].Y, _Coords[1].X - _Coords[0].X);
      var xOffset = _Coords[0].X;
      var yOffset = _Coords[0].Y;

      PointD[] coords = new PointD[4];
      for (int i = 0; i < 4; i++)
      {
        coords[i] = new PointD(
          (int)Math.Round((_Coords[i].X - xOffset) * Math.Cos(angle) + (_Coords[i].Y - yOffset) * Math.Sin(angle)),
          (int)Math.Round((_Coords[i].X - xOffset) * Math.Sin(angle) + (_Coords[i].Y - yOffset) * Math.Cos(angle)));
      }

      double ho = Math.Max(coords[3].Y - coords[0].Y, coords[2].Y - coords[1].Y);

      coords[0] = new PointD(0, 0);
      coords[1] = new PointD(overlay.Width - 1, 0);
      if (Fit == TshirtFit.Distort)
        coords[2] = new PointD(overlay.Width - 1, overlay.Height - 1);
      else
        coords[2] = new PointD(overlay.Width - 1, scale * ho);
      coords[3] = new PointD(0, coords[2].Y);

      return coords;
    }

    private PointD[] CreateTshirtCoordinates(MagickImage overlay, double scale, double topWidth)
    {
      if (Rotation == 0)
        return _Coords;

      var rotate = (Math.PI / 180) * Rotation;
      var xcent = Math.Round(0.5 * topWidth) + _Coords[0].X;
      var ycent = Math.Round(0.5 * (overlay.Height / scale) + _Coords[0].Y);

      var coords = new PointD[4];
      for (int i = 0; i < 4; i++)
      {
        coords[i] = new PointD(
          (int)Math.Round(xcent + (_Coords[i].X - xcent) * Math.Cos(rotate) - (_Coords[i].Y - ycent) * Math.Sin(rotate)),
          (int)Math.Round(ycent + (_Coords[i].X - xcent) * Math.Sin(rotate) + (_Coords[i].Y - ycent) * Math.Cos(rotate)));
      }

      return coords;
    }

    private MagickImage CropOverlay(MagickImage image, PointD[] coords)
    {
      var result = image.Clone();
      if (Fit == TshirtFit.Crop)
      {
        int height = (int)coords[2].Y + 1;
        if (image.Height > height)
          result.Crop(image.Width, height, Gravity);
      }

      return result;
    }

    private MagickImage DisplaceOverlay(MagickImage overlay, MagickImage light, MagickImage blur)
    {
      var mergedAlpha = overlay.Clone();
      mergedAlpha.Alpha(AlphaOption.Extract);

      var output = overlay.Clone();
      output.Composite(light, CompositeOperator.HardLight);

      output.Alpha(AlphaOption.Off);
      output.Composite(mergedAlpha, CompositeOperator.CopyAlpha);
      mergedAlpha.Dispose();

      var args = string.Format(CultureInfo.InvariantCulture, "{0},{0}", -Displace);
      output.Composite(blur, 0, 0, CompositeOperator.Displace, args);

      return output;
    }

    private MagickImage DistortOverlay(MagickImage grayShirt, MagickImage overlay, PointD[] overlayCoordinates, PointD[] tshirtCoordinates)
    {
      using (var images = new MagickImageCollection())
      {
        grayShirt.Alpha(AlphaOption.Transparent);
        grayShirt.BackgroundColor = MagickColors.Transparent;
        images.Add(grayShirt);

        var croppedOverlay = CropOverlay(overlay, overlayCoordinates);
        croppedOverlay.VirtualPixelMethod = VirtualPixelMethod.Transparent;

        var arguments = CreateArguments(overlayCoordinates, tshirtCoordinates);
        croppedOverlay.Distort(DistortMethod.Perspective, true, arguments);
        ApplySharpen(croppedOverlay);

        images.Add(croppedOverlay);

        return images.Merge();
      }
    }

    private MagickImage ExtractAlpha(MagickImage image)
    {
      if (!image.HasAlpha)
        return null;

      using (var alpha = image.Clone())
      {
        alpha.Alpha(AlphaOption.Extract);
        alpha.Blur(0, AntiAlias);
        alpha.Level((Percentage)50, (Percentage)100);

        return alpha;
      }
    }
  }
}
