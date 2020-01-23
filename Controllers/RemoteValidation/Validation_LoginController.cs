using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Helpers;
using HealthUp.Models;
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
#warning nao consigo por a mensagem a dizer "paga as cotas"
            var Pessoa = _context.Pessoas.SingleOrDefault(p => p.Username == Username);
            //verificar se tem as cotas pagas
            Socio s = _context.Socios.FirstOrDefault(x => x.NumCC == Pessoa.NumCC);
            if(s!=null)
            {
                string registo = s.DataRegisto.ToString();
                DateTime dataRegisto = DateTime.Parse(registo);
                var nMeses = Math.Abs((DateTime.Now.Month - dataRegisto.Month) + 12 * (DateTime.Now.Year - dataRegisto.Year)) + 1;

                int cotasPagas = _context.Cota.Where(x => x.NumSocio == s.NumCC.ToString()).Count();
                int cotasNaoPagas = nMeses - cotasPagas;

                if (cotasNaoPagas > 0)
                {
                    return Json(false, new string("Tens de pagar as cotas em atraso para poderes efetuar login"));
                }
            }
            
            if (Pessoa == null)
            {
                return Json(false,new string("Este username não existe!"));
            }
            else
            {
                return Json(true);
            }
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