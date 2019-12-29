using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class PlanoTreino
    {
        public PlanoTreino()
        {
            Contem = new HashSet<Contem>();
        }

        [Key]
        public int IdPlano { get; set; }
        public string NumSocio { get; set; }
        public string NumProfessor { get; set; }
        public bool Ativo { get; set; }
        [Required]
        [StringLength(500)]
        public string Descricao { get; set; }

        [ForeignKey(nameof(NumProfessor))]
        [InverseProperty(nameof(Professor.PlanoTreino))]
        public virtual Professor NumProfessorNavigation { get; set; }
        [ForeignKey(nameof(NumSocio))]
        [InverseProperty(nameof(Socio.PlanoTreino))]
        public virtual Socio NumSocioNavigation { get; set; }
        [InverseProperty("IdPlanoNavigation")]
        public virtual ICollection<Contem> Contem { get; set; }
    }
}

