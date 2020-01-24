using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class PedidoSocio
    {
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Número de cartão de cidadão")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Tem de possuir 8 caracteres!")]
        [Remote("IsValidNumCC", "Validation_Register", HttpMethod = "POST", ErrorMessage = "Número de cartão de cidadão inválido!")]
        public string NumCC { get; set; }

        [Display(Name = "Número de administrador")]
        public string NumAdmin { get; set; }
        
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(30)]
        public string Nome { get; set; }
        
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(3)]
        public string Sexo { get; set; }
        
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(100)]
        public string Fotografia { get; set; }
        
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [Display(Name = "Data de nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }
        
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(50)]
        [Remote("IsValidEmail", "Validation_Register", HttpMethod = "POST", ErrorMessage = "Insira um email válido!")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [Display(Name = "Número de telemóvel")]
        [Remote("IsValidPhoneNumber", "Validation_Register", HttpMethod = "POST", ErrorMessage = "Insira um número de telemóvel válido!")]
        public string Telemovel { get; set; }
        
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(20)]
        public string Nacionalidade { get; set; }
        
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(20)]
        [Remote("IsValidUsername", "Validation_Register", HttpMethod = "POST", ErrorMessage = "Insira um username válido!")]
        public string Username { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.PedidosSocio))]
        [Display(Name = "Número de navegação do administrador")]
        public virtual Admin NumAdminNavigation { get; set; }
    }
}