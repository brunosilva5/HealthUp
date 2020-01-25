using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class PlanoTreino
    {
        public PlanoTreino()
        {
            Contem = new HashSet<Contem>();
        }

        [Key]
        [Display(Name = "Id do plano de treino")]
        public int IdPlano { get; set; }

        [Display(Name = "Número do sócio")]
        public string NumSocio { get; set; }

        [Display(Name = "Número do professor")]
        public string NumProfessor { get; set; }
        
        public bool Ativo { get; set; }
        
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(500)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [ForeignKey(nameof(NumProfessor))]
        [InverseProperty(nameof(Professor.PlanoTreino))]
        [Display(Name = "Número de navegação do professor")]
        public virtual Professor NumProfessorNavigation { get; set; }
        
        [ForeignKey(nameof(NumSocio))]
        [InverseProperty(nameof(Socio.PlanoTreino))]
        [Display(Name = "Número de navegação do sócio")]
        public virtual Socio NumSocioNavigation { get; set; }
        
        [InverseProperty("IdPlanoNavigation")]
        public virtual ICollection<Contem> Contem { get; set; }
    }
}

