using System;
using System.Collections.Generic;
using System.Linq;

namespace Design_Patterns.Behavioral_Patterns.MediatorPattern
{
    /*
     * The Mediator design pattern facilitates the communication between components,
     * by letting the components be unaware of each other's presence or absence in the system.
     * A Mediator is used to reduce communication complexity between multiple objects or classes.
     *
     * Motivation for using a mediator:
     * • Components may go in and out of a system at any time:
     *     - Chat room participants
     *     - Players in a MMORPG
     * • It makes no sense for participants to have direct references to one another
     *     - Those references may go dead
     * • Solution: have them all refer to some central components that facilitates communication
     *
     * The classic illustration of a mediator is the idea of a char room - The reason for that is
     * that the chatroom is precisely what the mediator is.
     * It is a way of letting users interact with one another without necessarily having direct
     * references/pointers to one another. Instead, every message goes through the chatroom,
     * and the chatroom acts as a mediator.
     */
    public class MediatorPattern
    {
        public static void Test()
        {
            var room = new ChatRoom();
            
            var john = new Person("John");
            var jane = new Person("Jane");
            
            room.Join(john);
            room.Join(jane);
            
            john.Say("hi");
            jane.Say("on, hey johm");
            
            var simon = new Person("Simon");
            room.Join(simon);
            simon.Say("hi everyone!");
            
            jane.PrivateMessage("Simon", "glad you could join us!");
            
            
        }
    }

    public class Person
    {
        public string Name;
        public ChatRoom Room;
        private List<string> chatLog = new List<string>();

        public Person(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public void Say(string message)
        {
            Room.Broadcast(Name, message);
        }

        public void PrivateMessage(string who, string message)
        {
            Room.Message(Name, who, message);
        }

        public void Receive(string sender, string message)
        {
            string s = $"{sender}: '{message}'";
            chatLog.Add(s);
            Console.WriteLine($"[{Name}'s chat session] {s}");
        }
    }

    public class ChatRoom
    {
        private List<Person> people = new List<Person>();

        public void Join(Person p)
        {
            string joinMsg = $"{p.Name} joins the chat";
            Broadcast("room", joinMsg);

            p.Room = this;
            people.Add(p);
        }

        public void Broadcast(string source, string message)
        {
            // sends a broadcast of the message to all users except the sender
            foreach (var p in people)
            {
                if (p.Name != source)
                {
                    p.Receive(source, message);
                }
            }
        }

        public void Message(string source, string destination, string message)
        {
            people.FirstOrDefault(p => p.Name == destination)
                ?.Receive(source, message);
        }
    }
}