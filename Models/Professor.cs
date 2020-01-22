using HealthUp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
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

        [Display(Name = "Número do administrador")]
        public string NumAdmin { get; set; }

        [Display(Name = "Id de solicitação")]
        public int? IdSolicitacao { get; set; }
        
        [StringLength(200)]
        public string Motivo { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Data de suspensão")]
        public DateTime? DataSuspensao { get; set; }
        
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(30)]
        public string Especialidade { get; set; }

        [ForeignKey(nameof(IdSolicitacao))]
        [InverseProperty(nameof(SolicitacaoProfessor.Professor))]
        [Display(Name = "Id de navegação da solicitação")]
        public virtual SolicitacaoProfessor IdSolicitacaoNavigation { get; set; }
        
        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.ProfessoresSuspensos))]
        [Display(Name = "Número de navegação do administrador")]
        public virtual Admin NumAdminNavigation { get; set; }

        // -----------------------------------------------------------------------------------------
        // REFERENCIA A PESSOA
        [ForeignKey(nameof(NumCC))]
        [InverseProperty(nameof(Pessoa.Professor))]
        [Display(Name = "Número de navegação do professor")]
        public virtual Pessoa NumProfessorNavigation { get; set; }
        // -----------------------------------------------------------------------------------------

        [InverseProperty("NumProfessorNavigation")]
        public virtual ICollection<Aula> Aula { get; set; }

        [InverseProperty("NumProfessorNavigation")]
        public virtual ICollection<PlanoTreino> PlanoTreino { get; set; }

        // Lista de alunos
        [InverseProperty("NumProfessorNavigation")]
        public virtual ICollection<Socio> Socio { get; set; }

        // string json
        [Display(Name = "Registo dos pesos")]
        public string RegistoPesos { get; set; }

        public List<SimpleReportViewModel> GetRegistoPesosSocio(string idSocio)
        {
            List<SimpleReportViewModel> ListaPesagens = new List<SimpleReportViewModel>();
            if (ListaPesagens.Count==0)
            {
                return ListaPesagens;
            }
            var RegistosSocio = HelperFunctions.JSONDeserialize(RegistoPesos);

            string data=RegistosSocio.Get(idSocio);
            string[] splittedData = data.Split(',');
            foreach (var item in splittedData)
            {
                var subitem=item.Split("-");
                ListaPesagens.Add(new SimpleReportViewModel() { Quantity = float.Parse(subitem[0], CultureInfo.InvariantCulture.NumberFormat), DimensionOne = subitem[1] });
            }

            return ListaPesagens;
        }

        public void RegistarPesoSocio(float peso, string Data, string idSocio)
        {
            var RegistosSocios = HelperFunctions.JSONDeserialize(RegistoPesos);
            RegistosSocios.Add(idSocio, peso.ToString()+"-"+Data);
            RegistoPesos = HelperFunctions.JSONSerialize(RegistosSocios);
        }
    }
}
