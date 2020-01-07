using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class SolicitacaoProfessor
    {
        public SolicitacaoProfessor()
        {
            Professor = new HashSet<Professor>();
            Socio = new HashSet<Socio>();
        }

        [Key]
        [Display(Name = "Id de solicitação")]
        public int? IdSolicitacao { get; set; }

        [Display(Name = "Número de administrador")]
        public string NumAdmin { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? Data { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.SolicitacaoProfessor))]
        [Display(Name = "Número de navegação do administrador")]
        public virtual Admin NumAdminNavigation { get; set; }
        
        [InverseProperty("IdSolicitacaoNavigation")]
        public virtual ICollection<Professor> Professor { get; set; }
        
        [InverseProperty("IdSolicitacaoNavigation")]
        public virtual ICollection<Socio> Socio { get; set; }
    }
}
