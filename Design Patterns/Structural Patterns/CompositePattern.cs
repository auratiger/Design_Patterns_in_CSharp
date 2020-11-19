using System;
using System.Collections.Generic;
using System.Text;

namespace Design_Patterns.Structural_Patterns
{
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