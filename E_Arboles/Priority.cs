using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
namespace E_Arboles
{
    public class PriorityQueue<T, Y> where T : IComparable
    {
        public class Node
        {
            internal Node Right;
            internal Node Left;
            internal Node Parent;
            T Keys;
            Y Datas;
            public Node(T k, Y d)
            {
                Keys = k;
                Datas = d;
            }
            public T Key
            {
                get => Keys;
                set => Keys = value;
            }
            public Y Data
            {
                get => Datas;
                set => Datas = value;
            }
        }

        public int Length
        {
            get => Queue.Length;
        }
        Node root;
        static Node[] Queue;
        static int pos = 1;
        public PriorityQueue(int x)
        {
            Queue = new Node[x];
        }
        public void Add(T k, Y d)
        {
            Node a = new Node(k, d);
            if (root == null)
            {
                root = a;
                Queue[pos] = root;
                pos++;
            }
            else
            {
                Add(root, a);
            }
        }
        void Add(Node Root, Node add)
        {
            if (Root.Left == null)
            {
                Root.Left = add;
                add.Parent = Root;
                Queue[pos] = add;
                pos++;
            }
            else if (Root.Right == null)
            {
                Root.Right = add;
                add.Parent = Root;
                Queue[pos] = add;
                pos++;
            }
            else
            {
                if (Root.Left.Left == null || Root.Left.Right == null)
                {
                    Add(Root.Left, add);
                }
                else
                {
                    Add(Root.Right, add);
                }
            }
            Balance(root, null);
        }
        void Balance(Node Root, Node prev)
        {
            if (Root != null)
            {
                if (Root.Left != null)
                {
                    if (Root.Key.CompareTo(Root.Left.Key) > 0)
                    {
                        Swap(Root,Root.Left);
                        if (Root != root)
                        {
                            Balance(prev, prev.Parent);
                        }
                    }
                }
                if (Root.Right != null)
                {
                    if (Root.Key.CompareTo(Root.Right.Key) > 0)
                    {
                        Swap(Root, Root.Left);
                        if (Root != root)
                        {
                            Balance(prev, prev.Parent);
                        }
                    }
                }
                Balance(Root.Left, Root);
                Balance(Root.Right, Root);

            }
        }

        void Swap(Node s1, Node s2)
        {
            T k = s1.Key;
            Y d = s1.Data;
            s1.Key = s2.Key;
            s1.Data = s2.Data;
            s2.Key = k;
            s2.Data = d;
        }

        public Node Peek()
        {
            return root;
        }

        public Y Pop()
        {
            Node safe = root;
            Node remp = FindLast();
            if (safe != remp)
            {
                root.Key = remp.Key;
                root.Data = remp.Data;
                Balance(root, null);
                for (int i = 1; i < Queue.Length-1; i++)
                {
                    Queue[i] = Queue[i + 1];
                } 
            }
            else
            {
                root = null;
            }
            return safe.Data;
        }

        Node FindLast()
        {
            if (pos - 1 != 1)
            {
                Node last = Queue[pos - 1];
                Node parentdata = last.Parent;
                if ((pos - 1) % 2 == 0)
                {
                    parentdata.Left = null;
                }
                else
                {
                    parentdata.Right = null;
                }
                pos--;
                return last;
            }
            else
            {
                return root;
            }
        }

        Node Find(Node search, Node top)
        {
            if (top != null)
            {
                if (top.Data.Equals(search.Data) && top.Key.Equals(search.Data))
                {
                    return top;
                }
                else
                {
                    Node found = Find(search, top.Left);
                    if (found == null)
                    {
                        found = Find(search, top.Right);
                    }
                    return found;
                }
            }
            else
            {
                return null;
            }
        }

        public void Clear()
        {
            root = null;
            for (int i = 0; i < Queue.Length; i++)
            {
                Queue[i] = null;
            }
            pos = 1;
        }

        public Y[] ReturnQueue()
        {
            Y[] rqueue = new Y[Queue.Length];
            for (int i = 0; i < Queue.Length; i++)
            {
                if(Queue[i] != null)
                {
                    rqueue[i] = Queue[i].Data;
                }
            }
            return rqueue;
        }
    }
}