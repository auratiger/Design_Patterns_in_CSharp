using System;
using System.Collections.Generic;

namespace Design_Patterns.Behavioral_Patterns.MementoPattern
{
    /*
     * The Memento Design Pattern offers a solution to implement the capability of
     * undoing executed actions. We can do this by saving the state of an object at a given
     * instant and restoring it if the actions performed since need to be undone.
     * Practically, a objects whose state needs to be saved is called a Memento.
     * Often, special objects named CareTakers are used to store a List of Mementos,
     * acting like a history object.
     * The Memento object should expose as little information as possible to the CareTaker.
     * This is to ensure that we don't expose the internal state of the Originator
     * to the outside world, as it would break encapsulation principles.
     * However, the Originator object should access enough information in order to restore
     * it's original state.
     *
     * Intent:
     *
     * • Without violation encapsulation, capture and externalize an object's internal state
     *     so that the object can be returned to this state later.
     * • A magic cookie that encapsulates a "check point" capability.
     * • Promote undo or rollback to full object status.
     */
    public class MementoPattern
    {
        public static void Test()
        {
            // Without the use of a caretaker
            var ba = new BankAccount("Steave", 100);
            Console.WriteLine($"Account {ba.Name}'s balance before deposit: {ba}");
            ba.Deposit(50);
            ba.Deposit(25);
            Console.WriteLine(ba);

            ba.Undo();
            Console.WriteLine($"Undo 1: {ba}");

            ba.Undo();
            Console.WriteLine($"Undo 2: {ba}");

            ba.Redo();
            Console.WriteLine($"Redo 1: {ba}");
            
            
            // With the use of a caretaker
            var ba2 = new BankAccount("Dave", 500);
            var ct = new CareTaker();
            ct.AddState(ba2.Snapshot());
            Console.WriteLine($"Account {ba2.Name}'s balance before deposit: {ba2}");
            
            ct.AddState(ba2.Deposit(50));
            Console.WriteLine($"Deposit 50 {ba2}");
            
            ct.AddState(ba2.Deposit(70));
            Console.WriteLine($"Deposit 70 {ba2}");
            
            ba2.Restore(ct.GetStateFromMemento(3));
            Console.WriteLine($"Account after restoring {ba2}");
            
        }
    }

    // The Originator
    // We can store the states in separate Memento objects or we can use a
    // caretaker class to add each state to our list.
    public class BankAccount
    {
        private int Balance;
        public string Name;
        
        private List<Memento> changes = new List<Memento>();
        private int current;

        public class Memento
        {
            public int Balance { get; private set; }

            public Memento(int balance)
            {
                Balance = balance;
            }
        }

        public BankAccount(string name, int balance)
        {
            Balance = balance;
            Name = name;
            changes.Add(new Memento(balance));
        }

        public Memento Deposit(int amount)
        {
            Balance += amount;
            var m = new Memento(Balance);
            changes.Add(m);
            current++;
            return m;
        }
        
        public Memento Snapshot()
        {
            return new Memento(Balance);
        }

        public void Restore(Memento m)
        {
            if (m != null)
            {
                Balance = m.Balance;
                changes.Add(m);
                current = changes.Count - 1;
            }
        }

        public Memento Undo()
        {
            if (current > 0)
            {
                var m = changes[--current];
                Balance = m.Balance;
                return m;
            }

            return null;
        }

        public Memento Redo()
        {
            if (current + 1 < changes.Count)
            {
                var m = changes[++current];
                Balance = m.Balance;
                return m;
            }

            return null;
        }

        public override string ToString()
        {
            return $"{nameof(Balance)}: {Balance}";
        }
    }

    public class CareTaker
    {
        private List<BankAccount.Memento> states = new List<BankAccount.Memento>();

        public void AddState(BankAccount.Memento state)
        {
            states.Add(state);
        }

        public BankAccount.Memento GetStateFromMemento(int i)
        {
            if (i > states.Count - 1)
            {
                Console.WriteLine($"There's no Memento at index {i}. Rolling back to last known Memento");
                return states[states.Count - 2];
            }
            else
            {
                return states[i];
            }
        }
    }
}