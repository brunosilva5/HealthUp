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
        public int IdPlano { get; set; }
        public int IdExercicio { get; set; }
        public int NumRepeticoes { get; set; }
        public int PeriodoDescanso { get; set; }
        public int QuantidadeSeries { get; set; }

        [ForeignKey(nameof(IdExercicio))]
        [InverseProperty(nameof(Exercicio.Contem))]
        public virtual Exercicio IdExercicioNavigation { get; set; }
        [ForeignKey(nameof(IdPlano))]
        [InverseProperty(nameof(PlanoTreino.Contem))]
        public virtual PlanoTreino IdPlanoNavigation { get; set; }


        
    }
}
