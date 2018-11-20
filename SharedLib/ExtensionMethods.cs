using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib
{
    /// <summary>
    /// Useful extension methods for logic of Angle
    /// </summary>
    [SpecialClass(5)]
    public static class ExtensionMethods
    {
        /// <summary>
        /// This method determines if one decimal value is approximately 
        /// equal to another. Normally, we do not have to worry about 
        /// round-off issues when using decimal values. However, since 
        /// the value π is irrational and has an infinite number of decimal 
        /// places, only an approximate decimal value can be used to represent 
        /// it. Therefore, any time we convert to/from radians, a slight error 
        /// may be introduced. This method helps to work around that issue by 
        /// allowing two values to be considered equal even if they are very 
        /// slightly different. Since the numbers we are working with are generally 
        /// between 0 and 400 (gradians being the largest value), the margin of error, 
        /// indicated by the precision parameter, does not have to be too small. Instead 
        /// of using a hard-coded constant, however, it is an optional parameter allowing 
        /// the caller to supply their own value.
        /// </summary>
        /// <param name="v1">(decimal) input value to be compared</param>
        /// <param name="v2">(decimal) input value to be compared</param>
        /// <param name="precision">(decimal) input value for precision</param>
        /// <returns>boolean true if approximately equal, false otherwise</returns>
        public static bool ApproximatelyEquals(this decimal v1, decimal v2, decimal precision = 0.0000000001M)
        {

            return (Math.Abs(v2 - v1) < precision) ? true : false;
        }

        /// <summary>
        /// The Constrain() method ensures that the parameter value is between 
        /// min and max. If value is less than min, then value is set to min. 
        /// If value is greater than max, then value is set to max. Return value.
        /// </summary>
        /// <param name="value">(int) value of number to be constrained</param>
        /// <param name="min">(int) minimum value range</param>
        /// <param name="max">(int) maximum value range</param>
        /// <returns>(int) constrained value</returns>
        public static int Constrain(this int value, int min, int max)
        {
            if (value < min)
            {
                value = min;
            }

            if (value > max)
            {
                value = max;
            }

            return value;
        }

        /// <summary>
        /// This method accepts a value of type AngleUnits and returns 
        /// the appropriate string symbol that represents that unit type:
        /// </summary>
        /// <param name="units">(AngleUnits) units value</param>
        /// <returns>(string) of the unit symbol</returns>
        public static string ToSymbol(this AngleUnits units)
        {
            switch (units)
            {
                case AngleUnits.Degrees:
                    return "°";
                case AngleUnits.Gradians:
                    return "g";
                case AngleUnits.Radians:
                    return "rad";
                case AngleUnits.Turns:
                    return "tr";
                default:
                    throw new ArgumentException("The units value enum is invalid");
            }
        }

    } // end of class
} // end of namespace
