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

namespace FredsImageMagickScripts.Shared
{
  /// <summary>
  /// Class that is used to calculate an aspect ratio.
  /// </summary>
  public static class AspectRatio
  {
    /// <summary>
    /// Calculates the aspect ratio using the specified coordinates.
    /// </summary>
    /// <param name="topLeft">Top left coordinate</param>
    /// <param name="topRight">Top right coordinate</param>
    /// <param name="bottomRight">Bottom right coordinate</param>
    /// <param name="bottomLeft">Bottom left coordinate</param>
    /// <returns></returns>
    public static double Calculate(PointD topLeft, PointD topRight, PointD bottomRight, PointD bottomLeft)
    {
      // get centroid of quadrilateral
      var centroidX = (bottomLeft.X + bottomRight.X + topLeft.X + topRight.X) / 4;
      var centroidY = (bottomLeft.Y + bottomRight.Y + topLeft.Y + topRight.Y) / 4;

      return Calculate(topLeft, topRight, bottomRight, bottomLeft, centroidX, centroidY);
    }

    /// <summary>
    /// Calculates the aspect ratio using the specified coordinates.
    /// </summary>
    /// <param name="topLeft">Top left coordinate</param>
    /// <param name="topRight">Top right coordinate</param>
    /// <param name="bottomRight">Bottom right coordinate</param>
    /// <param name="bottomLeft">Bottom left coordinate</param>
    /// <param name="centroidX">Centroid x</param>
    /// <param name="centroidY">Centroid y</param>
    /// <returns></returns>
    public static double Calculate(PointD topLeft, PointD topRight, PointD bottomRight, PointD bottomLeft, double centroidX, double centroidY)
    {
      // convert to proper x,y coordinates relative to center
      var m1x = bottomLeft.X - centroidX;
      var m1y = centroidY - bottomLeft.Y;
      var m2x = bottomRight.X - centroidX;
      var m2y = centroidY - bottomRight.Y;
      var m3x = topLeft.X - centroidX;
      var m3y = centroidY - topLeft.Y;
      var m4x = topRight.X - centroidX;
      var m4y = centroidY - topRight.Y;

      // simplified equations, assuming u0=0, v0=0, s=1
      var k2 = ((m1y - m4y) * m3x - (m1x - m4x) * m3y + m1x * m4y - m1y * m4x) / ((m2y - m4y) * m3x - (m2x - m4x) * m3y + m2x * m4y - m2y * m4x);
      var k3 = ((m1y - m4y) * m2x - (m1x - m4x) * m2y + m1x * m4y - m1y * m4x) / ((m3y - m4y) * m2x - (m3x - m4x) * m2y + m3x * m4y - m3y * m4x);
      var ff = ((k3 * m3y - m1y) * (k2 * m2y - m1y) + (k3 * m3x - m1x) * (k2 * m2x - m1x)) / ((k3 - 1) * (k2 - 1));
      var f = Math.Sqrt(Math.Sqrt(ff * ff));
      var aspect = Math.Sqrt((Math.Pow(k2 - 1, 2) + Math.Pow(k2 * m2y - m1y, 2) / Math.Pow(f, 2) + Math.Pow(k2 * m2x - m1x, 2) / Math.Pow(f, 2)) / (Math.Pow(k3 - 1, 2) + Math.Pow(k3 * m3y - m1y, 2) / Math.Pow(f, 2) + Math.Pow(k3 * m3x - m1x, 2) / Math.Pow(f, 2)));

      return aspect;
    }
  }
}
