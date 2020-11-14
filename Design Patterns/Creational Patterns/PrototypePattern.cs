using System;
using static System.Console;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Design_Patterns.Creational_Patterns.PrototypePattern
{
    
    /*
     * The Prototype pattern is a creation pattern based on cloning a pre-configured object.
     * The idea is that you pick an object that is configured for either the default
     * or in the ballpark of some specific use case and then you clone this object
     * and configure to your exact needs.
     * The pattern is useful to remove a bunch of boilerplate code,
     * when the configuration required would be onerous.
     */
    public class PrototypePattern
    {
        public static void TestPrototype()
        {
           Person person = new Person("Henry", "Ruse", "Developer", new Address("221", "idk"));

           Person person2 = person.deepClone();
           person2.Name = "Cavil";

           WriteLine(person);
           WriteLine(person2);


           Point point = new Point(3, 3);
           Point point2 = point.DeepCopy();
           point2.x = 6;
           
           WriteLine(point);
           WriteLine(point2);


           Point point3= new Point();
           Point point4 = point.DeepCopyXml();
           point2.x = 10;
           
           WriteLine(point);
           WriteLine(point2);
        }
    }
    
    // 1.
    // The Prototype pattern in C# can be implemented in several ways.
    // One of them is by using the ICloneable interface, however the 
    // problems with that is that it does not enforce in creating a deep clone
    // method and second, that due to it being created before generics it's return type 
    // is object and will need to by type casted.
    // A better way of using this approach would be to create a custom interface using 
    // Generics and better naming conventions 

    interface IPrototype<T> 
    where T : new() // in java this would be a {Serializable} constraint
    {
        public T deepClone();

        // public T shallowCopy();
    }

    // 2.
    // Another implementation could be done by creating a copy constructor for all 
    // objects that will need to be copied. However we need to be careful not to create
    // shallow copies in the copy constructors!! That's why if using this method all 
    // copied objects need to implement a copy constructor
    // OPTIONAL: Using this approach one could the copy constructors private and
    // and implement a {clone()} witch will use the constructor to clone the object
    public class Person : IPrototype<Person>
    {
        public string Name { get; set; }
        
        public string City { get; set; }
        
        public string Position { get; set; }

        public Address Address { get; set; }

        public Person()
        {
        }
        
        public Person(string name, string city, string position, Address address)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            City = city ?? throw new ArgumentNullException(nameof(city));
            Position = position ?? throw new ArgumentNullException(nameof(position));
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }

        // (look at point 2)
        // Person copy constructor
        public Person(Person person)
        {
            Name = person.Name;
            City = person.City;
            Position = person.Position;
            Address = new Address(person.Address);
        }

        // look at point 1 and 2
        public Person deepClone()
        {
            return new Person(this);
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(City)}: {City}, {nameof(Position)}: {Position}";
        }
    }

    public class Address : IPrototype<Address>
    {
        public string StreetAddress { get; set; }
        public string Postcode { get; set; }

        public Address()
        {
            
        }
        
        public Address(string streetAddress, string postcode)
        {
            StreetAddress = streetAddress ?? throw new ArgumentNullException(nameof(streetAddress));
            Postcode = postcode ?? throw new ArgumentNullException(nameof(postcode));
        }

        // (look at point 2)
        // Address copy constructor
        public Address(Address address)
        {
            StreetAddress = address.StreetAddress;
            Postcode = address.Postcode;
        }

        // look at point 1 and 2
        public Address deepClone()
        {
            return new Address(this); 
        }

        public override string ToString()
        {
            return $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(Postcode)}: {Postcode}";
        }
    }
    
    
    // 3.
    // A third approach in implementing the prototype pattern is 
    // by creating a serializable extension method.
    // In this example DeepCopy can be called by any object
    // however for it to work the objects need to be {Serializable}!!
    // Or one could use DeepCopyXml and avoid implementing the Serializable annotation
    // for all objects, however for it to work all objects need to have a parameterless constructor
    public static class ExtensionMethods
    {
        public static T DeepCopy<T>(this T self)
        {
            var stream = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, self);
            stream.Seek(0, SeekOrigin.Begin);
            object copy = formatter.Deserialize(stream);
            stream.Close();
            return (T) copy;
        }

        public static T DeepCopyXml<T>(this T self)
        {
            using (var ms = new MemoryStream())
            {
                var s = new XmlSerializer(typeof(T));
                s.Serialize(ms, self);
                ms.Position = 0;
                return (T) s.Deserialize(ms); 
            }
        }
    }

    [Serializable]
    public class Point
    {
        public float x, y;

        public Point()
        {
        }

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"{nameof(x)}: {x}, {nameof(y)}: {y}";
        }
    }
    
    
    
    
}