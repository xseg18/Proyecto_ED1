using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;


namespace E_Arboles
{
    public class AVL<T, Y> where T : IComparable
    {
        public class Node
        {
            public Node Left;
            public Node Right;
            public T Key;
            public Y Data;
            public Node(T Key, Y Data)
            {
                this.Key = Key;
                this.Data = Data;
            }
        }
        Node Root;
        public string Order = "";

        public void Add(T key, Y data)
        {
            Node item = new Node(key, data);
            if (Root == null)
            {
                Root = item;
            }
            else
            {
                Root = Add(Root, item);
            }
        }

        private Node Add(Node actual, Node item)
        {
            if (actual == null)
            {
                actual = item;
                return actual;
            }
            else if (item.Key.CompareTo(actual.Key) < 0)
            {
                actual.Left = Add(actual.Left, item);
                actual = Balance(actual);
            }
            else if (item.Key.CompareTo(actual.Key) > 0)
            {
                actual.Right = Add(actual.Right, item);
                actual = Balance(actual);
            }
            return actual;
        }

        public void Delete(T key)
        {
            Root = Delete(Root, key);
        }

        public Node Delete(Node actual, T key)
        {
            if (actual == null)
            {
                return actual;
            }
            else if (key.CompareTo(actual.Key) < 0)
            {
                actual.Left = Delete(actual.Left, key);
            }
            else if (key.CompareTo(actual.Key) > 0)
            {
                actual.Right = Delete(actual.Right, key);
            }
            else
            {
                if (actual.Left == null || actual.Right == null)
                {
                    Node temp = null;
                    if (temp == actual.Left)
                    {
                        temp = actual.Right;
                    }
                    else
                    {
                        temp = actual.Left;
                    }

                    if (temp == null)
                    {
                        actual = null;
                    }
                    else
                    {
                        actual = temp;
                    }
                }
                else
                {
                    Node temp = actual.Left;
                    while (temp.Right != null)
                    {
                        temp = temp.Right;
                    }
                    actual.Key = temp.Key;
                    actual.Data = temp.Data;
                    actual.Left = Delete(actual.Left, temp.Key);
                }
            }
            if (actual == null)
            {
                return actual;
            }
            actual = Balance(actual);
            return actual;
        }

        private Node Balance(Node actual)
        {
            if (dBalance(actual) < -1)
            {
                if (dBalance(actual.Left) > 0)
                {
                    actual = RotLR(actual);
                }
                else
                {
                    actual = RotLL(actual);
                }
            }
            else if (dBalance(actual) > 1)
            {
                if (dBalance(actual.Right) > 0)
                {
                    actual = RotRR(actual);
                }
                else
                {
                    actual = RotRL(actual);
                }
            }
            return actual;
        }

        private int dBalance(Node actual)
        {
            int Lbalance = Height(actual.Left);
            int Rbalance = Height(actual.Right);
            return Rbalance - Lbalance;
        }

        private int Height(Node actual)
        {
            if (actual == null)
            {
                return 0;
            }
            else
            {
                int Lheight = Height(actual.Left);
                int Rheight = Height(actual.Right);
                return Lheight > Rheight ? Lheight + 1 : Rheight + 1;
            }
        }

        private Node RotRR(Node root)
        {
            Node temp = root.Right;
            root.Right = temp.Left;
            temp.Left = root;
            return temp;
        }

        private Node RotLL(Node root)
        {
            Node temp = root.Left;
            root.Left = temp.Right;
            temp.Right = root;
            return temp;
        }

        private Node RotLR(Node root)
        {
            Node temp = root.Left;
            root.Left = RotRR(temp);
            return RotLL(root);
        }

        private Node RotRL(Node root)
        {
            Node temp = root.Right;
            root.Right = RotLL(temp);
            return RotRR(root);
        }

        public Y Find(T key)
        {
            return Find(Root, key);
        }

        private Y Find(Node head, T key)
        {
            if (head == null)
            {
                return default(Y);
            }
            else if (key.CompareTo(head.Key) < 0)
            {
                return Find(head.Left, key);
            }
            else if (key.CompareTo(head.Key) > 0)
            {
                return Find(head.Right, key);
            }
            return head.Data;
        }

        public string PreOrder()
        {
            return PreOrder(Root);
        }
        
        private string PreOrder(Node head)
        {
            if (head == null)
            {
                return "";
            }
            Order += head.Key.ToString() + " =>";
            PreOrder(head.Left);
            PreOrder(head.Right);
            return Order;
        }

        public string InOrder()
        {
            return InOrder(Root);
        }

        private string InOrder(Node head)
        {
            if (head == null)
            {
                return "";
            }
            InOrder(head.Left);
            Order += head.Key.ToString() + " =>";
            InOrder(head.Right);
            return Order;
        }

        public string PostOrder()
        {
            return PostOrder(Root);
        }

        private string PostOrder(Node head)
        {
            if (head == null)
            {
                return "";
            }
            PostOrder(head.Left);
            PostOrder(head.Right);
            Order += head.Key.ToString() + " =>";
            return Order;
        }

       
        public ELineales.Lista<Y> FindAll(T search){

            return FindAll(Root, search);
        }

        private ELineales.Lista<Y> FindAll(Node head, T key)
        {
            ELineales.Lista<Y> found = new ELineales.Lista<Y>();
            if (head == null)
            {
                return null;
            }
            else if (key.CompareTo(head.Key) < 0)
            {
                return FindAll(head.Left, key);
            }
            else if (key.CompareTo(head.Key) > 0)
            {
                return FindAll(head.Right, key);
            }
            found.Add(head.Data);
            return found;
        }
    }
}