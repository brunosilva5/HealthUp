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
        [Display(Name = "Id da mensagem")]
        public int IdMensagem { get; set; }

        [Display(Name = "Id da pessoa")]
        public string IdPessoa { get; set; }
        
        public DateTime? DataEnvio { get; set; }
        
        public bool Lida { get; set; }
        
        public bool Arquivada { get; set; }
        
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(500)]
        [Display(Name = "Conteúdo")]
        public string Conteudo { get; set; }

        [ForeignKey(nameof(IdPessoa))]
        [InverseProperty(nameof(Pessoa.Mensagem))]
        [Display(Name = "Id de navegação")]
        public virtual Pessoa IdNavigation { get; set; }
    }
}
