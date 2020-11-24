using System;
using System.Collections.Generic;
using System.Linq;

namespace Design_Patterns.Behavioral_Patterns.IteratorPattern
{
    /*
     * The Iterator pattern is a relatively simple and frequently used design pattern.
     * Each collection must provide an iterator that lets it iterate through its objects,
     * and while doing so, making sure it is not exposing it's implementation.
     * 
     * Most collections store their elements in simple lists. However, some of them are based on stacks,
     * trees graphs and other complex data structures. But no matter how a collection is structured,
     * it must provide some way of accessing its elements so that other code can use these elements.
     * There should be a way to go through each element of the collection without accessing the same elements
     * over and over. This may sound like an easy job if you have a collection based on a list.
     * You just loop over all of the elements. But how do you sequentially traverse elements of a complex
     * data structure, such as a tree? For example, one day you might be just fine with depth-first traversal
     * of a tree. Yet the next day you might require breadth-first traversal. And the next week,
     * you might need something else, like random access to the tree elements. Adding more and more
     * traversal algorithms to the collection gradually blurs its primary responsibility,
     * which is efficient data storage. Additionally, some algorithms might be tailored
     * for a specific application, so including them into a generic collection class would be weird.
     *
     * So how do we solve this?
     *
     * The main idea of the Iterator pattern is to extact the traversal behavior of a collection into
     * a separate object called an iterator. In addition to implementing the algorithm itself,
     * an iterator object encapsulates all of the traversal details, such as the current position
     * and how many elements are left till the end. Because of this, several iterators can go
     * through the same collection at the same time, independently of each other.
     *
     * All iterators much implement the same interface. This makes the client code compatible
     * with any collection type or any traversal algorithm as long as there's a proper iterator.
     * If you need a special way to traverse a collection, you just create a new iterator class,
     * without having to change the collection or the client.
     *
     * This approach exactly solves the issue that we might face. Instead of creating a new algorithm
     * in the collection itself, we extract this logically separately to not violate the OCP and SRP.
     *
     */
    public class IteratorPattern
    {
        public static void Test()
        {
            //    1
            //   / \
            //  2   3
            
            // in-order: 213
            var root = new Node<int>(1,
            new Node<int>(2), new Node<int>(3));
            
            // C++ Style
            var it = new InOrderIterator<int>(root);
            while (it.MoveNext())
            {
                Console.WriteLine(it.Current.Value);
                Console.WriteLine(',');
            }
            Console.WriteLine();
            
            // C$ style
            var tree = new BinaryTree<int>(root);
            
            Console.WriteLine(string.Join(",", tree.NaturalInOrder.Select(x => x.Value)));
            
            // duck typing!
            foreach(var node in tree)
                Console.WriteLine(node.Value);
        }
    }

    public class Node<T>
    {
        public T Value;
        public Node<T> Left, Right;
        public Node<T> Parent;

        public Node(T value)
        {
            Value = value;
        }

        public Node(T value, Node<T> left, Node<T> right)
        {
            Value = value;
            Left = left;
            Right = right;

            left.Parent = right.Parent = this;
        }
    }
    
    // C++ approach
    public class InOrderIterator<T>
    {
        public readonly Node<T> Root;
        public Node<T> Current { get; set; }
        private bool yieldedStart;

        public InOrderIterator(Node<T> root)
        {
            Root = root;
            Current = root;
            while (Current.Left != null)
                Current = Current.Left;
            
            //    1 <- root
            //   / \
            //  2   3
            //  ^ Current
            
            
        }

        public bool MoveNext()
        {
            if (!yieldedStart)
            {
                yieldedStart = true;
                return true;
            }

            if (Current.Right != null)
            {
                Current = Current.Right;
                while (Current.Left != null)
                {
                    Current = Current.Left;
                }

                return true;
            }
            else
            {
                var p = Current.Parent;
                while (p != null && Current == p.Right)
                {
                    Current = p;
                    p = p.Parent;
                }

                Current = p;
                return Current != null;
            }
        }

        public void Reset()
        {
            Current = Root;
            yieldedStart = false;
        }
    }

    // recursive C# approach
    public class BinaryTree<T> 
    {
        private Node<T> Root;

        public BinaryTree(Node<T> root)
        {
            Root = root;
        }

        public InOrderIterator<T> GetEnumerator()
        {
            return new InOrderIterator<T>(Root);
        }

        public IEnumerable<Node<T>> NaturalInOrder
        {
            get
            {
                IEnumerable<Node<T>> TraverseInOrder(Node<T> current)
                {
                    if (current.Right != null)
                    {
                        foreach (var left in TraverseInOrder(current.Left))
                            yield return left;
                    }

                    yield return current;

                    if (current.Right != null)
                    {
                        foreach (var right in TraverseInOrder(current.Right))
                            yield return right;
                    }
                }

                foreach (var node in TraverseInOrder(Root))
                    yield return node;
            }
        }
    }
    
}