using System;
using System.Collections;
using System.Collections.Generic;

namespace ELineales
{
	public class Lista<T> : IEnumerable<T> 
	{
		private class Node
		{
			public Node Next;
			public T Data;
		}
		Node Top;

		public void Add(T item)
		{
			Node agregar = new Node();
			agregar.Data = item;
			agregar.Next = null;
			if (Top == null)
			{
				Top = agregar;
			}
			else
			{
				Node temp = Top;
				while (temp.Next != null)
				{
					temp = temp.Next;
				}
				temp.Next = agregar;
			}
		}

		public int Count()
		{
			int contador = 0;
			Node count = Top;
			while (count != null)
			{
				contador++;
				count = count.Next;
			}
			return contador;
		}

		private bool RemoveAt(int index)
		{
			if (index == 0 && Top == null)
			{
				throw new System.ArgumentException("ListaNoExistente");
			}
			if (index == 0)
			{
				Top = Top.Next;
			}
			Node inicio = Top;
			Node Prev;
			int count = 0;
			while (inicio != null && count != index)
			{
				if (count == index - 1)
				{
					Prev = inicio;
					Prev.Next = Prev.Next.Next;
					return true;
				}
				else
				{
					count++;
					inicio = inicio.Next;
				}
			}
			return false;
		}
		public int IndexOf(T item)
		{
			int pos = 0;
			Node actual = Top;
			while (actual.Next != null)
			{
				if (actual.Data.Equals(item))
				{
					return pos;
				}
				else
				{
					actual = actual.Next;
					pos++;
				}
			}
			return -1;
		}

		public void Foreach(Action<T> action)
		{
			Node temp = Top;
			while (temp != null)
			{
				action(temp.Data);
				temp = temp.Next;
			}
		}
		private IEnumerable<T> Events()
		{
			Node temp = Top;
			while (temp != null)
			{
				yield return temp.Data;
				temp = temp.Next;
			}
		}
		public IEnumerator<T> GetEnumerator()
		{

			return Events().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public T this[int index]
		{
			get
			{
				Node temp = Top;
				int cont = 0;
				while (temp != null)
				{
					if (cont == index)
					{
						return temp.Data;
					}
					else
					{
						temp = temp.Next;
						cont++;
					}
				}
				throw new System.ArgumentNullException("OutOfRange");
			}
			set
			{
				Node temp = Top;
				int cont = 0;
				while (temp != null)
				{
					if (cont == index)
					{
						temp.Data = value;
						break;
					}
					else
					{
						temp = temp.Next;
						cont++;
					}
				}
			}
		}
		public void Clear()
		{
			Top = null;
		}
	}
}
