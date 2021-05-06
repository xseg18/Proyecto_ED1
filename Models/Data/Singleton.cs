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
        public ELineales.Lista<string> simIndex;
        public ELineales.Lista<E_Arboles.PriorityQueue<int, int>> simQueue;
        public ELineales.Lista<ELineales.Lista<int>> simVaccinated;
        public ELineales.Lista<Pacient> SearchList;

        private Singleton()
        {
            Nombre = new E_Arboles.AVL<string, int>();
            Apellido = new E_Arboles.AVL<string, int>();
            CUI = new E_Arboles.AVL<long, int>();
            hashTable = new ELineales.Lista<Pacient>[20];
            simIndex = new ELineales.Lista<string>();
            simQueue = new ELineales.Lista<E_Arboles.PriorityQueue<int, int>>();
            simVaccinated = new ELineales.Lista<ELineales.Lista<int>>();
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
