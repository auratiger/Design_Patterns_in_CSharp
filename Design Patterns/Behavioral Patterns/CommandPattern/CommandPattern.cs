using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Dynamitey.DynamicObjects;

namespace Design_Patterns.Behavioral_Patterns.CommandPattern.CommandPattern
{
    /*
     * The Command is a behavioral design pattern that turns a request into
     * a stand-alone object that contains all information about the request.
     * This transformation lets you parameterize methods with different requests,
     * delay or queue a request's execution, and support undoable operations.
     * It also allows us to undo an execution of a command. Think of a software like
     * Photoshop - you have a list of commands performed on your image, you can undo
     * and re-do those commands.
     *
     * Suppose you are providing Bank Transferring services for depositing and
     * withdrawing money from your bank account. Using the Command pattern
     * it is easy to queue a chain of commands which should execute one after
     * another, and if at any point there is a problem, the whole transaction
     * chain could be reverted with the undo command.
     */
    public class CommandPattern
    {
        public static void Test()
        {
            var ba = new BankAccount();
            var commands = new List<BankAccountCommand>()
            {
                new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100),
                new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 50)
            };
            
            Console.WriteLine(ba);

            // goes through all the queued commands and executes them
            foreach (var c in commands)
            {
                c.Call();
            }
            
            Console.WriteLine(ba);

            // goes through all the queued commands and undoes all commands which
            // have successfully executed.
            foreach (var c in Enumerable.Reverse(commands))
            {
                c.Undo();
            }
            
            Console.WriteLine(ba);
            
        }
    }

    public class BankAccount
    {
        private int balance;
        private int overdraftLimit = -500;

        // methods are made internal so they may not be used outside the assembly
        internal void Deposit(int amount)
        {
            balance += amount;
            Console.WriteLine($"Deposited ${amount}, balance is now {balance}");
        }

        internal bool Withdraw(int amount)
        {
            if (balance - amount >= overdraftLimit)
            {
                balance -= amount;
                Console.WriteLine($"Withdrew ${amount}, balance is now {balance}");
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{nameof(balance)}: {balance}";
        }
    }

    public interface ICommand
    {
        void Call();
        void Undo();
    }

    public class BankAccountCommand : ICommand
    {
        private BankAccount Account;
        
        public enum Action
        {
            Deposit, Withdraw
        }

        private Action action;
        private int amount;
        private bool Succeded;

        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            Account = account ?? throw new ArgumentNullException(nameof(account));
            this.action = action;
            this.amount = amount;
        }

        public void Call()
        {
            switch (action)
            {
                case Action.Deposit:
                    Account.Deposit(amount);
                    Succeded = true;
                    break;
                case Action.Withdraw:
                    Succeded = Account.Withdraw(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Undo()
        {
            // Undoes only the successfully executed commands
            if (!Succeded) return;
            switch (action)
            {
                case Action.Deposit:
                    Account.Withdraw(amount);
                    break;
                case Action.Withdraw:
                    Account.Deposit(amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    
    
}