using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Helpers;
using HealthUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthUp.Controllers
{
    public class UtilizadoresController : Controller
    {
        private readonly HealthUpContext _context;
        public UtilizadoresController(HealthUpContext contexto)
        {
            _context = contexto;
            


        }
        #region PedidoSocio
        public IActionResult PedidoSocio()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PedidoSocio(IFormCollection data)
        {
            var indicativo = data["Indicativo"];
            var telemovel = HelperFunctions.NormalizeWhiteSpace(data["Telemovel"]);
            var nome = HelperFunctions.NormalizeWhiteSpace(data["Nome"]);


            //if (!HelperFunctions.HelperFunctions.IsJustNumbers(telemovel))
            //{
            //    ModelState.AddModelError("Telemovel", "O campo telemóvel não está correto");
            //}

            if (ModelState.IsValid)
            {
                PedidoSocio p = new PedidoSocio()
                {
                    DataNascimento = DateTime.Parse(data["DataNascimento"]),
                    Email = data["Email"],
                    Fotografia = data["Fotografia"],
                    Nacionalidade = data["Nacionalidade"],
                    Nome = data["Nome"],
                    Sexo = data["sexo"],
                    Username = data["Username"],
                    Telemovel = new string("+" + indicativo + telemovel),
                    NumCC = data["NumCC"]

                };
                _context.PedidosSocios.Add(p);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        #endregion

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(IFormCollection data)
        {
            string Password = data["Password"];
            string Username = data["Username"];

            if (String.IsNullOrWhiteSpace(Password) || String.IsNullOrWhiteSpace(Username))
            {
                ModelState.AddModelError("", "Por favor preencha todos os campos!");
            }
           
            Pessoa p = _context.Pessoas.Include(p => p.Admin).Include(p => p.Professor).Include(p => p.Socio).SingleOrDefault(p => p.Username == Username);

            
           

            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("Nome", p.Nome);
                HttpContext.Session.SetString("UserId", p.NumCC);

                if (p.Admin != null)
                {
                    HttpContext.Session.SetString("Role", "Admin");
                }

                if (p.Socio != null)
                {
                    HttpContext.Session.SetString("Role", "Socio");
                }
                if (p.Professor != null)
                {
                    HttpContext.Session.SetString("Role", "Professor");
                }
                return LocalRedirect("/");
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region Logout
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("CookieSessao");
            return LocalRedirect("/");
        }
        #endregion

        #region PlanoSemanal

        public IActionResult PlanoSemanal()
        {
            DateTime agora = DateTime.Now;
            DateTime segunda = HelperFunctions.GetMonday(agora);
            DateTime domingo = HelperFunctions.Next(agora, DayOfWeek.Monday);

            ViewBag.Segunda = segunda.ToShortDateString();
            ViewBag.Domingo = domingo.ToShortDateString();
            var lista = _context.Aulas.Where(x => x.ValidoAte >= domingo && x.ValidoDe <= segunda);//limitar a uma certa semana
            lista = lista.Include(x => x.AulaGrupo).Where(x => x.AulaGrupo.IdAula == x.IdAula);// limitar às aulas de grupo


            return View(lista);
        }


        #endregion

    }
}