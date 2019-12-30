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

namespace HealthUp.Controllers
{
    public class GinasiosController : Controller
    {
        private readonly HealthUpContext _context;

        public GinasiosController(HealthUpContext context)
        {
            _context = context;
        }

        // GET: Ginasios
        public IActionResult Index()
        {
            if (_context.Ginasios.FirstOrDefault()==null)
            {
                return RedirectToAction(nameof(Edit), null);
            }

            return RedirectToAction(nameof(Edit), _context.Ginasios.FirstOrDefault().Id);

        }

        // GET: Ginasios/Edit/5
        public async Task<IActionResult> Edit()
        {
            var ginasio = _context.Ginasios.FirstOrDefault();
            // apagar o indicativo
            ginasio.Telemovel=ginasio.Telemovel.Substring(4);
            return View(ginasio);
        }

        // POST: Ginasios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumAdmin,Nome,Endereco,Email,Telemovel,LocalizacaoGps")] Ginasio ginasio, string Indicativo)
        {
            if (id != ginasio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ginasio.Telemovel = "+" +Indicativo + ginasio.Telemovel;
                    ginasio.NumAdmin = HttpContext.Session.GetString("UserId");
                    _context.Update(ginasio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GinasioExists(ginasio.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), "Admins");
            }
            return View(ginasio);
        }

       

        private bool GinasioExists(int id)
        {
            return _context.Ginasios.Any(e => e.Id == id);
        }
    }
}
