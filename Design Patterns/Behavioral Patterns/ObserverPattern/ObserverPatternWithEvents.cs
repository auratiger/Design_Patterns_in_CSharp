using System;

namespace Design_Patterns.Behavioral_Patterns.ObserverPattern
{
    public class ObserverPatternWithEvents
    {
        public static void Test()
        {
            
            var person = new Person();
            person.FallsIll += CallDoctor;
            
            person.CatchACold();

            person.FallsIll -= CallDoctor;
        }

        public static void CallDoctor(object? sender, FallsIllEventArgs eventArgs)
        {
            Console.WriteLine($"A doctor has been called to {eventArgs.Address}");
        }
        
    }

    public class FallsIllEventArgs : EventArgs
    {
        public string Address;
    }
    
    public class Person
    {
        public event EventHandler<FallsIllEventArgs> FallsIll;

        public void CatchACold()
        {
            FallsIll?.Invoke(this, new FallsIllEventArgs {Address = "123 London Road"});
        }
        
    }
}