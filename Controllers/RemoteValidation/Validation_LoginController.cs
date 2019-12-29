using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace HealthUp.Controllers.RemoteValidation
{
    public class Validation_LoginController : Controller
    {
        private readonly HealthUpContext _context;
        public Validation_LoginController(HealthUpContext context)
        {
            _context = context;
        }
        
        public JsonResult IsValidUsername(string Username)
        {
            var Pessoa = _context.Pessoas.SingleOrDefault(p => p.Username == Username);
            if (Pessoa==null)
            {
                return Json(new string("Este username não existe!"));
            }

            if (Pessoa.Password==null)
            {
                if (Pessoa.IsNotified == false)
                {
                    Pessoa.IsNotified = true;
                    _context.Pessoas.Update(Pessoa);
                    _context.SaveChanges();
                    return Json(new string("Este username ainda não tem uma password definida. Defina a password na caixa abaixo!"));
                }
               if (Pessoa.IsNotified == true)
                {
                    Pessoa.IsNotified = false;
                    _context.Pessoas.Update(Pessoa);
                    _context.SaveChanges();
                    return Json(true);
                }
                

            }
            else
            {
                return Json(true);
            }
            return Json(false);
        }
        public JsonResult IsValidPassword(string Password, string Username)
        {
            if (Password==null)
            {
                return Json(new string("Password incorrecta!"));
            }
            var Pessoa = _context.Pessoas.SingleOrDefault(p => p.Username == Username);
            
            if (Pessoa==null)
            {
                return Json(false);
            }
            if (Pessoa.Password==null)
            {
                if (HelperFunctions.IsValidPassword(Password))
                {
                    return Json(true);
                }
                else
                {
                    return Json(new string("A Password tem de possuir 8 carateres, um carater maiúsculo, um mínusculo e um número!"));
                }
            }
            if (SecurePasswordHasher.Verify(Password,Pessoa.Password))
            {
                return Json(true);
            }
            else
            {
                return Json(new string("A password está incorrecta!"));
            }
        }
    }

    
}