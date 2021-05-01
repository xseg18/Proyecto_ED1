using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_ED1.Models.Data
{
    public class Singleton
    {
        private readonly static Singleton _instance = new Singleton();

        public E_Arboles.AVL<string, int> Nombre;
        public E_Arboles.AVL<string, int> Apellido;
        public E_Arboles.AVL<long, int> CUI;
        public ELineales.Lista<Pacient>[] hashTable;
        public E_Arboles.PriorityQueue<int, int> PQueue;
        public ELineales.Lista<Pacient> SearchList;

        private Singleton()
        {
            Nombre = new E_Arboles.AVL<string, int>();
            Apellido = new E_Arboles.AVL<string, int>();
            CUI = new E_Arboles.AVL<long, int>();
            hashTable = new ELineales.Lista<Pacient>[20];
            PQueue = new E_Arboles.PriorityQueue<int, int>(20);
            SearchList = new ELineales.Lista<Pacient>();
        }

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
