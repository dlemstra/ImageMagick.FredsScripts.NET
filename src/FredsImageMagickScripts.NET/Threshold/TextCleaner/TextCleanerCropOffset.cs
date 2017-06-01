// Copyright 2015-2017 Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/FredsImageMagickScripts.NET)
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

namespace FredsImageMagickScripts
{
    /// <summary>
    /// Crop offsets for the TextCleaner script.
    /// </summary>
    public sealed class TextCleanerCropOffset
    {
        internal TextCleanerCropOffset()
        {
            Bottom = 0;
            Left = 0;
            Right = 0;
            Top = 0;
        }

        /// <summary>
        /// Gets or sets the width, in pixels, of the lower side of the bounding rectangle.
        /// </summary>
        public int Bottom
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width, in pixels, of the left side of the bounding rectangle.
        /// </summary>
        public int Left
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width, in pixels, of the right side of the bounding rectangle.
        /// </summary>
        public int Right
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width, in pixels, of the upper side of the bounding rectangle.
        /// </summary>
        public int Top
        {
            get;
            set;
        }

        internal bool IsSet
        {
            get
            {
                return Bottom != 0 || Left != 0 || Right != 0 || Top != 0;
            }
        }

        internal bool IsValid
        {
            get
            {
                return Bottom >= 0 && Left >= 0 && Right >= 0 && Top >= 0;
            }
        }
    }
}
