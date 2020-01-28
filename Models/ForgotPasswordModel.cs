using HealthUp.Controllers;
using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HealthUp.Models
{
    [NotMapped]
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório")]
        [Remote("IsValidPassword1", "ForgotPassword", HttpMethod = "POST", ErrorMessage = "Esta password não é válida!", AdditionalFields = "IdPessoa")]
        public string Password1 { get; set; }

        [Remote("IsValidPassword2", "ForgotPassword", HttpMethod = "POST", ErrorMessage = "Esta password não é válida!", AdditionalFields = "IdPessoa")]
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório")]
        [Compare("Password1", ErrorMessage = "As Passwords não coincidem.")]
        public string Password2 { get; set; }

        [Required]
        public string IdPessoa { get; set; }

    }
    [AjaxOnly]
    public class ForgotPasswordController : BaseController
    {
        private readonly HealthUpContext _context;

        public ForgotPasswordController(HealthUpContext context)
        {
            _context = context;
        }
        public JsonResult IsValidPassword1(string Password1, string IdPessoa)
        {
            if (Password1 == null)
            {
                return Json(new string("Password inválida!"));
            }
            Pessoa Pessoa = _context.Pessoas.SingleOrDefault(p => p.NumCC == IdPessoa);

            if (Pessoa == null)
            {
                return Json(false);
            }
            if (SecurePasswordHasher.Verify(Password1, Pessoa.Password))
            {
                return Json(new string("A nova password não pode ser igual à sua password antiga!"));
            }
            if (HelperFunctions.IsValidPassword(Password1))
            {
                return Json(true);
            }
            else
            {
                return Json(new string("A Password tem de possuir 8 carateres, um carater maiúsculo, um mínusculo e um número!"));
            }
        }

        public JsonResult IsValidPassword2(string Password2, string IdPessoa)
        {
            if (Password2 == null)
            {
                return Json(new string("Password inválida!"));
            }
            Pessoa Pessoa = _context.Pessoas.SingleOrDefault(p => p.NumCC == IdPessoa);

            if (Pessoa == null)
            {
                return Json(false);
            }
            if (HelperFunctions.IsValidPassword(Password2))
            {
                return Json(true);
            }
            else
            {
                return Json(new string("A Password tem de possuir 8 carateres, um carater maiúsculo, um mínusculo e um número!"));
            }
        }


    }
}
