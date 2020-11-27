using System;

namespace Design_Patterns.Behavioral_Patterns.VisitorPattern
{
    /*
     * The visitor pattern is a pattern where one interface (Visitor) defines a computation / operation
     * and another (Visitable) is responsible for providing data access.
     *
     * It helps your code adhere to the single responsibility principle as if a class is just responsible for holding data,
     * it does not clutter it up with adding computation or additional logic to it, and any additional operations
     * can be added externally in the Visitor class.
     *
     * The reason to use it is given in Head First Design Patterns as:
     * Use when you want to add capabilities to a composite of objects and encapsulation is not important.
     *
     * The advantage of using the visitor DP instead of simply extending a class is that it can handle
     * different data types through method overloading. Annoyingly though, every time a new data type has to be handled
     * it needs to be added to the visitable interface, which is an OCP violation.
     * But if you know there is a definite, relatively small number of data types, this might not be such a problem.
     *
     * If you are at a point where you have to break the open closed principle to add new methods to an already used,
     * existing class, the best way is to implement a visitor.
     * By doing so, you have to modify the existing class only once and then you can supply the new methods
     * by using different visitors, instead of adding more and more methods to your class.
     *
     * Let's say you have different credit cards which provide a cashback opportunity for different services.
     * Usually, you'd need to add new methods for each new service which you introduce as a cashback opportunity,
     * making your code very inconvenient, and in time impossible to change & scale. By introducing a visitor
     * which introduces the requested method each time it is needed we avoid this severe violation of the OCP.
     *
     * 
     */
    public class VisitorPattern
    {
        public static void Test()
        {
            /*
             * Scenario: we've had a concrete credit card class and we needed new methods
             * for air travel and food supplies cashback calculation. Instead of adding to the interface,
             * and then implementing them in each sub-class, we simply defined only 1 method
             * in the CreditCard interface - accept() which takes on a visitor who supplies
             * the functionality we need. Thus, we had to violate the OCP in the CreditCard interface
             * and implementations **only** once, and have opened our hierarchy to future behavioral extensions.
             */
            
            var bronze = new BronzeCreditCard();
            var silver = new SilverCreditCard();
            var gold = new GoldCreditCard();
            
            // we only need to create 1 visitor, which can interact with our 3 objects
            var airtraverlVisitor = new AirtravelOfferVisitor();
            
            bronze.Accept(airtraverlVisitor);
            silver.Accept(airtraverlVisitor);
            gold.Accept(airtraverlVisitor);
            
            var foodVisitor = new FoodOfferVisitor();
            
            gold.Accept(foodVisitor);
        }
    }

    // visitable
    public interface CreditCard
    {
        string GetName();
        void Accept(OfferVisitor visitor); // visitor method
    }

    // visitor
    public interface OfferVisitor
    {
        void VisitBronzeCreditCard(BronzeCreditCard card);

        void VisitSilverCreditCard(SilverCreditCard card);

        void VisitGoldCreditCard(GoldCreditCard card);
    }

    public class BronzeCreditCard : CreditCard
    {
        public string GetName()
        {
            return "Bronze credit card.";
        }

        public void Accept(OfferVisitor visitor)
        {
            visitor.VisitBronzeCreditCard(this);
        }
    }
    
    public class SilverCreditCard : CreditCard
    {
        public string GetName()
        {
            return "Silver credit card.";
        }

        public void Accept(OfferVisitor visitor)
        {
            visitor.VisitSilverCreditCard(this);
        }
    }

    public class GoldCreditCard : CreditCard
    {
        public string GetName()
        {
            return "Gold credit card.";
        }

        public void Accept(OfferVisitor visitor)
        {
            visitor.VisitGoldCreditCard(this);
        }
    }
    
    public class AirtravelOfferVisitor : OfferVisitor
    {
        public void VisitBronzeCreditCard(BronzeCreditCard card)
        {
            Console.WriteLine("The Bronze credit card has 1% cashback on air-travel purchases.");
        }

        public void VisitSilverCreditCard(SilverCreditCard card)
        {
            Console.WriteLine("The Silver credit card has 2.5% cashback on air-travel purchases.");
        }

        public void VisitGoldCreditCard(GoldCreditCard card)
        {
            Console.WriteLine("The Gold credit card has 5.2% cashback on air-travel purchases.");
        }
    }
    
    public class FoodOfferVisitor : OfferVisitor
    {
        public void VisitBronzeCreditCard(BronzeCreditCard card)
        {
            Console.WriteLine("The Bronze credit card has 0.1% cashback for food supplies.");
        }

        public void VisitSilverCreditCard(SilverCreditCard card)
        {
            Console.WriteLine("The Silver credit card has 0.3% cashback for food supplies.");
        }

        public void VisitGoldCreditCard(GoldCreditCard card)
        {
            Console.WriteLine("The Gold credit card has 0.6% cashback for food supplies.");
        }
    }
}