using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Helpers;
using HealthUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil ="Admin")]
    public class AdminController : Controller
    {
        #region PrivateVariables
        private readonly HealthUpContext _context;
        #endregion


        #region Constructors
        public AdminController(HealthUpContext context)
        {
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
                DataRegisto = DateTime.Now,
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

            // Apagar da tabela
            RejeitarSocio(id);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RejeitarSocio(int id)
        {
            var pedido = _context.PedidosSocios.FirstOrDefault(p => p.NumCC == id.ToString());
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
            var solicitacao = _context.SolicitacaoProfessores.Include(s => s.Socio).ThenInclude(s => s.NumSocioNavigation).Include(s => s.Professor).ThenInclude(p => p.NumProfessorNavigation).Include(a => a.NumAdminNavigation).ThenInclude(x => x.NumAdminNavigation).FirstOrDefault(s => s.IdSolicitacao == id.ToString());

            // Atribuir o ID do admin a esta solicitacao
            solicitacao.NumAdmin = HttpContext.Session.GetString("UserId");

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
            var solicitacao = _context.SolicitacaoProfessores.FirstOrDefault(p => p.IdSolicitacao == id.ToString());
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
                _context.Professores.Update(professor);
                admin.ProfessoresSuspensos.Add(professor);
            }
            _context.Admins.Update(admin);
            _context.SaveChanges();
            return RedirectToAction(nameof(SuspenderUtilizador));
        }
        #endregion

        #region Aulas
        public IActionResult Aulas()
        {
            return View(_context.Aulas.ToList());
        }
        #endregion

        #region AulasDeGrupo
        public IActionResult  AulasGrupo()
        {
            return View(_context.AulasGrupo.ToList());
        }
        #endregion
    }
}