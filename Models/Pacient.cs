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
        [Display(Name = "Número de DPI/Partida de Nacimiento")]
        [Required]
        public long CUI { get; set; }
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
        [Display(Name = "Observaciones")]
        [Required]
        public string Observations { get; set; }
        [Display(Name = "Vacunado")]
        [Required]
        public bool Vaccinated { get; set; }
        [Display(Name = "Fecha")]
        [Required]
        public DateTime Date { get; set; }
    }
}
