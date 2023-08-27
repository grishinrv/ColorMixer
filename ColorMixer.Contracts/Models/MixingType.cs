namespace ColorMixer.Contracts.Models
{
    /// <summary>
    /// Type of color mixing operation.
    /// </summary>
    public enum MixingType : byte
    {
        /// <summary>
        /// Yet not defined which type to use.
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// Two beams of light that are superimposed mix their colors additively. 
        /// </summary>
        Additive = 1,
        /// <summary>
        /// Correspond to the mixing of physical substances (such as paint).
        /// </summary>
        Subtractive = 2,
        /// <summary>
        /// Obtains a new color out of two component colors, with brightness equal to the average of the two components.
        /// </summary>
        Average = 3
    }
}
