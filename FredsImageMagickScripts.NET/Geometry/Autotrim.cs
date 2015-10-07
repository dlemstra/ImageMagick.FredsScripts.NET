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
using System.Drawing;
using ImageMagick;

namespace FredsImageMagickScripts
{
  /// <summary>
  /// Automatically trim a (nearly) uniform color border around an image. If the image is rotated,
  /// one can trim to the bounding box around the image area or alternately trim to the maximum
  /// central area that contains no border pixels. The excess border does not have to completely
  /// surround the image. It may be only on one side. However, one must identify a coordinate
  /// within the border area for the algorithm to extract the base border color and also specify
  /// a fuzz value when the border color is not uniform. For simple border trimming of a normally
  /// oriented image or the bounding box of a rotated image, you may err somewhat towards larger
  /// than optimal fuzz values. For images that contain rotated picture data, when you want to
  /// trim to the central area, you should choose the smallest fuzz value that is appropriate.
  /// For images that contain rotated picture data, an estimate of the rotation angle is needed
  /// for the algorithm to work. However, setting the rotation angle to zero will let the
  /// algorithm determine the rotation angle. The resulting trim is usually pretty good for
  /// angles >= 5 degrees. If the result is off a little, you may use the left/right/top/bottom
  /// arguments to adjust the automatically determined trim region.
  /// </summary>
  public sealed class AutotrimScript
  {
    private MagickColor _BorderColor;

    private class Line
    {
      public int X1;
      public int X2;
      public int Y;

      public Line(int x, int y)
      {
        X1 = x;
        X2 = x;
        Y = y;
      }
    }

    private void Crop(MagickImage image, MagickGeometry area)
    {
      ShiftGeometry(area);
      image.Crop(area.X, area.Y, area.Width, area.Height);
      image.RePage();
    }

    private MagickColor GetBorderColor(MagickImage image)
    {
      using (PixelCollection pixels = image.GetReadOnlyPixels())
      {
        return pixels.GetPixel((int)BorderColorLocation.X, (int)BorderColorLocation.Y).ToColor();
      }
    }

    private MagickGeometry GetLargestArea(MagickImage image)
    {
      Line[] points = new Line[4];

      using (PixelCollection pixels = image.GetReadOnlyPixels())
      {
        Line line = new Line(0, 0);

        while (IsBorderColor(pixels, line.X1, line.Y) && line.Y < image.Height - 1 && line.X1 < image.Width - 1)
        {
          line.Y++;
          line.X1++;
        }

        line.X2 = image.Width - 1;
        while (IsBorderColor(pixels, line.X2, line.Y) && line.X2 > 0)
          line.X2--;

        points[0] = line;


        line = new Line(image.Width - 1, 0);

        while (IsBorderColor(pixels, line.X2, line.Y) && line.Y < image.Height && line.X2 > 0)
        {
          line.Y++;
          line.X2--;
        }

        line.X1 = 0;
        while (IsBorderColor(pixels, line.X1, line.Y) && line.X1 < image.Width - 1)
          line.X1++;

        points[1] = line;


        line = new Line(0, image.Height - 1);

        while (IsBorderColor(pixels, line.X1, line.Y) && line.Y > 0 && line.X1 < image.Width - 1)
        {
          line.Y--;
          line.X1++;
        }

        line.X2 = image.Width - 1;
        while (IsBorderColor(pixels, line.X2, line.Y) && line.X2 > 0)
          line.X2--;

        points[2] = line;


        line = new Line(image.Width - 1, image.Height - 1);

        while (IsBorderColor(pixels, line.X2, line.Y) && line.Y > 0 && line.X2 > 0)
        {
          line.Y--;
          line.X2--;
        }

        line.X1 = 0;
        while (IsBorderColor(pixels, line.X1, line.Y) && line.X1 < image.Width - 1)
          line.X1++;

        points[3] = line;
      }

      MagickGeometry geometry = new MagickGeometry(0, 0);

      SwapPoints(points);

      geometry = TestGeometry(geometry, points[0], points[3]);
      geometry = TestGeometry(geometry, points[1], points[2]);
      geometry = TestGeometry(geometry, points[0], points[2]);
      geometry = TestGeometry(geometry, points[1], points[3]);

      return geometry;
    }

