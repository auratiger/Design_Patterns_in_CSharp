using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace Design_Patterns.Creational_Patterns
{
    
    /*
     * Builder design pattern-а ти позволява да създаваш обекти стъпка по стъпка,
     * исползвайки само определени стъпки, които са нужни. Затова той има смисъл
     * да се ползва за създаването на комплексни обекти, които в противен случай
     * би се налагало да се създават чрез огромни конструктори в които не винаги
     * ше са нужни всики параметри.
     */
    
     // The Builder pattern is a creational design pattern that lets you construct 
     // complex objects step by step. Builders are supposed to be able to chain method
     // calls and allow the user to produce different types and representations of an object
     // using the save construction code
     public class BuilderPattern
     {
         public static void HtmlBuilderTest()
         {
             // Without using the builder pattern!!
             
             // var hello = "hello";
             // var sb = new StringBuilder();
             // sb.Append("<p>");
             // sb.Append(hello);
             // sb.Append("</p>");
             // WriteLine(sb);
             //
             // var words = new[] {"Hello", "world"};
             // sb.Clear();
             // sb.Append("<ul>");
             // foreach (var word in words)
             // {
             //     sb.AppendFormat("<li>{0}</li>", word);
             // }
             //
             // sb.Append("</ul>");
             // WriteLine(sb);
             
             // 
             var builder = new HtmlBuilder("ul");
             builder.AddChild("li", "hello").AddChild("li", "word");
             WriteLine(builder);
 
         } 
         
         public static void FunctionalBuilderTest()
         {
             var person = new PersonBuilder()
                 .Called("Sarah")
                 .WorksAs("Developer")
                 .Build();
         }

         public static void FacetBuilderTest()
         {
             var pb = new PersonBuilderFacade();
             Person person = pb
                 .works.At("Fabrikam")
                     .AsA("Engineer")
                     .Earning(1200)
                 .lives.At("Ruse")
                     .In("London")
                     .WithPostCode("221bbs")
                 .Build();
         }
     }
     

#region Html Element Builder
     public class HtmlElement
     {
         public string Name { get; set; }
         public string Text { get; set; }
         
         private List<HtmlElement> Elements = new List<HtmlElement>();
         private const int indentSize = 2;
 
         public HtmlElement()
         {
             
         }
 
         public HtmlElement(string name, string text)
         {
             Name = name ?? throw new ArgumentNullException(nameof(name));
             Text = text ?? throw new ArgumentNullException(nameof(text));
         }

         public void addElement(HtmlElement element)
         {
             Elements.Add(element);
         }
         
         public string ToStringImpl(int indent)
         {
             var sb = new StringBuilder();
             var i = new string(' ', indentSize * indent);
             
             //print the opening brace of the html element 
             sb.AppendLine($"{i} <{Name}>");
 
             if (!string.IsNullOrWhiteSpace(Text))
             {
                 sb.Append(new string(' ', indentSize * (indent + 1)));
                 sb.AppendLine(Text);
             }
 
             foreach (var e in Elements)
             {
                 sb.Append(e.ToStringImpl(indent + 1));
             }
            
             //print the closing brace of the html element 
             sb.AppendLine($"{i} </{Name}>");

             return sb.ToString();
         }
 
         public override string ToString()
         {
             return ToStringImpl(0);
         }
     }
 
     public class HtmlBuilder
     {
         private readonly string rootName;
         HtmlElement root = new HtmlElement();
 
         public HtmlBuilder(string rootName)
         {
             this.rootName = rootName;
             root.Name = rootName;
         }
 
         public HtmlBuilder AddChild(string childName, string childText)
         {
             var e = new HtmlElement(childName, childText);
             root.addElement(e);
             return this;
         }
 
         public override string ToString()
         {
             return root.ToString();
         }
 
         public void Clear()
         {
             root = new HtmlElement{Name = rootName};
         }
     }
     

#endregion

public class Person
{
    public string Name { get; set; }
    
    public string City { get; set; }
    
    public string StreetAddress { get; set; }
    
    public string Position { get; set; }
    
    public string Postcode { get; set; }
    
    public int AnnualIncome { get; set; }
    
    public string CompanyName { get; set; }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(City)}: {City}, {nameof(StreetAddress)}: {StreetAddress}, {nameof(Position)}: {Position}, {nameof(Postcode)}: {Postcode}, {nameof(AnnualIncome)}: {AnnualIncome}, {nameof(CompanyName)}: {CompanyName}";
    }
}


#region Functional Builder
    // The Functional Builder works using lambda expressions to pass parameters.
    // It easily conserves the open and closed principal by adding additional 
    // functionality by using static classes {see @PersonBuilderExtensions}.


    // The abstract class implements the core functionality 
    // for the functional builder pattern, and due to it being generic 
    // it serves as a reusable component which can be used for implementing
    // all kinds of builders by inheriting it
    public abstract class FunctionalBuilder<TSubject, TSelf>
        where TSelf : FunctionalBuilder<TSubject, TSelf>
        where TSubject : new()
    {
       private readonly List<Func<TSubject, TSubject>> actions = new List<Func<TSubject, TSubject>>();
        
       public TSelf Do(Action<TSubject> action) => AddAction(action);

       public TSubject Build() 
           => actions.Aggregate(new TSubject(), (p, f) => f(p));
       
       private TSelf AddAction(Action<TSubject> action)
       {
           actions.Add(p =>
           {
               action(p);
               return p;
           });

           return (TSelf) this;
       }
    }

    public sealed class PersonBuilder : FunctionalBuilder<Person, PersonBuilder>
    {

       public PersonBuilder Called(string name) => Do(p => p.Name = name);

       public PersonBuilder Postcode(string postcode) => Do(p => p.Postcode = postcode);


    }
    
    // for keeping the open and closed principal, when using functional builders
    // one can create a static function with the builder as an extension parameter
    public static class PersonBuilderExtensions
    {
        public static PersonBuilder WorksAs
            (this PersonBuilder builder, string position)
            => builder.Do(p => p.Position = position);

        public static PersonBuilder WorksAt
            (this PersonBuilder builder, string companyName)
            => builder.Do(p => p.CompanyName = companyName);
        
    }
    
    
#endregion

#region Faceted Builder

    public class PersonBuilderFacade // facade
    {
        // reference
        protected Person person = new Person();
        
        public PersonJobBuilder works => new PersonJobBuilder(person);
        public PersonAddressBuilder lives => new PersonAddressBuilder(person);

        public Person Build()
        {
            return person;
        }
    }

    public class PersonAddressBuilder : PersonBuilderFacade
    {
        public PersonAddressBuilder(Person person)
        {
            this.person = person;
        }

        public PersonAddressBuilder At(string streetAddress)
        {
            person.StreetAddress = streetAddress;
            return this;
        }

        public PersonAddressBuilder WithPostCode(string postcode)
        {
            person.Postcode = postcode;
            return this;
        }

        public PersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }
        
    }
    
    public class PersonJobBuilder : PersonBuilderFacade
    {
        public PersonJobBuilder(Person person)
        {
            this.person = person;
        }

        public PersonJobBuilder At(string companyName)
        {
            person.CompanyName = companyName;
            return this;
        }

        public PersonJobBuilder AsA(string position)
        {
            person.Position = position;
            return this;
        }

        public PersonJobBuilder Earning(int amount)
        {
            person.AnnualIncome = amount;
            return this;
        }
    }

#endregion


}




















