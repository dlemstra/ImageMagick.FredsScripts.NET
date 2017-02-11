namespace FredsImageMagickScripts
{
    /// <summary>
    /// The algorithm to be used.
    /// The second method is faster but the results are also not that good (the edge lines are e.g. thinner and therefore less outstanding)
    /// </summary>
    public enum CartoonMethod
    {
        /// <summary>
        /// First method, which is the default and yields the best results but is more expensive
        /// </summary>
        Method1,
        /// <summary>
        /// Second method, which is cheaper to compute
        /// </summary>
        Method2
    }
}
