using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthUp.Models
{
    public partial class Mensagem
    {
        [Key]
        [Display(Name = "Id da mensagem")]
        public int IdMensagem { get; set; }

        [Display(Name = "Id da pessoa")]
        public string IdPessoaReceiver { get; set; }

        public string IdPessoaSender { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Data de envio")]
        public DateTime? DataEnvio { get; set; }

        public bool Lida { get; set; } = false;

        public bool Arquivada_Receiver { get; set; } = false;
        public bool Arquivada_Sender { get; set; } = false;


        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(500)]
        [Display(Name = "Conteúdo")]
        public string Conteudo { get; set; }

        [ForeignKey(nameof(IdPessoaReceiver))]
        [InverseProperty(nameof(Pessoa.MensagensEntrada))]
        [Display(Name = "Id de navegação")]
        public virtual Pessoa IdPessoaReceiverNavigation { get; set; }

        [ForeignKey(nameof(IdPessoaSender))]
        [InverseProperty(nameof(Pessoa.MensagensSaida))]
        public virtual Pessoa IdPessoaSenderNavigation { get; set; }

        public void ArquivarMensagem_Sender()
        {
            Arquivada_Sender = true;
        }
        public void ArquivarMensagem_Receiver()
        {
            Arquivada_Receiver = true;
        }
    }
}
