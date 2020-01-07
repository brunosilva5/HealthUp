using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class Inscreve
    {
        [Key]
        [Display(Name = "Número de sócio")]
        public string NumSocio { get; set; }

        [Display(Name = "Id da aula")]
        public int IdAula { get; set; }

        public DateTime? Data { get; set; }

        [ForeignKey(nameof(IdAula))]
        [InverseProperty(nameof(Aula.Inscreve))]
        [Display(Name = "Id de navegação da aula")]
        public virtual Aula IdAulaNavigation { get; set; }
        
        [ForeignKey(nameof(NumSocio))]
        [InverseProperty(nameof(Socio.Inscreve))]
        [Display(Name = "Número de navegação do sócio")]
        public virtual Socio NumSocioNavigation { get; set; }
    }
}
