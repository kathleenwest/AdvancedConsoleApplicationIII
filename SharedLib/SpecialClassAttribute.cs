using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLib
{
    /// <summary>
    /// The SpecialClassAttribute inherits from the attribute class
    /// to allow class type custom attributes with identifier (integer)
    /// numbers to identify itself. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SpecialClassAttribute : Attribute
    {
        // Identifer for the Attitube
        public int ID { get; set; } = 0;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SpecialClassAttribute()
        {
            ID = 0;
        } // end of constructor

        /// <summary>
        /// Paramterized Constructor
        /// </summary>
        /// <param name="id">identifer for the attitube (integer)</param>
        public SpecialClassAttribute(int id)
        {
            ID = id;
        } // end of parameterized constructor

    } // end of class
} // end of namespace
