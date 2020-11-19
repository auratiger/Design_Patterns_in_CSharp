using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Design_Patterns.Structural_Patterns
{
    
    /*
     * The Decorator is a structural design pattern that
     * lets you attach new behaviors to objects by placing
     * these objects inside special wrapper objects that
     * contain the behaviors.
     *
     * The Decorator is mostly used when you need to assign extra
     * behaviors to objects at runtime without breaking the code that uses
     * these objects conforming with the Open and Closed principal!
     * Or when it is awkward or not possible to extend an object's behavior
     * using inheritance, in many cases due to the object being sealed {or final}.
     * 
     */
    public class DecoratorPattern
    {
        public static void Test()
        {
            var h = new HashTableDecorator(new Hashtable());
            h.Add("one", "hello");
            Console.WriteLine($"Value of key one is {h["one"]}");
        }
    }

    /*
     * In this example the Decorator class extends all the behavior
     * of the Hashtable class and adds custom behavior by overriding
     * and modifying the methods to encode and decode the values
     * of the hashtable to base64 strings.
     */
    public class HashTableDecorator : Hashtable
    {
        private Hashtable m_Hashtable;

        public HashTableDecorator(Hashtable hashtable)
        {
            m_Hashtable = hashtable;
        }

        public override void Add(object key, object? value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, value);
                string newValue = Convert.ToBase64String(ms.ToArray());
                Console.WriteLine($"Added {value} as ");
                Console.WriteLine(newValue);
                base.Add(key, newValue);
            }
        }

        public override bool ContainsValue(object? value)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, value);
                return base.ContainsValue(Convert.ToBase64String(ms.ToArray()));
            }
        }

        public override object? this[object key]
        {
            get
            {
                byte[] bytes = Convert.FromBase64String((string) base[key]);
                using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
                {
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Position = 0;
                    return new BinaryFormatter().Deserialize(ms);
                }
            }
            
            set
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(ms, value);
                    base[key] = Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }
    
    
}