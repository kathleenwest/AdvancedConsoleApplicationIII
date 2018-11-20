using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tester
{
    /// <summary>
    /// This project class is a test driver for demonstrating
    /// reflection, dynamics, and custom attributes.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main Entry Method for Program Class
        /// </summary>
        /// <param name="args">No arguments used</param>
        static void Main(string[] args)
        {
            // Create Run-Time Assembly Reference
            Assembly asm = Assembly.Load("SharedLib");

            // Reflection Test
            ReflectionTest(asm);

            // Person Test
            PersonTest(asm);

            // MathStuff Test
            MathStuffTest(asm);

            // Custom Attribute Test
            CustomAttributeTest(asm);

            // Wait for User 
            Console.Write("Press <ENTER> to quit...");
            Console.ReadLine();

        } // end of method

        /// <summary>
        /// This method tests dynamics on a Person type object
        /// in run-time given an assembly reference.
        /// </summary>
        /// <param name="asm">The reference assembly object (Assembly)</param>
        public static void PersonTest(Assembly asm)
        {
            // Print the Header
            Console.WriteLine();
            Console.WriteLine("{0} Person Test {0}", new string('=', 20));
            Console.WriteLine();

            // Create a Person and change the default properties
            // to custom fields at run-time
            dynamic p1 = asm.CreateInstance("SharedLib.Person");
            p1.LastName = "Smith";
            p1.FirstName = "Jane";
            p1.DOB = DateTime.Now;
            Type enumType = asm.GetType("SharedLib.Person+Genders");
            p1.Gender = (dynamic)Enum.Parse(enumType, "Female");
            Console.WriteLine(p1);

            // Create a Person object with the parameterized constructor
            dynamic p2 = asm.CreateInstance("SharedLib.Person", true, BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance, null,
            new object[] { "Smith", "John", DateTime.Parse("1/1/2000"), (dynamic)Enum.Parse(enumType, "Male") }, null, null);
            Console.WriteLine(p2);        

        } // end of method

        /// <summary>
        /// This method uses reflection to determine and print the
        /// hierarchy of a given assembly types, methods, fields, etc.
        /// </summary>
        /// <param name="asm">an assembly object of type Assembly</param>
        static void ReflectionTest(Assembly asm)
        {
            // Print Header
            Console.WriteLine();
            Console.WriteLine("{0} Reflection Test {0}", new string('=', 20));
            Console.WriteLine();

            // Print the Assembly Full Name
            Console.WriteLine("Assembly: {0}", asm.FullName);

            // Main Loop Print the Module
            foreach (Module m in asm.Modules)
            {
                Console.WriteLine(" Module: {0}", m.Name);

                // Main Loop to Print Types
                foreach (Type t in asm.GetTypes())
                {
                    Console.Write("  Type: {0} ", t.Name);

                    if (t.IsClass)
                    {
                        // Class
                        Console.Write("class ");
                    }

                    if (t.IsValueType)
                    {
                        Console.Write("struct ");
                    }


                    if (t.IsEnum)
                    {
                        Console.Write("enum ");
                    }

                    Console.WriteLine();


                    // Loop to print Constructors
                    foreach (ConstructorInfo ci in t.GetConstructors())
                    {
                        // Prints the Constructor Parameters
                        Console.WriteLine("    Constructor: {0}", ci.Name);
                        Console.WriteLine("      Parameters:");
                        foreach (ParameterInfo pi in ci.GetParameters())
                        {
                            Console.WriteLine("       {0} {1}", pi.ParameterType, pi.Name);
                        } // end of foreach
                    } // end of foreach

                    // Loop to print Events
                    foreach (EventInfo ei in t.GetEvents())
                    {
                        Console.WriteLine("    Events: {0} {1}", ei.EventHandlerType, ei.Name);
                    } // end of loop

                    // Loop to print Fields
                    foreach (FieldInfo fi in t.GetFields())
                    {
                        Console.WriteLine("    Field: {0} {1}", fi.FieldType, fi.Name);
                    } // end of loop

                    // Loop to print Properties
                    foreach (PropertyInfo pi in t.GetProperties())
                    {
                        Console.WriteLine("    Property: {0} {1}", pi.PropertyType, pi.Name);
                    } // end of loop

                    // Loop to print Methods
                    foreach (MethodInfo mi in t.GetMethods())
                    {
                        Console.WriteLine("    Method: {0} returns {1}", mi.Name, mi.ReturnType);
                        // Loop to print method parameters

                        Console.WriteLine("      Parameters:");
                        foreach (ParameterInfo pi in mi.GetParameters())
                        {
                            Console.WriteLine("       {0} {1}", pi.ParameterType, pi.Name);
                        } // end of loop
                    } // end of loop

                } // end of loop

            } // end of main loop


        } // end of method

        /// <summary>
        /// This method references an assembly's method at run-time
        /// and then invokes the method to perform mathematical functions
        /// </summary>
        /// <param name="asm">an object assembly of type Assembly</param>
        public static void MathStuffTest(Assembly asm)
        {
            // Prints Header
            Console.WriteLine();
            Console.WriteLine("{0} MathStuff Test {0}", new string('=', 20));
            Console.WriteLine();

            // Get the type for the class
            Type mathType = asm.GetType("SharedLib.MathStuff");
            // Get the Method for the class
            MethodInfo areaMethod = mathType.GetMethod("CalculateAreaCircle", BindingFlags.Public | BindingFlags.Static);
            // Invoke the method
            double area = (double)areaMethod.Invoke(null, new object[] { 12.34 });
            // Print result to screen
            Console.WriteLine(area);
        } // end of method

        /// <summary>
        /// This method demoinstrates the Custom Attribute capabilities
        /// by determining and printing the custom attribute - an identifer
        /// for each class. 
        /// </summary>
        /// <param name="asm">a custom attribute object of type CustomAttribute that inherits from Attribute</param>
        public static void CustomAttributeTest(Assembly asm)
        {
            // Print Header
            Console.WriteLine();
            Console.WriteLine("{0} Custom Attribute Test {0}", new string('=', 20));
            Console.WriteLine();

            // Get the Class types at run-time
            Type mathType = asm.GetType("SharedLib.MathStuff");
            Type personType = asm.GetType("SharedLib.Person");
            Type extensionType = asm.GetType("SharedLib.ExtensionMethods");
            Type angleFormatterType = asm.GetType("SharedLib.AngleFormatter");
            Type angleType = asm.GetType("SharedLib.Angle");

            // This is the CustomAttribute class type we are using to filter
            // our tests on. 
            Type specialType = asm.GetType("SharedLib.SpecialClassAttribute");

            // Get and Print the Custom Attribute Information for the MathStuff Class
            var attrs = mathType.GetCustomAttributes(specialType);

            foreach (dynamic attr in attrs)
            {
                Console.WriteLine($"{mathType.Name} has the special class ID of {attr.ID}");
            }

            // Get and Print the Custom Attribute Information for the Person class
            attrs = personType.GetCustomAttributes(specialType);

            foreach (dynamic attr in attrs)
            {
                Console.WriteLine($"{personType.Name} has the special class ID of {attr.ID}");
            }

            // Get and Print the Custom Attribute Information for the Extension class
            attrs = extensionType.GetCustomAttributes(specialType);

            foreach (dynamic attr in attrs)
            {
                Console.WriteLine($"{extensionType.Name} has the special class ID of {attr.ID}");
            }

            // Get and Print the Custom Attribute Information for the AngleFormatter class
            attrs = angleFormatterType.GetCustomAttributes(specialType);

            foreach (dynamic attr in attrs)
            {
                Console.WriteLine($"{angleFormatterType.Name} has the special class ID of {attr.ID}");
            }

            // Get and Print the Custom Attribute Information for the Angle class
            attrs = angleType.GetCustomAttributes(specialType);

            foreach (dynamic attr in attrs)
            {
                Console.WriteLine($"{angleType.Name} has the special class ID of {attr.ID}");
            }

        } // end of method

    } // end of class
} // end of namespace
