using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthUp.Models
{
    public partial class Ginasio
    {
        [Key]
        [Display(Name = "Id do ginásio")]
        public int Id { get; set; }

        [Display(Name = "Número do administrador")]
        public string NumAdmin { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(30)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(200)]
        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [Display(Name = "Hora de abertura")]
        public TimeSpan Hora_Abertura { get; set; }
        [Display(Name = "Hora de fecho")]
        public TimeSpan Hora_Fecho { get; set; }


        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(13)]
        [Display(Name = "Telemóvel")]
        [Remote("IsValidPhoneNumber", "Validation_Register", HttpMethod = "POST", ErrorMessage = "Insira um número de telemóvel válido!")]
        public string Telemovel { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(200)]
        [Remote("IsValidCoordinates", "Validation_Register", HttpMethod = "POST", ErrorMessage = "Insira as coordernadas válidas!")]
        [Display(Name = "Coordenadas GPS")]
        public string LocalizacaoGps { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.Ginasio))]
        [Display(Name = "Número de navegação do administrador")]
        public virtual Admin NumAdminNavigation { get; set; }
    }
}
