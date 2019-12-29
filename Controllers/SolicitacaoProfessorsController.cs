using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthUp.Data;
using HealthUp.Models;

namespace HealthUp.Controllers
{
    public class SolicitacaoProfessorsController : Controller
    {
        private readonly HealthUpContext _context;

        public SolicitacaoProfessorsController(HealthUpContext context)
        {
            _context = context;
        }

        // GET: SolicitacaoProfessors
        public async Task<IActionResult> Index()
        {
            var healthUpContext = _context.SolicitacaoProfessores.Include(s => s.NumAdminNavigation);
            return View(await healthUpContext.ToListAsync());
        }

        // GET: SolicitacaoProfessors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitacaoProfessor = await _context.SolicitacaoProfessores
                .Include(s => s.NumAdminNavigation)
                .FirstOrDefaultAsync(m => m.IdSolicitacao == id);
            if (solicitacaoProfessor == null)
            {
                return NotFound();
            }

            return View(solicitacaoProfessor);
        }

        // GET: SolicitacaoProfessors/Create
        public IActionResult Create()
        {
            ViewData["NumAdmin"] = new SelectList(_context.Admins, "NumCC", "NumCC");
            return View();
        }

        // POST: SolicitacaoProfessors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSolicitacao,NumAdmin,Data")] SolicitacaoProfessor solicitacaoProfessor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(solicitacaoProfessor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NumAdmin"] = new SelectList(_context.Admins, "NumCC", "NumCC", solicitacaoProfessor.NumAdmin);
            return View(solicitacaoProfessor);
        }

        // GET: SolicitacaoProfessors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitacaoProfessor = await _context.SolicitacaoProfessores.FindAsync(id);
            if (solicitacaoProfessor == null)
            {
                return NotFound();
            }
            ViewData["NumAdmin"] = new SelectList(_context.Admins, "NumCC", "NumCC", solicitacaoProfessor.NumAdmin);
            return View(solicitacaoProfessor);
        }

        // POST: SolicitacaoProfessors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("IdSolicitacao,NumAdmin,Data")] SolicitacaoProfessor solicitacaoProfessor)
        {
            if (id != solicitacaoProfessor.IdSolicitacao)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solicitacaoProfessor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitacaoProfessorExists(solicitacaoProfessor.IdSolicitacao))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["NumAdmin"] = new SelectList(_context.Admins, "NumCC", "NumCC", solicitacaoProfessor.NumAdmin);
            return View(solicitacaoProfessor);
        }

        // GET: SolicitacaoProfessors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitacaoProfessor = await _context.SolicitacaoProfessores
                .Include(s => s.NumAdminNavigation)
                .FirstOrDefaultAsync(m => m.IdSolicitacao == id);
            if (solicitacaoProfessor == null)
            {
                return NotFound();
            }

            return View(solicitacaoProfessor);
        }

        // POST: SolicitacaoProfessors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var solicitacaoProfessor = await _context.SolicitacaoProfessores.FindAsync(id);
            _context.SolicitacaoProfessores.Remove(solicitacaoProfessor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SolicitacaoProfessorExists(int? id)
        {
            return _context.SolicitacaoProfessores.Any(e => e.IdSolicitacao == id);
        }
    }
}
