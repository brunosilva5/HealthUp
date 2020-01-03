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
        [Required(ErrorMessage ="Este campo é obrigatório")]
        public string NumProfessor { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string NumAdmin { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [DataType(DataType.Date)]
        public DateTime? ValidoDe { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ValidoAte { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int Lotacao { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public TimeSpan? HoraInicio { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int DiaSemana { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.Aula))]
        public virtual Admin NumAdminNavigation { get; set; }
        
        [ForeignKey(nameof(NumProfessor))]
        [InverseProperty(nameof(Professor.Aula))]
        public virtual Professor NumProfessorNavigation { get; set; }

        [InverseProperty("AulaNavigation")]
        public virtual AulaGrupo AulaGrupo { get; set; }


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