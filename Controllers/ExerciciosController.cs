using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil = "Admin")]
    public class ExerciciosController : BaseController
    {
        private readonly HealthUpContext _context;
        private readonly IHostEnvironment _hostEnvironment;
        public ExerciciosController(HealthUpContext context, IHostEnvironment he)
        {
            _context = context;
            _hostEnvironment = he;
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

        [RequestSizeLimit(1048576000)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Nome, string Descricao, IFormFile Fotografia, IFormFile Video)
        {
            Exercicio exercicio = new Exercicio
            {
                Nome = Nome,
                Descricao = Descricao,
                Fotografia = Fotografia.FileName,
                Video = Video.FileName
            };

            if (ModelState.IsValid)
            {
                //--------------------------------------------------------------------------------------------------------------------------------------
                // Adicionar na tabela de solicitacoes do admin
                Admin admin = _context.Admins.Include(x => x.Exercicio).Include(x => x.NumAdminNavigation).SingleOrDefault(x => x.NumCC == HttpContext.Session.GetString("UserId"));
                admin.Exercicio.Add(exercicio);
                _context.Admins.Update(admin);
                // --------------------------------------------------------------------------------------------------------------------------------------

                exercicio.NumAdmin = _context.Admins.Include(x => x.NumAdminNavigation).SingleOrDefault(a => a.NumCC == HttpContext.Session.GetString("UserId")).NumCC;

                _context.Add(exercicio);
                await _context.SaveChangesAsync();

                //guardar ficheiros no wwwroot
                string caminho = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\Ficheiros");
                string nome_ficheiro = Path.GetFileName(Fotografia.FileName);
                string caminho_completo = Path.Combine(caminho, nome_ficheiro);

                FileStream f = new FileStream(caminho_completo, FileMode.Create);
                Fotografia.CopyTo(f);

                f.Close();


                string caminho1 = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\Ficheiros");
                string nome_ficheiro1 = Path.GetFileName(Video.FileName);
                string caminho_completo1 = Path.Combine(caminho1, nome_ficheiro1);

                FileStream ff = new FileStream(caminho_completo1, FileMode.Create);
                Video.CopyTo(ff);

                ff.Close();

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

            Exercicio exercicio = await _context.Exercicios.FindAsync(id);
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

            Exercicio exercicio = await _context.Exercicios
                .Include(e => e.NumAdminNavigation).ThenInclude(e => e.NumAdminNavigation)
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
            Exercicio exercicio = await _context.Exercicios.FindAsync(id);
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
