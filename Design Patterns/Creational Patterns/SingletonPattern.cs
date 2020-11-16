using System;
using System.Threading;

namespace Design_Patterns.Creational_Patterns
{
    public class SingletonPattern
    {

        public static void TestSingleton()
        {
            // The client code.
            NormalSingleton s1 = NormalSingleton.GetInstance();
            NormalSingleton s2 = NormalSingleton.GetInstance();

            if (s1 == s2)
            {
                Console.WriteLine("Singleton works, both variables contain the same instance.");
            }
            else
            {
                Console.WriteLine("Singleton failed, variables contain different instances.");
            }
        }
        
        public static void TestThreadSingleton()
        {
            // The client code.
            
            Console.WriteLine(
                "{0}\n{1}\n\n{2}\n",
                "If you see the same value, then singleton was reused (yay!)",
                "If you see different values, then 2 singletons were created (booo!!)",
                "RESULT:"
            );
            
            Thread process1 = new Thread(() =>
            {
                TestSingleton("FOO");
            });
            Thread process2 = new Thread(() =>
            {
                TestSingleton("BAR");
            });
            
            process1.Start();
            process2.Start();
            
            process1.Join();
            process2.Join();
        }
        
        private static void TestSingleton(string value)
        {
            ThreadSafeSingleton singleton = ThreadSafeSingleton.GetInstance(value);
            Console.WriteLine(singleton.Value);
        } 
    }

    public class NormalSingleton
    {
        /*
        The Singleton's instance is stored in a static field. There a multiple
        ways to initialize this field, all of them with various pros and cons.
        
        One approach is by initializing the instance immediately on runtime by 
        setting the instance variable to equal {new NormalSingleton()}.
        */ 
        
        // private static NormalSingleton _instance = new NormalSingleton();

        /*
         * The second approach is called lazy initialization which means that
         * we create the instance when calling GetInstance the moment it is needed.
         * This method is handy when the Singleton isn't needed at the start of the program,
         * and saves memory by not keeping unnecessary objects in the stack.
         */ 
        private static NormalSingleton _instance;

        // The Singleton's constructor should always be private to prevent
        // direct construction calls with the 'new' operator
        private NormalSingleton()
        {
            
        }

        /*
         * This is the static method that controls the access to the singleton instance.
         * On the first run, it creates a singleton object and places it into the static field.
         * On subsequent runs, it returns the client existing object stored in the static field. 
         */
        public static NormalSingleton GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NormalSingleton();
            }

            return _instance;
        }
    }

    public class ThreadSafeSingleton
    {
        // We now have a lock object that will be used to synchronize threads 
        // during first access to the Singleton
        private static readonly object _lock = new object();

        private static ThreadSafeSingleton _instance;
        
        // We'll use this property to prove that our Singleton really works.
        public string Value { get; set; }

        private ThreadSafeSingleton(string value)
        {
            Value = value;
        }

        public static ThreadSafeSingleton GetInstance(string value)
        {
            // This conditional is needed to prevent threads stumbling over the
            // lock once the instance is ready.
            if (_instance == null)
            {
                // Now, imagine that the program has just been launched.
                // Since there's no Singleton instance yet, multiple threads
                // can simultaneously pass the previous conditional and reach this
                // point almost at the same time. The first of them will acquire
                // lock and will proceed further, while the rest will wait here.
                lock (_lock)
                {
                    // The first thread to acquire the lock, reached this conditional, 
                    // goad inside and creates the Singleton instance.
                    // Once it leaves the lock block, a thread that might have been waiting
                    // for the lock release may then enter this section. But since the 
                    // Singleton field is already initialized, the thread won't create a
                    // new object.
                    if (_instance == null)
                    {
                        _instance = new ThreadSafeSingleton(value);
                    }
                }
            }

            return _instance;
        }
    }
}