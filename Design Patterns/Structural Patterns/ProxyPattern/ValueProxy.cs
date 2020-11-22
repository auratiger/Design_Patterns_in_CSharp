using System;
using System.Diagnostics;

namespace Design_Patterns.Structural_Patterns.ProxyPattern
{
    
    /*
     * The value proxy is a proxy that is typically constructed on primitive types.
     * This approach is very rare but is useful for creating stronger typing.
     * This value proxy is essentially a wrapper around a primitive type which
     * then provides a bunch of conversion operators to, and from that value.
     */
    public class ValueProxy
    {
        public static void Test()
        {
            Console.WriteLine(
                10f * 5.Percent() // 10 * 5%
                );
            Console.WriteLine(
                2.Percent() + 3.Percent() // 2% + 3%
                );
        }
    }

    public static class PercentageExtensions
    {
        [DebuggerDisplay("{Value * 100.0f}%")]
        public readonly struct Percentage
        {
            private readonly float Value;

            internal Percentage(float value)
            {
                Value = value;
            }

            public static float operator *(float f, Percentage p)
            {
                return f * p.Value;
            }

            public static float operator /(float f, Percentage p)
            {
                return f / p.Value;
            }

            public static Percentage operator +(Percentage a, Percentage b)
            {
                return new Percentage(a.Value + b.Value);
            }

            public static Percentage operator -(Percentage a, Percentage b)
            {
                return new Percentage(a.Value - b.Value);
            }

            public override string ToString()
            {
                return $"{Value * 100.0f}%"; 
            }

            public override bool Equals(object? obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                return obj is Percentage other && Equals(other);
            }

            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
        }

        public static Percentage Percent(this float value)
        {
            return new Percentage(value / 100.0f);
        }

        public static Percentage Percent(this int value)
        {
            return new Percentage(value / 100.0f);
        }
        
        // Overloads for all primitive value types....
    }
}