    private void ExecuteInnerTrim(MagickImage image)
    {
      MagickGeometry area = GetLargestArea(image);

      image.Rotate(90);
      MagickGeometry rotatedArea = GetLargestArea(image);

      if (rotatedArea > area)
      {
        Crop(image, rotatedArea);
        image.Rotate(-90);
      }
      else
      {
        image.Rotate(-90);
        Crop(image, area);
      }
    }

    private void ExecuteOuterTrim(MagickImage image)
    {
      image.BackgroundColor = _BorderColor;
      image.ColorFuzz = ColorFuzz;
      image.Trim();
      image.RePage();

      MagickGeometry geometry = new MagickGeometry(0, 0, image.Width, image.Height);
      ShiftGeometry(geometry);
      Crop(image, geometry);
    }

    private bool IsBorderColor(PixelCollection pixels, int x, int y)
    {
      MagickColor color = pixels.GetPixel(x, y).ToColor();
      return color.FuzzyEquals(_BorderColor, ColorFuzz);
    }

    private void ShiftGeometry(MagickGeometry geometry)
    {
      geometry.X += PixelShift.Left;
      geometry.Y += PixelShift.Top;
      geometry.Width -= PixelShift.Right;
      geometry.Height -= PixelShift.Bottom;
    }

    private static void SwapPoints(Line[] points)
    {
      Line swap;
      if (points[0].Y > points[1].Y)
      {
        swap = points[0];
        points[0] = points[1];
        points[1] = swap;
      }
      if (points[3].Y < points[2].Y)
      {
        swap = points[2];
        points[2] = points[3];
        points[3] = swap;
      }
    }

    private MagickGeometry TestGeometry(MagickGeometry geometry, Line line1, Line line2)
    {
      int x = Math.Max(line1.X1, line2.X1);
      int y = line1.Y;

      int width = Math.Min(line1.X2, line2.X2) - x;
      int height = line2.Y - line1.Y;

      MagickGeometry newGeometry = new MagickGeometry(x, y, width, height);

      return newGeometry > geometry ? newGeometry : geometry;
    }

    /// <summary>
    /// Creates a new instance of the AutotrimScript class.
    /// </summary>
    public AutotrimScript()
    {
      Reset();
    }

    /// <summary>
    /// Any location within the border area for the algorithm to find the base border color.
    /// </summary>
    public Coordinate BorderColorLocation
    {
      get;
      set;
    }

    /// <summary>
    /// The fuzz amount specified as a percent 0 to 100. The default is zero which indicates that
    /// border is a uniform color. Larger values are needed when the border is not a uniform color
    /// and to trim the border of the rotated area where the image data is a blend with the
    /// border color.
    /// </summary>
    public Percentage ColorFuzz
    {
      get;
      set;
    }

    /// <summary>
    /// Mode of trim. Default is outer trim (false).
    /// </summary>
    public bool InnerTrim
    {
      get;
      set;
    }

    /// <summary>
    /// The number of extra pixels to shift the trim of the image.
    /// </summary>
    public AutotrimPixelShift PixelShift
    {
      get;
      private set;
    }

    /// <summary>
    /// Automatically unrotates a rotated image and trims the surrounding border.
    /// </summary>
    /// <param name="input">The image to execute the script on.</param>
    public MagickImage Execute(MagickImage input)
    {
      if (input == null)
        throw new ArgumentNullException("input");

      MagickImage output = input.Clone();
      _BorderColor = GetBorderColor(output);

      if (InnerTrim)
        ExecuteInnerTrim(output);
      else
        ExecuteOuterTrim(output);

      return output;
    }

    /// <summary>
    /// Resets the script to the default setttings.
    /// </summary>
    public void Reset()
    {
      BorderColorLocation = new Coordinate(0, 0);
      ColorFuzz = (Percentage)0;
      InnerTrim = false;
      PixelShift = new AutotrimPixelShift();
    }
  }
}
