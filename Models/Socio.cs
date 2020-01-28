using HealthUp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HealthUp.Models
{
    public partial class Socio
    {
        public Socio()
        {
            Inscreve = new HashSet<Inscreve>();
            PlanoTreino = new HashSet<PlanoTreino>();

        }
        public Socio(Pessoa p) : base()
        {
            NumCC = p.NumCC;
            Cotas = new Cota(NumCC);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Tem de possuir 8 caracteres!")]
        [Remote("IsValidNumCC", "Validation", HttpMethod = "POST", ErrorMessage = "Número de cartão de cidadão inválido!")]
        [Display(Name = "Número de cartão de cidadão")]
        public string NumCC { get; set; }

        [Display(Name = "Número do administrador")]
        public string NumAdmin { get; set; }

        [Display(Name = "Id de solicitação")]
        public int? ID_Solicitacao { get; set; }

        [StringLength(3)]
        public string Altura { get; set; }

        [Range(0, 300, ErrorMessage = "Insira um valor entre 0 e 300")]
        public double? Peso { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de registo do peso")]
        public DateTime? DataRegisto_Peso { get; set; } = null;

        [StringLength(200)]
        public string Motivo { get; set; }



        [DataType(DataType.Date)]
        [Display(Name = "Data de suspensão")]
        public DateTime? DataSuspensao { get; set; }


        [ForeignKey(nameof(ID_Solicitacao))]
        [InverseProperty(nameof(SolicitacaoProfessor.Socio))]
        [Display(Name = "Id de navegação da solicitação")]
        public virtual SolicitacaoProfessor IdSolicitacaoNavigation { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.SociosSuspensos))]
        [Display(Name = "Número de navegação do administrador")]
        public virtual Admin NumAdminNavigation { get; set; }

        [ForeignKey(nameof(NumProfessor))]
        [InverseProperty(nameof(Professor.Socio))]
        [Display(Name = "Número de navegação do professor")]
        public virtual Professor NumProfessorNavigation { get; set; }

        [Display(Name = "Número do professor")]
        public string NumProfessor { get; set; }


        [ForeignKey(nameof(NumCC))]
        [InverseProperty(nameof(Pessoa.Socio))]
        [Display(Name = "Número de navegação do sócio")]
        public virtual Pessoa NumSocioNavigation { get; set; }

        [InverseProperty("NumSocioNavigation")]
        public virtual ICollection<Inscreve> Inscreve { get; set; }

        [InverseProperty("NumSocioNavigation")]
        public virtual ICollection<PlanoTreino> PlanoTreino { get; set; }

        [InverseProperty("NumSocioNavigation")]
        public virtual Cota Cotas { get; set; }

        public List<SimpleReportViewModel> GetHistoricoPeso(HealthUpContext context)
        {
            List<SimpleReportViewModel> ListaPesos = new List<SimpleReportViewModel>();
            foreach (Professor professor in context.Professores.Include(x => x.Socio))
            {
                List<SimpleReportViewModel> aux = professor.GetRegistoPesosSocio(NumCC);
                foreach (SimpleReportViewModel item in aux)
                {
                    ListaPesos.Add(item);
                }
            }
            // Ainda nao foi pesado por nenhum professor, adicionar o peso inserido por ele inicialmente
            // este peso inicial vai ser perdido após o registo de algum professor...
            if (ListaPesos.Count == 0)
            {
                ListaPesos.Add(new SimpleReportViewModel() { DimensionOne = DataRegisto_Peso.GetValueOrDefault().ToShortDateString(), Quantity = Peso.GetValueOrDefault() });
            }

            return ListaPesos;
        }

        public void TornarPlanosInativos()
        {
            foreach (PlanoTreino item in PlanoTreino)
            {
                item.Ativo = false;
            }
        }

        public void DeleteEntities(HealthUpContext context)
        {

            // apagar Lista inscricoes
            context.Inscricoes.RemoveRange(Inscreve);

            // apagar planos treino
            context.PlanosTreino.RemoveRange(PlanoTreino);

            // apagar cota
            Cota cota = context.Cota.SingleOrDefault(p => p.NumSocio == NumCC);
            context.Cota.Remove(cota);

            context.SaveChanges();

        }
    }
}
