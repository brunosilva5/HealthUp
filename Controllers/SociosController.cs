using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil ="Socio")]
    public class SociosController : Controller
    {
        #region PrivateVariables
        private readonly HealthUpContext _context;
        #endregion

        #region Constructors
        public SociosController(HealthUpContext context)
        {
            _context = context;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View(_context.Socios.Include(s=>s.NumProfessorNavigation).SingleOrDefault(s=>s.NumCC==HttpContext.Session.GetString("UserId")));
        }

        #endregion

        #region SolicitarPersonalTrainer
        public IActionResult SolicitarPT()
        {
            List<Pessoa> Lista = new List<Pessoa>();
            // Obter lista de nomes de professores + especialidade 
            foreach (var pessoa in _context.Pessoas.Include(p => p.Professor))
            {
                if (pessoa.Professor != null)
                {
                    Lista.Add(pessoa);
                }
            }
            ViewBag.Professores = Lista.Select(c => new SelectListItem()
            {
                Text = c.Nome + " | Especialidade: " + _context.Professores.SingleOrDefault(s => s.NumCC == c.NumCC).Especialidade,
                Value = c.NumCC
            });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SolicitarPT(string ProfessorEscolhido)
        {
            var Prof = _context.Professores.SingleOrDefault(p => p.NumCC == ProfessorEscolhido);
            var Socio = _context.Socios.SingleOrDefault(s => s.NumCC == HttpContext.Session.GetString("UserId"));
            SolicitacaoProfessor solicitacao = new SolicitacaoProfessor();

            if (_context.Socios.SingleOrDefault(s=>s.NumCC==HttpContext.Session.GetString("UserId")).ID_Solicitacao!=null)
            {
                ModelState.AddModelError("", "Já existe um pedido de personal trainer pendente! Aguarde a aprovação ou rejeição por parte de um administrador!");
            }
            if (ModelState.IsValid)
            {
                 
                solicitacao.Professor.Add(Prof);
                solicitacao.Socio.Add(Socio);
                solicitacao.Data = DateTime.Now;

                _context.Add(solicitacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Passar para a view de novo a lista de professores
            List<Pessoa> Lista = new List<Pessoa>();
            // Obter lista de nomes de professores + especialidade 
            foreach (var pessoa in _context.Pessoas.Include(p => p.Professor))
            {
                if (pessoa.Professor != null)
                {
                    Lista.Add(pessoa);
                }
            }
            ViewBag.Professores = Lista.Select(c => new SelectListItem()
            {
                Text = c.Nome + " | Especialidade: " + _context.Professores.SingleOrDefault(s => s.NumCC == c.NumCC).Especialidade,
                Value = c.NumCC
            }); 
            return View(solicitacao);
        }
        #endregion

        #region ConsultarHistoricoPeso
        public IActionResult ConsultarHistoricoPeso()
        {
            var lstModel = new List<SimpleReportViewModel>();
            //foreach (var item in collection)
            //{

            //}
            return View();
        }
        #endregion
    }
}