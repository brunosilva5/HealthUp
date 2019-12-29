using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class Aula
    {
        public Aula()
        {
            Inscreve = new HashSet<Inscreve>();
        }

        [Key]
        public int IdAula { get; set; }
        public string NumProfessor { get; set; }
        public string NumAdmin { get; set; }
        public DateTime? ValidoDe { get; set; }
        public DateTime? ValidoAte { get; set; }
        public int Lotacao { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public int DiaSemana { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.Aula))]
        public virtual Admin NumAdminNavigation { get; set; }
        [ForeignKey(nameof(NumProfessor))]
        [InverseProperty(nameof(Professor.Aula))]
        public virtual Professor NumProfessorNavigation { get; set; }
        [InverseProperty("IdAulaNavigation")]
        public virtual AulaGrupo AulaGrupo { get; set; }
        [InverseProperty("IdAulaNavigation")]
        public virtual ICollection<Inscreve> Inscreve { get; set; }
    }
}