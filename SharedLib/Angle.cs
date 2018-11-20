using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib
{
    /// <summary>
    /// Emulates the concept of a circular angle
    /// Units: Degrees, Gradians, Radians, and Turns
    /// Allows custom formatting of the Angle
    /// object when the ToString is called to 
    /// display the value, precision, and symbol
    /// </summary>
    [SpecialClass(4)]
    class Angle : IFormattable
    {
        #region fields and properties

        // The decimal pi is more precise than the Math.PI constant which will help 
        // with minimizing round-off errors due to multiple conversions 
        // from one angle type to another.
        public const decimal pi = 3.1415926535897932384626434M;
        public const decimal twoPi = 2M * pi;

        // Private variable for the property Value of the Angle 
        private decimal _Value = 0M;

        // Private variable for the property Units of the Angle enum
        private AngleUnits _Units = AngleUnits.Degrees;

        // Represents the conversion matrix to convert one angle type to another
        // The value of each cell of the above array represents what to multiply 
        // an angle value by to convert from the column angle type to the row 
        // angle type.So, to convert an angle from radians to gradians you would 
        // multiply the radian value by 200/π.Also, the row and column index 
        // values correspond to the AngleUnits enum values
        private static readonly decimal[,] _ConversionFactors = {
            { 1M, 9M/10M, 180M/pi, 360M},
            { 10M/9M, 1M, 200M/pi, 400M},
            {pi/180M, pi/200M, 1M, twoPi},
            {1M/360M, 1M/400M, 1M/twoPi, 1M} };

        // Property for the Angle Value
        // Calls additional methods prior to assigning the incoming 
        // values to their respective fields
        public decimal Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = Normalize(value, Units);
            }
        }

        // Property for the Angle Unit enum
        // Calls additional methods prior to assigning the incoming 
        // values to their respective fields
        public AngleUnits Units
        {
            get
            {
                return _Units;
            }
            set
            {
                _Value = ConvertAngleValue(Value, Units, value);
                _Units = value;
            }
        }

        #endregion fields and properties

        #region constructors

        /// <summary>
        /// Default Constructor
        /// Calls Main Chained Constructor with default values
        /// Default is an angle of 0 degrees
        /// </summary>
        public Angle() : this(0M, AngleUnits.Degrees)
        {

        }

        /// <summary>
        /// Copy constructor
        /// Calls main chained constructor
        /// </summary>
        /// <param name="other">(Angle) object to copy from</param>
        public Angle(Angle other) : this(other.Value, other.Units)
        {

        }

        /// <summary>
        /// Main constructor
        /// Accepts the value and units parameters
        /// Assign these values to their corresponding property 
        /// mutators in the following order: Units then Value. 
        /// This ensures the Value mutator logic is executed 
        /// only after the correct angle unit of measure is assigned.
        /// Otherwise the Normalize() method will assume degrees 
        /// and cause data loss if another unit of measure is desired.
        /// </summary>
        /// <param name="value">(decimal) value of the angle</param>
        /// <param name="units">(AngleUnits) enum of the Angle</param>
        public Angle(decimal value, AngleUnits units = AngleUnits.Degrees)
        {
            Units = units;
            Value = value;
        }

        #endregion constructors

        #region Normalize methods

        /// <summary>
        /// The Normalize() method ensures that the provided value is within the legal range of angle values
        ///for the given unit of measure. Therefore, if the units parameter indicates AngleUnits.Degrees, the
        ///value parameter must be greater than or equal to 0 and less than 360. If the value is less than 0
        ///then 360 must be added to value repeatedly until it is non-negative.Likewise, if the value is
        ///greater than or equal to 360, then 360 must be repeatedly subtracted from value until it is within
        ///the specified range.The same technique is used for the other three angle types, just with a
        ///different maximum value:
        ///• Degrees: 360
        ///• Gradians: 400
        ///• Radians: 2π 
        // • Turns: 1
        /// Finally, return the normalized value.
        /// </summary>
        /// <param name="value">(decimal) value of the angle</param>
        /// <param name="units">(AngleUnits) enum unit of measure</param>
        /// <returns>return the normalized value. (decimal)</returns>
        private static decimal Normalize(decimal value, AngleUnits units)
        {

            decimal minimum;
            decimal maximum;

            switch (units)
            {
                case AngleUnits.Degrees:

                    minimum = 0M;
                    maximum = 360M;

                    return NormalizeValue(value, minimum, maximum);

                case AngleUnits.Gradians:

                    minimum = 0M;
                    maximum = 400M;

                    return NormalizeValue(value, minimum, maximum);

                case AngleUnits.Radians:

                    minimum = 0M;
                    maximum = twoPi;

                    return NormalizeValue(value, minimum, maximum);

                case AngleUnits.Turns:

                    minimum = 0M;
                    maximum = 1M;

                    return NormalizeValue(value, minimum, maximum);

                default:
                    throw new ArgumentException("The AngleUnits enum value is not valid");
            } // end of switch statement

        } // end of method Normalize

        /// <summary>
        /// Normalizes a unitless value based on a minimum and maximum.
        /// This method was written to eliminate redundant code in the
        /// Normalize method
        /// </summary>
        /// <param name="value">(decimal) value to be normalized</param>
        /// <param name="minimum">(decimal) minimum value range</param>
        /// <param name="maximum">(decimal) maximum value range</param>
        /// <returns>(decimal) normalized unitless value</returns>
        private static decimal NormalizeValue(decimal value, decimal minimum, decimal maximum)
        {

            while ((value < minimum) || (value > maximum))
            {
                if (value < 0)
                {
                    value += maximum;
                }

                if (value > maximum)
                {
                    value -= maximum;
                }
            } // end of while loop

            return value;
        } // end of method NormalizeValue

        #endregion Normalize methods

        #region conversion methods

        /// <summary>
        /// Converts the given angle value from one unit type to another
        /// </summary>
        /// <param name="angle">(decimal) angle value</param>
        /// <param name="fromUnits">(AngleUnits) enum from Angle unit</param>
        /// <param name="toUnits">(AngleUnits) enum to Angle unit</param>
        /// <returns>(decimal) angle converted value</returns>
        private static decimal ConvertAngleValue(decimal angle, AngleUnits fromUnits, AngleUnits toUnits)
        {
            decimal factor = _ConversionFactors[(int)toUnits, (int)fromUnits];

            return Normalize(angle * factor, toUnits);

        }

        /// <summary>
        /// Converts the current Angle object to a new Angle object
        /// with the Degrees Unit of Measurement
        /// </summary>
        /// <returns>(Angle) object with Degrees unit</returns>
        public Angle ToDegrees()
        {
            return new Angle(ConvertAngleValue(Value, Units, AngleUnits.Degrees), AngleUnits.Degrees);
        }

        /// <summary>
        /// Converts the current Angle object to a new Angle object
        /// with the Gradians Unit of Measurement
        /// </summary>
        /// <returns>(Angle) object with Gradians unit</returns>
        public Angle ToGradians()
        {
            return new Angle(ConvertAngleValue(Value, Units, AngleUnits.Gradians), AngleUnits.Gradians);
        }

        /// <summary>
        /// Converts the current Angle object to a new Angle object
        /// with the Radians Unit of Measurement
        /// </summary>
        /// <returns>(Angle) object with Radians unit</returns>
        public Angle ToRadians()
        {
            return new Angle(ConvertAngleValue(Value, Units, AngleUnits.Radians), AngleUnits.Radians);
        }

        /// <summary>
        /// Converts the current Angle object to a new Angle object
        /// with the Turns Unit of Measurement
        /// </summary>
        /// <returns>(Angle) object with Turns unit</returns>
        public Angle ToTurns()
        {
            return new Angle(ConvertAngleValue(Value, Units, AngleUnits.Turns), AngleUnits.Turns);
        }

        /// <summary>
        /// Convert the given object to a new angle unit of measure
        /// </summary>
        /// <param name="targetUnits"></param>
        /// <returns>(Angle) object of converted angle with measured units</returns>
        public Angle ConvertAngle(AngleUnits targetUnits)
        {
            switch (targetUnits)
            {
                case AngleUnits.Degrees:
                    return ToDegrees();
                case AngleUnits.Gradians:
                    return ToGradians();   
                case AngleUnits.Radians:
                    return ToRadians();
                case AngleUnits.Turns:
                    return ToTurns();
                default:
                    throw new ArgumentException("The target unit is not a valid enum of type AngleUnits");
            }
        }

        #endregion conversion methods

        #region Mathematical Operators Overrides

        /// <summary>
        /// Adds two Angle objects together, returning the resulting 
        /// value as a new Angle object. Since the two angles may use 
        /// different units of measure we are going to assume that the 
        /// resulting angle will always have the same unit of measure 
        /// as the first angle (which corresponds to the left operand).
        /// </summary>
        /// <param name="a1">(Angle) first angle</param>
        /// <param name="a2">(Angle) second angle</param>
        /// <returns>(Angle) resultant angle after the operation</returns>
        public static Angle operator + (Angle a1, Angle a2)
        {
            decimal converted = ConvertAngleValue(a2.Value, a2.Units, a1.Units);
            decimal result = a1.Value + converted;
            return new Angle(result, a1.Units);
        }

        /// <summary>
        /// Subtracts two Angle objects together, returning the resulting 
        /// value as a new Angle object. Since the two angles may use 
        /// different units of measure we are going to assume that the 
        /// resulting angle will always have the same unit of measure 
        /// as the first angle (which corresponds to the left operand).
        /// </summary>
        /// <param name="a1">(Angle) first angle</param>
        /// <param name="a2">(Angle) second angle</param>
        /// <returns>(Angle) resultant angle after the operation</returns>
        public static Angle operator - (Angle a1, Angle a2)
        {
            decimal converted = ConvertAngleValue(a2.Value, a2.Units, a1.Units);
            decimal result = a1.Value - converted;
            return new Angle(result, a1.Units);
        }

        /// <summary>
        /// Adds one Angle object and a scalar together, returning the resulting 
        /// value as a new Angle object. 
        /// </summary>
        /// <param name="a1">(Angle) first angle</param>
        /// <param name="a2">(decimal) scalar</param>
        /// <returns>(Angle) resultant angle after the operation</returns>
        public static Angle operator + (Angle a, decimal scalar)
        {
            decimal result = a.Value + scalar;
            return new Angle(result, a.Units);
        }

        /// <summary>
        /// Subtracts one Angle object and a scalar together, returning the resulting 
        /// value as a new Angle object. 
        /// </summary>
        /// <param name="a1">(Angle) first angle</param>
        /// <param name="a2">(decimal) scalar</param>
        /// <returns>(Angle) resultant angle after the operation</returns>
        public static Angle operator - (Angle a, decimal scalar)
        {
            decimal result = a.Value - scalar;
            return new Angle(result, a.Units);
        }

        /// <summary>
        /// Multiples one Angle object and a scalar together, returning the resulting 
        /// value as a new Angle object. 
        /// </summary>
        /// <param name="a1">(Angle) first angle</param>
        /// <param name="a2">(decimal) scalar</param>
        /// <returns>(Angle) resultant angle after the operation</returns>
        public static Angle operator * (Angle a, decimal scalar)
        {
            decimal result = a.Value * scalar;
            return new Angle(result, a.Units);
        }

        /// <summary>
        /// Divides one Angle object and a scalar together, returning the resulting 
        /// value as a new Angle object. 
        /// </summary>
        /// <param name="a1">(Angle) first angle</param>
        /// <param name="a2">(decimal) scalar</param>
        /// <returns>(Angle) resultant angle after the operation</returns>
        public static Angle operator / (Angle a, decimal scalar)
        {
            if (scalar == 0)
            {
                throw new DivideByZeroException("Your scalar input cannot be 0");
            }

            decimal result = a.Value / scalar;
            return new Angle(result, a.Units);
        }

        #endregion Mathematical Operators Overrides

        #region Comparison Operators Overrides

        /// <summary>
        /// Equality comparison of two Angle objects together, returning the resulting 
        /// value as a new Angle object. Since the two angles may use 
        /// different units of measure we are going to assume that the 
        /// resulting angle will always have the same unit of measure 
        /// as the first angle (which corresponds to the left operand).
        /// </summary>
        /// <param name="a1">(Angle) first angle</param>
        /// <param name="a2">(Angle) second angle</param>
        /// <returns>(boolean) true if left operand , false otherwise</returns>
        public static bool operator == (Angle a, Angle b)
        {
            object o1 = a;
            object o2 = b;

            if (o1 == null && o2 == null)
            {
                return true;
            }
            else if (o1 == null ^ o2 == null)
            {
                return false;
            }
            else
            {
                Angle normAngle = b.ConvertAngle(a.Units);
                return a.Value.ApproximatelyEquals(normAngle.Value);
            }

        }

        /// <summary>
        /// Not Equality comparison of two Angle objects together, returning the resulting 
        /// value as a new Angle object. Since the two angles may use 
        /// different units of measure we are going to assume that the 
        /// resulting angle will always have the same unit of measure 
        /// as the first angle (which corresponds to the left operand).
        /// </summary>
        /// <param name="a1">(Angle) first angle</param>
        /// <param name="a2">(Angle) second angle</param>
        /// <returns>(boolean) true if left operand , false otherwise</returns>
        public static bool operator != (Angle a, Angle b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Less than comparison of two Angle objects together, returning the resulting 
        /// value as a new Angle object. Since the two angles may use 
        /// different units of measure we are going to assume that the 
        /// resulting angle will always have the same unit of measure 
        /// as the first angle (which corresponds to the left operand).
        /// </summary>
        /// <param name="a1">(Angle) first angle</param>
        /// <param name="a2">(Angle) second angle</param>
        /// <returns>(boolean) true if left operand , false otherwise</returns>
        public static bool operator < (Angle a, Angle b)
        {

            if (a == null && b == null)
            {
                return true;
            }
            else if (a == null)
            {
                return true;
            }
            else if (b == null)
            {
                return false;
            }
            else
            {
                Angle normAngle = b.ConvertAngle(a.Units);
                return !(a.Value.ApproximatelyEquals(normAngle.Value)) && (a.Value < normAngle.Value);
            }
        }

        /// <summary>
        /// Greater than comparison of two Angle objects together, returning the resulting 
        /// value as a new Angle object. Since the two angles may use 
        /// different units of measure we are going to assume that the 
        /// resulting angle will always have the same unit of measure 
        /// as the first angle (which corresponds to the left operand).
        /// </summary>
        /// <param name="a1">(Angle) first angle</param>
        /// <param name="a2">(Angle) second angle</param>
        /// <returns>(boolean) true if left operand , false otherwise</returns>
        public static bool operator > (Angle a, Angle b)
        {
            return !(a == b || a < b);
        }

        /// <summary>
        /// Less than or Equal comparison of two Angle objects together, returning the resulting 
        /// value as a new Angle object. Since the two angles may use 
        /// different units of measure we are going to assume that the 
        /// resulting angle will always have the same unit of measure 
        /// as the first angle (which corresponds to the left operand).
        /// </summary>
        /// <param name="a1">(Angle) first angle</param>
        /// <param name="a2">(Angle) second angle</param>
        /// <returns>(boolean) true if left operand , false otherwise</returns>
        public static bool operator <= (Angle a, Angle b)
        {
            return (a < b || a == b);
        }

        /// <summary>
        /// Greater than comparison of two Angle objects together, returning the resulting 
        /// value as a new Angle object. Since the two angles may use 
        /// different units of measure we are going to assume that the 
        /// resulting angle will always have the same unit of measure 
        /// as the first angle (which corresponds to the left operand).
        /// </summary>
        /// <param name="a1">(Angle) first angle</param>
        /// <param name="a2">(Angle) second angle</param>
        /// <returns>(boolean) true if left operand , false otherwise</returns>
        public static bool operator >= (Angle a, Angle b)
        {
            return (a > b || a == b);
        }

        #endregion Comparison Operators Overrides

        #region Object Overrides

        /// <summary>
        /// Object Equals override for Angle object
        /// </summary>
        /// <param name="obj">(object) angle</param>
        /// <returns>boolean true if this equal to input</returns>
        public override bool Equals(object obj)
        {
            return this == obj as Angle;
        }

        /// <summary>
        /// Unique enough hash code override for the Angle object
        /// </summary>
        /// <returns>(int) hash code </returns>
        public override int GetHashCode()
        {
            return ConvertAngleValue(Value, Units, AngleUnits.Degrees).GetHashCode();
        }

        #endregion Object Overrides

        #region Angle Class Conversion Operators

        /// <summary>
        /// casts that converts an Angle to another decimal value type
        /// </summary>
        /// <param name="a">(Angle) a to convert to another numeric type</param>
        public static explicit operator decimal(Angle a)
        {
            return a.Value;
        }

        /// <summary>
        /// casts that converts an Angle to another double value type
        /// </summary>
        /// <param name="a">(Angle) a to convert to another numeric type</param>
        public static explicit operator double(Angle a)
        {
            return (double)a.Value;
        }

        #endregion Angle Class Conversion Operators

        #region Support Angle Formatting

        /// <summary>
        /// Method to allow Angle to be tied to the formatter class
        /// This method is invoked whenever an Angle object is passed 
        /// to string.Format() or used in string interpolation.
        /// </summary>
        /// <param name="format">(string) format code</param>
        /// <param name="formatProvider">format Provider object</param>
        /// <returns>(string) Formated String</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return new AngleFormatter().Format(format, this, formatProvider);
        }

        /// <summary>
        /// Method to allow Angle to be tied to the formatter class
        /// This method is invoked whenever an Angle object is passed 
        /// to string.Format() or used in string interpolation.
        /// </summary>
        /// <param name="format">(string) format code</param>
        /// <returns>(string) Formated String</returns>
        public string ToString(string format)
        {
            AngleFormatter fmt = new AngleFormatter();
            return fmt.Format(format, this, fmt);
        }

        /// <summary>
        /// Method to allow Angle to be tied to the formatter class
        /// This method is invoked whenever an Angle object is passed 
        /// to string.Format() or used in string interpolation.
        /// </summary>
        /// <returns>(string) Formated String</returns>
        public override string ToString()
        {
            return ToString(string.Empty);
        }

        #endregion Support Angle Formatting

    } // end of class
} // end of namespace
