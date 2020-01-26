using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HealthUp.Data;
using HealthUp.Models;
using HealthUp.Filters;
using Microsoft.AspNetCore.Http;

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil = "Admin")]
    public class ExerciciosController : Controller
    {
        private readonly HealthUpContext _context;

        public ExerciciosController(HealthUpContext context)
        {
            _context = context;
        }

        // GET: Exercicios
        public async Task<IActionResult> Index()
        {           
            return View(await _context.Exercicios.Include(e => e.NumAdminNavigation).ThenInclude(e => e.NumAdminNavigation).ToListAsync());
        }

     

        // GET: Exercicios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Exercicios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdExercicio,NumAdmin,Nome,Descricao,Video,Fotografia")] Exercicio exercicio)
        {
            if (ModelState.IsValid)
            {
                exercicio.NumAdmin = HttpContext.Session.GetString("UserId");
                _context.Add(exercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exercicio);
        }

        // GET: Exercicios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null)
            {
                return NotFound();
            }
            return View(exercicio);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdExercicio,NumAdmin,Nome,Descricao,Video,Fotografia")] Exercicio exercicio)
        {
            if (id != exercicio.IdExercicio)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exercicio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExercicioExists(exercicio.IdExercicio))
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
            return View(exercicio);
        }

        // GET: Exercicios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercicio = await _context.Exercicios
                .Include(e => e.NumAdminNavigation)
                .FirstOrDefaultAsync(m => m.IdExercicio == id);
            if (exercicio == null)
            {
                return NotFound();
            }

            return View(exercicio);
        }

        // POST: Exercicios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            _context.Exercicios.Remove(exercicio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExercicioExists(int id)
        {
            return _context.Exercicios.Any(e => e.IdExercicio == id);
        }
    }
}
