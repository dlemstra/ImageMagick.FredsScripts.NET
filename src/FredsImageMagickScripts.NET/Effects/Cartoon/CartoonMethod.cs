// Copyright 2015-2020 Dirk Lemstra, Fred Weinhaus (https://github.com/dlemstra/FredsImageMagickScripts.NET)
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
    /// The algorithm to be used.
    /// The second method is faster but the results are also not that good (the edge lines are e.g. thinner and therefore less outstanding).
    /// </summary>
    public enum CartoonMethod
    {
        /// <summary>
        /// First method, which is the default and yields the best results but is also the most expensive.
        /// </summary>
        Method1,

        /// <summary>
        /// Second method, which is cheaper than Method1 to compute.
        /// </summary>
        Method2,

        /// <summary>
        /// Third method, which has the same computational complexity as Method2.
        /// The result is much smoother which means a reduction in edge highlighting but it can also make it look slightly blurry.
        /// </summary>
        Method3,

        /// <summary>
        /// Very similar to Method3 but looks less blurry and looks like a watercolour painting.
        /// </summary>
        Method4
    }
}
