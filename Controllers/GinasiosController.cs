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
            if (_context.Ginasios.FirstOrDefault() == null)
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
            ginasio.Telemovel = ginasio.Telemovel.Substring(4);
            return View(ginasio);
        }

        // POST: Ginasios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormCollection dados)
        {
            Ginasio g = _context.Ginasios.FirstOrDefault();
            if (dados["Id"] != g.Id.ToString())
            {
                return NotFound();
            }

            g.Telemovel = "+" + dados["Indicativo"] + dados["Telemovel"];
            g.NumAdmin = HttpContext.Session.GetString("UserId");
            g.Id = int.Parse(dados["Id"]);
            g.LocalizacaoGps = dados["LocalizacaoGps"];
            g.Email = dados["Email"];
            g.Telemovel = dados["Telemovel"];
            g.Endereco = dados["Endereco"];
            g.Nome = dados["Nome"];

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Ginasios.Update(g);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GinasioExists(g.Id))
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
            return RedirectToAction(nameof(Index), "Admins");

        }



        private bool GinasioExists(int id)
        {
            return _context.Ginasios.Any(e => e.Id == id);
        }
    }
}