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
                Singleton.Instance3.hashTable[pos].Add(newPacient);
                Singleton.Instance4.PQueue.Add(newPacient.Priority, pos);
                return View();
            }
            catch
            {
                ViewData["Error"] = "LLene todos los datos solicitados, porfavor";
                return View(Error());
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
    }
}
