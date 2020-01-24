using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Helpers;
using HealthUp.Models;
using Microsoft.AspNetCore.Authorization;
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


            if (ModelState.IsValid)
            {
                PedidoSocio p = new PedidoSocio()
                {
                    DataNascimento = DateTime.Parse(data["DataNascimento"]),
                    Email = data["Email"],
                    Fotografia = data["Fotografia"],
                    Nacionalidade = data["Nacionalidade"],
                    Nome = nome,
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
        [NaoAutenticado]
        public IActionResult Login()
        {
            return View();
        }

        [NaoAutenticado]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(IFormCollection data)
        {
            string Password = data["Password"];
            string Username = data["Username"];

            Pessoa p = _context.Pessoas.Include(p => p.Admin).Include(p => p.Professor).Include(p => p.Socio).SingleOrDefault(p => p.Username == Username);

            if (ModelState.IsValid)
            {
                // Definir a password (primeiro login)
                if (p.Password==null)
                {
                    p.Password = Password;
                    _context.Pessoas.Update(p);
                    _context.SaveChanges();
                }

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
            return View();
        }
        public IActionResult PartialPlanoSemanal(string week)
        {
            
            DateTime data =HelperFunctions.GetData(week);
            DateTime segunda = HelperFunctions.GetMonday(data);
            DateTime domingo = HelperFunctions.Next(data, DayOfWeek.Monday);
            ViewBag.Segunda = segunda.ToShortDateString();
            ViewBag.Domingo = domingo.ToShortDateString();
            List<Aula> lista = new List<Aula>();
            foreach (var aula in _context.Aulas)
            {
                if (aula.VerificarValidade(segunda, domingo))
                {
                    lista.Add(aula);
                }
            }

            
            return PartialView(nameof(PartialPlanoSemanal),lista);
        }


        #endregion

        #region Completar Perfil
        [Autenticado]
        public IActionResult CompletarPerfil()
        {
            return View();
        }

        [Autenticado]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompletarPerfil(IFormCollection dados)
        {
            if (ModelState.IsValid)
            {
                if (HelperFunctions.IsCurrentUserProfessor(HttpContext))
                {
                    var prof = _context.Professores.SingleOrDefault(p => p.NumCC == HttpContext.Session.GetString("UserId"));
                    prof.Especialidade = dados["Professor.Especialidade"].ToString();
                    _context.Professores.Update(prof);
                    _context.SaveChanges();
                }

                if (HelperFunctions.IsCurrentUserSocio(HttpContext))
                {
                    var socio = _context.Socios.SingleOrDefault(s => s.NumCC == HttpContext.Session.GetString("UserId"));
                    socio.Peso = double.Parse(dados["Socio.Peso"].ToString(), CultureInfo.InvariantCulture);
                    socio.Altura = dados["Socio.Altura"].ToString();
                    _context.Socios.Update(socio);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        #endregion

        #region VerificarNovoUser
        [AjaxOnly]
        public JsonResult IsNewUser(string Username)
        {
            var pessoa = _context.Pessoas.SingleOrDefault(p => p.Username == Username);

            // your logic
            if (pessoa != null && pessoa.Password == null)
            {
                return Json(true);

            }
            return Json(false);
        }
        #endregion
    }


}