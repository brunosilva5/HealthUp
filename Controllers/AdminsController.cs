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
    [MyRoleFilter(Perfil = "Admin")]
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
            Pessoa P = new Pessoa
            {
                DataNascimento = pedido.DataNascimento,
                NumCC = pedido.NumCC,
                Email = pedido.Email,
                Fotografia = pedido.Fotografia,
                Sexo = pedido.Sexo,
                Username = pedido.Username,
                Nome = pedido.Nome,
                Nacionalidade = pedido.Nacionalidade,
                NumAdmin = (HttpContext.Session.GetString("UserId")),
                Telemovel = pedido.Telemovel
            };
            Socio S = new Socio()
            {
                
                NumCC = P.NumCC,
                NumSocioNavigation = P,
                NumAdmin = (HttpContext.Session.GetString("UserId")),
                NumAdminNavigation = _context.Admins.FirstOrDefault(a => a.NumCC == (HttpContext.Session.GetString("UserId"))),
                
            };
            S.Cotas = new Cota(S.NumCC);
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


        #region Gerir Pessoas
        public IActionResult GerirPessoas()
        {
            return View(_context.Pessoas.Include(p => p.Admin).Include(p => p.Professor).Include(p => p.Socio).Where(p=>p.NumCC != HttpContext.Session.GetString("UserId")).ToList());
        }

        public IActionResult CriarAdmin(string id)
        {
            Pessoa p = _context.Pessoas.Include(p => p.Admin).Include(p => p.Professor).Include(p => p.Socio).FirstOrDefault(p => p.NumCC == id);

            // era professor
            if (p.Professor!=null)
            {
                _context.Professores.Remove(p.Professor);
            }

            // era socio
            if (p.Socio != null)
            {
                _context.Socios.Remove(p.Socio);
            }
            // tornar admin
            p.Admin = new Admin(p);

            _context.Admins.Add(p.Admin);
            _context.Pessoas.Update(p);

            _context.SaveChanges();
            return RedirectToAction(nameof(GerirPessoas));
        }

        public IActionResult CriarProfessor(string id)
        {
            Pessoa p = _context.Pessoas.Include(p => p.Admin).Include(p => p.Professor).Include(p => p.Socio).FirstOrDefault(p => p.NumCC == id);
            p.Admin = null;
            p.Professor = new Professor(p);
            p.Socio = null;
            _context.Professores.Add(p.Professor);
            _context.Update(p);
            _context.SaveChanges();
            return RedirectToAction(nameof(GerirPessoas));
        }

        public IActionResult CriarSocio(string id)
        {
            Pessoa p = _context.Pessoas.Include(p=>p.Admin).Include(p=>p.Professor).Include(p=>p.Socio).FirstOrDefault(p => p.NumCC == id);
            p.Admin = null;
            p.Professor = null;
            p.Socio = new Socio(p);
            _context.Socios.Add(p.Socio);
            _context.Update(p);
            _context.SaveChanges();
            return RedirectToAction(nameof(GerirPessoas));
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

            return RedirectToAction(nameof(AprovarPedidoProf));
        }

        public IActionResult PedidoProf_Rejeitado(int id)
        {
            var solicitacao = _context.SolicitacaoProfessores.FirstOrDefault(p => p.IdSolicitacao == id);
            _context.SolicitacaoProfessores.Remove(solicitacao);
            _context.SaveChanges();

            return RedirectToAction(nameof(AprovarPedidoProf));
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
        [RequestSizeLimit(1048576000)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CriarExercicio(string Nome, string Descricao, IFormFile Fotografia, IFormFile Video)
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
                var admin = _context.Admins.Include(x => x.Exercicio).Include(x => x.NumAdminNavigation).SingleOrDefault(x => x.NumCC == HttpContext.Session.GetString("UserId"));
                admin.Exercicio.Add(exercicio);
                _context.Admins.Update(admin);
                // --------------------------------------------------------------------------------------------------------------------------------------

                exercicio.NumAdmin = _context.Admins.Include(x => x.NumAdminNavigation).SingleOrDefault(a => a.NumCC == HttpContext.Session.GetString("UserId")).NumCC;

                _context.Add(exercicio);
                await _context.SaveChangesAsync();

                //guardar ficheiros no wwwroot
                string caminho = Path.Combine(_e.ContentRootPath, "wwwroot\\Ficheiros");
                string nome_ficheiro = Path.GetFileName(Fotografia.FileName);
                string caminho_completo = Path.Combine(caminho, nome_ficheiro);

                FileStream f = new FileStream(caminho_completo, FileMode.Create);
                Fotografia.CopyTo(f);

                f.Close();


                string caminho1 = Path.Combine(_e.ContentRootPath, "wwwroot\\Ficheiros");
                string nome_ficheiro1 = Path.GetFileName(Video.FileName);
                string caminho_completo1 = Path.Combine(caminho1, nome_ficheiro1);

                FileStream ff = new FileStream(caminho_completo1, FileMode.Create);
                Video.CopyTo(ff);

                ff.Close();

                return RedirectToAction(nameof(Index), "Home");
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


        #region Aulas
        public async Task<IActionResult> ListAulas()
        {

            var healthUpContext = _context.Aulas.Include(a => a.NumAdminNavigation).ThenInclude(a=>a.NumAdminNavigation).Include(a => a.NumProfessorNavigation).ThenInclude(p=>p.NumProfessorNavigation);
            return View(await healthUpContext.ToListAsync());
        }

        public IActionResult CreateAula()
        {
            ViewData["NomeProfessor"] = new SelectList(_context.Professores.Include(x => x.NumProfessorNavigation), "NumProfessorNavigation.NumCC", "NumProfessorNavigation.Nome");
            List<string> dias = new List<string>() { "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira", "Sábado", "Domingo" };
            ViewData["DiaSemana"] = new SelectList(dias);
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(100_000_000)]
        public IActionResult CreateAula(IFormCollection dados, IFormFile FotografiaDivulgacao, IFormFile VideoDivulgacao)
        {
            if (Path.GetExtension(FotografiaDivulgacao.FileName) != ".jpg") ModelState.AddModelError("FotografiaDivulgacao", "O formato do ficheiro tem de ser.jpg");
            if (Path.GetExtension(VideoDivulgacao.FileName) != ".mp4") ModelState.AddModelError("VideoDivulgacao", "O formato do ficheiro tem de ser .mp4");
            if (DateTime.Parse(dados["ValidoDe"]) > DateTime.Parse(dados["ValidoAte"])) ModelState.AddModelError("ValidoAte", "A validade está incorreta");

            Aula aula = new Aula
            {
                NumAdminNavigation = _context.Admins.FirstOrDefault(x => x.NumCC == HttpContext.Session.GetString("UserId")),

                // Guardar o id do admin que criou
                NumAdmin = HttpContext.Session.GetString("UserId"),
                // Guardar o professor associado a esta aula
                NumProfessor = dados["IdProfessor"],
                // alterar
                DiaSemana = HelperFunctions.GetDay(dados["DiaSemana"]),

                HoraInicio = TimeSpan.Parse(dados["HoraInicio"]),
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

                return RedirectToAction(nameof(ListAulas));

            }
            ViewData["NomeProfessor"] = new SelectList(_context.Professores.Include(x => x.NumProfessorNavigation), "NumProfessorNavigation.NumCC", "NumProfessorNavigation.Nome");
            List<string> dias = new List<string>() { "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira", "Sábado", "Domingo" };
            ViewData["DiaSemana"] = new SelectList(dias);
            return View(aula);
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
            ViewData["NomeProfessor"] = new SelectList(_context.Professores.Include(x => x.NumProfessorNavigation), "NumProfessorNavigation.NumCC", "NumProfessorNavigation.Nome");
            List<string> dias = new List<string>() { "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira", "Sábado", "Domingo" };
            ViewData["DiaSemana"] = new SelectList(dias);
            return View(aula);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> EditAula(int id, IFormCollection dados, IFormFile FotografiaDivulgacao, IFormFile VideoDivulgacao)
        {
            if (Path.GetExtension(FotografiaDivulgacao.FileName) != ".jpg") ModelState.AddModelError("FotografiaDivulgacao", "O formato do ficheiro tem de ser.jpg");
            if (Path.GetExtension(VideoDivulgacao.FileName) != ".mp4") ModelState.AddModelError("VideoDivulgacao", "O formato do ficheiro tem de ser .mp4");
            if (DateTime.Parse(dados["ValidoDe"]) > DateTime.Parse(dados["ValidoAte"])) ModelState.AddModelError("ValidoAte", "A validade está incorreta");



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
                    aula.Nome = dados["Nome"];
                    aula.Descricao = dados["Descricao"];
                    aula.FotografiaDivulgacao = Path.GetFileName(FotografiaDivulgacao.FileName);
                    aula.VideoDivulgacao = Path.GetFileName(VideoDivulgacao.FileName);

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
            //guardar ficheiros no wwwroot
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

            ViewData["NomeProfessor"] = new SelectList(_context.Professores.Include(x => x.NumProfessorNavigation), "NumProfessorNavigation.NumCC", "NumProfessorNavigation.Nome");
            List<string> dias = new List<string>() { "Segunda-Feira", "Terça-Feira", "Quarta-Feira", "Quinta-Feira", "Sexta-Feira", "Sábado", "Domingo" };
            ViewData["DiaSemana"] = new SelectList(dias);
            return View(aula);
        }

        public IActionResult DetailsAula(int id)
        {
            Aula a = _context.Aulas.Include(x => x.NumProfessorNavigation).Include(x => x.NumAdminNavigation).FirstOrDefault(x => x.IdAula == id);
            ViewBag.Admin = _context.Pessoas.First(x => x.NumCC == a.NumAdmin).Nome;
            ViewBag.Professor = _context.Pessoas.First(x=>x.NumCC == a.NumProfessor).Nome;
            return View(a);
        }
        public async Task<IActionResult> DeleteAula(int? id)
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
            ViewBag.Admin = _context.Pessoas.First(x => x.NumCC == aula.NumAdmin).Nome;
            ViewBag.Professor = _context.Pessoas.First(x => x.NumCC == aula.NumProfessor).Nome;
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

        #region Cotas
        public IActionResult GerirCotas()
        {
            var lista = _context.Socios.Include(x=>x.NumSocioNavigation).Include(s=>s.Cotas).ToList();
            return View(lista);
        }

        public IActionResult AcertarCotas(string id)
        {
            
            Cota cotas = _context.Cota.Include(c => c.NumSocioNavigation).SingleOrDefault(c => c.NumSocio==id);
            cotas.AcertarCotas();
            _context.Update(cotas);
            _context.SaveChanges();

            return RedirectToAction(nameof(GerirCotas));
        }


        #endregion
    }
}