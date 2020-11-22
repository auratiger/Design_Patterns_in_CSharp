using System;

namespace Design_Patterns.Structural_Patterns.ProxyPattern
{
    /*
     * Protection Proxies are used for checking certain conditions.
     * Some objects or resources might need appropriate authorization for accessing them,
     * so using a proxy is one of the ways so using a proxy is one of the ways 
     * in which such conditions can be checked.
     * With protection proxies, we also get the flexibility of having many variations of access control.
     * 
     * For example, we have a car, and a car should only be driven by people who are at least 16 years old or older.
     * Our Car class will have a method drive(), which should only be used by drivers of the appropriate age.
     * In order to accomplish this, so using a proxy is one of the ways 
     * we'll create a CarProxy class which will make a call to the super.() if the conditions are met
     */
    
    public class ProtectionProxy
    {
        public static void Test()
        {
            ICar car = new CarProxy(new Driver(12)); // to young to drive
            car.Drive();
            
            ICar car2 = new CarProxy(new Driver(18));  
            car2.Drive();
        }
    }

    public interface ICar
    {
        void Drive();
    }

    public class Car : ICar
    {
        public void Drive()
        {
            Console.WriteLine("Car is being driven");
        }
    }

    public class Driver
    {
        public int Age { get; set; }

        public Driver(int age)
        {
            Age = age;
        }
    }

    // Protected proxy which provides authorization for access
    public class CarProxy : ICar
    {
        private Driver driver;
        private Car car = new Car();

        public CarProxy(Driver driver)
        {
            this.driver = driver;
        }

        public void Drive()
        {
            if (driver.Age >= 16)
            {
                car.Drive();
            }
            else
            {
                Console.WriteLine("too young");
            }
        }
    }
}