﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_ED1.Models.Data
{
    public class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public E_Arboles.AVL<string, int> Nombre;
        private readonly static Singleton _instance1 = new Singleton();
        public E_Arboles.AVL<string, int> Apellido;
        private readonly static Singleton _instance2 = new Singleton();
        public E_Arboles.AVL<int, int> CUI;
        private readonly static Singleton _instance3 = new Singleton();
        public ELineales.Lista<Pacient>[] hashTable;

        private Singleton()
        {
            Nombre = new E_Arboles.AVL<string, int>();
            Apellido = new E_Arboles.AVL<string, int>();
            CUI = new E_Arboles.AVL<int, int>();
            hashTable = new ELineales.Lista<Pacient>[20];
        }

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }
        public static Singleton Instance1
        {
            get
            {
                return _instance1;
            }
        }
        public static Singleton Instance2
        {
            get
            {
                return _instance2;
            }
        }

        public static Singleton Instance3
        {
            get
            {
                return _instance3;
            }
        }
    }
}
