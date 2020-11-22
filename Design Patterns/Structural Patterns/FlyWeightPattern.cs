using System;
using System.Collections.Generic;
using System.Text;

namespace Design_Patterns.Structural_Patterns
{
    
    /*
     * The Flyweight pattern describes how to share objects to allow their use
     * at fine granularity without prohibitive cost. Each "flyweight" object is
     * divided into two pieces: the state-dependent (extrinsic) part, and the
     * state-independent (intrinsic) part. Intrinsic state is stored (shared)
     * in the Flyweight object. Extrinsic state is stored or computed by client objects,
     * and passed to the Flyweight when its operations are invoked.
     * 
     * Since the same flyweight object can be used in different contexts,
     * you have to make sure that its state can’t be modified.
     * A flyweight should initialize its state just once, via constructor parameters.
     * It shouldn’t expose any setters or public fields to other objects.
     *
     * The benefit of applying the pattern depends heavily on how and where it’s used. It’s most useful when:
     *
     *  - an application needs to spawn a huge number of similar objects
     *  - this drains all available RAM on a target device
     *  - the objects contain duplicate states which can be extracted and shared between multiple objects
     */
    public class FlyWeightPattern
    {
        public static void Test()
        {
            // Normal and inefficient Text Formatter for Capitalizing, bolding and italic
            var ft = new FormattedText("This is a brave new world");
            ft.Capitalize(10, 15);
            Console.WriteLine(ft);
            
            // A better approach to creating a text formatter complying with the 
            // FlyWeight design pattern
            var bft = new BetterFormatterText("This is a brave new world");
            bft.GetRange(10, 15).Capitalize = true;
            Console.WriteLine(bft);

        }
    }

    /*
     * This is an inefficient way of creating a text formatter
     * mostly in terms of memory, due to it creating a new array of
     * bool values with the length of the text for each text format,
     * which will scale immensely with the size of the text and with
     * every additional format support.
     */
    public class FormattedText
    {
        private readonly string plainText;
        private bool[] capitalize;
        private bool[] bold;
        private bool[] italic;

        public FormattedText(string plainText)
        {
            this.plainText = plainText;
            capitalize = new bool[plainText.Length];
            bold = new bool[plainText.Length];
            italic = new bool[plainText.Length];
        }
        
        public void Capitalize(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                capitalize[i] = true;
            }
        }

        public void Bold(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                bold[i] = true;
            }
        }

        public void Italic(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                italic[i] = true;
            }
        }

        // Iterates the text and using the index of every
        // character checks if it should be formatted
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];
                sb.Append(
                    capitalize[i] ? char.ToUpper(c) : 
                        bold[i] ? MakeBold(c) :
                            italic[i] ? MakeItalic(c) : c);
            }

            return sb.ToString();
        }

        private char MakeBold(char c)
        {
            // make the character bold
            return c;
        }

        private char MakeItalic(char c)
        {
            // make the character italic
            return c;
        }
    }


    /*
     * This is a better approach to the text formatter complying with
     * the FlyWeight pattern. This time instead of having separate
     * arrays for every format, we have a single List witch holds
     * range objects in contain the range and type for the format.
     */
    public class BetterFormatterText
    {
        private string plainText;
        private List<TextRange> formatting = new List<TextRange>();
        
        public BetterFormatterText(string plainText)
        {
            this.plainText = plainText;
        }

        public TextRange GetRange(int start, int end)
        {
            var range = new TextRange {Start = start, End = end};
            formatting.Add(range);
            return range;
        }

        /*
         * Now instead of storing the same data in multiple objects,
         * it’s kept in just a one flyweight object and linked to appropriate TextFormatter
         * object which act as context. 
         */
        public class TextRange
        {
            public int Start, End;
            public bool Capitalize, Bold, Italic;

            public bool Covers(int position)
            {
                return position >= Start && position <= End;
            }
        }

        private char MakeBold(char c)
        {
            // make character bold
            return c;
        }

        private char MakeItalic(char c)
        {
            // make character italic
            return c;
        }

        // Iterates the text and using the index of every
        // character checks if it should be formatted
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var i = 0; i < plainText.Length; i++)
            {
                var c = plainText[i];
                foreach(var range in formatting)
                    if (range.Covers(i))
                        if(range.Capitalize)
                            c = char.ToUpper(c);
                        else if (range.Bold)
                            c = MakeBold(c);
                        else if (range.Italic)
                            c = MakeItalic(c);
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
    
}