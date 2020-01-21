using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class Contem
    {
        
        [Key]
        [Display(Name = "Id do plano de treino")]
        public int IdPlano { get; set; }

        [Display(Name = "Id do exercício")]
        public int IdExercicio { get; set; }
        
        [Display(Name ="Número de repetições")]
        [Range(0, Int32.MaxValue)]
        public int NumRepeticoes { get; set; }
        
        [Display(Name = "Período de descanso")]
        [Range(0, Int32.MaxValue)]
        public int PeriodoDescanso { get; set; }
        
        [Display(Name = "Quantidade de séries")]
        [Range(0,Int32.MaxValue)]
        public int QuantidadeSeries { get; set; }

        [ForeignKey(nameof(IdExercicio))]
        [InverseProperty(nameof(Exercicio.Contem))]
        [Display(Name = "Id de navegação do exercício")]
        public virtual Exercicio IdExercicioNavigation { get; set; }
        
        [ForeignKey(nameof(IdPlano))]
        [InverseProperty(nameof(PlanoTreino.Contem))]
        [Display(Name = "Id de navegação do plano de treino")]
        public virtual PlanoTreino IdPlanoNavigation { get; set; }


        
    }
}
