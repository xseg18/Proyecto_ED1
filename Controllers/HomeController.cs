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
        public static string SN, LN;
        public static bool start = false;
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
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
                                Name = fields[0],
                                LName = fields[1],
                                CUI = Convert.ToInt32(fields[2]),
                                Departamento = fields[3],
                                Municipio = fields[4],
                                Priority = Convert.ToInt32(fields[5]),
                                Age = fields[6],
                                Occupation = fields[7],
                                Details = fields[8],
                                Diseases = fields[9]
                            };

                            if (Singleton.Instance.hashTable[getHashcode(fields[2])] == null)
                            {
                                Singleton.Instance.hashTable[getHashcode(fields[2])] = new ELineales.Lista<Pacient>();
                            }

                            Singleton.Instance.hashTable[getHashcode(fields[2])].Add(newPacient);
                            Singleton.Instance.Nombre.Add(fields[0], getHashcode(fields[2]));
                            Singleton.Instance.Apellido.Add(fields[1], getHashcode(fields[2]));
                            Singleton.Instance.CUI.Add(Convert.ToInt32(fields[2]), getHashcode(fields[2]));
                            Singleton.Instance.PQueue.Add(Convert.ToInt32(fields[5]), getHashcode(fields[2]));
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

                if (Singleton.Instance.hashTable[pos] == null)
                {
                    Singleton.Instance.hashTable[pos] = new ELineales.Lista<Pacient>();
                }

                Singleton.Instance.hashTable[pos].Add(newPacient);
                Singleton.Instance.Nombre.Add(newPacient.Name, pos);
                Singleton.Instance.Apellido.Add(newPacient.LName, pos);
                Singleton.Instance.CUI.Add(newPacient.CUI, pos);
                Singleton.Instance.PQueue.Add(newPacient.Priority, pos);

                ViewData["Success"] = "Paciente agregado correctamente.";
                updateFile();
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
                if(Singleton.Instance.hashTable[i] != null)
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
                ELineales.Lista<int> hashpos = Singleton.Instance.Apellido.FindAll(collection["Name"]);
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
                        if (Singleton.Instance.hashTable[hashpos[i]].Count() == 1)
                        {
                            Singleton.Instance.SearchList.Add(Singleton.Instance.hashTable[hashpos[i]][0]);
                        }
                        else
                        {
                            for (int j = 1; j < Singleton.Instance.hashTable[hashpos[i]].Count(); j++)
                            {
                                if (Singleton.Instance.hashTable[hashpos[i]][j].Name == collection["Name"])
                                {
                                    Singleton.Instance.SearchList.Add(Singleton.Instance.hashTable[hashpos[i]][j]);
                                }
                            }
                        }
                    }
                }
                //mensaje de éxito y retornar vista
            }
            catch
            {
                ViewData["Error"] = "Por favor, llene todos los campos solicitados.";
            }
            return View();
        }

        public IActionResult SearchLN(IFormCollection collection)
        {
            try
            {
                Singleton.Instance.SearchList.Clear();
                ELineales.Lista<int> hashpos = Singleton.Instance.Apellido.FindAll(collection["LName"]);
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
                        if(Singleton.Instance.hashTable[hashpos[i]].Count() == 1)
                        {
                            Singleton.Instance.SearchList.Add(Singleton.Instance.hashTable[hashpos[i]][0]);
                        }
                        else
                        {
                            for (int j = 1; j < Singleton.Instance.hashTable[hashpos[i]].Count(); j++)
                            {
                                if(Singleton.Instance.hashTable[hashpos[i]][j].LName == collection["LName"])
                                {
                                    Singleton.Instance.SearchList.Add(Singleton.Instance.hashTable[hashpos[i]][j]);
                                }
                            }
                        }
                    }
                }
                //mensaje de éxito y retornar vista
            }
            catch
            {
                ViewData["Error"] = "Por favor, llene todos los campos solicitados.";
            }
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
                        "Por favor, regrese a la pestaña de Inscripción e intente de nuevo";
                    return View();
                }
                else
                {
                    for (int j = 0; j < Singleton.Instance.hashTable[hashpos].Count(); j++)
                    {
                        if(Singleton.Instance.hashTable[hashpos][j].CUI == collection["CUI"])
                        {
                            Singleton.Instance.SearchList.Add(Singleton.Instance.hashTable[hashpos][j]);
                        }
                    }
                }
            }
            catch
            {
                ViewData["Error"] = "Por favor, llene todos los campos solicitados.";
            }
            return View();
        }

        public IActionResult Search()
        {
            return View(Singleton.Instance.SearchList);
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
                        writer.WriteLine(item.Name + ";" + item.LName + ";" + item.CUI + ";" + item.Departamento + ";" + item.Municipio + ";" + item.Priority + ";" + item.Age + ";" + item.Occupation + ";" + item.Details + ";" + item.Diseases);
                    }
                }
            }
            writer.Close();
        }
        
        public void SearcherN(Pacient p, string search)
        {
            if (p.Name == search)
            {
                Singleton.Instance.SearchList.Add(p);
            }
        }

        //public void SearcherLN(Pacient p)
        //{
        //    if (p.LName == search)
        //    {
        //        hashpos.Add();
        //    }
        //}
    }
}
