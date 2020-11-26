using System;
using System.Text;

namespace Design_Patterns.Behavioral_Patterns.StatePattern
{
    public class SwitchStateMachine
    {
        public static void Test()
        {
            string code = "1234";
            var state = State.Locked;
            var entry = new StringBuilder();

            while (true)
            {
                switch (state)
                {
                    case State.Locked:
                        entry.Append(Console.ReadKey().KeyChar);

                        if (entry.ToString() == code)
                        {
                            state = State.Unlocked;
                            break;
                        }

                        if (!code.StartsWith(entry.ToString())) state = State.Failed;
                        
                        break;
                    case State.Failed:
                        Console.CursorLeft = 0;
                        Console.WriteLine("FAILED");
                        entry.Clear();
                        state = State.Locked;
                        break;
                    case State.Unlocked:
                        Console.CursorLeft = 0;
                        Console.WriteLine("UNLOCKED");
                        break;
                }
            }
        }
    }

    public enum State
    {
        Locked,
        Failed,
        Unlocked
    }
    
    
}