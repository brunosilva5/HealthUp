using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthUp.Data;
using HealthUp.Models;
using Microsoft.AspNetCore.Http;
using HealthUp.Filters;

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil = "Professor")]
    public class ProfessoresController : Controller
    {
        private readonly HealthUpContext _context;

        public ProfessoresController(HealthUpContext context)
        {
            _context = context;
        }

        // GET: Professores
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegistarPesoSocio()
        {
            List<Socio> Lista = _context.Socios.Include(s => s.NumSocioNavigation).Where(s => s.DataSuspensao == null && s.Motivo == null).ToList();

            ViewBag.Socios = Lista.Select(s => new SelectListItem()
            {
                Text = "CC: " + s.NumCC + " | "  + s.NumSocioNavigation.Nome,
                Value = s.NumCC
            });


            return View();
        }

        // POST: Exercicios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistarPesoSocio(string SocioEscolhido, float Peso)
        {
            var socio = _context.Socios.SingleOrDefault(s => s.NumCC == SocioEscolhido);
            

            if (ModelState.IsValid)
            {
                socio.Peso = Peso;
                socio.DataRegisto_Peso = DateTime.Now.Date;
                socio.NumProfessor = HttpContext.Session.GetString("UserId");

                var newsocio = socio;
                _context.Socios.Update(socio);
                

                //--------------------------------------------------------------------------------------------------------------------------------------
                // Adicionar na tabela de Socios do professor
                var professor = _context.Professores.Include(p => p.Socio).SingleOrDefault(p => p.NumCC == HttpContext.Session.GetString("UserId"));
                newsocio.NumProfessor = null;
                professor.Socio.Append(new Socio()
                {
                    NumCC=socio.NumCC,
                    DataRegisto_Peso=socio.DataRegisto_Peso,
                    Peso=socio.Peso,
                });
                _context.Professores.Update(professor);
                // --------------------------------------------------------------------------------------------------------------------------------------
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            List<Socio> Lista = _context.Socios.Include(s => s.NumSocioNavigation).Where(s => s.DataSuspensao == null && s.Motivo == null).ToList();

            ViewBag.Socios = Lista.Select(s => new SelectListItem()
            {
                Text = "CC: " + s.NumCC + " | " + s.NumSocioNavigation.Nome,
                Value = s.NumCC
            });

            return View(socio);
        }
    }
}
