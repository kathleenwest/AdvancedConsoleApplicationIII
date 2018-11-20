using System;
using System.Collections.Generic;
using System.Text;

namespace SharedLib
{
    /// <summary>
    /// MathStuff contains a utility class of non-instance methods
    /// that provide some basic math functions such as calculating 
    /// Area, etc.. It also has a SpecialAttribute identifer to
    /// work with dynamic runtime testing. 
    /// </summary>
    [SpecialClass(2)]
    public class MathStuff
    {
        /// <summary>
        /// Overridden method to calculate area of the rectangle
        /// standard rectangle area calculation utilizing Width and Height
        /// </summary>
        /// <returns>(double) value area of the rectangle</returns>
        public static double Area(double width, double height)
        {
            return width * height;
        }// end of method Area

        /// <summary>
        /// Calculates the area of a rectangle
        /// </summary>
        /// <param name="width">double width of rectangle</param>
        /// <param name="height">double height of rectangle</param>
        /// <returns>double area of rectangle</returns>
        public static double CalculateAreaRectangle(double width, double height)
        {
            return width * height;
        } // end of method

        /// <summary>
        /// Returns the area for a circle
        /// Formula for circle area is pi * radius2
        /// </summary>
        /// <param name="radius">double radius of the circle</param>
        /// <returns>double area of the circle</returns>
        public static double CalculateAreaCircle(double radius)
        {
            return Math.PI * radius * radius;
        } // end of method

        /// <summary>
        /// Calculates distance between two points (x1, y1) and (x2,y2)
        /// </summary>
        /// <param name="x1">double x coordinate of first point pair</param>
        /// <param name="y1">double y coordinate of first point pair</param>
        /// <param name="x2">double x coordinate of second point pair</param>
        /// <param name="y2">double y coordinate of second point pair</param>
        /// <returns>double distance between the two points</returns>
        public static double CalculatePointDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        } //end of method

    } // end of class
} // end of namespace
