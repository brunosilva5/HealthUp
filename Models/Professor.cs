using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class Professor
    {
        public Professor()
        {
            Aula = new HashSet<Aula>();
            PlanoTreino = new HashSet<PlanoTreino>();
            Socio = new HashSet<Socio>();
        }
        public Professor(Pessoa p)
        {
            Aula = new HashSet<Aula>();
            PlanoTreino = new HashSet<PlanoTreino>();
            Socio = new HashSet<Socio>();

            NumCC = p.NumCC;
            NumProfessorNavigation = p;

        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Tem de possuir 8 caracteres!")]
        [Remote("IsValidNumCC", "Validation", HttpMethod = "POST", ErrorMessage = "Número de cartão de cidadão inválido!")]
        [Display(Name = "Número de cartão de cidadão")]
        public string NumCC { get; set; }
        public string NumAdmin { get; set; }
        public int? IdSolicitacao { get; set; }
        [StringLength(200)]
        public string Motivo { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DataSuspensao { get; set; }
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(30)]
        public string Especialidade { get; set; }

        [ForeignKey(nameof(IdSolicitacao))]
        [InverseProperty(nameof(SolicitacaoProfessor.Professor))]
        public virtual SolicitacaoProfessor IdSolicitacaoNavigation { get; set; }
        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.ProfessoresSuspensos))]
        public virtual Admin NumAdminNavigation { get; set; }

        // -----------------------------------------------------------------------------------------
        // REFERENCIA A PESSOA
        [ForeignKey(nameof(NumCC))]
        [InverseProperty(nameof(Pessoa.Professor))]
        public virtual Pessoa NumProfessorNavigation { get; set; }
        // -----------------------------------------------------------------------------------------

        [InverseProperty("NumProfessorNavigation")]
        public virtual ICollection<Aula>? Aula { get; set; }

        [InverseProperty("NumProfessorNavigation")]
        public virtual ICollection<PlanoTreino> PlanoTreino { get; set; }

        [InverseProperty("NumProfessorNavigation")]
        public virtual ICollection<Socio> Socio { get; set; }
    }
}
