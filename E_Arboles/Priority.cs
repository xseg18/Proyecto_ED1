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
            public Node Right;
            public Node Left;
            public Node Parent;
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
        static Y[] Queue;
        static int pos = 1;
        public PriorityQueue(int x)
        {
            Queue = new Y[x];
        }
        public void Add(T k, Y d)
        {
            Node a = new Node(k, d);
            if (root == null)
            {
                root = a;
                Queue[pos] = root.Data;
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
                Queue[pos] = add.Data;
                pos++;
            }
            else if (Root.Right == null)
            {
                Root.Right = add;
                add.Parent = Root;
                Queue[pos] = add.Data;
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
                        Swap(Array.IndexOf(Queue, Root.Data), Array.IndexOf(Queue, Root.Left.Data));
                        T k = Root.Key;
                        Y d = Root.Data;
                        Root.Key = Root.Left.Key;
                        Root.Data = Root.Left.Data;
                        Root.Left.Key = k;
                        Root.Left.Data = d;
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
                        Swap(Array.IndexOf(Queue, Root.Data), Array.IndexOf(Queue, Root.Right.Data));
                        T k = Root.Key;
                        Y d = Root.Data;
                        Root.Key = Root.Right.Key;
                        Root.Data = Root.Right.Data;
                        Root.Right.Key = k;
                        Root.Right.Data = d;
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
        void Swap(int pos1, int pos2)
        {
            Y safe = Queue[pos1];
            Queue[pos1] = Queue[pos2];
            Queue[pos2] = safe;
        }
        public Y Peek()
        {
            return root.Data;
        }
        public Y Pop()
        {
            Y safe = root.Data;
            Node remp = FindLast();
            if (remp != root)
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
            return safe;
        }

        Node FindLast()
        {
            if (pos - 1 != 1)
            {
                Y last = Queue[pos - 1];
                decimal parentpos = Math.Floor(Convert.ToDecimal((pos - 1) / 2));
                Y parentdata = Queue[Convert.ToInt32(parentpos)];
                Node newroot = Find(last, root);
                Node oldroot = Find(parentdata, root);
                if ((pos - 1) % 2 == 0)
                {
                    oldroot.Left = null;
                }
                else
                {
                    oldroot.Right = null;
                }
                pos--;
                return newroot;
            }
            else
            {
                return root;
            }
        }
        Node Find(Y data, Node top)
        {
            if (top != null)
            {
                if (top.Data.Equals(data))
                {
                    return top;
                }
                else
                {
                    Node found = Find(data, top.Left);
                    if (found == null)
                    {
                        found = Find(data, top.Right);
                    }
                    return found;
                }
            }
            else
            {
                return null;
            }
        }
        public Y[] ReturnQueue()
        {
            return Queue;
        }
    }
}