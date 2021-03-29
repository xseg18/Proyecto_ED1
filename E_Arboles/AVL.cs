using System;
using System.Collections.Generic;
using System.Text;

namespace E_Arboles
{
    public class AVL<T, Y> where T:IComparable
    {
        public class Node
        {
            public Node Right;
            public Node Left;
            public T Key;
            public Y Data;
        }
        public Node Root;
        string order;
        public void Add(Node root, Y data, T key)
        {
            Node Adding = new Node();
            Adding.Data = data;
            Adding.Key = key;
            if(Root == null)
            {
                Root = Adding;
            }
            else
            {
                if(Adding.Key.CompareTo(root.Key) > 0)
                {
                    if(root.Right == null)
                    {
                        root.Right = Adding;
                    }
                    else
                    {
                        Add(root.Right, data, key);
                        
                    }
                }
                else if(Adding.Key.CompareTo(root.Key) < 0)
                {
                    if(root.Left == null)
                    {
                        root.Left = Adding;
                    }
                    else
                    {
                        Add(root.Left, data, key);
                    }
                }
            }
            if (Height(Root) >= 1)
            {
                //
            }
            else if (Height(Root) <= -1)
            {
                //rotación derecha
            }
        }
        int Height(Node root)
        {
            if(root == null)
            {
                return 0;
            }
            else
            {
                int Lheight = Height(root.Left);
                int Rheight = Height(root.Right);
                int Fheight = Lheight - Rheight;
                return Fheight;
            }
        }
    }
}
