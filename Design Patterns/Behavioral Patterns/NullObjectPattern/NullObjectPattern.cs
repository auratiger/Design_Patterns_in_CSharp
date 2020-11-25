using System;
using System.Collections.Generic;

namespace Design_Patterns.Behavioral_Patterns.NullObjectPattern
{
    /*
     * In most object-oriented languages, such as Java or C#, references may be null.
     * These references need to be checked to ensure they are not null before invoking
     * any methods, because methods typically cannot be invoked on null references.
     *
     * Instead of using a null reference to convey absence of an object
     * (for instance, a non-existent customer), one uses an object which implements
     * the expected interface, but whose method body is empty. The advantage of this approach
     * over a working default implementation is that a null object is very predictable and had
     * no side effects: it does nothing.
     *
     * For example, a function may retrieve a list of files in a folder and perform some action on each.
     * In the case of an empty folder, one response may be to throw an exception or return a null reference
     * rather than a list.
     * Thus, the code which expects a list must verify that if in fact has one before continuing,
     * which can complicate the design.
     * By returning a null object (i.e. an empty List) instead, there is no need to verify that
     * the return value is in fact a list. The calling function may simply iterate the list as normal,
     * effectively doing nothing. It is, however, still possible to check whether the return value
     * is a null object (an empty list), by adding a IsNull function and react differently if desired.
     * The null object pattern can also be used to act as a stub for testing, if a certain feature
     * such as a database is not available for testing.
     *
     * 
     */
    public class NullObjectPattern
    {
        public static void Test()
        {
            // fetching customers which exist in database
            ICustomer john = CustomerFactory.FindCustomer("John");
            Console.WriteLine(john.GetName());
            
            ICustomer tealc = CustomerFactory.FindCustomer("Tealc");
            Console.WriteLine(tealc.GetName());
            
            // fetch customer who doesn't exist;
            // Without NOP would result in null pointer Exception, or we'd have to do checks
            ICustomer daniel = CustomerFactory.FindCustomer("Daniel Jackson");
            Console.WriteLine(daniel.GetName());
            
        }
    }

    // Imagine we have a database with customers and someone queries it, 
    // searching for customers by name. Some customers will not exist,
    // so I'll treat them as a NullCustomer, instead of performing null checks.
    public interface ICustomer
    {
        bool IsNull();
        string GetName();
    }

    public class Customer : ICustomer
    {
        private string Name;

        public Customer(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public bool IsNull()
        {
            return false;
        }

        public string GetName()
        {
            return Name;
        }
    }

    public class NullCustomer : ICustomer
    {
        public bool IsNull()
        {
            return true;
        }

        public string GetName()
        {
            return "Customer is not available";
        }
    }

    public class CustomerFactory
    {
        private static List<string> realCustomer = new List<string>(){"John", "Samantha", "Tealc"};

        public static ICustomer FindCustomer(string name)
        {
            foreach(var customer in realCustomer)
            {
                if(customer.Equals(name)) return new Customer(name);
            }
            return new NullCustomer();
        }
    }
}