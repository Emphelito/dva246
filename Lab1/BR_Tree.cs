using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dva246_lab1 {
    interface IOrderedSet<TElement> : IEnumerable<TElement> where TElement : struct, IComparable<TElement>
    {
        int Count { get; }
        void Insert(TElement element);
        bool Search(TElement element);
        TElement? Minimum();
        TElement? Maximum();
        TElement? Successor(TElement element);
        TElement? Predecessor(TElement element);
        void UnionWith(IEnumerable<TElement> other);
    }

    public class OrderedSet<TElement> : IOrderedSet<TElement> where TElement : struct, IComparable<TElement>
    {
        private enum NodeColor { Red, Black }
        public int Count { get; private set; }
        private class Node
        {
            public Node(TElement element)
            {
                Element = element;
                Color = NodeColor.Red;
            }

            public TElement Element;
            public Node Left, Right, Parent;
            public NodeColor Color;
        }
        private Node root;

        //Inserts new elements, if tree is empty insert element as root.
        public void Insert(TElement element)
        {
            var newNode = new Node(element);

            if (root == null)
            {
                Count++;

                root = newNode;
                root.Color = NodeColor.Black;

                return;
            }

            Node current = root;
            Node parent = null;

            while (current != null)
            {
                parent = current;
                if (element.CompareTo(current.Element) > 0)
                {
                    current = current.Right;
                }
                else if (element.CompareTo(current.Element) < 0)
                {
                    current = current.Left;
                }
                else return;
            }
            newNode.Parent = parent;
            //newNode.Color = NodeColor.Red;

            if (element.CompareTo(parent.Element) > 0)
            {
                parent.Right = newNode;
            }
            else
            {
                parent.Left = newNode;
            }
            BalancedInsertion(newNode);

            Count++;
        }
        //Recolor nodes to maintian rule #4. And uses rotate functions to maintain a balanced tree 
        private void BalancedInsertion(Node node)
        {
            Node parent = node.Parent;
            Node uncle;
            while(node != root && parent.Color == NodeColor.Red)
            {
                parent = node.Parent;
                if (parent.Parent.Right == parent)
                {
                    uncle = parent.Parent.Left;
                    if(uncle != null && uncle.Color == NodeColor.Red)
                    {
                        uncle.Color = NodeColor.Black;
                        parent.Color = NodeColor.Black;
                        parent.Parent.Color = NodeColor.Red;

                        node = parent.Parent;
                        parent = node.Parent;
                    }
                    else
                    {
                        if (node == parent.Left)
                        {
                            node = parent;
                            RotateRight(node);
                            parent = node.Parent;
                        }
                        node.Parent.Color = NodeColor.Black;
                        parent.Parent.Color = NodeColor.Red;
                        RotateLeft(parent.Parent);
                    }
                }
                else
                {
                    uncle = parent.Parent.Right;
                    if(uncle != null && uncle.Color == NodeColor.Red)
                    {
                        uncle.Color = NodeColor.Black;
                        parent.Color = NodeColor.Black;
                        parent.Parent.Color = NodeColor.Red;

                        node = parent.Parent;
                        parent = node.Parent;
                    }
                    else
                    {
                        if(node == parent.Right)
                        {
                            node = parent;
                            RotateLeft(node);
                            parent = node.Parent;
                        }
                        parent.Color = NodeColor.Black;
                        parent.Parent.Color = NodeColor.Red;
                        RotateRight(parent.Parent);
                    }
                }
            }
            root.Color = NodeColor.Black;
        }
        //Makes it so that "node's" right subtree "y" becomes "node's" parent and "node" becomes "y" left subtree
        private void RotateLeft(Node node)
        {
            Node y = node.Right;
            node.Right = y.Left;

            if (y.Left != null) y.Left.Parent = node;

            y.Parent = node.Parent;

            if (node.Parent == null) root = y;
            else if (node.Parent.Left == node) node.Parent.Left = y;
            else node.Parent.Right = y;

            y.Left = node;
            node.Parent = y;

        }
        //Makes it so that "node's" left subtree "y" becomes "node's" parent and "node" becomes "y" right subtree
        private void RotateRight(Node node)
        {
            Node y = node.Left;
            node.Left = y.Right;

            if(y.Right != null) y.Right.Parent = node;

            y.Parent = node.Parent;

            if (node.Parent == null) root = y;
            else if(node.Parent.Right == node) node.Parent.Right = y;
            else node.Parent.Left = y;

            y.Right = node;
            node.Parent = y;
        } 
        //Compares given element with the element of current node to find the node belonging to given element
        public bool Search(TElement element)
        {
            Node current = root;

            while(current != null)
            {
                if(element.CompareTo(current.Element) == 0)
                {
                    return true;
                }
                else if(element.CompareTo(current.Element) > 0)
                {
                    current = current.Right;
                }
                else if(element.CompareTo(current.Element) < 0)
                {
                    current = current.Left;
                }
                else return false;
            }
            return false;
        }
        //Goes to the right as far as possible to find smallest element
        public TElement? Minimum()
        {
            Node current = root;

            while(current != null && current.Left != null)
            {
                current = current.Left;
            }
            return current.Element;
        }
        //Goes to the right as far as possible to find largest element
        public TElement? Maximum()
        {
            Node current = root;

            while(current != null && current.Right != null)
            { 
                current = current.Right; 
            }
            return current.Element;
        }
        //if we cant find the value in the tree return null. if <element> matches a value in tree return the current value. 
        //Ex. match: 43, ...41,43,45... break and return 43
        public TElement? Successor(TElement element)
        {
            TElement successor = element;

            if(!Search(element)) return null;

            foreach(var value in this)
            {
                if(element.CompareTo(value) < 0) { return value; }
            }
            return null;
        }

        //if we cant find the value in the tree return null. if <element> matches a value in tree return the last value. 
        //Ex. match: 43, ...41,43,45... break and return 41
        public TElement? Predecessor(TElement element)
        {
            TElement successor = element;

            if (!Search(element)) return null;

            foreach (var value in this)
            {
                if (element.CompareTo(value) == 0) { break; }
                successor = value;
            }
            if(element.CompareTo(successor) == 0) return null;
            return successor;
        }

        //Using enumerator to enumerate through "other" and adding each value to "this"
        public void UnionWith(IEnumerable<TElement> other)
        {
            foreach(var value in other)
            {
                this.Insert(value);
            }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return InOrder(root).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        //Recursivly enumerates through the tree, breaks on null and recursively returns element of nodes.
        private IEnumerable<TElement> InOrder(Node node)
        {
            if(node == null) yield break; //If tree has no nodes or node is leaf

            if(node.Left  != null)
            {
                foreach(TElement element in InOrder(node.Left))
                {
                    yield return element;
                }
            }
            yield return node.Element;

            if(node.Right != null)
            {
                foreach(TElement element in InOrder(node.Right))
                {
                    yield return element;
                }
            }
        }
    }
}

