using System;
using System.Collections.Generic;
using System.Text;

namespace Design_Patterns.Structural_Patterns
{
    /*
     * The Composite is a structural design pattern that lets you compose
     * objects into tree structures and they work with these structures
     * as if they were individual objects
     *
     * Using the Composite pattern makes sense only when the core model
     * of your app can be represented as a tree.
     *
     * For example, imagine that you have two types of objects: Products and Boxes
     * A Box can contain several Products as well as a number of even smaller Boxes,
     * and so on.
     *
     * Say you decide to create an ordering system that uses these classes.
     * Orders could contain simple products without any wrapping,
     * as well as boxes stuffed with products... and other boxes.
     * How would you determine the total price of such an order?
     *
     * You could try the direct approach: unwrap all the boxes,
     * go over all the products and then calculate the total.
     * That would be doable in the real world; but in a program,
     * it's not as simple as running a loop. You have to know the classes
     * of Products and Boxes you are going through, the nesting level of
     * the nesting level of the boxes and other nasty details beforehand.
     * All of this makes the direct approach either too awkward or ever impossible.
     *
     * The Composite pattern suggest that you work with Products And Boxes through
     * a common interface which declares a method for calculating the total price.
     * The greates benefit of this approach is that you don't need to care about
     * the concrete classes of objects that compose the tree.
     * You can treat them all the same via the common interface.
     * When you call a method, the objects themselves pass the request down the tree.
     */
    public class CompositePattern
    {
        public static void Test()
        {
            var drawing = new GraphicObject() {Name = "My Drawing"};
            drawing.AddChildren(new Square {Color = "Red"});            
            drawing.AddChildren(new Circle {Color = "Yellow"});

            var group = new GraphicObject();
            group.AddChildren(new Circle {Color="Blue"});
            group.AddChildren(new Square {Color="Blue"});
            drawing.AddChildren(group);
            
            Console.WriteLine(drawing);
        }
    }

    public class GraphicObject
    {
        public virtual string Name { get; set; } = "Group";
        public string Color;
        
        private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();
        public List<GraphicObject> Children => children.Value;

        private void Print(StringBuilder sb, int depth)
        {
            sb.Append(new string('*', depth))
                .Append(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"{Color}")
                .AppendLine(Name);
            foreach (var child in Children)
            {
                child.Print(sb, depth+1);
            }
        }

        public void AddChildren(GraphicObject obj)
        {
            Children.Add(obj);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            Print(sb, 0);
            return sb.ToString();
        }
        
    }

    public class Circle : GraphicObject
    {
        public override string Name => "Circle";
    }

    public class Square : GraphicObject
    {
        public override string Name => "Square";
    }
}