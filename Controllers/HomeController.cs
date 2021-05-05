using System;
using System.IO;
using System.Data;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Extensions.Logging;
using Proyecto_ED1.Models;
using Proyecto_ED1.Models.Data;

namespace Proyecto_ED1.Controllers
{
    public class HomeController : Controller
    {
        public static string SN, LN, simDep, simMun;
        public static int pacientPrio, pacientPos, dayTotal;
        public static bool start = false, sim = false;
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                //Boolean para que no se dupliquen los datos en el archivo de almacenamiento
                if (!start)
                {
                    start = true;
                    using (TextFieldParser txtParser = new TextFieldParser("Storage_File.txt"))
                    {
                        txtParser.CommentTokens = new string[] { "#" };
                        txtParser.SetDelimiters(new string[] { ";" });
                        txtParser.HasFieldsEnclosedInQuotes = true;

                        while (!txtParser.EndOfData)
                        {
                            string[] fields = txtParser.ReadFields();

                            var newPacient = new Pacient
                            {
                                Name = fields[0].ToUpper(),
                                LName = fields[1].ToUpper(),
                                CUI = Convert.ToInt64(fields[2]),
                                Departamento = fields[3],
                                Municipio = fields[4],
                                Priority = Convert.ToInt32(fields[5]),
                                Age = fields[6],
                                Occupation = fields[7],
                                Details = fields[8],
                                Diseases = fields[9],
                                Observations = fields[10],
                                Vaccinated = Convert.ToBoolean(fields[11])
                            };
                            //Verificar que la posición en la tabla hash no sea null
                            if (Singleton.Instance.hashTable[getHashcode(fields[2])] == null)
                            {
                                Singleton.Instance.hashTable[getHashcode(fields[2])] = new ELineales.Lista<Pacient>();
                            }
                            //Verificar si es la primera vez que se ingresa dicho departamento y municipio
                            if (Singleton.Instance.simIndex.IndexOf(fields[3] + ", " + fields[4]) == -1)
                            {
                                Singleton.Instance.simIndex.Add(fields[3] + ", " + fields[4]);
                                Singleton.Instance.simQueue.Add(new E_Arboles.PriorityQueue<int, int>(20));
                            }
                            //Añade toda la información a las estructuras correspondientes
                            Singleton.Instance.hashTable[getHashcode(fields[2])].Add(newPacient);
                            Singleton.Instance.Nombre.Add(fields[0].ToUpper(), getHashcode(fields[2]));
                            Singleton.Instance.Apellido.Add(fields[1].ToUpper(), getHashcode(fields[2]));
                            Singleton.Instance.CUI.Add(Convert.ToInt64(fields[2]), getHashcode(fields[2]));
                            if (!Convert.ToBoolean(fields[11]))
                            {
                                Singleton.Instance.simQueue[Singleton.Instance.simIndex.IndexOf(fields[3] + ", " + fields[4])].Add(Convert.ToInt32(fields[5]), getHashcode(fields[2]));
                            }
                        }
                    }
                }
                return View();
            }
            catch
            {
                using (new FileStream("Storage_File.txt", FileMode.CreateNew)) { }
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public ActionResult Inscription()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Inscription(IFormCollection collection)
        {
            try
            {
                if (Convert.ToString(collection["CUI"]).Length == 13)
                {
                    var newPacient = new Pacient
                    {
                        Name = Convert.ToString(collection["Name"]).ToUpper(),
                        LName = Convert.ToString(collection["LName"]).ToUpper(),
                        CUI = Convert.ToInt64(collection["CUI"]),
                        Departamento = collection["Departamento"],
                        Municipio = collection["Municipio"],
                        Age = collection["Age"],
                        Occupation = collection["Occupation"],
                        Details = collection["Details"],
                        Diseases = collection["Diseases"],
                        Vaccinated = false
                    };
                    //definir prioridad
                    //prioridad 1: personal de salud y estudiantes de medicina
                    if (newPacient.Details == "Salud" || newPacient.Details == "Estudiante de medicina en 4to año (REALIZANDO PRÁCTICAS)")
                    {
                        newPacient.Priority = 1;
                        goto skip;
                    }
                    //prioridad 2: residentes de asilo
                    else if (newPacient.Details == "Residencia en asilo")
                    {
                        newPacient.Priority = 2;
                        goto skip;
                    }
                    //prioridad 3: viejitos > 70 y adultos con enfermedades
                    else if (newPacient.Age == "70 en adelante" || newPacient.Diseases != "Ninguna de las anteriores")
                    {
                        newPacient.Priority = 3;
                        goto skip;
                    }
                    //prioridad 4: adultos de 50-69
                    else if (newPacient.Age == "50-69")
                    {
                        newPacient.Priority = 4;
                        goto skip;
                    }
                    //prioridad 5: seguridad y servicios básicos
                    else if (newPacient.Details == "Seguridad Nacional o servicios básicos")
                    {
                        newPacient.Priority = 5;
                        goto skip;
                    }
                    //prioridad 6: Eduacación
                    else if (newPacient.Details == "Educación")
                    {
                        newPacient.Priority = 6;
                        goto skip;
                    }
                    //prioridad 7: Judicial
                    else if (newPacient.Details == "Judicial")
                    {
                        newPacient.Priority = 7;
                        goto skip;
                    }
                    //prioridad 8: adultos de edad media
                    else if (newPacient.Age == "40-49")
                    {
                        newPacient.Priority = 8;
                        goto skip;
                    }
                    //Prioridad 9: chavo rukos
                    else if (newPacient.Age == "18-39")
                    {
                        newPacient.Priority = 9;
                        goto skip;
                    }
                skip:
                    //Verificar que la posición en la tabla hash no sea null
                    if (Singleton.Instance.hashTable[getHashcode(Convert.ToString(newPacient.CUI))] == null)
                    {
                        Singleton.Instance.hashTable[getHashcode(Convert.ToString(newPacient.CUI))] = new ELineales.Lista<Pacient>();
                    }
                    //Verificar si es la primera vez que se ingresa dicho departamento y municipio
                    if (Singleton.Instance.simIndex.IndexOf(newPacient.Departamento + ", " + newPacient.Municipio) == -1)
                    {
                        Singleton.Instance.simIndex.Add(newPacient.Departamento + ", " + newPacient.Municipio);
                        Singleton.Instance.simQueue.Add(new E_Arboles.PriorityQueue<int, int>(20));
                    }
                    //Añade toda la información a las estructuras correspondientes
                    Singleton.Instance.hashTable[getHashcode(Convert.ToString(newPacient.CUI))].Add(newPacient);
                    Singleton.Instance.Nombre.Add(newPacient.Name, getHashcode(Convert.ToString(newPacient.CUI)));
                    Singleton.Instance.Apellido.Add(newPacient.LName, getHashcode(Convert.ToString(newPacient.CUI)));
                    Singleton.Instance.CUI.Add(newPacient.CUI, getHashcode(Convert.ToString(newPacient.CUI)));
                    Singleton.Instance.simQueue[Singleton.Instance.simIndex.IndexOf(newPacient.Departamento + ", " + newPacient.Municipio)].Add(newPacient.Priority, getHashcode(Convert.ToString(newPacient.CUI)));

                    ViewData["Success"] = "Paciente agregado correctamente.";
                    updateFile();
                    return View();
                }
                ViewData["Error"] = "Por favor, ingrese un documento de identificación válido.";
                return View();
            }
            catch
            {
                ViewData["Error"] = "Por favor, llene todos los campos solicitados.";
                return View();
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AllPacients()
        {
            ELineales.Lista<Pacient> list = new ELineales.Lista<Pacient>();
            for (int i = 0; i < Singleton.Instance.hashTable.Length; i++)
            {
                if (Singleton.Instance.hashTable[i] != null)
                {
                    foreach (var item in Singleton.Instance.hashTable[i])
                    {
                        list.Add(item);
                    }
                }
            }
            return View(list);
        }

        public IActionResult SearchN()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SearchN(IFormCollection collection)
        {
            try
            {
                Singleton.Instance.SearchList.Clear();
                ELineales.Lista<int> hashpos = Singleton.Instance.Nombre.FindAll(Convert.ToString(collection["Name"]).ToUpper());
                if (hashpos == null)
                {
                    ViewData["Error"] = "El paciente que busca todavía no se ha registrado en la lista de espera." +
                        "Por favor, regrese a la pestaña de Inscripción e intente de nuevo";
                    return View();
                }
                else
                {
                    for (int i = 0; i < hashpos.Count(); i++)
                    {
                        bool repeated = false;
                        for (int j = 0; j < i; j++)
                        {
                            if(hashpos[i] == hashpos[j])
                            {
                                repeated = true;
                            }
                        }
                        if (!repeated)
                        {
                            foreach(var p in Singleton.Instance.hashTable[hashpos[i]])
                            {
                                if(p.Name == Convert.ToString(collection["Name"]).ToUpper())
                                {
                                    Singleton.Instance.SearchList.Add(p);
                                }
                            }
                        }
                    }
                }
                return RedirectToAction(nameof(Search));
            }
            catch
            {
                ViewData["Error"] = "Por favor, llene todos los campos solicitados.";
                return View();
            }
        }
        public IActionResult SearchLN()
        {
            return View();
        }
        public IActionResult SearchLN(IFormCollection collection)
        {
            try
            {
                Singleton.Instance.SearchList.Clear();
                ELineales.Lista<int> hashpos = Singleton.Instance.Apellido.FindAll(Convert.ToString(collection["LName"]).ToUpper());
                if (hashpos == null)
                {
                    ViewData["Error"] = "El paciente que busca todavía no se ha registrado en la lista de espera." +
                        "Por favor, regrese a la pestaña de Inscripción e intente de nuevo";
                    return View();
                }
                else
                {
                    for (int i = 0; i < hashpos.Count(); i++)
                    {
                        bool repeated = false;
                        for (int j = 0; j < i; j++)
                        {
                            if (hashpos[i] == hashpos[j])
                            {
                                repeated = true;
                            }
                        }
                        if (!repeated)
                        {
                            foreach (Pacient p in Singleton.Instance.hashTable[hashpos[i]])
                            {
                                if (p.Name == collection["LName"])
                                {
                                    Singleton.Instance.SearchList.Add(p);
                                }
                            }
                        }
                    }
                }
                return RedirectToAction(nameof(Search));
            }
            catch
            {
                ViewData["Error"] = "Por favor, llene todos los campos solicitados.";
                return View();
            }
        }
        public IActionResult SearchC()
        {
            return View();
        }
        public IActionResult SearchC(IFormCollection collection)
        {
            try
            {
                Singleton.Instance.SearchList.Clear();
                int hashpos = Singleton.Instance.CUI.Find(Convert.ToInt32(collection["CUI"]));
                if (Singleton.Instance.hashTable[hashpos] == null)
                {
                    ViewData["Error"] = "El paciente que busca todavía no se ha registrado en la lista de espera." +
                        " Por favor, regrese a la pestaña de Inscripción e intente de nuevo";
                    return View();
                }
                else
                {
                    foreach(Pacient p in Singleton.Instance.hashTable[hashpos])
                    {
                        if (p.CUI == collection["CUI"])
                        {
                            Singleton.Instance.SearchList.Add(p);
                        }
                    }
                    return RedirectToAction(nameof(Search));
                }
            }
            catch
            {
                ViewData["Error"] = "Por favor, llene todos los campos solicitados.";
                return View();
            }
        }

        public IActionResult Search()
        {
            return View(Singleton.Instance.SearchList);
        }

        public IActionResult Details(int id)
        {
            int pos = getHashcode(id.ToString());
            Pacient pa = new Pacient();
            foreach(var p in Singleton.Instance.hashTable[pos])
            {
                if(p.CUI == id)
                {
                    pa = p;
                }
            }
            return View(pa);
        }

        public IActionResult simIndex()
        {
            if (!sim)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(simPacient));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult simIndex(IFormCollection collection)
        {
            try
            {
                simDep = collection["Departamento"];
                simMun = collection["Municipio"];
                return RedirectToAction(nameof(simPacient));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult simPacient()
        {
            if (Singleton.Instance.simIndex.IndexOf(simDep + ", " + simMun) != -1 && Singleton.Instance.simQueue[Singleton.Instance.simIndex.IndexOf(simDep + ", " + simMun)].Peek() != null)
            {
                if (dayTotal < 3)
                {
                    pacientPos = Singleton.Instance.simQueue[Singleton.Instance.simIndex.IndexOf(simDep + ", " + simMun)].Peek().Data;
                    pacientPrio = Singleton.Instance.simQueue[Singleton.Instance.simIndex.IndexOf(simDep + ", " + simMun)].Peek().Key;

                    foreach (var item in Singleton.Instance.hashTable[pacientPos])
                    {
                        if (item.Departamento == simDep && item.Municipio == simMun && item.Priority == pacientPrio && !item.Vaccinated)
                        {
                            return View(item);
                        }
                    }
                    //Nunca entra aquí juasjuasjuas
                    return View();
                }
                else
                {
                    //Vista de fin del día
                    return RedirectToAction(nameof(dayEnd));
                }
            }
            else
            {
                //Vista de fin de la simulación
                return RedirectToAction(nameof(simEnd));
            }
        }

        public IActionResult simEnd()
        {
            ViewData["Departamento"] = percentDep() + "%";
            ViewData["Municipio"] = percentMun() + "%";
            ViewData["Nacional"] = percentNac() + "%";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult simEnd(IFormCollection collection)
        {
            sim = false;
            return RedirectToAction(nameof(simIndex));
        }

        public IActionResult dayEnd()
        {
            ViewData["Departamento"] = percentDep() + "%";
            ViewData["Municipio"] = percentMun() + "%";
            ViewData["Nacional"] = percentNac() + "%";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult dayEnd(IFormCollection collection)
        {
            dayTotal = 0;
            return RedirectToAction(nameof(simPacient));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult vaccinatePacient(IFormCollection collection)
        {
            Singleton.Instance.simQueue[Singleton.Instance.simIndex.IndexOf(simDep + ", " + simMun)].Pop();
            foreach (var item in Singleton.Instance.hashTable[pacientPos])
            {
                if (item.Departamento == simDep && item.Municipio == simMun && item.Priority == pacientPrio && !item.Vaccinated)
                {
                    item.Observations = collection["Observations"];
                    item.Vaccinated = true;
                }
            }
            dayTotal++;
            updateFile();
            return RedirectToAction(nameof(simPacient));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult reassignPacient(IFormCollection collection)
        {
            Singleton.Instance.simQueue[Singleton.Instance.simIndex.IndexOf(simDep + ", " + simMun)].Pop();
            foreach (var item in Singleton.Instance.hashTable[pacientPos])
            {
                if (item.Departamento == simDep && item.Municipio == simMun && item.Priority == pacientPrio && !item.Vaccinated)
                {
                    item.Priority += 10;
                }
            }
            Singleton.Instance.simQueue[Singleton.Instance.simIndex.IndexOf(simDep + ", " + simMun)].Add(pacientPrio + 10, pacientPos);
            dayTotal++;
            updateFile();
            return RedirectToAction(nameof(simPacient));
        }

        public int getHashcode(string key)
        {
            byte[] code = Encoding.ASCII.GetBytes(key);
            int hash = 0;
            for (int i = 0; i < code.Count(); i++)
            {
                hash += Convert.ToInt32(code[i]);
            }
            hash = (hash * code.Count()) % 20;
            return hash;
        }

        public void updateFile()
        {
            System.IO.File.WriteAllText("Storage_File.txt", String.Empty);
            StreamWriter writer = new StreamWriter("Storage_File.txt", true);
            for (int i = 0; i < Singleton.Instance.hashTable.Length; i++)
            {
                if (Singleton.Instance.hashTable[i] != null)
                {
                    foreach (var item in Singleton.Instance.hashTable[i])
                    {
                        writer.WriteLine(item.Name + ";" + item.LName + ";" + item.CUI + ";" + item.Departamento + ";" + item.Municipio + ";" + item.Priority + ";" + item.Age + ";" + item.Occupation + ";" + item.Details + ";" + item.Diseases + ";" + item.Observations + ";" + item.Vaccinated);
                    }
                }
            }
            writer.Close();
        }

        public decimal percentDep()
        {
            decimal total = 0, vacunados = 0;
            for (int i = 0; i < Singleton.Instance.hashTable.Length; i++)
            {
                if (Singleton.Instance.hashTable[i] != null)
                {
                    foreach (var item in Singleton.Instance.hashTable[i])
                    {
                        if (item.Departamento == simDep)
                        {
                            total++;
                            if (item.Vaccinated)
                            {
                                vacunados++;
                            }
                        }
                    }
                }
            }
            if (total == 0)
            {
                return 0;
            }
            return (vacunados / total) * 100;
        }

        public decimal percentMun()
        {
            decimal total = 0, vacunados = 0;
            for (int i = 0; i < Singleton.Instance.hashTable.Length; i++)
            {
                if (Singleton.Instance.hashTable[i] != null)
                {
                    foreach (var item in Singleton.Instance.hashTable[i])
                    {
                        if (item.Departamento == simDep && item.Municipio == simMun)
                        {
                            total++;
                            if (item.Vaccinated)
                            {
                                vacunados++;
                            }
                        }
                    }
                }
            }
            if (total == 0)
            {
                return 0;
            }
            return (vacunados / total) * 100;
        }

        public decimal percentNac()
        {
            decimal total = 0, vacunados = 0;
            for (int i = 0; i < Singleton.Instance.hashTable.Length; i++)
            {
                if (Singleton.Instance.hashTable[i] != null)
                {
                    foreach (var item in Singleton.Instance.hashTable[i])
                    {
                        total++;
                        if (item.Vaccinated)
                        {
                            vacunados++;
                        }
                    }
                }
            }
            if (total == 0)
            {
                return 0;
            }
            return (vacunados / total) * 100;
        }
    }
}