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

        Node root;
        Y[] Queue;
        int pos = 1;
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
        public Y Peek()
        {
            return root.Data;
        }
        public Y Pop()
        {
            Node safe = root;
            Node remp = FindLast();
            root.Key = remp.Key;
            root.Data = remp.Data;
            Balance(root, null);
            return safe.Data;
        }
        
        Node FindLast()
        {
            Y last = default(Y);
            decimal pos = 0;
            for (int i = 1; i < Queue.Length; i++)
            {
                if (i + 1 < Queue.Length)
                {
                    if (Queue[i + 1] == null)
                    {
                        last = Queue[i];
                        pos = i;
                        break;
                    }
                }
                else
                {
                    last = Queue[i];
                    pos = i;
                }
            }
            decimal parentpos = Math.Floor(pos / 2);
            Y parentdata = Queue[Convert.ToInt32(parentpos)];
            Node newroot = Find(last, root);
            Node oldroot = Find(parentdata, root);
            if(pos%2 == 0)
            {
                oldroot.Left = null;
            }
            else
            {
                oldroot.Right = null;
            }
            return newroot;
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
                    if(found == null)
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
        int Height(Node top)
        {
            if(top == null)
            {
                return 0;
            }
            else
            {
                int rheight = Height(top.Right);
                int lheight = Height(top.Left);
                if (rheight > lheight)
                {
                    return rheight + 1;
                }
                else return lheight + 1;
            }
        }
    }
}