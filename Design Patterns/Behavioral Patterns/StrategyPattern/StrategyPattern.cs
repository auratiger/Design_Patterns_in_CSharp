using System;

namespace Design_Patterns.Behavioral_Patterns.StrategyPattern
{
    /*
     * The Strategy Design Pattern is a type of behavioral design pattern that encapsulates
     * a "family" of algorithms and selects one from the pool for use during runtime.
     * The algorithms are interchangeable, meaning that they are substitutable for each other.
     *
     * The key idea is to create objects which represent various strategies. These objects form
     * a pool of strategies from which the context object can choose from to vary it's behavior as per it's strategy.
     * These objects (strategies) perform the same operation, have the same (single) job and compose
     * the same interface strategy.
     *
     * Let's take the sorting algorithms we have for example. Sorting algorithms have a set of rules
     * specific to each other, which they follow to effectively sort an array of number.
     * Then, int our program, we need different sorting algorithms at a time during execution.
     * Using SP allows us to group these algorithms and select from the pool when needed.
     * It is more like a plugin, like the plug & and play in Windows or in the Device Drivers.
     * All the plugins must follow a signature or rule.
     *
     * Imagine we've created our own data structure, an array list, we've compiled our code
     * and everything is running smoothly. However, at a certain time we run into a problem.
     * Our sorting algorithm is not optimal - it is slow, and has the wrong strategy for our data.
     * Because we've tightly coupled our code, we'd need to stop running our program, change the algorithm,
     * compile again and hope for the best. This is a very bad approach - we might need the old algorithm
     * at some point, or to introduce a new one.
     *
     * So we have to design our code in a way we can provide our data structures with different
     * sorting **strategies**. The SP heavily implies to use composition over inheritance,
     * which will allow us to accomplish the functionality which we need.
     *
     * Solution:
     *     - Implement a "strategy" interface, which will outline the functionality needed by each concrete strategy
     *     - Delegate classes (strategies) while implementing the strategy interface -- All strategies must have the same signature
     *     - Model our context class (the one using the strategy) to require a dependency injection
     *       for each one of it's algorithms (inject concrete strategies)
     *
     * 
     */
    public class StrategyPattern
    {
        public static void Test()
        {
            // create behaviors 
            var flyBehavior = new SimpleFlying();
            var quackBehavior = new SimpleQuick();
            var walkingBehavior = new SimpleWalking();
            
            var duck = new Duck("Bob", flyBehavior, quackBehavior, walkingBehavior);
            
            duck.Fly();
            duck.Quack();
            duck.Walk();
            
            // change behavior with a strategy at runtime
            Console.WriteLine("\n Changing behavior.. \n");
            duck.SetFlyBehavior(new AdvancedFlying());
            duck.SetQuackBehavior(new WeirdQuack());
            duck.SetWalkBehavior(new FastWalking());
            
            duck.Fly();
            duck.Quack();
            duck.Walk();
        }
    }

    public interface IFlyBehavior
    {
        void Fly();
    }

    public interface IQuackBehavior
    {
        void Quack();
    }

    public interface IWalkBehavior
    {
        void Walk();
    }

    // Context class which takes strategies for different algorithms and can swap at runtime
    public class Duck
    {
        public string Name { get; private set; }
        private IFlyBehavior m_FlyBehavior;
        private IQuackBehavior m_QuackBehavior;
        private IWalkBehavior m_WalkBehavior;

        public Duck(string name, IFlyBehavior flyBehavior, IQuackBehavior quackBehavior, IWalkBehavior walkBehavior)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            m_FlyBehavior = flyBehavior ?? throw new ArgumentNullException(nameof(flyBehavior));
            m_QuackBehavior = quackBehavior ?? throw new ArgumentNullException(nameof(quackBehavior));
            m_WalkBehavior = walkBehavior ?? throw new ArgumentNullException(nameof(walkBehavior));
        }

        public void Fly()
        {
            m_FlyBehavior.Fly();
        }

        public void Quack()
        {
            m_QuackBehavior.Quack();
        }

        public void Walk()
        {
            m_WalkBehavior.Walk();
        }

        public void SetFlyBehavior(IFlyBehavior flyBehavior)
        {
            m_FlyBehavior = flyBehavior;
        }

        public void SetQuackBehavior(IQuackBehavior quackBehavior)
        {
            m_QuackBehavior = quackBehavior;
        }

        public void SetWalkBehavior(IWalkBehavior walkBehavior)
        {
            m_WalkBehavior = walkBehavior;
        }

    }

    // concrete implementations of behaviors

    // flying
    class SimpleFlying : IFlyBehavior
    {
        public void Fly()
        {
            Console.WriteLine("Simple flying!");
        }
    }

    public class AdvancedFlying : IFlyBehavior
    {
        public void Fly()
        {
            Console.WriteLine("FLying in a very advanced way!");
        }
    }

    // quacking
    public class SimpleQuick : IQuackBehavior
    {
        public void Quack()
        {
            Console.WriteLine("Quacking in the most normal way. Quack quack.");
        }
    }

    public class WeirdQuack : IQuackBehavior
    {
        public void Quack()
        {
            Console.WriteLine("Quacking in a very strange way. Quirk quirk");
        }
    }
    
    // walking
    public class SimpleWalking : IWalkBehavior
    {
        public void Walk()
        {
            Console.WriteLine("Walking like a normal duck.");
        }
    }

    public class FastWalking : IWalkBehavior
    {
        public void Walk()
        {
            Console.WriteLine("Walking very fast. You'd better run!");
        }
    }
}