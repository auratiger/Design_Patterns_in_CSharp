using System;

namespace Design_Patterns.Behavioral_Patterns.StatePattern
{
    public class SwitchExpressionState
    {
        public static void Test()
        {
            var chest = Chest.Locked;
            Console.WriteLine($"Chest is {chest}");

            chest = Manipulate(chest, Action.Open, true);
            Console.WriteLine($"Chest is {chest}");
            
            chest = Manipulate(chest, Action.Close, false);
            Console.WriteLine($"Chest is {chest}");
            
            chest = Manipulate(chest, Action.Close, false);
            Console.WriteLine($"Chest is {chest}");
        }

        // The problem with this implementation using switch expressions is,
        // that you are not able to add additional functionality when changing the states
        // like logging, due to it being restricted to returning only expressions.
        // ** One solution to this could be using a decorator of the returned state.
        static Chest Manipulate
            (Chest chest, Action action, bool haveKey) =>
            (chest, action, haveKey) switch
            {
                (Chest.Locked, Action.Open, true) => Chest.Open,
                (Chest.Closed, Action.Open, _) => Chest.Open,
                (Chest.Open, Action.Close, true) => Chest.Locked,
                (Chest.Open, Action.Close, false) => Chest.Closed,
                _ => chest // default case
            };
    }

    public enum Chest
    {
        Open,
        Closed,
        Locked
    }

    public enum Action
    {
        Open,
        Close
    }
    
}