using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class Pessoa
    {
        public Pessoa()
        {
            Mensagem = new HashSet<Mensagem>();
        }
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Tem de possuir 8 caracteres!")]
        [Display(Name = "Número de cartão de cidadão")]
        public string NumCC { get; set; }
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


        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Data de nascimento")]
        [DataType(DataType.Date)]
        public DateTime DataNascimento { get; set; }


        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [MinLength(5, ErrorMessage = "O telemóvel tem um número de carateres entre 5 e 13")]
        [MaxLength(13, ErrorMessage = "O telemóvel tem um número de carateres entre 5 e 13")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Número de telemóvel")]
        public string Telemovel { get; set; }


        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(20)]
        public string Nacionalidade { get; set; }


        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(20)]
        //[Remote("IsValidUserName", "Validation_Login", HttpMethod = "POST", ErrorMessage = "Este username não é válido!")]
        public string Username { get; set; }
        public bool IsNotified { get; set; } = false; // variavel auxiliar para a resposta do isvalid

        [StringLength(100)]
        [Remote("IsValidPassword", "Validation_Login", HttpMethod = "POST", ErrorMessage = "Esta Password não é válida!", AdditionalFields = "Username")]
        public string Password { get; set; }


       
        //----------------------------------------------------------------------------------------
        // REFERENCIA A ADMIN
        [InverseProperty("NumAdminNavigation")]
        public Admin Admin { get; set; }
        //----------------------------------------------------------------------------------------

        [InverseProperty("NumProfessorNavigation")]
        public virtual Professor Professor { get; set; }
        [InverseProperty("NumSocioNavigation")]
        public virtual Socio Socio { get; set; }
        [InverseProperty("IdNavigation")]
        public virtual ICollection<Mensagem> Mensagem { get; set; }

    }
}
