using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
namespace E_Arboles
{
    public class Binary<T> where T:IComparable
    {
        public class Node
        {
            public Node Right;
            public Node Left;
            public T Key;
            public int Data;
        }
        public Node Root;
        string order = "";
        public void Add(T add, int c)
        {
            Node temp = new Node();
            temp.Data = c;
            temp.Key = add;
            if(Root == null)
            {
                Root = temp;
                return;
            }
            else
            {
                Node run = Root;
                while(run != null)
                {
                    if(temp.Key.CompareTo(run.Key) > 0)
                    {
                        if(run.Right == null)
                        {
                            run.Right = temp;
                            return;
                        }
                        else
                        {
                            run = run.Right;
                        }
                    }
                    else
                    {
                        if (run.Left == null)
                        {
                            run.Left = temp; return;
                        }
                        else
                        {
                            run = run.Left;
                        }
                    }
                }
            }
        }
        public void Delete(T data)
        {
            if (data.CompareTo(Root.Key) == 0)
            {
                Node temp1 = Root.Right;
                Node prev1 = null;
                Node next = null;
                while (temp1.Left != null)
                {
                    prev1 = temp1;
                    temp1 = temp1.Left;
                    next = temp1.Right;
                }
                Root.Key = temp1.Key;
                Root.Data = temp1.Data;
                prev1 = next;
            }
            else if(data.CompareTo(Root.Key) > 0 )
            {
                Node temp = Root;
                Node prev = null;
                while(temp != null)
                {
                    if(temp.Right.Key.CompareTo(data) == 0)
                    {
                        prev = temp;
                        temp = temp.Right;break;
                    }
                    else if(temp.Left.Key.CompareTo(data) == 0)
                    {
                        prev = temp;
                        temp = temp.Left; break;
                    }
                    else
                    {
                        temp = temp.Right;
                    }
                }
                if(temp.Left == null && temp.Right == null)
                {
                    if(prev.Right == temp)
                    {
                        prev.Right = null;
                    }
                    else if(prev.Left == temp)
                    {
                        prev.Left = null;
                    }
                }
                else
                {
                    if(temp.Right != null && temp.Left == null)
                    {
                        temp = temp.Right;
                    }
                    else if(temp.Left != null && temp.Right == null)
                    {
                        temp = temp.Left;
                    }
                    else
                    {
                        Node temp1 = temp.Right;
                        Node prev1 = null;
                        Node next = null;
                        while (temp1.Left != null)
                        {
                            prev1 = temp1;
                            temp1 = temp1.Left;
                            next = temp1.Right;
                        }
                        temp.Key = temp1.Key;
                        temp.Data = temp1.Data;
                        prev1 = next;
                    }
                }
            }
            else
            {
                Node temp = Root;
                Node prev = null;
                while (temp != null)
                {
                    if (temp.Right.Key.CompareTo(data) == 0)
                    {
                        prev = temp;
                        temp = temp.Right; break;
                    }
                    else if (temp.Left.Key.CompareTo(data) == 0)
                    {
                        prev = temp;
                        temp = temp.Left; break;
                    }
                    else
                    {
                        temp = temp.Left;
                    }
                }
                if (temp.Left == null && temp.Right == null)
                {
                    if (prev.Right == temp)
                    {
                        prev.Right = null;
                    }
                    else if (prev.Left == temp)
                    {
                        prev.Left = null;
                    }
                }
                else
                {
                    if (temp.Right != null && temp.Left == null)
                    {
                        temp = temp.Right;
                    }
                    else if (temp.Left != null && temp.Right == null)
                    {
                        temp = temp.Left;
                    }
                    else
                    {
                        Node temp1 = temp.Right;
                        Node prev1 = null;
                        Node next = null;
                        while (temp1.Left != null)
                        {
                            prev1 = temp1;
                            temp1 = temp1.Left;
                            next = temp1.Right;
                        }
                        temp.Key = temp1.Key;
                        temp.Data = temp1.Data;
                        prev1 = next;
                    }
                }
            }
        }
        public int Find(T data)
        {
            if(Root.Key.CompareTo(data)== 0)
            {
                return Root.Data;
            }
            else if(Root.Key.CompareTo(data) < 0)
            {
                Node temp = Root.Right;
                while (temp != null)
                {
                    if (temp.Key.Equals(data))
                    {
                        return temp.Data;
                    }
                    else
                    {
                        if(temp.Left != null)
                        {
                            if (temp.Left.Key.Equals(data))
                            {
                                return temp.Left.Data;
                            }
                        }
                        temp = temp.Right;
                    }
                }
            }
            else
            {
                Node temp = Root.Left;  
                while (temp != null)
                {
                    if (temp.Key.Equals(data))
                    {
                        return temp.Data;
                    }
                    else
                    {
                        if(temp.Right != null)
                        {
                            if (temp.Right.Key.Equals(data))
                            {
                                return temp.Right.Data;
                            }
                        }
                        temp = temp.Left;
                    }
                }
            }
            return -1;
        }
        public string PreOrder(Node head)
        {
            
            if(head == null)
            {
                return "";
            }
            order += head.Key.ToString() + " => ";
            PreOrder(head.Right);
            PreOrder(head.Left);
            return order;
        }
        public string InOrder(Node head)
        {
            if(head == null)
            {
                return "";
            }
            PreOrder(head.Right);
            order += head.Key.ToString() + " => ";
            PreOrder(head.Left);
            return order;
        }
        public string PostOrder(Node head)
        {
            if (head == null)
            {
                return "";
            }
            PreOrder(head.Right);
            PreOrder(head.Left);
            order += head.Key.ToString() + "=> ";
            return order;
        }
    }
}
