using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public class Cota
    {
        [Key]
        public int IdCota { get; set; }

        public string NumSocio { get; set; }

        [Display(Name = "Número de navegação do sócio")]
        public virtual Socio NumSocioNavigation { get; set; }


    }
}
