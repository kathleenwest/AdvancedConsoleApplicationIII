using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLib
{
    /// <summary>
    /// Person class has the blueprints to make a person
    /// like object with associated properties to identify that
    /// person. Also includes a SpecialAttribute Identifer
    /// for dynamic runtime tests. 
    /// </summary>
    [SpecialClass(1)]
    public class Person
    {
        /// <summary>
        /// Enumeration of gender related values
        /// </summary>
        public enum Genders
        {
            Unknown,
            Male,
            Female,
            Other
        }; // end of enum

        // Properties that identify a Person class object
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime DOB { get; set; }
        public Genders Gender { get; set; }

        /// <summary>
        /// Default Constructor for the Person class
        /// </summary>
        public Person()
        {
            LastName = "Doe";
            FirstName = "John";
            DOB = DateTime.Now;
            Gender = Genders.Unknown;
        } // end of default constructor

        /// <summary>
        /// Parameterized Constructor for the Person class
        /// </summary>
        /// <param name="lastName">Last name of the person (string)</param>
        /// <param name="firstName">First name of the person (string)</param>
        /// <param name="dob">Date of Birth of the person (DateTime)</param>
        /// <param name="gender">Gender of the person (Genders enum)</param>
        public Person(string lastName, string firstName, DateTime dob, Genders gender)
        {
            LastName = lastName;
            FirstName = firstName;
            DOB = dob;
            Gender = gender;
        } // end of paramterized constructor

        /// <summary>
        /// ToString() override outputs a string representation
        /// of the object highlighting its properties 
        /// </summary>
        /// <returns>string override of the object</returns>
        public override string ToString()
        {
            return $"{LastName,10}, {FirstName,-10} {DOB,25} {Gender,8}";
        } // end of method

    } //end of class
} // end of namespace
