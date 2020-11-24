using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Design_Patterns.Structural_Patterns;

namespace Design_Patterns.Behavioral_Patterns.InterpreterPattern
{
    /*
     * Interpreter pattern provides a way to evaluate language grammar or expressions.
     * This type of pattern comes under behavioral pattern.
     * This pattern involves implementing an expression interface which tells to interpret a particular context.
     * The pattern is used in SQL parsing, symbol processing engine etx.
     *
     * Now the thing about interpreter is that this design pattern is actually a reflection of
     * an entire field of computer science called compiler theory.
     * - E.g., turned into OOP structures (when we're talking about Java or C#)
     *
     * Some examples:
     * • Programming language compiles, interpreters and IDEs. For example, IntelliJ performs
     * static analysis, they have to interpret your code - has to interpret every single construct that
     * you write and give you hints and suggestions as to whether you are doing things right.
     * • HTML, XLM and similar (interpreter reads it, understands it and changes it to something object-oriented).
     * • Numeric expressions (3+4/5) - in order for your computer to process this properly, it needs to be
     * turned into some sort of OO structure before it can be traversed and evaluated.
     * • Regular expressions - domain specific languages embedded into programming languages,
     * which allows us to check certain strings and evaluate them.
     *
     * Let's recap, an interpreter is a component that processes structured text data.
     * It does so by turning it into separate lexical tokens (lexing) and them interpreting
     * sequences of said tokens (parsing).
     */
    public class InterpreterPattern
    {
        public static List<Token> Lex(string input)
        {
            var result = new List<Token>();

            for (int i = 0; i < input.Length; i++)
            {
                switch (input[i])
                {
                    case '+':
                        result.Add(new Token(Token.Type.Plus, "+"));
                        break;
                    case '-':
                        result.Add(new Token(Token.Type.Minus, "-"));
                        break;
                    case '(':
                        result.Add(new Token(Token.Type.Lparen, "("));
                        break;
                    case ')': 
                        result.Add(new Token(Token.Type.Rparen, ")"));
                        break;
                    default:
                        var sb = new StringBuilder(input[i].ToString());
                        for (int j = i + 1; j < input.Length; j++)
                        {
                            if (char.IsDigit(input[j]))
                            {
                                sb.Append(input[j]);
                                ++i;
                            }
                            else
                            {
                                result.Add(new Token(Token.Type.Integer, sb.ToString()));
                                break;
                            }
                        }

                        break;
                }
            }

            return result;
        }

        public static IElement Parse(IReadOnlyList<Token> tokens)
        {
            var result = new BinaryOperation();
            bool haveLHS = false;
            for (int i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                // look at the type of token
                switch (token.MyType)
                {
                    case Token.Type.Integer:
                        var integer = new IntegerToken(int.Parse(token.Text));
                        if (!haveLHS)
                        {
                            result.Left = integer;
                            haveLHS = true;
                        } else
                        {
                            result.Right = integer;
                        }
                        break;
                    case Token.Type.Plus:
                        result.MyType = BinaryOperation.Type.Addition;
                        break;
                    case Token.Type.Minus:
                        result.MyType = BinaryOperation.Type.Subtraction;
                        break;
                    case Token.Type.Lparen:
                        int j = i;
                        for (; j < tokens.Count; ++j)
                            if (tokens[j].MyType == Token.Type.Rparen)
                                break; // found it!
                        // process subexpression w/o opening (
                        var subexpression = tokens.Skip(i+1).Take(j - i - 1).ToList();
                        var element = Parse(subexpression);
                        if (!haveLHS)
                        {
                            result.Left = element;
                            haveLHS = true;
                        } else result.Right = element;
                        i = j; // advance
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return result;
        }

        public static void Test()
        {
            string input = "(13+4)-(12+1)";
            var tokens = Lex(input);
            Console.WriteLine(string.Join("\t", tokens));

            var parsed = Parse(tokens);
            Console.WriteLine($"{input} = {parsed.Value}");


        }
    }

    public interface IElement
    {
        int Value { get; }
    }

    public class IntegerToken : IElement
    {
        public IntegerToken(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class BinaryOperation : IElement
    {
        public enum Type
        {
            Addition, Subtraction
        }

        public Type MyType;
        public IElement Left, Right;

        public int Value
        {
            get
            {
                switch (MyType)
                {
                    case Type.Addition:
                        return Left.Value + Right.Value;
                    case Type.Subtraction:
                        return Left.Value - Right.Value;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public class Token
    {
        public enum Type
        {
            Integer, 
            Plus, 
            Minus,
            Lparen, // rights parenthesis - (
            Rparen, // left parenthesis - )
        }
        
        public Type MyType { get; private set; }
        public string Text { get; private set; }

        public Token(Type myType, string text)
        {
            MyType = myType;
            Text = text ?? throw new ArgumentNullException(paramName: nameof(text));
        }

        public override string ToString()
        {
            return $"`{Text}`";
        }
    }
    
    
}