using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class Mensagem
    {
        [Key]
        public string IdMensagem { get; set; }
        public string IdPessoa { get; set; }
        public DateTime? DataEnvio { get; set; }
        public bool Lida { get; set; }
        public bool Arquivada { get; set; }
        [Required]
        [StringLength(500)]
        public string Conteudo { get; set; }

        [ForeignKey(nameof(IdPessoa))]
        [InverseProperty(nameof(Pessoa.Mensagem))]
        public virtual Pessoa IdNavigation { get; set; }
    }
}
