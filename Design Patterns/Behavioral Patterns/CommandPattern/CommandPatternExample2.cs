using System;

namespace Design_Patterns.Behavioral_Patterns.CommandPattern.CommandPatternExample2
{
    /*
     * Suppose you are building a home automation system.
     * There is a programmable remote which can be used to on and off
     * various items in your home like lights, stereo, AC atc.
     * A good example are the Philips HUE smart lights. You can program each
     * button on the remote to send a specific command by using Philips' app.
     * In sense of code, this means that we can swap in/out different commands at runtime.
     *
     * Let's get back to our supposed scenario. As smart homes become more popular,
     * a lot more devices with smart capabilities are being introduces each day.
     * So, you've bought this smart system, got the remote and you only have 2 smart
     * devices which you can interface with the smart programmable remote.
     * Below we will simulate this entire process - of you using the smart app and
     * adding/swapping additional functionality.
     */
    public class CommandPatternExample2
    {
        public static void Test()
        {
                
            // create our smart devices
            SimpleRemote Remote = new SimpleRemote();
            Light Light = new Light("Living room");
            Stereo Stereo = new Stereo();
            
            // let's first program our button to use thestereo
            Remote.SetCommand(new StereoOnCommand(Stereo));
            Remote.ShortButtonPress(); // turn it on
            Remote.LongButtonPress(); // revert the command
            
            // Now we use the app on our phone to set the light as our receiver
            // see how we can swap receivers at run time?
            Remote.SetCommand(new LightOffCommand(Light));
            Remote.ShortButtonPress();
            
        }
    }

    public interface ICommand
    {
        public void Execute();
        public void Revert();
    }
    
    // create the smart lights in our house
    public class Light
    {
        private string LightLocation;

        public Light(string lightLocation)
        {
            LightLocation = lightLocation;
        }

        public void On()
        {
            Console.WriteLine($"Light in {LightLocation} is switched on.");
        }

        public void Off()
        {
            Console.WriteLine($"Light in {LightLocation} is switched off.");
        }
    }
    
    // commands for the light
    public class LightOnCommand : ICommand
    {
        //aggregate
        private Light Light;

        public LightOnCommand(Light light)
        {
            Light = light;
        }

        public void Execute()
        {
            Light.On();
        }

        public void Revert()
        {
            Light.Off(); 
        }
    }

    public class LightOffCommand : ICommand
    {
        // aggregate
        private Light Light;

        public LightOffCommand(Light light)
        {
            Light = light;
        }

        public void Execute()
        {
           Light.Off(); 
        }

        public void Revert()
        {
            Light.On();
        }
    }

    // now we create a stereo;
    public class Stereo
    {
        // when you turn on a stereo system, a few things happen
        // - it goes on, the CD gets loaded and the volume is established
        public void On()
        {
            Console.WriteLine("Stereo is on");
        }

        public void Off()
        {
            Console.WriteLine("Stereo is off");
        }

        public void SetCD()
        {
            Console.WriteLine($"Stereo is set for CD Input");
        }

        public void SetRadio()
        {
            Console.WriteLine($"Stereo is set for Radio");
        }

        public void SetVolume(int volume)
        {
            Console.WriteLine($"Stereo volume set to {volume}");
        }
    }

    public class StereoOnCommand : ICommand
    {
        // aggregate
        private Stereo Stereo;

        public StereoOnCommand(Stereo stereo)
        {
            Stereo = stereo;
        }
        
        // when a stereo is turned out, several things happen at once
        public void Execute()
        {
            Stereo.On();
            Stereo.SetCD();
            Stereo.SetVolume(420);
        }

        public void Revert()
        {
            Stereo.Off();
        }
    }

    public class StereoOffCommand : ICommand
    {
        // aggregate
        private Stereo Stereo;

        public StereoOffCommand(Stereo stereo)
        {
            Stereo = stereo;
        }
        
        // when a stereo is turned out, several things happen at once
        public void Execute()
        {
            Stereo.Off();
        }

        public void Revert()
        {
            StereoOnCommand stereoOnCommand = new StereoOnCommand(Stereo);
            stereoOnCommand.Execute();
        }
    }
    
    // Invoker
    // remote control with only 1 button

    public class SimpleRemote
    {
        private ICommand Button;

        public SimpleRemote()
        {
            
        }
        
        // inject command to button
        public void SetCommand(ICommand command)
        {
            Button = command;
        }
        
        // when you click the button, execute said command
        public void ShortButtonPress()
        {
            Button.Execute();
        }
        
        // hold the button to undo command
        public void LongButtonPress()
        {
            Button.Revert();
        }
    }
        
    

}