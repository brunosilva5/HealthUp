using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class AulaGrupo
    {
        [Key]
        public int IdAula { get; set; }
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(30)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(100)]
        public string FotografiaDivulgacao { get; set; }
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(500)]
        public string Descricao { get; set; }

        [ForeignKey(nameof(IdAula))]
        [InverseProperty(nameof(Aula.AulaGrupo))]
        public virtual Aula IdAulaNavigation { get; set; }
    }
}
