using System;

namespace Design_Patterns.Structural_Patterns
{
    /*
     * The Bridge Pattern can be defined as
     * separating the abstraction from the implementation.
     *
     * To paraphrase, this could be represented as dividing
     * some complex functionality into two parts:
     * 
     *  - The abstraction part: Acts as a higher level
     *     control layer which delegates work for some
     *     entity. The abstraction uses Composition and
     *     contains a reference to the implementor.
     *
     *     The abstraction can be easily extended and
     *     add more functionality (refined abstraction).
     *     As well we can change the reference to the
     *     implementor of the abstraction at run-time.
     * 
     *  - The Implementation: contains the lower level
     *     primitive functionality.
     *
     * When talking about real applications, the abstraction can be
     * represented by a graphical user interface (GUI),
     * and the implementation could be the underlying
     * operating system code (API) which the GUI layer
     * calls in response to user interactions.
     */
    public class BridgePattern
    {

        public static void Test()
        {
            testDevice(new Tv());
            testDevice(new Radio());
        }
        
        private static void testDevice(IDevice device) {
            Console.WriteLine("Tests with basic remote.");
            BasicRemote basicRemote = new BasicRemote(device);
            basicRemote.Power();
            device.PrintStatus();

            Console.WriteLine("Tests with advanced remote.");
            AdvancedRemote advancedRemote = new AdvancedRemote(device);
            advancedRemote.Power();
            advancedRemote.Mute();
            device.PrintStatus();
        }
    }

    /*
     * The "implementation" interface declares methods common
     * to all concrete implementations classes. It doesn't
     * have to match the abstraction's interface. In fact, the two
     * interfaces can be entirely different. Typically the
     * implementation interface provides only primitive operations,
     * while the abstraction defines higher-level operations
     * based on those primitives.
     *
     * In this shown case scenario below the primitive operations
     * are represented by setting the volume or channel.. etc.
     * of different devices {Tv, Radio, ...}.
     */
    public interface IDevice
    {
        bool IsEnabled();

        void Enable();

        void Disable();

        int GetVolume();

        void SetVolume(int percent);

        int GetChannel();

        void SetChannel(int channel);
void PrintStatus(); } 
    
    // All devices follow the same interface
    public class Radio : IDevice
    {
        private bool m_On = false;
        private int m_Volume = 30;
        private int m_Channel = 1;

        public bool IsEnabled()
        {
            return m_On;
        }

        public void Enable()
        {
            m_On = true;
        }

        public void Disable()
        {
            m_On = false;
        }

        public int GetVolume()
        {
            return m_Volume;
        }

        public void SetVolume(int percent)
        {
            if (m_Volume > 100)
            {
                this.m_Volume = 100;
            }
            else if (m_Volume < 0)
            {
                this.m_Volume = 0;
            }
            else
            {
                this.m_Volume = m_Volume;
            }
        }

        public int GetChannel()
        {
            return m_Channel;
        }

        public void SetChannel(int channel)
        {
            m_Channel = channel;
        }

        public void PrintStatus()
        {
            Console.WriteLine("------------------------------------");
            Console.WriteLine("| I'm radio.");
            Console.WriteLine("| I'm " + (m_On ? "enabled" : "disabled"));
            Console.WriteLine("| Current volume is " + m_Volume + "%");
            Console.WriteLine("| Current channel is " + m_Channel);
            Console.WriteLine("------------------------------------\n");
        }
    }

    public class Tv : IDevice
    {
        private bool m_On = false;
        private int m_Volume = 30;
        private int m_Channel = 1;

        public bool IsEnabled()
        {
            return m_On;
        }

        public void Enable()
        {
            m_On = true;
        }

        public void Disable()
        {
            m_On = false;
        }

        public int GetVolume()
        {
            return m_Volume;
        }

        public void SetVolume(int percent)
        {
             if (m_Volume > 100)
             {
                 this.m_Volume = 100;
             }
             else if (m_Volume < 0)
             {
                 this.m_Volume = 0;
             }
             else
             {
                 this.m_Volume = m_Volume;
             }
        }

        public int GetChannel()
        {
            return m_Channel;
        }

        public void SetChannel(int channel)
        {
            m_Channel = channel;
        }

        public void PrintStatus()
        {
             Console.WriteLine("------------------------------------");
             Console.WriteLine("| I'm radio.");
             Console.WriteLine("| I'm " + (m_On ? "enabled" : "disabled"));
             Console.WriteLine("| Current volume is " + m_Volume + "%");
             Console.WriteLine("| Current channel is " + m_Channel);
             Console.WriteLine("------------------------------------\n");
        }
    }
    // == //

    /*
     * The IRemote interface declares common functionality
     * common between all remotes
     */
    public interface IRemote
    {
        void Power();

        void VolumeDown();

        void VolumeUp();

        void ChanelDown();

        void ChannelUp();
    }

    /*
     * The "abstraction" defines the interface for the "control"
     * part of the two class hierarchies. It maintains a reference
     * to an object of the "implementation" hierarchy {IDevice} and delegates
     * all of the real work to this object.
     */
    public class BasicRemote : IRemote
    {
        protected IDevice Device;
        
        public BasicRemote(){}

        public BasicRemote(IDevice device)
        {
            Device = device;
        }

        public void Power()
        {
           Console.WriteLine("Remote: power toggle");
           if (Device.IsEnabled())
           {
               Device.Disable();
           }
           else
           {
               Device.Enable();
           }
        }

        public void VolumeDown()
        {
            Console.WriteLine("Remote: volume down");
            Device.SetVolume(Device.GetVolume() - 10);
        }

        public void VolumeUp()
        {
             Console.WriteLine("Remote: volume down");
             Device.SetVolume(Device.GetVolume() + 10);
        }

        public void ChanelDown()
        {
            Console.WriteLine("Remote: channel down");
            Device.SetChannel(Device.GetChannel() - 1);
        }

        public void ChannelUp()
        {
            Console.WriteLine("Remote: channel up");
            Device.SetChannel(Device.GetChannel() + 1);
        }
    }

    /*
     * You can easily extend classes from the abstraction
     * hierarchy independently from device classes.
     */
    public class AdvancedRemote : BasicRemote
    {
        public AdvancedRemote(IDevice device) : base(device){}

        public void Mute()
        {
            Console.WriteLine("Remote: mute");
            Device.SetVolume(0);
        }
    }
}