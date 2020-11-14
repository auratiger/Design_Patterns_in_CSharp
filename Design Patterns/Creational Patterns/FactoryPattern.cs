using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Design_Patterns.Creational_Patterns
{
    public class FactoryPattern
    {

    }

#region Factory Method Pattern 

    // When creating a Point object, one 
    // might what to instantiate it using either the Cartesian or Polar
    // coordinate system. In this scenario using a constructor won't suffice
    // because we can't have multiple constructors with the same signature.
    // To Avoid adding needless optional parameters and checks in the constructor
    // the best approach is to use Factory methods for creating both instances!
    public class Point
    {
        private double x, y;

        private Point()
        {
            
        }
        
        private Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        
        // Method 1 //
        // Factory Methods //
        public static Point NewCartesianPoint(double x, double y)
        {
            return new Point(x, y);
        }

        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
        // == //

        // Async Factory method
        public static Point CreateAsync()
        {
            Point point = new Point();
            point.InitAsync();
            return point;
        }

        private async void InitAsync()
        {
            await Task.Delay(100);
        }

        public override string ToString()
        {
            return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
        }
        
        // Method 2 //
        // Inner Point Factory
        public static class PointFactory
        {
            public static Point NewCartesianPoint(double x, double y)
            {
                return new Point(x, y);
            }

            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }
        // == //
        
    }


#endregion

#region Abstract Factory Pattern



#endregion





}