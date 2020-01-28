using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthUp.Models
{
    public partial class Exercicio
    {
        public Exercicio()
        {
            Contem = new HashSet<Contem>();
        }

        [Key]
        [Display(Name = "Id do exercício")]
        public int IdExercicio { get; set; }

        [Display(Name = "Número do administrador")]
        public string NumAdmin { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(50)]
        [Remote("DoesExerciceExist", "Validation_Register", HttpMethod = "POST", ErrorMessage = "Esta imagem não é válida!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(500)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(100)]
        [Remote("IsValidVideo", "Validation_Register", HttpMethod = "POST", ErrorMessage = "Este vídeo não é válido!")]
        [Display(Name = "Vídeo")]
        public string Video { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(100)]
        [Remote("IsJpg", "Validation_Register", HttpMethod = "POST", ErrorMessage = "Esta imagem não é válida!")]
        public string Fotografia { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.Exercicio))]
        [Display(Name = "Número de navegação do administrador")]
        public virtual Admin NumAdminNavigation { get; set; }

        [InverseProperty("IdExercicioNavigation")]
        public virtual ICollection<Contem> Contem { get; set; }
    }
}

