using System;

namespace Design_Patterns.Behavioral_Patterns.ChainOfResponsibilityPattern
{
    /*
     * Let's imagine that we are creating a game where we have different
     * kinds of creatures, every one of them with their own base stats.
     * And let's suppose that the creature goes around finding magic
     * items which improve it's attack and/or defense values, or maybe it
     * encounters a which which casts some sort of spell that either
     * increases, decreases or negates those values.
     *
     * The way we would handle this is by having a chain of all the
     * Modifiers for every creature, and based on them modify
     * the creatures stats during the game.
     */
    public class MethodChains
    {
        public static void Test()
        {
            var goblin = new Creature("Goblin", 2, 2);
            Console.WriteLine(goblin);
            
            var root = new CreatureModifier(goblin);
            
            // This modifier stops all other modifiers from executing, 
            // that's why if activated, it should be one of the first 
            // modifiers to execute.
            
            // root.Add(new NoBonusesModifier(goblin));
            
            Console.WriteLine("Let's double the goblin's attack");
            root.Add(new DoubleAttackModifier(goblin));
            
            Console.WriteLine("Let's increase the goblin's defense");
            root.Add(new IncreaseDefenseModifier(goblin));
            
            root.Handle();
            
            Console.WriteLine(goblin);
        }
    }

    public class Creature
    {
        public string Name;
        public int Attack, Defence;

        public Creature(string name, int attack, int defense)
        {
            Name = name ?? throw new ArgumentNullException(paramName: nameof(name));
            Attack = attack;
            Defence = defense;
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defence)}: {Defence}";
        }
    }

    public class CreatureModifier
    {
        // store reference to the we are modifying
        protected Creature Creature;
        // keep in mind that creature modifiers can be chained together,
        // so we can gave several ones being applied to the same creature.
        protected CreatureModifier next;

        public CreatureModifier(Creature creature)
        {
            Creature = creature ?? throw new ArgumentNullException(nameof(creature));
        }

        public void Add(CreatureModifier cm)
        {
            // recursive call throughout the cain.
            if (next != null) next.Add(cm);
            else next = cm;
        }

        public virtual void Handle() => next?.Handle();

    }

    public class DoubleAttackModifier : CreatureModifier
    {
        public DoubleAttackModifier(Creature creature) : base(creature)
        {
            
        }

        public override void Handle()
        {
            Console.WriteLine($"Doubling {Creature.Name}'s attack");
            Creature.Attack *= 2;
            base.Handle();
        }
    }

    public class IncreaseDefenseModifier : CreatureModifier
    {
        public IncreaseDefenseModifier(Creature creature) : base(creature)
        {
            
        }

        public override void Handle()
        {
            Console.WriteLine($"Increasing {Creature.Name}'s Defense");
            Creature.Defence += 3;
            base.Handle();
        }
    }

    public class NoBonusesModifier : CreatureModifier
    {
        public NoBonusesModifier(Creature creature) : base(creature)
        {
            
        }

        public override void Handle()
        {
            Console.WriteLine($"No modifiers can be applied. Creature {Creature.Name} got cursed!");
            return;
        }
    }
}