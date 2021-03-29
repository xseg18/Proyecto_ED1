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
            Node Right;
            Node Left;
            T Key;
            Y Data;
            Node(Y k, T d)
            {
                Key = k;
                Data = d;
            }
        }
        Node root;
        public void Add(T k, Y, d)
        {
            Node a = new Node(k, d);
            if(root == null)
            {
                root = a;
            }
            else
            {
                Add(root, a)
            }
        }
        void Add(Node Root, Node add)
        {
            if(Root.Left == null)
            {
                Root.left = add;

            }
            else if(Root.right == null)
            {
                Root.Right = add;
            }
            else
            {
                if(Root.Left.Left == null || Root.Left.Right == null)
                {
                    Add(Root.Left, add);
                }
                else
                {
                    Add(Root.Right, add);
                }
            }
            Balance(root);
        }

        void Balance(Node Root)
        {
            if(Root != null)
            {
                if(Root.Key < Root.Left.Key)
                {
                    Node safe = Root;
                    Root.Key = Root.Left.Key;
                    Root.Data = Root.Left.Data;
                    Root.Left.Key = safe.Key;
                    Root.Left.Data = safe.Data;
                }
                else if(Root.Key < Root.Right.Key)
                {
                    Node safe = Root;
                    Root.Key = Root.Right.Key;
                    Root.Data = Root.Right.Data;
                    Root.Right.Key = safe.Key;
                    Root.Right.Data = safe.Data;
                }
                Balance(Root.Left);
                Balance(Root.Right);
            }
        }
    }
}