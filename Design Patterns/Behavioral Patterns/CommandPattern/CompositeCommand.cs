using System;
using System.Collections.Generic;
using System.Linq;

namespace Design_Patterns.Behavioral_Patterns.CommandPattern.CompositeCommand
{
    
    public class CompositeCommand
    {
        public static void Test()
        {
            // var ba = new BankAccount();           
            // var deposit = new BankAccountCommand(ba, BankAccountCommand.Action.Deposit, 100);
            // var withdraw = new BankAccountCommand(ba, BankAccountCommand.Action.Withdraw, 50);
            // var composite = new CompositeBankAccountCommand(
            //     new []{deposit, withdraw});
            //
            // composite.Call();
            // Console.WriteLine(ba);
            //
            // composite.Undo();
            // Console.WriteLine(ba);
            
            var from = new BankAccount();
            from.Deposit(100); // we have deposited 100 dollars to this account
            var to = new BankAccount();
            
            Console.WriteLine($"making transaction of 100 dollars");
            var mtc = new MoneyTransferCommand(from, to, 100); // making a transaction of 100 dollars to another account
            mtc.Call();
            
            Console.WriteLine(from); // the from account is left with 0 dollars
            Console.WriteLine(to); // the to account has received the 100 dollars
            
            // undoes all successful transactions
            Console.WriteLine("Undoing previous transaction");
            mtc.Undo();
            Console.WriteLine(from);
            Console.WriteLine(to);
            
            // Suppose we make a transaction of 1000 dollars while the user from only has 100 dollars in his account
            // In this case, the Call() of the transaction will catch the error and will fail, undoing the whole
            // transaction in the process
            Console.WriteLine($"making transaction of 1000 dollars, which will fail, due to insufficient resources");
            var mtc2 = new MoneyTransferCommand(from, to, 1000);
            mtc2.Call();
            
            Console.WriteLine(from);
            Console.WriteLine(to);
        }
    }

    public class BankAccount
    {
        private int Balance;
        private int overdraftLimit = -500;

        public BankAccount(int balance = 0)
        {
            Balance = balance;
        }

        public void Deposit(int amount)
        {
            Balance += amount;
            Console.WriteLine($"Deposit ${amount}, balance is now {Balance}");
        }

        public bool Withdraw(int amount)
        {
            if (Balance - amount >= overdraftLimit)
            {
                Balance -= amount;
                Console.WriteLine($"Withdrew ${amount}, balance is now {Balance}");
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{nameof(Balance)}: {Balance}";
        }
    }

    public interface ICommand
    {
        public void Call();
        public void Undo();
        public bool Success { get; set; }
    }

    public class BankAccountCommand : ICommand
    {
        private BankAccount Account;

        public enum Action
        {
            Deposit, Withdraw
        }

        private Action action;
        private int Amount;
        public bool Success { get; set; }

        public BankAccountCommand(BankAccount account, Action action, int amount)
        {
            Account = account;
            this.action = action;
            Amount = amount;
        }

        public void Call()
        {
            switch (action)
            {
                case Action.Deposit:
                    Account.Deposit(Amount);
                    Success = true;
                    break;
                case Action.Withdraw:
                    Success = Account.Withdraw(Amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Undo()
        {
            if(!Success) return;
            switch (action)
            {
                case Action.Deposit:
                    Account.Withdraw(Amount);
                    break;
                case Action.Withdraw:
                    Account.Deposit(Amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }

    public class CompositeBankAccountCommand : List<BankAccountCommand>, ICommand
    {
        public CompositeBankAccountCommand()
        {
        }

        public CompositeBankAccountCommand(IEnumerable<BankAccountCommand> collection) : base(collection)
        {
        }

        public virtual void Call()
        {
            ForEach(cmd => cmd.Call());
        }

        public virtual void Undo()
        {
            foreach (var cmd in ((IEnumerable<BankAccountCommand>) this).Reverse())
            {
                if(cmd.Success) cmd.Undo();
            }
        }

        public bool Success
        {
            get
            {
                return this.All(cmd => cmd.Success);
            }
            set
            {
                foreach (var cmd in this)
                    cmd.Success = value;
            }
        }
    }

    public class MoneyTransferCommand : CompositeBankAccountCommand
    {
        public MoneyTransferCommand(BankAccount from, BankAccount to, int amount)
        {
            AddRange(new[]
            {
                new BankAccountCommand(from,
                    BankAccountCommand.Action.Withdraw, amount), 
                new BankAccountCommand(to,
                    BankAccountCommand.Action.Deposit, amount), 
            });
        }

        public override void Call()
        {
            BankAccountCommand last = null;
            foreach (var cmd in this)
            {
                if (last == null || last.Success)
                {
                    cmd.Call();
                    last = cmd;
                }
                else
                {
                    cmd.Undo();
                    break;
                }
            }
        }
    }
}