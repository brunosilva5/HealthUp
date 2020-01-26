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
using System.IO;
using HealthUp.Helpers;
using Microsoft.Extensions.Hosting;

namespace HealthUp.Controllers
{
    public class AulasController : Controller
    {
        private readonly HealthUpContext _context;
        private readonly IHostEnvironment _hostEnvironment;

        public AulasController(HealthUpContext context, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Aulas
        public async Task<IActionResult> Index()
        {
            var healthUpContext = _context.Aulas.Include(a => a.NumAdminNavigation).Include(a => a.NumProfessorNavigation);
            return View(await healthUpContext.ToListAsync());
        }

        // GET: Aulas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aula = await _context.Aulas
                .Include(a => a.NumAdminNavigation)
                .Include(a => a.NumProfessorNavigation)
                .FirstOrDefaultAsync(m => m.IdAula == id);
            if (aula == null)
            {
                return NotFound();
            }

            return View(aula);
        }

        // GET: Aulas/Create
        public IActionResult Create()
        {
            List<string> Horas = new List<string>();
            ViewData["NomeProfessor"] = new SelectList(_context.Professores.Include(x => x.NumProfessorNavigation), "NumProfessorNavigation.NumCC", "NumProfessorNavigation.Nome");
            List<string> dias = new List<string>() { "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira", "Sábado", "Domingo" };
            ViewData["DiaSemana"] = new SelectList(dias);
            for (int i = 6; i < 24; i=i+2)
            {
                Horas.Add(i.ToString());
            }
            ViewData["HorasInicio"] = new SelectList(Horas);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(100_000_000)]
        public IActionResult Create(IFormCollection dados, IFormFile FotografiaDivulgacao, IFormFile VideoDivulgacao)
        {
            //if (Path.GetExtension(dados[]FotografiaDivulgacao.FileName) != ".jpg") ModelState.AddModelError("FotografiaDivulgacao", "O formato do ficheiro tem de ser.jpg");
            //if (Path.GetExtension(VideoDivulgacao.FileName) != ".mp4") ModelState.AddModelError("VideoDivulgacao", "O formato do ficheiro tem de ser .mp4");
            //if (DateTime.Parse(dados["ValidoDe"]) > DateTime.Parse(dados["ValidoAte"])) ModelState.AddModelError("ValidoAte", "A validade está incorreta");

            Aula aula = new Aula
            {

                // Guardar o id do admin que criou
                NumAdmin = HttpContext.Session.GetString("UserId"),
                // Guardar o professor associado a esta aula
                NumProfessor = dados["IdProfessor"],
                // alterar
                DiaSemana = HelperFunctions.GetDay(dados["DiaSemana"]),

                HoraInicio = TimeSpan.Parse(dados["HoraInicio"]+":00"),
                Lotacao = int.Parse(dados["Lotacao"]),
                ValidoDe = DateTime.Parse(dados["ValidoDe"]),
                ValidoAte = DateTime.Parse(dados["ValidoAte"]),
                Nome = dados["Nome"],
                Descricao = dados["Descricao"],
                FotografiaDivulgacao = Path.GetFileName(FotografiaDivulgacao.FileName),
                VideoDivulgacao = Path.GetFileName(VideoDivulgacao.FileName)
            };
            if (ModelState.IsValid)
            {


                _context.Add(aula);
                _context.SaveChanges();

                //guardar ficheiros no wwwroot
                string caminho = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\Ficheiros");
                string nome_ficheiro = Path.GetFileName(FotografiaDivulgacao.FileName);
                string caminho_completo = Path.Combine(caminho, nome_ficheiro);

                FileStream f = new FileStream(caminho_completo, FileMode.Create);
                FotografiaDivulgacao.CopyTo(f);

                f.Close();


                string caminho1 = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\Ficheiros");
                string nome_ficheiro1 = Path.GetFileName(VideoDivulgacao.FileName);
                string caminho_completo1 = Path.Combine(caminho1, nome_ficheiro1);

                FileStream ff = new FileStream(caminho_completo1, FileMode.Create);
                VideoDivulgacao.CopyTo(ff);

                ff.Close();

                return RedirectToAction(nameof(Index));

            }
            ViewData["NomeProfessor"] = new SelectList(_context.Professores.Include(x => x.NumProfessorNavigation), "NumProfessorNavigation.NumCC", "NumProfessorNavigation.Nome");
            List<string> dias = new List<string>() { "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira", "Sábado", "Domingo" };
            ViewData["DiaSemana"] = new SelectList(dias);
            return View(aula);
        }

        // GET: Aulas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aula = await _context.Aulas.FindAsync(id);
            if (aula == null)
            {
                return NotFound();
            }
            ViewData["NumAdmin"] = new SelectList(_context.Admins, "NumCC", "NumCC", aula.NumAdmin);
            ViewData["NumProfessor"] = new SelectList(_context.Professores, "NumCC", "NumCC", aula.NumProfessor);
            return View(aula);
        }

        // POST: Aulas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAula,Nome,NumProfessor,NumAdmin,ValidoDe,ValidoAte,Lotacao,HoraInicio,DiaSemana,FotografiaDivulgacao,VideoDivulgacao,Descricao")] Aula aula)
        {
            if (id != aula.IdAula)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AulaExists(aula.IdAula))
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
            ViewData["NumAdmin"] = new SelectList(_context.Admins, "NumCC", "NumCC", aula.NumAdmin);
            ViewData["NumProfessor"] = new SelectList(_context.Professores, "NumCC", "NumCC", aula.NumProfessor);
            return View(aula);
        }

        // GET: Aulas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aula = await _context.Aulas
                .Include(a => a.NumAdminNavigation)
                .Include(a => a.NumProfessorNavigation)
                .FirstOrDefaultAsync(m => m.IdAula == id);
            if (aula == null)
            {
                return NotFound();
            }

            return View(aula);
        }

        // POST: Aulas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aula = await _context.Aulas.FindAsync(id);
            _context.Aulas.Remove(aula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AulaExists(int id)
        {
            return _context.Aulas.Any(e => e.IdAula == id);
        }
    }
}
