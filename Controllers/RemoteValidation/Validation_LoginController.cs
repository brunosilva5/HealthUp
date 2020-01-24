using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Helpers;
using HealthUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var Pessoa = _context.Pessoas.Include(p=>p.Socio).Include(p=>p.Professor).SingleOrDefault(p => p.Username == Username);
            if (Pessoa == null)
            {
                return Json(new string("Este username não existe!"));
            }

            // verificar se não é um professor suspenso!
            if (Pessoa.Professor!=null)
            {
                if (Pessoa.Professor.DataSuspensao!=null && Pessoa.Professor.Motivo!=null)
                {
                    return Json("Esta conta encontra-se suspensa pelo seguinte motivo:" + Environment.NewLine + Pessoa.Professor.Motivo);
                }
            }
            if (Pessoa.Socio!=null)
            {
                // verificar se o socio encontra-se suspenso!
                if (Pessoa.Socio.DataSuspensao != null && Pessoa.Socio.Motivo != null)
                {
                    return Json("Esta conta encontra-se suspensa pelo seguinte motivo:" + Environment.NewLine + Pessoa.Socio.Motivo);
                }

                // agora verificamos se tem as cotas pagas
                DateTime dataRegisto = DateTime.Parse(Pessoa.Socio.DataRegisto.ToString());
                var nMeses = Math.Abs((DateTime.Now.Month - dataRegisto.Month) + 12 * (DateTime.Now.Year - dataRegisto.Year)) + 1;
                int cotasPagas = _context.Cota.Where(x => x.NumSocio == Pessoa.Socio.NumCC.ToString()).Count();
                int cotasNaoPagas = nMeses - cotasPagas;

                if (cotasNaoPagas > 0)
                {
                    return Json(new string("Tem de pagar as cotas em atraso para poder efetuar login!"));
                }
            }
            
            // se chegar aqui esta tudo bem!
            return Json(true);
            
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