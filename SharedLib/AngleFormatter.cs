using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib
{
    /// <summary>
    /// Angle Custom Formatting
    /// </summary>
    [SpecialClass(3)]
    class AngleFormatter : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        /// This method is essentially a “boilerplate” method, 
        /// in that it will be the same for any formatter under 
        /// most circumstances.
        /// </summary>
        /// <param name="formatType">Type of the format</param>
        /// <returns>a generic object</returns>
        public object GetFormat(Type formatType)
        {
            if (typeof(ICustomFormatter).Equals(formatType))
            {
                return this;
            }
            return null;
        }

        /// <summary>
        /// Method is where the actual formatting of an Angle object will occur.
        /// Takes to account the format type and the Angle units type
        /// </summary>
        /// <param name="format">(string) format code</param>
        /// <param name="arg">(object) angle object</param>
        /// <param name="formatProvider">formatProvider</param>
        /// <returns>(string) formated string</returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            char formatCode;
            int digits;

            if (arg == null)
            {
                throw new NullReferenceException("Input object argument is null");
            }

            if (!(arg is Angle))
            {
                if (arg is IFormattable)
                {
                    return ((IFormattable)arg).ToString(format, formatProvider);
                }
                else
                {
                    return arg.ToString();
                }
            }
            // Process Angle Object
            else
            {
                Angle angle = arg as Angle;

                if (string.IsNullOrEmpty(format) || char.ToUpper(format.First()) == 'C' )
                {
                    // set format to the appropriate code based on the input angle’s Units value.
                    // So, AngleUnits.Degrees would use the code “d”.

                    switch (angle.Units)
                    {
                        case AngleUnits.Degrees:
                            // Format code "d"
                            formatCode = 'd';
                            break;
                        case AngleUnits.Gradians:
                            // Format code "g"
                            formatCode = 'g';
                            break;
                        case AngleUnits.Radians:
                            // Format code "r"
                            formatCode = 'r';
                            break;
                        case AngleUnits.Turns:
                            // Format code "t"
                            formatCode = 't';
                            break;
                        default:
                            throw new ArgumentException("Angle not a valid enum unit of AngleUnits");
                    }
                }

                else
                {
                    // Take the format code from the format string's first value
                    formatCode = char.ToLower(format.First());
                }

                if (format != null && format.Length > 1 && int.TryParse(format.Substring(1), out digits))
                {
                    // If format’s length is greater than 1 then attempt to convert 
                    // the remaining text after the first character to an int. 
                    // The Constrain() extension method can ensure the int value is kept between 0 and 9.

                    digits.Constrain(0, 9);
                    
                }

                else
                {
                    // If no digits are provided after the format code letter, 
                    // then assume 2 decimal places for the output (unless it is one of the radian types which should be 5).

                    if (formatCode == 'r' || formatCode == 'p')
                    {
                        digits = 5;
                        
                    }
                    else
                    {
                        digits = 2;
                    }
               
                }

                // Returns the Formated String 
                return FormatAngle(angle, formatCode, digits);

            }
        } // end of method format

        /// <summary>
        /// Supplemental Helper Method for Format to reduce
        /// redundant code and make it look more logical and concise
        /// </summary>
        /// <param name="angle">(Angle) object angle to be formatted</param>
        /// <param name="format">(Character) The format code</param>
        /// <param name="digits">(int) number of precision format f digits to display</param>
        /// <returns>(string) formatted string with the Angle units plus symbol</returns>
        public string FormatAngle(Angle angle, char format = 'c', int digits = 1)
        {
            decimal convertedDecimal;
            string outputDecimal;
            string outputUnit;
            Angle convertedAngle;

            // Convert angle to format
           
            // Format String On Unit Suffix to Append
            switch (char.ToLower(format))
            {
                case 'd':
                    // Covert first to degrees
                    convertedAngle = angle.ToDegrees();
                    // Set the Unit Text
                    outputUnit = AngleUnits.Degrees.ToSymbol();
                    break;
                case 'g':
                    // Convert first to gradians
                    convertedAngle = angle.ToGradians();
                    // Set the Unit Text
                    outputUnit = AngleUnits.Gradians.ToSymbol();
                    break;
                case 'p':
                    // Convert first to pi-radians
                    convertedAngle = angle.ToRadians();
                    convertedAngle.Value /= Angle.pi;
                    // Set the Unit Text
                    outputUnit = 'π' + AngleUnits.Radians.ToSymbol();
                    break;
                case 'r':
                    // Convert first to radians
                    convertedAngle = angle.ToRadians();
                    // Set the Unit Text
                    outputUnit = AngleUnits.Radians.ToSymbol();
                    break;
                case 't':
                    // Convert first to turns
                    convertedAngle = angle.ToTurns();
                    // Set the Unit Text
                    outputUnit = AngleUnits.Turns.ToSymbol();
                    break;
                default:
                    throw new FormatException("Invalid Format code");
            }

            // Format String on Digits
            convertedDecimal = convertedAngle.Value;

            // Make String Format Code (Example: F3) for precision
            string formatCode = "f" + digits.ToString();

            // Convert Decimal to Digits
            //outputDecimal = $"{convertedDecimal:{formatCode}";
            string formatString = String.Format("F{0:D}", digits);
            outputDecimal = convertedDecimal.ToString(formatString);

            // Return the Formated String
            return outputDecimal + outputUnit;
        }

    } // end of class
} // end of namespace
