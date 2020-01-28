using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Helpers;
using HealthUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthUp.Controllers.RemoteValidation
{
    [AjaxOnly]
    public class Validation_LoginController : BaseController
    {
        private readonly HealthUpContext _context;
        public Validation_LoginController(HealthUpContext context)
        {
            _context = context;
        }
        
        public JsonResult IsValidUsername(string Username)
        {
            var Pessoa = _context.Pessoas.Include(p=>p.Socio).ThenInclude(s=>s.Cotas).Include(p=>p.Professor).SingleOrDefault(p => p.Username == Username);
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

                
                
                if (!Pessoa.Socio.Cotas.AreCotasPagas())
                {
                    return Json(new string("Tem de pagar as cotas em atraso para poder efetuar login! Meses em atraso: " + Pessoa.Socio.Cotas.NumeroCotasNaoPagas));
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

        public JsonResult IsValidEmail(string Email)
        {
            if (Email==null)
            {
                return Json(false);
            }
            var Pessoa = _context.Pessoas.SingleOrDefault(p => p.Email == Email);
            if (Pessoa==null)
            {
                return Json("Este email não está associado a nenhuma conta!");
            }
            else
            {
                return Json(true);
            }
        }
    }


}