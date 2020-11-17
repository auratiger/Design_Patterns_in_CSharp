using System;
using System.Collections.Generic;
using System.Reflection;

namespace Design_Patterns.Structural_Patterns
{
    /*
     * The Adapter pattern is a structural pattern that allows
     * objects with incompatible interfaces to collaborate.
     * 
     * When you travel from the US to Europe for the first time,
     * you may get a surprise when trying to charge your laptop.
     * The power plug and sockets standards are different in different countries.
     * That’s why your US plug won’t fit a German socket.
     * The problem can be solved by using a power plug adapter that
     * has the American-style socket and the European-style plug.
     *
     * The Adapter pattern lets you create a middle-layer class that serves as a
     * translator between your code and a legacy class, a 3rd-party class
     * or any other class with a weird interface.
     */
    public class AdapterPattern
    {
        public static void TestAdapter()
        {
            // Round fits round, no surprise.
            RoundHole hole = new RoundHole(5);
            RoundPeg rpeg = new RoundPeg(5);
            if (hole.Fits(rpeg)) {
                Console.WriteLine("Round peg r5 fits round hole r5.");
            }

            SquarePeg smallSqPeg = new SquarePeg(2);
            SquarePeg largeSqPeg = new SquarePeg(20);
            // hole.fits(smallSqPeg); // Won't compile.

            // Adapter solves the problem.
            SquarePegAdapter smallSqPegAdapter = new SquarePegAdapter(smallSqPeg);
            SquarePegAdapter largeSqPegAdapter = new SquarePegAdapter(largeSqPeg);
            if (hole.Fits(smallSqPegAdapter)) {
                Console.WriteLine("Square peg w2 fits round hole r5.");
            }
            if (!hole.Fits(largeSqPegAdapter)) {
                Console.WriteLine("Square peg w20 does not fit into round hole r5.");
            }
        }
    }

    /**
     * RoundHoles are compatible with RoundPegs.
     */
    public class RoundHole
    {
        private double Radius;

        public RoundHole(double radius)
        {
            Radius = radius;
        }

        public double GetRadius()
        {
            return Radius;
        }

        public bool Fits(RoundPeg peg)
        {
            bool result;
            result = (GetRadius() >= peg.GetRadius());
            return result;
        }
    }

    /**
     * RoundPegs are compatible with RoundHoles.
     */
    public class RoundPeg
    {
        private double Radius;

        public RoundPeg()
        {
        }

        public RoundPeg(double radius)
        {
            Radius = radius;
        }

        public virtual double GetRadius()
        {
            return Radius;
        }
    }

    /**
     * SquarePegs are not compatible with RoundHoles (they were implemented by
     * previous development team). But we have to integrate them into our program.
     */
    public class SquarePeg
    {
        private double Width;

        public SquarePeg()
        {
        }

        public SquarePeg(double width)
        {
            Width = width;
        }

        public double GetWidth()
        {
            return Width;
        }

        public double GetSquare()
        {
            double result;
            result = Math.Pow(Width, 2);
            return result;
        }
    }

    /**
     * Adapter allows fitting square pegs into round holes.
     */
    public class SquarePegAdapter : RoundPeg
    {
        private SquarePeg Peg;

        public SquarePegAdapter(SquarePeg peg)
        {
            Peg = peg;
        }

        public override double GetRadius()
        {
            double result;
            // Calculate a minimum circle radius, which can fit this peg.
            result = (Math.Sqrt(Math.Pow((Peg.GetWidth() / 2), 2) * 2));
            return result;
        }
    }
}