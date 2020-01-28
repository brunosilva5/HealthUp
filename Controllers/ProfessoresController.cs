using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil = "Professor")]

    public class ProfessoresController : BaseController
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
                Text = "CC: " + s.NumCC + " | " + s.NumSocioNavigation.Nome,
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
            Socio socio = _context.Socios.SingleOrDefault(s => s.NumCC == SocioEscolhido);


            if (ModelState.IsValid)
            {
                socio.Peso = Peso;
                socio.DataRegisto_Peso = DateTime.Now.Date;
                socio.NumProfessor = HttpContext.Session.GetString("UserId");

                _context.Socios.Update(socio);

                //--------------------------------------------------------------------------------------------------------------------------------------
                // Adicionar à string json do professor
                Professor professor = _context.Professores.Include(p => p.Socio).SingleOrDefault(p => p.NumCC == HttpContext.Session.GetString("UserId"));
                professor.RegistarPesoSocio(Peso, DateTime.Now.ToShortDateString(), SocioEscolhido);

                _context.Professores.Update(professor);

                // --------------------------------------------------------------------------------------------------------------------------------------
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(RegistarPesoSocio));
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
            IQueryable<Pessoa> x = _context.Pessoas.Include(x => x.Socio).Where(x => x.Socio.NumProfessor == HttpContext.Session.GetString("UserId"));
            return View(_context.Pessoas.Include(x => x.Socio).Where(x => x.Socio.NumProfessor == HttpContext.Session.GetString("UserId")));
        }

        public IActionResult HistoricoAulas(int id)
        {

            RouteValueDictionary values = new RouteValueDictionary(new
            {
                action = "HistoricoAulas",
                controller = "Socios",
                IdSocio = id.ToString()
            });

            return RedirectToAction("HistoricoAulas", "Socios", values);
        }
        #endregion

        #region ConsultarSociosInscritosAulas
        public IActionResult ConsultarSociosInscritosAulas()
        {
            List<Aula> Lista = _context.Aulas.Where(a => a.NumProfessor == HttpContext.Session.GetString("UserId")).ToList();

            ViewBag.Aulas = Lista.Select(s => new SelectListItem()
            {

                Value = s.IdAula.ToString()
            });

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConsultarSociosInscritosAulas(string AulaEscolhida)
        {
            List<Pessoa> ListaSocios = new List<Pessoa>();
            foreach (Inscreve item in _context.Inscricoes.Include(i => i.IdAulaNavigation))
            {
                if (item.IdAula == Convert.ToInt32(AulaEscolhida))
                {
                    Pessoa p = _context.Pessoas.SingleOrDefault(p => p.NumCC == item.NumSocio);
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

        #region Consultar aulas que leciona

        public IActionResult ConsultarAulasQueLeciona()
        {
            IQueryable<Aula> lista = _context.Aulas.Where(x => x.NumProfessor == HttpContext.Session.GetString("UserId"))
                .Where(y => y.ValidoAte > DateTime.Now && y.ValidoDe < DateTime.Now);
            return View(lista);
        }

        #endregion

    }
}
