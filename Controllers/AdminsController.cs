using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Helpers;
using HealthUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil ="Admin")]
    public class AdminsController : Controller
    {
        #region PrivateVariables
        private readonly HealthUpContext _context;
        private readonly IHostEnvironment _e;

        #endregion

        #region Constructors
        public AdminsController(HealthUpContext context, IHostEnvironment e)
        {

            _e = e;
            _context = context;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region AprovarSócios
        public IActionResult AprovarSocios()
        {
            return View(_context.PedidosSocios);
        }
        public IActionResult AprovarSocio(int id)
        {
            var pedido = _context.PedidosSocios.FirstOrDefault(p => p.NumCC == id.ToString());
            Pessoa P = new Pessoa();
            P.DataNascimento = pedido.DataNascimento;
            P.NumCC = pedido.NumCC;
            P.Email = pedido.Email;
            P.Fotografia = pedido.Fotografia;
            P.Sexo = pedido.Sexo;
            P.Username = pedido.Username;
            P.Nome = pedido.Nome;
            P.Nacionalidade = pedido.Nacionalidade;
            P.NumAdmin = (HttpContext.Session.GetString("UserId"));
            P.Telemovel = pedido.Telemovel;
            Socio S = new Socio()
            {
                NumCC = P.NumCC,
                NumSocioNavigation = P,
                NumAdmin = (HttpContext.Session.GetString("UserId")),
                NumAdminNavigation = _context.Admins.FirstOrDefault(a => a.NumCC == (HttpContext.Session.GetString("UserId")))
            };
            P.Socio = S;
            _context.Socios.Add(S);

            // --------------------------------------------------------------------------------------------------------------------------------------
            // Adicionar na tabela de socios do admin
            var admin = _context.Admins.Include(x => x.PedidosSocio).Include(x => x.NumAdminNavigation).SingleOrDefault(x => x.NumCC == HttpContext.Session.GetString("UserId"));
            admin.PedidosSocio.Add(pedido);
            _context.Admins.Update(admin);
            // --------------------------------------------------------------------------------------------------------------------------------------


            P.Password = null;
            _context.Pessoas.Add(P);
            HelperFunctions.SendEmailConfirmacao(true, P.Email);
            // Apagar da tabela
            RejeitarSocio(id,true);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RejeitarSocio(int id, bool? flag)
        {
            var pedido = _context.PedidosSocios.FirstOrDefault(p => p.NumCC == id.ToString());
            if (flag==null)
            {
                HelperFunctions.SendEmailConfirmacao(false, pedido.Email);

            }

            _context.PedidosSocios.Remove(pedido);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region CriarSociosProfessores
        public IActionResult CriarAdminProf()
        {
            return View(_context.Pessoas.Include(p => p.Admin).Include(p => p.Professor).Include(p => p.Socio).Where(p => p.Socio != null).ToList());
        }

        public IActionResult CriarAdmin(string id)
        {
            Pessoa p = _context.Pessoas.FirstOrDefault(p => p.NumCC == id);
            Admin a = new Admin(p);
            _context.Admins.Add(a);
            Socio s = _context.Socios.FirstOrDefault(p => p.NumCC == id.ToString());
            _context.Remove(s);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult CriarProfessor(string id)
        {
            Pessoa p = _context.Pessoas.FirstOrDefault(p => p.NumCC == id);
            Professor prof = new Professor(p);
            prof.Especialidade = "Indefinido";
            _context.Professores.Add(prof);
            Socio s = _context.Socios.FirstOrDefault(p => p.NumCC == id.ToString());
            _context.Remove(s);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region AprovarPedidoProf
        public IActionResult AprovarPedidoProf()
        {
            return View(_context.SolicitacaoProfessores.Include(s => s.Socio).ThenInclude(s => s.NumSocioNavigation).Include(s => s.Professor).ThenInclude(p => p.NumProfessorNavigation).Include(a => a.NumAdminNavigation).ThenInclude(x => x.NumAdminNavigation).Where(s => s.NumAdmin == null).ToList().OrderByDescending(p => p.Data));
        }
        public IActionResult PedidoProf_Aprovado(int id)
        {
            var solicitacao = _context.SolicitacaoProfessores.Include(s => s.Socio).ThenInclude(s => s.NumSocioNavigation).Include(s => s.Professor).ThenInclude(p => p.NumProfessorNavigation).Include(a => a.NumAdminNavigation).ThenInclude(x => x.NumAdminNavigation).FirstOrDefault(s => s.IdSolicitacao == id);

            // Atribuir o ID do admin a esta solicitacao
            solicitacao.NumAdmin = HttpContext.Session.GetString("UserId");
            var socio=solicitacao.Socio.SingleOrDefault();
            var prof = solicitacao.Professor.SingleOrDefault();

            socio.NumProfessor = prof.NumCC;
            socio.ID_Solicitacao = null;

            prof.IdSolicitacao = null;
            prof.Socio.Add(socio);
            _context.Socios.Update(socio);
            _context.Professores.Update(prof);

            // --------------------------------------------------------------------------------------------------------------------------------------
            // Adicionar na tabela de solicitacoes do admin
            var admin = _context.Admins.Include(x => x.SolicitacaoProfessor).Include(x => x.NumAdminNavigation).SingleOrDefault(x => x.NumCC == HttpContext.Session.GetString("UserId"));
            admin.SolicitacaoProfessor.Add(solicitacao);
            _context.Admins.Update(admin);
            // --------------------------------------------------------------------------------------------------------------------------------------
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult PedidoProf_Rejeitado(int id)
        {
            var solicitacao = _context.SolicitacaoProfessores.FirstOrDefault(p => p.IdSolicitacao == id);
            _context.SolicitacaoProfessores.Remove(solicitacao);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region CriarExercicio

        public IActionResult CriarExercicio()
        {
            return View();
        }

        // POST: Exercicios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [RequestSizeLimit(100_000_000)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarExercicio(string Nome, string Descricao, IFormFile Fotografia, IFormFile Video)
        {
            Exercicio exercicio = new Exercicio();
            exercicio.Nome = Nome;
            exercicio.Descricao = Descricao;
            exercicio.Fotografia = Fotografia.FileName;
            exercicio.Video = Video.FileName;

            if (ModelState.IsValid)
            {
                //--------------------------------------------------------------------------------------------------------------------------------------
                // Adicionar na tabela de solicitacoes do admin
                var admin = _context.Admins.Include(x => x.Exercicio).Include(x => x.NumAdminNavigation).SingleOrDefault(x => x.NumCC == HttpContext.Session.GetString("UserId"));
                admin.Exercicio.Add(exercicio);
                _context.Admins.Update(admin);
                // --------------------------------------------------------------------------------------------------------------------------------------

                exercicio.NumAdmin = _context.Admins.Include(x => x.NumAdminNavigation).SingleOrDefault(a => a.NumCC == HttpContext.Session.GetString("UserId")).NumCC;

                _context.Add(exercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exercicio);
        }


        #endregion

        #region SuspenderUtilizador
        public IActionResult SuspenderUtilizador()
        {
            List<string> ListaIds = new List<string>();
            // Construcao da lista de Pessoas que nao sao admins e nao estao suspensas
            foreach (var socio in _context.Socios)
            {
                if (socio.DataSuspensao==null && socio.Motivo==null)    // double check
                {
                    ListaIds.Add(socio.NumCC);
                }
            }
            foreach (var professor in _context.Professores)
            {
                if (professor.DataSuspensao == null && professor.Motivo == null)    // double check
                {
                    ListaIds.Add(professor.NumCC);
                }
            }


            return View(_context.Pessoas.Include(p => p.Socio).Include(p => p.Professor).Where(x =>ListaIds.Contains(x.NumCC)));
        }
        
        public IActionResult SuspenderUtilizador_Selecionado(int id)
        {
            ViewBag.Id = id;
            return PartialView("SuspenderUtilizador_MotivoPartial");
        }
        public IActionResult SuspenderUtilizador_Confirmar(string Motivo, string id)
        {
            var Pessoa = _context.Pessoas.Include(p => p.Socio).Include(p => p.Professor).SingleOrDefault(p => p.NumCC == id);
            if (Pessoa==null)
            {
                return RedirectToAction(nameof(SuspenderUtilizador));
            }
            var admin = _context.Admins.SingleOrDefault(a => a.NumCC == HttpContext.Session.GetString("UserId"));
            
            if (HelperFunctions.IsSocio(_context, id))
            {
                var socio = _context.Socios.SingleOrDefault(s => s.NumCC == id);
                socio.DataSuspensao = DateTime.Now;
                socio.Motivo = Motivo;
                socio.NumAdmin = HttpContext.Session.GetString("UserId");
                _context.Socios.Update(socio);
                admin.SociosSuspensos.Add(socio);
            }
            if (HelperFunctions.IsProfessor(_context, id))
            {
                var professor = _context.Professores.SingleOrDefault(p => p.NumCC == id);
                professor.DataSuspensao = DateTime.Now;
                professor.Motivo = Motivo;
                professor.NumAdmin = HttpContext.Session.GetString("UserId");
                _context.Professores.Update(professor);
                admin.ProfessoresSuspensos.Add(professor);
            }
            _context.Admins.Update(admin);
            _context.SaveChanges();
            return RedirectToAction(nameof(SuspenderUtilizador));
        }
        #endregion

        #region LevantarSuspensao
        public IActionResult LevantarSuspensao()
        {
            List<string> ListaIds = new List<string>();
            // Construcao da lista de Pessoas que nao sao admins e estao suspensas
            foreach (var socio in _context.Socios)
            {
                if (socio.DataSuspensao != null && socio.Motivo != null)    // double check
                {
                    ListaIds.Add(socio.NumCC);
                }
            }
            foreach (var professor in _context.Professores)
            {
                if (professor.DataSuspensao != null && professor.Motivo != null)    // double check
                {
                    ListaIds.Add(professor.NumCC);
                }
            }


            return View(_context.Pessoas.Include(p => p.Socio).Include(p => p.Professor).Where(x => ListaIds.Contains(x.NumCC)));
        }

        public IActionResult LevantarSuspensao_Selecionado(string id)
        {
            var Pessoa = _context.Pessoas.Include(p => p.Socio).Include(p => p.Professor).SingleOrDefault(p => p.NumCC == id);
            if (Pessoa == null)
            {
                return RedirectToAction(nameof(SuspenderUtilizador));
            }
            var admin = _context.Admins.SingleOrDefault(a => a.NumCC == HttpContext.Session.GetString("UserId"));

            if (HelperFunctions.IsSocio(_context, id))
            {
                var socio = _context.Socios.SingleOrDefault(s => s.NumCC == id);
                socio.DataSuspensao = null;
                socio.Motivo = null;
                socio.NumAdmin = null;
                _context.Socios.Update(socio);
                admin.SociosSuspensos.Add(socio);
            }
            if (HelperFunctions.IsProfessor(_context, id))
            {
                var professor = _context.Professores.SingleOrDefault(p => p.NumCC == id);
                professor.DataSuspensao = null;
                professor.Motivo = null;
                professor.NumAdmin = null;
                _context.Professores.Update(professor);
                admin.ProfessoresSuspensos.Add(professor);
            }
            _context.Admins.Update(admin);
            _context.SaveChanges();
            return RedirectToAction(nameof(LevantarSuspensao));
        }
        #endregion

        #region Informaçoes Ginasio
        public IActionResult EditarInfoHealthUp()
        {
            return RedirectToAction(nameof(Index), "Ginasios");
        }
        #endregion

        #region Aulas de Grupo

        public IActionResult ListAulasGrupo()
        {
            return View(_context.AulasGrupo);
        }

        public IActionResult CreateAulaGrupo()
        {
            return View();
        }
        [HttpPost]
        [RequestSizeLimit(100_000_000)]
        public IActionResult CreateAulaGrupo(IFormCollection dados, IFormFile FotografiaDivulgacao, IFormFile VideoDivulgacao)
        {
            if (Path.GetExtension(FotografiaDivulgacao.FileName) != ".jpg") ModelState.AddModelError("FotografiaDivulgacao", "O formato do ficheiro tem de ser.jpg");
            if (Path.GetExtension(VideoDivulgacao.FileName) != ".mp4") ModelState.AddModelError("VideoDivulgacao", "O formato do ficheiro tem de ser .mp4");

            if (ModelState.IsValid)
            {

                string caminho = Path.Combine(_e.ContentRootPath, "wwwroot\\Ficheiros");
                string nome_ficheiro = Path.GetFileName(FotografiaDivulgacao.FileName);
                string caminho_completo = Path.Combine(caminho, nome_ficheiro);

                FileStream f = new FileStream(caminho_completo, FileMode.Create);
                FotografiaDivulgacao.CopyTo(f);

                f.Close();


                string caminho1 = Path.Combine(_e.ContentRootPath, "wwwroot\\Ficheiros");
                string nome_ficheiro1 = Path.GetFileName(VideoDivulgacao.FileName);
                string caminho_completo1 = Path.Combine(caminho1, nome_ficheiro1);

                FileStream ff = new FileStream(caminho_completo1, FileMode.Create);
                VideoDivulgacao.CopyTo(ff);

                ff.Close();




                AulaGrupo novo = new AulaGrupo();
                novo.Nome = dados["Nome"];
                novo.Descricao = dados["Descricao"];
                novo.FotografiaDivulgacao = Path.GetFileName(FotografiaDivulgacao.FileName);
                novo.VideoDivulgacao = Path.GetFileName(VideoDivulgacao.FileName);

                _context.AulasGrupo.Add(novo);

                _context.SaveChanges();

                return RedirectToAction("ListAulasGrupo");
            }


            return View();
        }
        public IActionResult EditAulaGrupo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aulaGrupo = _context.AulasGrupo.FirstOrDefault(m => m.IdAula == id);
            if (aulaGrupo == null)
            {
                return NotFound();
            }

            return View(aulaGrupo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> EditAulaGrupo(int id, IFormCollection dados, IFormFile FotografiaDivulgacao, IFormFile VideoDivulgacao)
        {
            if (FotografiaDivulgacao != null)
            {
                if (Path.GetExtension(FotografiaDivulgacao.FileName) != ".jpg") ModelState.AddModelError("FotografiaDivulgacao", "O formato do ficheiro tem de ser.jpg");
            }

            if (VideoDivulgacao != null)
            {
                if (Path.GetExtension(VideoDivulgacao.FileName) != ".mp4" && VideoDivulgacao != null) ModelState.AddModelError("VideoDivulgacao", "O formato do ficheiro tem de ser .mp4");
            }

            if (ModelState.IsValid)
            {
                AulaGrupo x = _context.AulasGrupo.FirstOrDefault(x => x.IdAula == id);
                try
                {
                    if (x.Nome != dados["Nome"]) x.Nome = dados["Nome"];
                    if (x.Descricao != dados["Descricao"]) x.Descricao = dados["Descricao"];
                    if (VideoDivulgacao != null)
                    {
                        if (x.VideoDivulgacao != Path.GetFileName(VideoDivulgacao.FileName))
                        {
                            string caminho = Path.Combine(_e.ContentRootPath, "wwwroot\\Ficheiros");
                            string nome_ficheiro = Path.GetFileName(VideoDivulgacao.FileName);
                            string caminho_completo = Path.Combine(caminho, nome_ficheiro);

                            FileStream f = new FileStream(caminho_completo, FileMode.Create);
                            VideoDivulgacao.CopyTo(f);

                            f.Close();

                            x.VideoDivulgacao = Path.GetFileName(VideoDivulgacao.FileName);
                        }
                    }
                    if (FotografiaDivulgacao != null)
                    {
                        if (x.FotografiaDivulgacao != Path.GetFileName(FotografiaDivulgacao.FileName))
                        {
                            string caminho = Path.Combine(_e.ContentRootPath, "wwwroot\\Ficheiros");
                            string nome_ficheiro = Path.GetFileName(FotografiaDivulgacao.FileName);
                            string caminho_completo = Path.Combine(caminho, nome_ficheiro);

                            FileStream f = new FileStream(caminho_completo, FileMode.Create);
                            FotografiaDivulgacao.CopyTo(f);

                            x.FotografiaDivulgacao = Path.GetFileName(FotografiaDivulgacao.FileName);

                        }
                    }

                    _context.Update(x);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AulaGrupoExists(x.IdAula))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ListAulasGrupo");
            }
            return View(_context.AulasGrupo.FirstOrDefault(x => x.IdAula == id));
        }

        public async Task<IActionResult> DeleteAulaGrupo(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aulaGrupo = await _context.AulasGrupo
                .FirstOrDefaultAsync(m => m.IdAula == id);
            if (aulaGrupo == null)
            {
                return NotFound();
            }

            return View(aulaGrupo);
        }

        // POST: asd/Delete/5
        [HttpPost, ActionName("DeleteAulaGrupo")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAulaGrupo(int id)
        {
            var aulaGrupo = await _context.AulasGrupo.FindAsync(id);
            _context.AulasGrupo.Remove(aulaGrupo);
            await _context.SaveChangesAsync();
            return RedirectToAction("ListAulasGrupo");
        }
        private bool AulaGrupoExists(int id)
        {
            return _context.AulasGrupo.Any(e => e.IdAula == id);
        }

        #endregion

        #region Aulas

        public async Task<IActionResult> ListAulas()
        {

            var healthUpContext = _context.Aulas.Include(a => a.AulaGrupoNavigation).Include(a => a.NumAdminNavigation).Include(a => a.NumProfessorNavigation);
            return View(await healthUpContext.ToListAsync());
        }

        public IActionResult CreateAula()
        {
            ViewData["NomeAula"] = new SelectList(_context.AulasGrupo, "IdAula", "Nome");
            ViewData["NomeProfessor"] = new SelectList(_context.Professores.Include(x => x.NumProfessorNavigation), "NumProfessorNavigation.NumCC", "NumProfessorNavigation.Nome");
            List<string> dias = new List<string>() { "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira", "Sábado", "Domingo" };
            ViewData["DiaSemana"] = new SelectList(dias);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAula(IFormCollection dados)
        {
            if (ModelState.IsValid)
            {
                Aula aula = new Aula();
                string x = dados["IdAula"];
                aula.AulaGrupoNavigation = _context.AulasGrupo.FirstOrDefault(x => x.IdAula == int.Parse(dados["IdAula"]));
                aula.NumAdminNavigation = _context.Admins.FirstOrDefault(x => x.NumCC == HttpContext.Session.GetString("UserId"));
                string idP = dados["IdProfessor"];
                aula.NumProfessorNavigation = _context.Professores.First(x => x.NumCC == idP);
                aula.DiaSemana = HelperFunctions.GetDay(dados["DiaSemana"]);
                aula.HoraInicio = TimeSpan.Parse(dados["HoraInicio"]);
                aula.Lotacao = int.Parse(dados["Lotacao"]);
                aula.ValidoDe = DateTime.Parse(dados["ValidoDe"]);
                if (string.IsNullOrEmpty(dados["ValidoAte"]))
                {
                    aula.ValidoAte = new DateTime(2050, 1, 1);
                }
                else
                {
                    aula.ValidoAte = DateTime.Parse(dados["ValidoAte"]);
                }
                _context.Add(aula);
                _context.SaveChanges();
                return RedirectToAction(nameof(ListAulas));

            }
            ViewData["NomeAula"] = new SelectList(_context.AulasGrupo, "IdAula", "Nome");
            ViewData["NomeProfessor"] = new SelectList(_context.Professores.Include(x => x.NumProfessorNavigation), "NumProfessorNavigation.NumCC", "NumProfessorNavigation.Nome");
            List<string> dias = new List<string>() { "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira", "Sábado", "Domingo" };
            ViewData["DiaSemana"] = new SelectList(dias);
            return View();
        }

        public async Task<IActionResult> EditAula(int? id)
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
            ViewData["NomeAula"] = new SelectList(_context.AulasGrupo, "IdAula", "Nome");
            ViewData["NomeProfessor"] = new SelectList(_context.Professores.Include(x => x.NumProfessorNavigation), "NumProfessorNavigation.NumCC", "NumProfessorNavigation.Nome");
            List<string> dias = new List<string>() { "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira", "Sábado", "Domingo" };
            ViewData["DiaSemana"] = new SelectList(dias);
            return View(aula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAula(int id, IFormCollection dados)
        {

            Aula aula = _context.Aulas.First(x => x.IdAula == id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (aula.HoraInicio != TimeSpan.Parse(dados["HoraInicio"])) aula.HoraInicio = TimeSpan.Parse(dados["HoraInicio"]);
                    if (aula.Lotacao != int.Parse(dados["Lotacao"])) aula.Lotacao = int.Parse(dados["Lotacao"]);
                    string idP = dados["IdProfessor"];
                    if (aula.NumProfessor != idP) aula.NumProfessor = idP;
                    if (aula.DiaSemana != HelperFunctions.GetDay(dados["DiaSemana"])) aula.DiaSemana = HelperFunctions.GetDay(dados["DiaSemana"]);
                    if (aula.ValidoAte != DateTime.Parse(dados["ValidoAte"]))
                    {
                        if (string.IsNullOrEmpty(dados["ValidoAte"]))
                        {
                            aula.ValidoAte = new DateTime(2050, 1, 1);
                        }
                        else
                        {
                            aula.ValidoAte = DateTime.Parse(dados["ValidoAte"]);
                        }
                    }
                    if (aula.ValidoDe != DateTime.Parse(dados["ValidoDe"])) aula.ValidoDe = DateTime.Parse(dados["ValidoDe"]);

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
                return RedirectToAction(nameof(ListAulas));
            }
            ViewData["NomeAula"] = new SelectList(_context.AulasGrupo, "IdAula", "Nome");
            ViewData["NomeProfessor"] = new SelectList(_context.Professores.Include(x => x.NumProfessorNavigation), "NumProfessorNavigation.NumCC", "NumProfessorNavigation.Nome");
            List<string> dias = new List<string>() { "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira", "Sábado", "Domingo" };
            ViewData["DiaSemana"] = new SelectList(dias);
            return View(aula);
        }

        public async Task<IActionResult> DeleteAula(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aula = await _context.Aulas
                .Include(a => a.AulaGrupoNavigation)
                .Include(a => a.NumAdminNavigation)
                .Include(a => a.NumProfessorNavigation)
                .FirstOrDefaultAsync(m => m.IdAula == id);
            if (aula == null)
            {
                return NotFound();
            }

            return View(aula);
        }

        [HttpPost, ActionName("DeleteAula")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAula(int id)
        {
            var aula = await _context.Aulas.FindAsync(id);
            _context.Aulas.Remove(aula);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListAulas));
        }

        private bool AulaExists(int id)
        {
            return _context.Aulas.Any(e => e.IdAula == id);
        }


        #endregion
    }
}