using HealthUp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace HealthUp.Models
{
    public partial class Admin
    {
        public Admin()
        {
            Aula = new HashSet<Aula>();
            Exercicio = new HashSet<Exercicio>();
            Ginasio = new HashSet<Ginasio>();
            PedidosSocio = new HashSet<PedidoSocio>();
            ProfessoresSuspensos = new HashSet<Professor>();
            SociosSuspensos = new HashSet<Socio>();
            SolicitacaoProfessor = new HashSet<SolicitacaoProfessor>();
        }
        public Admin(Pessoa p) : base ()
        {
            NumCC = p.NumCC;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Tem de possuir 8 caracteres!")]
        [Remote("IsValidNumCC", "Validation", HttpMethod = "POST", ErrorMessage = "Número de cartão de cidadão inválido!")]
        [Display(Name = "Número de cartão de cidadão")]
        public string NumCC { get; set; }

        //----------------------------------------------------------------------------------------
        // REFERENCIA A PESSOA
        [ForeignKey(nameof(NumCC))]
        [InverseProperty(nameof(Pessoa.Admin))]
        [Display(Name = "Número de navegação do administrador")]
        public virtual Pessoa NumAdminNavigation { get; set; }

        //----------------------------------------------------------------------------------------
        [InverseProperty("NumAdminNavigation")]
        public virtual ICollection<Aula> Aula { get; set; }
        
        [InverseProperty("NumAdminNavigation")]
        [Display(Name = "Exercício")]
        public virtual ICollection<Exercicio> Exercicio { get; set; }
        
        [InverseProperty("NumAdminNavigation")]
        [Display(Name = "Ginásio")]
        public virtual ICollection<Ginasio> Ginasio { get; set; }
        
        [InverseProperty("NumAdminNavigation")]
        [Display(Name = "Pedidos de sócio")]
        public virtual ICollection<PedidoSocio> PedidosSocio { get; set; }
        
        [InverseProperty("NumAdminNavigation")]
        [Display(Name = "Professores suspensos")]
        public virtual ICollection<Professor> ProfessoresSuspensos { get; set; }
        
        [InverseProperty("NumAdminNavigation")]
        [Display(Name = "Sócios suspensos")]
        public virtual ICollection<Socio> SociosSuspensos { get; set; }
        
        [InverseProperty("NumAdminNavigation")]
        [Display(Name = "Solicitação de professor")]
        public virtual ICollection<SolicitacaoProfessor> SolicitacaoProfessor { get; set; }

        public void DeleteEntities(HealthUpContext context)
        {

            // apagar Lista inscricoes
            context.SolicitacaoProfessores.RemoveRange(SolicitacaoProfessor);

            // apagar planos treino
            context.PedidosSocios.RemoveRange(PedidosSocio);

            context.Exercicios.RemoveRange(Exercicio);

            context.Aulas.RemoveRange(Aula);

            context.SaveChanges();

        }
    }
}
