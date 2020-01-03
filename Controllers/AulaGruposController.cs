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

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil = "Admin")]
    public class AulaGruposController : Controller
    {
        private readonly HealthUpContext _context;

        public AulaGruposController(HealthUpContext context)
        {
            _context = context;
        }

        //// GET: AulaGrupos
        //public async Task<IActionResult> Index()
        //{
        //    var healthUpContext = _context.AulasGrupo.Include(a => a.IdAulaNavigation);
        //    return View(await healthUpContext.ToListAsync());
        //}

        //// GET: AulaGrupos/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var aulaGrupo = await _context.AulasGrupo
        //        .Include(a => a.IdAulaNavigation)
        //        .FirstOrDefaultAsync(m => m.IdAula == id);
        //    if (aulaGrupo == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(aulaGrupo);
        //}

        //// GET: AulaGrupos/Create
        //public IActionResult Create()
        //{
        //    ViewData["IdAula"] = new SelectList(_context.Aulas, "IdAula", "IdAula");
        //    return View();
        //}

        //// POST: AulaGrupos/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IdAula,Nome,FotografiaDivulgacao,VideoDivulgacao,Descricao")] AulaGrupo aulaGrupo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(aulaGrupo);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdAula"] = new SelectList(_context.Aulas, "IdAula", "IdAula", aulaGrupo.IdAula);
        //    return View(aulaGrupo);
        //}

        //// GET: AulaGrupos/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var aulaGrupo = await _context.AulasGrupo.FindAsync(id);
        //    if (aulaGrupo == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["IdAula"] = new SelectList(_context.Aulas, "IdAula", "IdAula", aulaGrupo.IdAula);
        //    return View(aulaGrupo);
        //}

        //// POST: AulaGrupos/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("IdAula,Nome,FotografiaDivulgacao,VideoDivulgacao,Descricao")] AulaGrupo aulaGrupo)
        //{
        //    if (id != aulaGrupo.IdAula)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(aulaGrupo);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AulaGrupoExists(aulaGrupo.IdAula))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdAula"] = new SelectList(_context.Aulas, "IdAula", "IdAula", aulaGrupo.IdAula);
        //    return View(aulaGrupo);
        //}

        //// GET: AulaGrupos/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var aulaGrupo = await _context.AulasGrupo
        //        .Include(a => a.IdAulaNavigation)
        //        .FirstOrDefaultAsync(m => m.IdAula == id);
        //    if (aulaGrupo == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(aulaGrupo);
        //}

        //// POST: AulaGrupos/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var aulaGrupo = await _context.AulasGrupo.FindAsync(id);
        //    _context.AulasGrupo.Remove(aulaGrupo);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool AulaGrupoExists(int id)
        //{
        //    return _context.AulasGrupo.Any(e => e.IdAula == id);
        //}
    }
}
