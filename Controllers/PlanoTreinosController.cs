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
using HealthUp.Helpers;
using HealthUp.Filters;

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil = "Professor")]
    public class PlanoTreinosController : BaseController
    {
        private readonly HealthUpContext _context;

        public PlanoTreinosController(HealthUpContext context)
        {
            _context = context;
        }

        // GET: PlanoTreinos
        public IActionResult Index()
        {
            // lista de socios nao suspensos
            List<Socio> Lista = _context.Socios.Include(s => s.NumSocioNavigation).Where(s => s.DataSuspensao == null && s.Motivo == null).ToList();
            ViewBag.Socios = Lista.Select(s => new SelectListItem()
            {
                Text = "CC: " + s.NumCC + " | " + s.NumSocioNavigation.Nome,
                Value = s.NumCC
            });

            return View();
        }
        [HttpPost]
        public IActionResult Index_Filtered(IFormCollection data)
        {
            var socio = _context.Socios.Include(x=>x.PlanoTreino).ThenInclude(p => p.Contem).Include(p=>p.PlanoTreino).ThenInclude(x => x.NumProfessorNavigation).ThenInclude(x=>x.NumProfessorNavigation).SingleOrDefault(s => s.NumCC == data["SocioEscolhido"].ToString());
            ViewBag.Socio = socio;
            return PartialView(nameof(Index_Filtered), socio.PlanoTreino.OrderBy(x => x.Descricao).ToList());
        }
        // GET: PlanoTreinos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var planoTreino = await _context.PlanosTreino
                .Include(p => p.NumProfessorNavigation)
                .Include(p => p.NumSocioNavigation)
                .FirstOrDefaultAsync(m => m.IdPlano == id);
            if (planoTreino == null)
            {
                return NotFound();
            }

            return View(planoTreino);
        }

        // GET: PlanoTreinos/Create
        public IActionResult Create()
        {
            // lista de socios nao suspensos
            List<Socio> Lista = _context.Socios.Include(s => s.NumSocioNavigation).Where(s => s.DataSuspensao == null && s.Motivo == null).ToList();
            ViewBag.Socios = Lista.Select(s => new SelectListItem()
            {
                Text = "CC: " + s.NumCC + " | " + s.NumSocioNavigation.Nome,
                Value = s.NumCC
            });

            return View();
        }

        // POST: PlanoTreinos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlano,NumSocio,NumProfessor,Ativo,Descricao")] PlanoTreino planoTreino)
        {
            if (ModelState.IsValid)
            {
                planoTreino.NumProfessor = HttpContext.Session.GetString("UserId");
                planoTreino.Ativo = false;
                _context.Add(planoTreino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // lista de socios nao suspensos
            List<Socio> Lista = _context.Socios.Include(s => s.NumSocioNavigation).Where(s => s.DataSuspensao == null && s.Motivo == null).ToList();
            ViewBag.Socios = Lista.Select(s => new SelectListItem()
            {
                Text = "CC: " + s.NumCC + " | " + s.NumSocioNavigation.Nome,
                Value = s.NumCC
            });

            return View(planoTreino);
        }

        

        // GET: PlanoTreinos/Delete/5
        public async Task<IActionResult> Delete(int? IdPlano)
        {
            if (IdPlano == null)
            {
                return NotFound();
            }

            var planoTreino = await _context.PlanosTreino
                .Include(p => p.NumProfessorNavigation)
                .Include(p => p.NumSocioNavigation)
                .FirstOrDefaultAsync(m => m.IdPlano == IdPlano);
            if (planoTreino == null)
            {
                return NotFound();
            }

            return View(planoTreino);
        }

        // POST: PlanoTreinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int IdPlano)
        {
            var planoTreino = await _context.PlanosTreino.FindAsync(IdPlano);
            _context.PlanosTreino.Remove(planoTreino);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanoTreinoExists(int id)
        {
            return _context.PlanosTreino.Any(e => e.IdPlano == id);
        }


        //-----------------------------------------

        public IActionResult VerExerciciosSocio(string IdPlano)
        {
           
            var ListaContem = _context.PlanosTreino.Include(p => p.Contem).ThenInclude(c => c.IdExercicioNavigation).SingleOrDefault(p => p.IdPlano == Convert.ToInt32(IdPlano)).Contem;
            
            ViewBag.PlanoId = IdPlano;

            return View(nameof(VerExerciciosSocio), ListaContem.ToList());
        }

        public IActionResult AdicionarExercicioSocio(string IdPlano)
        {
            if (IdPlano==null)
            {
                return NotFound();
            }
            ViewBag.PlanoId = IdPlano;
            var plano = _context.PlanosTreino.Include(x=>x.Contem).SingleOrDefault(x => x.IdPlano == Convert.ToInt32(IdPlano));

            List<Exercicio> Lista = _context.Exercicios.ToList();
            foreach (var item in plano.Contem)
            {
                if (Lista.Contains(_context.Exercicios.SingleOrDefault(p=>p.IdExercicio==item.IdExercicio)))
                {
                    Lista.Remove(_context.Exercicios.SingleOrDefault(p => p.IdExercicio == item.IdExercicio));
                }
            }
            
            ViewBag.Exercicios = Lista.Select(e => new SelectListItem()
            {
                Text = e.Nome,
                Value = e.IdExercicio.ToString()
            });

            return View(nameof(AdicionarExercicioSocio));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarExercicioSocio(IFormCollection data)
        {


            if (ModelState.IsValid)
            {
                var plano = _context.PlanosTreino.Include(x => x.Contem).Include(p => p.NumSocioNavigation).Include(p => p.NumProfessorNavigation).SingleOrDefault(p => p.IdPlano == Convert.ToInt32(data["PlanoId"]));
                var exercicio = _context.Exercicios.SingleOrDefault(e => e.IdExercicio == Convert.ToInt32(data["ExercicioEscolhido"]));
                Contem c = new Contem()
                {
                    IdExercicio = Convert.ToInt32(data["ExercicioEscolhido"]),
                    IdPlano = Convert.ToInt32(data["PlanoId"]),
                    NumRepeticoes = Convert.ToInt32(data["NumRepeticoes"]),
                    PeriodoDescanso = Convert.ToInt32(data["PeriodoDescanso"]),
                    QuantidadeSeries = Convert.ToInt32(data["QuantidadeSeries"])
                };
                plano.Contem.Add(c);
                _context.PlanosTreino.Update(plano);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            List<Exercicio> Lista = _context.Exercicios.ToList();
            ViewBag.Exercicios = Lista.Select(e => new SelectListItem()
            {
                Text = e.Nome,
                Value = e.IdExercicio.ToString()
            });

            return RedirectToAction(nameof(Index));
        }

        public IActionResult TornarPlanoAtivo(string IdPlano)
        {
            var plano = _context.PlanosTreino.Include(p => p.NumSocioNavigation).SingleOrDefault(p => p.IdPlano == Convert.ToInt32(IdPlano));
            var socio = _context.Socios.Include(x => x.PlanoTreino).ThenInclude(p => p.NumProfessorNavigation).ThenInclude(x => x.NumProfessorNavigation).SingleOrDefault(p => p.NumCC == plano.NumSocio);
            socio.TornarPlanosInativos();
            plano.Ativo = true;
            _context.PlanosTreino.Update(plano);
            _context.Socios.Update(socio);
            _context.SaveChanges();
            return PartialView("Index_Filtered", socio.PlanoTreino.OrderBy(x=>x.Descricao));
        }

        public async Task<IActionResult> ApagarExercicio(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contem = await _context.Contem
                .Include(c => c.IdExercicioNavigation)
                .Include(c => c.IdPlanoNavigation)
                .FirstOrDefaultAsync(m => m.IdPlano == id);
            if (contem == null)
            {
                return NotFound();
            }

            return View(contem);
        }

        // POST: Contems/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApagarExercicio(int id)
        {
            Contem contem = _context.Contem.SingleOrDefault(c => c.IdPlano == id);
            _context.Contem.Remove(contem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(VerExerciciosSocio), new { contem.IdPlano });
        }

        public IActionResult EditarExercicio(int IdExercicio, int IdPlano)
        {
            
            ViewBag.PlanoId = IdPlano;
            var contem = _context.Contem.SingleOrDefault(x => x.IdPlano==IdPlano && x.IdExercicio==IdExercicio);
            if (contem == null)
            {
                return NotFound();
            }

            return View(contem);
        }

        // POST: Contems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarExercicio([Bind("IdPlano,IdExercicio,NumRepeticoes,PeriodoDescanso,QuantidadeSeries")] Contem contem)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContemExists(contem.IdPlano))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(VerExerciciosSocio), new { contem.IdPlano });
            }
            
            return View(contem);
        }
        private bool ContemExists(int id)
        {
            return _context.Contem.Any(e => e.IdPlano == id);
        }

    }
}
