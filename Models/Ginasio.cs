using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class Ginasio
    {
        [Key]
        public int Id { get; set; }
        public string NumAdmin { get; set; }
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(30)]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(200)]
        [Display(Name ="Endereço")]
        public string Endereco { get; set; }
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(13)]
        [Display(Name ="Telemóvel")]
        [Remote("IsValidPhoneNumber", "Validation_Register", HttpMethod = "POST", ErrorMessage = "Insira um número de telemóvel válido!")]
        public string Telemovel { get; set; }
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(200)]
        [Display(Name = "Coordenadas GPS")]
        public string LocalizacaoGps { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.Ginasio))]
        public virtual Admin NumAdminNavigation { get; set; }
    }
}
