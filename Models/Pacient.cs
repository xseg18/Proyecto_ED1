using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_ED1.Models
{
    public class Pacient
    {
        [Display(Name ="Nombre")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "Apellido")]
        [Required]
        public string LName { get; set; }
        [Display(Name = "DPI/Partida de Nacimiento")]
        [Required]
        public int CUI { get; set; }
        [Display(Name = "Departamento")]
        [Required]
        public string Departamento { get; set; }
        [Display(Name = "Municipio")]
        [Required]
        public string Municipio { get; set; }
        [Display(Name = "Prioridad")]
        [Required]
        public int Priority { get; set; }
        [Display(Name = "Edad")]
        [Required]
        public string Age { get; set; }
        [Display(Name = "Ocupación")]
        [Required]
        public string Occupation { get; set; }
        [Display(Name = "Detalles")]
        [Required]
        public string Details { get; set; }
        [Display(Name = "Enfermedades")]
        [Required]
        public string Diseases { get; set; }
        public bool Vaccinated { get; set; }
    }
}
