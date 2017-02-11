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
    /// Applies the cartoon effect from Fred's ImageMagick scripts http://www.fmwconcepts.com/imagemagick/cartoon/index.php to a <see cref="MagickImage"/>
    /// </summary>
    /// <remarks>
    /// Assumes version of Magick.NET version is >= 6.8.5.4 but could work on older version as well.
    /// Some modifications might be required. Please see the initialization steps in the original script for details.
    /// </remarks>    
    /// <example> 
    /// Example1:
    /// <code>
    /// MagickImage image = new FredsImageMagickScripts.CartoonEffectScript().Execute(image);
    /// </code>
    /// Example2: Change some of the default properties.
    /// <code>
    /// MagickImage image = new FredsImageMagickScripts.CartoonEffectScript(){ Method = 2, (Percentage)Pattern = 70, Edgeamount = 5, Numlevels = 7}.Execute(image);
    /// </code>
    /// </example>
    public sealed class CartoonEffectScript
    {
        /// <summary>
        /// Constructor only sets default property values so that it is ready for a call to its  <see cref="Execute(MagickImage)"/> method.
        /// </summary>
        public CartoonEffectScript()
        {
            SetDefaults();
        }

        /// <summary>
        /// Resets the script to the default setttings.
        /// </summary>
        public void Reset()
        {
            SetDefaults();
        }

        /// <summary>
        /// Resets the script to the default setttings. Gets called upon construction of the class.
        /// </summary>
        private void SetDefaults()
        {
            Pattern = (Percentage)70;
            Numlevels = 6;
            Method = CartoonMethod.Method1;
            Edgeamount = 4;
            Brightness = (Percentage)100;
            Saturation = (Percentage)150;
            Edgewidth = 2;
            Edgethresh = (Percentage)90;
        }


        #region Properties
        /// <summary>
        ///  Either 1 or 2.
        /// </summary>
        public CartoonMethod Method
        {
            get
            {
                return _Method;
            }
            set
            {
                //if (value != 1 && value != 2)
                //{
                //    throw new ArgumentOutOfRangeException("value", Method, "Method must be either 1 or 2");
                //}
                _Method = value;
            }
        }

        /// <summary>
        /// Segmentation pattern.
        /// </summary>
        public Percentage Pattern
        {
            get
            {
                return _Pattern;
            }

            set
            {
                _Pattern = value;
            }
        }

        /// <summary>
        /// Number of levels, which must be &gt;= 2
        /// </summary>
        public int Numlevels
        {
            get
            {
                return _Numlevels;
            }
            set
            {
                if (value < 2)
                {
                    throw new ArgumentOutOfRangeException("value", Numlevels, "Numlevels must be >=2");
                }
                _Numlevels = value;
            }
        }

        /// <summary>
        /// Edge amount, which must be &gt;= 0
        /// </summary>
        public float Edgeamount
        {
            get
            {
                return _Edgeamount;
            }
            set
            {
                if (value < 0 || float.IsInfinity(value) || float.IsNaN(value))
                {
                    throw new ArgumentOutOfRangeException("value", Edgeamount, "Edgeamount must be >= 0");
                }
                _Edgeamount = value;
            }
        }

        /// <summary>
        /// Edge width, which must be &gt;= 0
        /// </summary>
        public int Edgewidth
        {
            get
            {
                return _EdgeWith;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value", Edgewidth, "EdgeWith must be >= 0");
                }
                _EdgeWith = value;
            }
        }

        /// <summary>
        /// Edge threshold
        /// </summary>
        public Percentage Edgethresh
        {
            get
            {
                return _Edgethresh;
            }

            set
            {
                _Edgethresh = value;
            }
        }

        /// <summary>
        /// The brightness factor. Valid values are zero or higher. The default is 1. Increase
        /// brightness is larger than 1, decrease brightness is less than 1.
        /// </summary>
        public Percentage Brightness
        {
            get
            {
                return _Brightness;
            }

            set
            {
                _Brightness = value;
            }
        }

        /// <summary>
        /// Saturation. Valid values are zero or higher. A value of 100 is no change. The default is 150.
        /// </summary>
        public Percentage Saturation
        {
            get
            {
                return _Saturation;
            }

            set
            {
                _Saturation = value;
            }
        }
        #endregion


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
                                                fifth_0.Evaluate(Channels.All, EvaluateOperator.Pow, Edgeamount);
                                                fifth_0.Threshold((Percentage)Edgethresh);
                                                fifth_0.Statistic(StatisticType.Median, 3, 3);

                                                //  -delete 0,1,3,4 -compose over -compose multiply -composite "$outfile"
                                                fifth_0.Composite(second_0, CompositeOperator.Multiply);

                                                return fifth_0; // return copy as it will otherwise get disposed
                                            }
                                        }
                                    }
                                }
                            }

                        case CartoonMethod.Method2:
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
                                        third.SetArtifact("convolve:scale", Edgegain.ToString(System.Globalization.CultureInfo.InvariantCulture));
                                        third.Morphology(MorphologyMethod.Convolve, Kernel.DoG, "0,0," + Edgewidth);
                                        third.Negate();
                                        third.Evaluate(Channels.All, EvaluateOperator.Pow, _Edgeamount);
                                        third.WhiteThreshold(Edgethresh);

                                        //  -delete 0,1 - compose over - compose multiply - composite "$outfile"
                                        var result = second_0.Clone();
                                        result.Composite(third, CompositeOperator.Multiply);
                                        return result;
                                    }
                                }
                            }

                        default:
                            throw new InvalidOperationException("Enum value of Method property not supported");
                    }
                }
            }
        }


        #region Constants
        /// <summary>
        /// Gain for method=2
        /// </summary>
        private const int Edgegain = 4;
        #endregion

        #region Backup variables for properties
        private CartoonMethod _Method;
        private int _Numlevels;
        private float _Edgeamount;
        private Percentage _Brightness;
        private Percentage _Edgethresh;
        private Percentage _Pattern;
        private Percentage _Saturation;
        private int _EdgeWith;
        #endregion
    }
}
