using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil = "Admin")]
    public class GinasiosController : BaseController
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
        public IActionResult Edit()
        {
            Ginasio ginasio = _context.Ginasios.FirstOrDefault();
            if (ginasio.Telemovel.Length > 9)
            {
                ginasio.Telemovel = ginasio.Telemovel.Substring(4, 9);
            }
            // apagar o indicativo
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
            g.Endereco = dados["Endereco"];
            g.Nome = dados["Nome"];

            g.Hora_Abertura = TimeSpan.Parse(dados["Hora_Abertura"]);
            g.Hora_Fecho = TimeSpan.Parse(dados["Hora_Fecho"]);
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
                return RedirectToAction(nameof(Index), "Home");
            }
            return RedirectToAction(nameof(Index), "Home");

        }



        private bool GinasioExists(int id)
        {
            return _context.Ginasios.Any(e => e.Id == id);
        }
    }
}