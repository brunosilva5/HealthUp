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
        [Range(1, 24, ErrorMessage = "A hora de abertura tem que ser entre a 1h e as 24h")]
        public int Hora_Abertura { get; set; }
        [Range(1, 24, ErrorMessage = "A hora de fecho tem que ser entre a 1h e as 24h")]
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        public int Hora_Fecho { get; set; }


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

        //[ForeignKey(nameof(NumAdmin))]
        //[InverseProperty(nameof(Admin.Ginasio))]
        [Display(Name = "Número de navegação do administrador")]
        public virtual Admin NumAdminNavigation { get; set; }
    }
}
