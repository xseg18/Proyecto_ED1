using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Proyecto_ED1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Proyecto_ED1.Models.Data;
using System.IO;
using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Proyecto_ED1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public static string SN;
        public static string LN;
        public IActionResult Index()
        {
            return View();
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
                var newPacient = new Models.Pacient
                {
                    Name = collection["Name"],
                    LName = collection["LName"],
                    Departamento = collection["Departamento"],
                    Municipio = collection["Municipio"],
                    CUI = Convert.ToInt32(collection["CUI"]),
                    Age = collection["Age"],
                    Occupation = collection["Occupation"],
                    Details = collection["Details"],
                    Diseases = collection["Diseases"]
                };
                //definir prioridad
                //prioridad 1: personal de salud y estudiantes de medicina
                if(newPacient.Details == "Salud" || newPacient.Details == "Estudiante de medicina en 4to año (REALIZANDO PRÁCTICAS)")
                {
                    newPacient.Priority = 1;
                    goto skip;
                }
                //prioridad 2: residentes de asilo
                else if(newPacient.Details == "Residencia en asilo")
                {
                    newPacient.Priority = 2;
                    goto skip;
                } 
                //prioridad 3: viejitos > 70 y adultos con enfermedades
                else if(newPacient.Age == "70 en adelante" || newPacient.Diseases != "Ninguna de las anteriores")
                {
                    newPacient.Priority = 3;
                    goto skip;
                }
                //prioridad 4: adultos de 50-69
                else if(newPacient.Age == "50-69")
                {
                    newPacient.Priority = 4;
                    goto skip;
                }
                //prioridad 5: seguridad y servicios básicos
                else if(newPacient.Details == "Seguridad Nacional o servicios básicos")
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
                int pos = getHashcode(Convert.ToString(newPacient.CUI));
                Singleton.Instance.Nombre.Add(newPacient.Name, pos);
                Singleton.Instance1.Apellido.Add(newPacient.LName, pos);
                Singleton.Instance2.CUI.Add(newPacient.CUI, pos);
                if (Singleton.Instance3.hashTable[pos] == null)
                {
                    Singleton.Instance3.hashTable[pos] = new ELineales.Lista<Pacient>();
                }
                Singleton.Instance3.hashTable[pos].Add(newPacient);
                Singleton.Instance4.PQueue.Add(newPacient.Priority, pos);
                ViewData["Success"] = "Tarea agregada existosamente.";
                return View();
            }
            catch
            {
                ViewData["Error"] = "LLene todos los datos solicitados, porfavor";
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
            for (int i = 0; i < Singleton.Instance3.hashTable.Length; i++)
            {
                if(Singleton.Instance3.hashTable[i] != null)
                {
                    for (int j = 0; j < Singleton.Instance3.hashTable[i].Count(); j++)
                    {
                        list.Add(Singleton.Instance3.hashTable[i][j]);
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
                Singleton.Instance5.SearchList.Clear();
                ELineales.Lista<int> hashpos = Singleton.Instance1.Apellido.FindAll(collection["Name"]);
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
                        if (Singleton.Instance3.hashTable[hashpos[i]].Count() == 1)
                        {
                            Singleton.Instance5.SearchList.Add(Singleton.Instance3.hashTable[hashpos[i]][0]);
                        }
                        else
                        {
                            for (int j = 1; j < Singleton.Instance3.hashTable[hashpos[i]].Count(); j++)
                            {
                                if (Singleton.Instance3.hashTable[hashpos[i]][j].Name == collection["Name"])
                                {
                                    Singleton.Instance5.SearchList.Add(Singleton.Instance3.hashTable[hashpos[i]][j]);
                                }
                            }
                        }
                    }
                }
                //mensaje de éxito y retornar vista
            }
            catch
            {
                ViewData["Error"] = "Por favor,ingrese nuevamente los datos pedidos";
            }
            return View();
        }

        public IActionResult SearchLN(IFormCollection collection)
        {
            try
            {
                Singleton.Instance5.SearchList.Clear();
                ELineales.Lista<int> hashpos = Singleton.Instance1.Apellido.FindAll(collection["LName"]);
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
                        if(Singleton.Instance3.hashTable[hashpos[i]].Count() == 1)
                        {
                            Singleton.Instance5.SearchList.Add(Singleton.Instance3.hashTable[hashpos[i]][0]);
                        }
                        else
                        {
                            for (int j = 1; j < Singleton.Instance3.hashTable[hashpos[i]].Count(); j++)
                            {
                                if(Singleton.Instance3.hashTable[hashpos[i]][j].LName == collection["LName"])
                                {
                                    Singleton.Instance5.SearchList.Add(Singleton.Instance3.hashTable[hashpos[i]][j]);
                                }
                            }
                        }
                    }
                }
                //mensaje de éxito y retornar vista
            }
            catch
            {
                ViewData["Error"] = "Por favor, ingrese nuevamente los datos pedidos";
            }
            return View();
        }

        public IActionResult SearchC(IFormCollection collection)
        {
            try
            {
                Singleton.Instance5.SearchList.Clear();
                int hashpos = Singleton.Instance2.CUI.Find(Convert.ToInt32(collection["CUI"]));
                if (Singleton.Instance3.hashTable[hashpos] == null)
                {
                    ViewData["Error"] = "El paciente que busca todavía no se ha registrado en la lista de espera." +
                        "Por favor, regrese a la pestaña de Inscripción e intente de nuevo";
                    return View();
                }
                else
                {
                    for (int j = 0; j < Singleton.Instance3.hashTable[hashpos].Count(); j++)
                    {
                        if(Singleton.Instance3.hashTable[hashpos][j].CUI == collection["CUI"])
                        {
                            Singleton.Instance5.SearchList.Add(Singleton.Instance3.hashTable[hashpos][j]);
                        }
                    }
                }
            }
            catch
            {
                ViewData["Error"] = "Por favor,ingrese nuevamente los datos pedidos";
            }
            return View();
        }
        public IActionResult Search()
        {
            return View(Singleton.Instance5.SearchList);
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
        
        public void SearcherN(Pacient p, string search)
        {
            if (p.Name == search)
            {
                Singleton.Instance5.SearchList.Add(p);
            }
        }

        public void SearcherLN(Pacient p)
        {
            if (p.LName == search)
            {
                hashpos.Add();
            }
        }
    }
}
