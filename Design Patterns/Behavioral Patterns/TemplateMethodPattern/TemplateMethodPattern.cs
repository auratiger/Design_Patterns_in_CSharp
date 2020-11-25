using System;
using System.Threading;

namespace Design_Patterns.Behavioral_Patterns.TemplateMethodPattern
{
    /*
     * The template method design pattern is to define an algorithm as a skeleton
     * of operations and leve the details to be implemented by the child classes.
     * The overall structure and sequence of the algorithm is preserved by the  parent class.
     * Template means a Preset format like HTML templates which have a fixed preset format.
     * Similarly in the template method pattern, we have a preset structure method called
     * template method which consists of steps. These steps can be an abstract method
     * which will be implemented by its subclasses.
     *
     * Let's imagine you want to create a login template for your website where you can log on
     * using different services like Google, Facebook, etc.
     * You'd define the general steps of the login which are all shared between different implementations.
     * Then declare specific abstract steps which have to be implemented/overridden by subclasses
     * to complete the algorithm. The login process in the abstract class is not complete, you need to
     * define concrete classes which supply the code for the missing pieces.
     *
     * Pseudo code:
     * abstract class AuthenticateUser{
     * ....
     *     public void authenticate(){
     * operation1(username, password);
     * do more logic..
     * operation2();
     * more logic..
     * }
     * 
     * abstract void operation1(String username, String password);
     * abstract void operation2();
     * // end of class
     * }
     *
     * Then you'd have concrete class implementations which extend the abstract class,
     * provide logic for operation1 & operation2, then you'd call the authenticate method
     * on the instantiated object of the concrete class -> facebookAuthentication.authenticate()
     *
     * ========
     *
     * *** The template method pattern is build around inheritance, so in my opinion it is
     * inherently dangerous to use. Unless, you have a scenario which is super appropriate
     * for the template method pattern. If you know that the structure of your algorithm will always
     * be invariant/fixed, the steps of the algorithm will never change, then use it by all means.
     * If you have concerns about whether that is true or not, maybe template pattern is not the answer.
     */
    public class TemplateMethodPattern
    {
        public static void Test()
        {
            var chess = new Chess();
            chess.Run();
        }
    }

    public abstract class Game
    {
        protected int currentPlayer;
        protected readonly int numberOfPlayers;

        public void Run()
        {
            Start();
            while(!HaveWinner)
                TakeTurn();
            Console.WriteLine($"Player {WinningPlayer} wins.");
        }

        protected Game(int numberOfPlayers)
        {
            this.numberOfPlayers = numberOfPlayers;
        }

        protected abstract void Start();
        protected abstract void TakeTurn();
        protected abstract bool HaveWinner { get; }
        protected abstract int WinningPlayer { get; }
    }
    
    public class Chess : Game
    {
        private int turn = 1;
        private int maxTurns = 10;
        
        public Chess() : base(2)
        {
        }

        public Chess(int numberOfPlayers) : base(numberOfPlayers)
        {
        }

        protected override void Start()
        {
            Console.WriteLine($"Starting a game of chess with {numberOfPlayers} players.");
            
        }

        protected override void TakeTurn()
        {
            Console.WriteLine($"Turn {turn++} taken by player {currentPlayer}");
            currentPlayer = (currentPlayer + 1) % numberOfPlayers;
        }

        protected override bool HaveWinner => turn == maxTurns;
        protected override int WinningPlayer => currentPlayer;
    }
}