using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class Aula
    {
        public Aula()
        {
            Inscreve = new HashSet<Inscreve>();
        }

        [Key]
        public int IdAula { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Número de professor")]
        public string NumProfessor { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Número de administrador")]
        public string NumAdmin { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [DataType(DataType.Date)]
        [Display(Name = "Válido de")]
        public DateTime ValidoDe { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Válido até")]
        public DateTime ValidoAte { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Lotação")]
        public int Lotacao { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Hora de início")]
        public TimeSpan HoraInicio { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [Display(Name = "Dia da semana")]
        public int DiaSemana { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.Aula))]
        [Display(Name = "Número de navegação do administrador")]
        public virtual Admin NumAdminNavigation { get; set; }

        [ForeignKey(nameof(NumProfessor))]
        [InverseProperty(nameof(Professor.Aula))]
        [Display(Name = "Número de navegação do professor")]
        public virtual Professor NumProfessorNavigation { get; set; }

        [ForeignKey(nameof(IdAula))]
        [InverseProperty(nameof(AulaGrupo.Aula))]
        [Display(Name = "Número de navegação da aula de grupo")]
        public virtual AulaGrupo AulaGrupoNavigation { get; set; }

        [InverseProperty("IdAulaNavigation")]
        public virtual ICollection<Inscreve> Inscreve { get; set; }

        public string GetDiaSemana()
        {
            switch (DiaSemana)
            {
                case 1: return "Domingo";
                case 2: return "Segunda-Feira";
                case 3: return "Terça-Feira";
                case 4: return "Quarta-Feira";
                case 5: return "Quinta-Feira";
                case 6: return "Sexta-Feira";
                case 7: return "Sábado";
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}