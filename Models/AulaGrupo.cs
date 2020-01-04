using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class AulaGrupo
    {
        [Key]
        public int IdAula { get; set; }
        [Required]
        [StringLength(30)]
        [Display(Name = "Aula de Grupo")]
        public string Nome { get; set; }
        [Required]
        [StringLength(100)]
        [Remote("IsValidFotografiaDivulgacao", "Validation_Files", HttpMethod = "POST", ErrorMessage = "A fotografia tem de ser no formato .jpg")]
        [Display(Name = "Fotografia")]
        public string FotografiaDivulgacao { get; set; }
        [Required]
        [StringLength(100)]
        [Remote("IsValidVideoDivulgacao", "Validation_Files", HttpMethod = "POST", ErrorMessage = "O video tem de ser no formato .mp4")]
        [Display(Name = "Video")]
        public string VideoDivulgacao { get; set; }
        [Required]
        [StringLength(500)]
        public string Descricao { get; set; }

        // 


        [InverseProperty("AulaGrupoNavigation")]
        public virtual Aula Aula { get; set; }
    }
}
