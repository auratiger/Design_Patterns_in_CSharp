using System;

namespace Design_Patterns.Behavioral_Patterns.StatePattern
{
    /*
     * State is a behavioral design pattern that lets an object alter its behavior
     * when it's internal state changes. It appears as if the object changes it's class.
     *
     * The State pattern is closely related to the concept of a Finite-State Machine - https://en.wikipedia.org/wiki/Finite-state_machine
     *
     * The main idea is that, at any given moment, there's a finite number of states which
     * a program can be in. Within any unique state, the program may or may not switch to certain other states.
     * These switching rules, called transitions, are also finite and predetermined,
     * meaning you need to think of all of the possible scenarios between different states
     * and their reactions to one another.
     *
     * You can also apply this approach to objects. Imagine that we have a Turntile class.
     * (The gates at metro stations which open once you pay)
     * A Turnstile can be in one of three states: Open, Closed and Processing(payments, etc.).
     * The enter method of the Turnstile works a little bit differently in each state:
     *     - In OpenState, it allows a person to pass
     *     - In ClosedState, it doesn't allow a person to pass until they pay
     *     - In ProcessingState, the gate remains in it's current state until the processing of the payment is complete
     *
     * Solution:
     * - The state pattern suggests that you create new classes for all possible states
     * of an object and extract all state-specific behaviours into these classes.
     * Instead of implementing all behaviours on its own, the original object, called context,
     * stores a reference to one of the state objects that represents it's current state,
     * and delegates all the state-related work to that object.
     */
    public class StatePattern
    {
        public static void Test()
        {
            var turnstile = new Turnstile();
            turnstile.Enter();
            turnstile.Pay();
            
        }
    }

    public class Turnstile
    {
        private ITurnstileState State;
        
        // always instance the class with an initial default state - closed
        public Turnstile()
        {
            State = new ClosedTurnstileState(this);
        }

        public void Enter()
        {
            State.Enter();
        }

        public void Pay()
        {
            State.Pay();
        }

        public void PayOk()
        {
            State.PayOK();
        }

        public void PayFailed()
        {
            State.PayFailed();
        }

        public void ChangeState(ITurnstileState state)
        {
            State = state;
        }
    }
    
    // called State
    public interface ITurnstileState
    {
        void Enter();
        void Pay();
        void PayOK();
        void PayFailed();
    }
    
    // concrete State
    public class OpenTurnstileState : ITurnstileState
    {
        private Turnstile Turnstile;

        public OpenTurnstileState(Turnstile turnstile)
        {
            Turnstile = turnstile;
        }


        public void Enter()
        {
            Console.WriteLine($"You went through the turnstile.");
            Turnstile.ChangeState(new ClosedTurnstileState(Turnstile));
        }

        public void Pay()
        {
            Console.WriteLine($"Gate is already open. Please proceed.");
        }

        public void PayOK()
        {
            Pay();
        }

        public void PayFailed()
        {
            Pay();
        }
    }

    public class ClosedTurnstileState : ITurnstileState
    {
        private Turnstile Turnstile;

        public ClosedTurnstileState(Turnstile turnstile)
        {
            Turnstile = turnstile;
        }

        public void Enter()
        {
            Console.WriteLine("Turnstile is closed. Pleases purchase a ticket");
        }

        public void Pay()
        {
            Console.WriteLine($"Processing payment");
            Turnstile.ChangeState(new ProcessingTurnstileState(Turnstile));
            Turnstile.Pay();
        }

        public void PayOK()
        {
            Pay();
        }

        public void PayFailed()
        {
            Pay();
        }
    }

    public class ProcessingTurnstileState : ITurnstileState
    {
        private Turnstile Turnstile;

        public ProcessingTurnstileState(Turnstile turnstile)
        {
            Turnstile = turnstile;
        }

        public void Enter()
        {
            Console.WriteLine($"Please wait, payment is still being processed.");
        }

        public void Pay()
        {
            // simulate payment functionality; no need to implement as this is just an overview of the pattern
            if (true)
            {
                PayOK();
            }
            else
            {
                PayFailed();
            }
        }

        public void PayOK()
        {
            Console.WriteLine($"Payment successful. You can now proceed.");
            Turnstile.ChangeState(new OpenTurnstileState(Turnstile));
        }

        public void PayFailed()
        {
            Console.WriteLine($"Payment unsuccessful. Please, try again.");
            Turnstile.ChangeState(new ClosedTurnstileState(Turnstile));
        }
    }
}