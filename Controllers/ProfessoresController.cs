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
using Newtonsoft.Json;
using System.Collections.Specialized;
using HealthUp.Helpers;

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil = "Professor")]
    public class ProfessoresController : Controller
    {
        #region PrivateVariables
        private readonly HealthUpContext _context;
        #endregion

        #region Constructor
        public ProfessoresController(HealthUpContext context)
        {
            _context = context;
        }
        #endregion

        #region Index
        // GET: Professores
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region RegistarPesoSocio
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

                _context.Socios.Update(socio);
                
                //--------------------------------------------------------------------------------------------------------------------------------------
                // Adicionar à string json do professor
                var professor = _context.Professores.Include(p => p.Socio).SingleOrDefault(p => p.NumCC == HttpContext.Session.GetString("UserId"));
                professor.RegistarPesoSocio(Peso, DateTime.Now.ToShortDateString(), SocioEscolhido);

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
        #endregion

        #region ConsultarMeusAlunos
        public IActionResult ConsultarMeusAlunos()
        {
            var x = _context.Pessoas.Include(x => x.Socio).Where(x => x.Socio.NumProfessor == HttpContext.Session.GetString("UserId"));
            return View(_context.Pessoas.Include(x => x.Socio).Where(x => x.Socio.NumProfessor == HttpContext.Session.GetString("UserId")));
        }
        #endregion

        #region ConsultarSociosInscritosAulas
        public IActionResult ConsultarSociosInscritosAulas()
        {
            List<Aula> Lista = _context.Aulas.Include(a=>a.AulaGrupoNavigation).Where(a=>a.NumProfessor==HttpContext.Session.GetString("UserId")).ToList();

            ViewBag.Aulas = Lista.Select(s => new SelectListItem()
            {
                Text = s.AulaGrupoNavigation.Nome,
                Value = s.IdAula.ToString()
            });

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConsultarSociosInscritosAulas(string AulaEscolhida)
        {
            List<Pessoa> ListaSocios = new List<Pessoa>();
            foreach (var item in _context.Inscricoes.Include(i=>i.IdAulaNavigation))
            {
                if (item.IdAula==Convert.ToInt32(AulaEscolhida))
                {
                    var p = _context.Pessoas.SingleOrDefault(p => p.NumCC == item.NumSocio);
                    ListaSocios.Add(p);
                }
            }
            return PartialView("Partial_ConsultarSociosInscritosAulas", ListaSocios);

        }
        #endregion

        #region CriarPlanoExercicioSocio
        public IActionResult PlanosExercicioSocios()
        {
            return RedirectToAction("Index", "PlanoTreinos");
        }
        #endregion
    }
}
