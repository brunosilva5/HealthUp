using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Helpers;
using HealthUp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthUp.Controllers
{
    [AllowAnonymous]
    public class UtilizadoresController : Controller
    {
        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

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
            var lista = _context.Aulas.Where(x => x.ValidoAte >= domingo && x.ValidoDe <= segunda);//limitar a uma certa semana
            

            
            return PartialView(nameof(PartialPlanoSemanal),lista.ToList());
        }


        #endregion

        public JsonResult IsNewUser(string Username)
        {
            var pessoa = _context.Pessoas.SingleOrDefault(p => p.Username == Username);

            // your logic
            if (pessoa != null && pessoa.Password == null)
            {
                return Json(true);

            }
            if (pessoa == null)
            {
                return Json(false);

            }
            if (pessoa != null && pessoa.Password != null)
            {
                return Json(false);
            }
            return Json(false);
        }

    }


}