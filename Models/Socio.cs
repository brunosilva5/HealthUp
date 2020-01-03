using HealthUp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class Socio
    {
        public Socio()
        {
            Inscreve = new HashSet<Inscreve>();
            PlanoTreino = new HashSet<PlanoTreino>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Tem de possuir 8 caracteres!")]
        [Remote("IsValidNumCC", "Validation", HttpMethod = "POST", ErrorMessage = "Número de cartão de cidadão inválido!")]
        [Display(Name = "Número de cartão de cidadão")]
        public string NumCC { get; set; }

        public string NumAdmin { get; set; }
        public int? ID_Solicitacao { get; set; }
        [StringLength(3)]
        public string Altura { get; set; }
        [Range(0, 300,ErrorMessage ="Insira um valor entre 0 e 300")]
        public double Peso { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataRegisto_Peso { get; set; }
        [StringLength(200)]
        public string Motivo { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DataSuspensao { get; set; }


        [ForeignKey(nameof(ID_Solicitacao))]
        [InverseProperty(nameof(SolicitacaoProfessor.Socio))]
        public virtual SolicitacaoProfessor IdSolicitacaoNavigation { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.SociosSuspensos))]
        public virtual Admin NumAdminNavigation { get; set; }

        //IDENTIFICAR QUEM PESOU
        [ForeignKey(nameof(NumProfessor))]
        [InverseProperty(nameof(Professor.Socio))]
        public virtual Professor NumProfessorNavigation { get; set; }
        public string NumProfessor { get; set; }


        [ForeignKey(nameof(NumCC))]
        [InverseProperty(nameof(Pessoa.Socio))]
        public virtual Pessoa NumSocioNavigation { get; set; }

        [InverseProperty("NumSocioNavigation")]
        public virtual ICollection<Inscreve> Inscreve { get; set; }

        [InverseProperty("NumSocioNavigation")]
        public virtual ICollection<PlanoTreino> PlanoTreino { get; set; }

        public List<SimpleReportViewModel> GetHistoricoPeso(HealthUpContext context)
        {
            List<SimpleReportViewModel> ListaPesos = new List<SimpleReportViewModel>();
            foreach (var professor in context.Professores.Include(x => x.Socio))
            {
                var aux=professor.GetRegistoPesosSocio(this.NumCC);
                foreach (var item in aux)
                {
                    ListaPesos.Add(item);
                }
            }
            return ListaPesos;
        }
    }
}
