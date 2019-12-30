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
    public class AulasController : Controller
    {
        private readonly HealthUpContext _context;

        //public AulasController(HealthUpContext context)
        //{
        //    _context = context;
        //    //Pessoa a = new Pessoa()
        //    //{
        //    //    DataNascimento = new DateTime(1999, 12, 12),
        //    //    Email = "a@gmail.com",
        //    //    Fotografia = "ola.jpg",
        //    //    Nacionalidade = "pt",
        //    //    Nome = "rui",
        //    //    NumCC = "12211221",
        //    //    NumAdmin = "1234567",
        //    //    Password = "qq",
        //    //    Sexo = "M",
        //    //    Telemovel = "911775676",
        //    //    Username = "ola",

        //    //};
        //    //Professor b = new Professor()
        //    //{
        //    //    Especialidade = "zumba",
        //    //    NumAdmin = "1234567",
        //    //    NumCC = "12211221",
        //    //};

        //    //_context.Pessoas.Add(a);_context.Professores.Add(b);_context.SaveChanges();

        //}

        //// GET: Aulas
        //public async Task<IActionResult> Index()
        //{
        //    var healthUpContext = _context.Aulas.Include(a => a.NumProfessorNavigation);

        //    return View(await healthUpContext.ToListAsync());
        //}

        //// GET: Aulas/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var aula = await _context.Aulas
        //        .Include(a => a.NumAdminNavigation)
        //        .Include(a => a.NumProfessorNavigation)
        //        .FirstOrDefaultAsync(m => m.IdAula == id);
        //    if (aula == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(aula);
        //}

        //// GET: Aulas/Create
        //public IActionResult Create()
        //{
        //    List<string> dias = new List<string>() { "Segunda-Feira", "Terça-Feira","Quarta-Feira","Quinta-Feira", "Sexta-Feira","Sábado","Domingo" };
        //    ViewData["NumProfessor"] = new SelectList(_context.Professores.Include(x => x.NumProfessorNavigation), "NumProfessorNavigation.Nome", "NumProfessorNavigation.Nome");
        //    ViewData["DiaSemana"] = new SelectList(dias);
        //    return View();
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(IFormCollection dados)
        //{
        //    Aula nova = new Aula();
        //    nova.NumProfessor = _context.Professores.FirstOrDefault(x => x.NumProfessorNavigation.Nome == dados["NomeProfessor"]).NumCC;
        //    nova.HoraInicio = TimeSpan.Parse(dados["Hora"]);
        //    nova.Lotacao = int.Parse(dados["Lotacao"]);
        //    nova.ValidoAte = DateTime.Parse(dados["ValidoAte"]);
        //    nova.ValidoDe = DateTime.Parse(dados["ValidoDe"]);
        //    nova.DiaSemana = Helpers.HelperFunctions.GetDay(dados["DiaSemana"]);

        //    _context.Add(nova);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));

        //}

        //// GET: Aulas/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var aula = await _context.Aulas.FindAsync(id);
        //    if (aula == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["NumAdmin"] = new SelectList(_context.Admins, "NumCC", "NumCC", aula.NumAdmin);
        //    ViewData["NumProfessor"] = new SelectList(_context.Professores, "NumCC", "NumCC", aula.NumProfessor);
        //    return View(aula);
        //}

        //// POST: Aulas/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("IdAula,NumProfessor,NumAdmin,ValidoDe,ValidoAte,Lotacao,HoraInicio,DiaSemana")] Aula aula)
        //{
        //    if (id != aula.IdAula)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(aula);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!AulaExists(aula.IdAula))
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
        //    ViewData["NumAdmin"] = new SelectList(_context.Admins, "NumCC", "NumCC", aula.NumAdmin);
        //    ViewData["NumProfessor"] = new SelectList(_context.Professores, "NumCC", "NumCC", aula.NumProfessor);
        //    return View(aula);
        //}

        //// GET: Aulas/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var aula = await _context.Aulas
        //        .Include(a => a.NumAdminNavigation)
        //        .Include(a => a.NumProfessorNavigation)
        //        .FirstOrDefaultAsync(m => m.IdAula == id);
        //    if (aula == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(aula);
        //}

        //// POST: Aulas/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var aula = await _context.Aulas.FindAsync(id);
        //    _context.Aulas.Remove(aula);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool AulaExists(int id)
        //{
        //    return _context.Aulas.Any(e => e.IdAula == id);
        //}
    }
}
