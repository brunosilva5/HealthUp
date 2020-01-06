using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil ="Socio")]
    public class SociosController : Controller
    {
        #region PrivateVariables
        private readonly HealthUpContext _context;
        private readonly IHostEnvironment _he;
        #endregion

        #region Constructors
        public SociosController(HealthUpContext context, IHostEnvironment he)
        {
            _context = context;
            _he = he;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View(_context.Socios.Include(s=>s.NumProfessorNavigation).SingleOrDefault(s=>s.NumCC==HttpContext.Session.GetString("UserId")));
        }

        #endregion

        #region SolicitarPersonalTrainer
        public IActionResult SolicitarPT()
        {
            List<Pessoa> Lista = new List<Pessoa>();
            // Obter lista de nomes de professores + especialidade 
            foreach (var pessoa in _context.Pessoas.Include(p => p.Professor))
            {
                if (pessoa.Professor != null)
                {
                    Lista.Add(pessoa);
                }
            }
            ViewBag.Professores = Lista.Select(c => new SelectListItem()
            {
                Text = c.Nome + " | Especialidade: " + _context.Professores.SingleOrDefault(s => s.NumCC == c.NumCC).Especialidade,
                Value = c.NumCC
            });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SolicitarPT(string ProfessorEscolhido)
        {
            var Prof = _context.Professores.SingleOrDefault(p => p.NumCC == ProfessorEscolhido);
            var Socio = _context.Socios.Include(s=>s.NumProfessorNavigation).SingleOrDefault(s => s.NumCC == HttpContext.Session.GetString("UserId"));
            SolicitacaoProfessor solicitacao = new SolicitacaoProfessor();

            if (_context.Socios.SingleOrDefault(s=>s.NumCC==HttpContext.Session.GetString("UserId")).ID_Solicitacao!=null)
            {
                ModelState.AddModelError("", "Já existe um pedido de personal trainer pendente! Aguarde a aprovação ou rejeição por parte de um administrador!");
            }
            if (ModelState.IsValid)
            {
                 
                solicitacao.Professor.Add(Prof);
                solicitacao.Socio.Add(Socio);
                solicitacao.Data = DateTime.Now;

                _context.Add(solicitacao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Passar para a view de novo a lista de professores
            List<Pessoa> Lista = new List<Pessoa>();
            // Obter lista de nomes de professores + especialidade 
            foreach (var pessoa in _context.Pessoas.Include(p => p.Professor))
            {
                if (pessoa.Professor != null)
                {
                    Lista.Add(pessoa);
                }
            }
            ViewBag.Professores = Lista.Select(c => new SelectListItem()
            {
                Text = c.Nome + " | Especialidade: " + _context.Professores.SingleOrDefault(s => s.NumCC == c.NumCC).Especialidade,
                Value = c.NumCC
            }); 
            return View(solicitacao);
        }
        #endregion

        #region ConsultarHistoricoPeso
        public IActionResult ConsultarHistoricoPeso()
        {
            // construcao da lista de historico de pesos para construcao do grafico
            List<SimpleReportViewModel> lstModel = GetHistoricoPeso();
            

            return View(lstModel);
        }
        public IActionResult HistoricoPesoParaExcel()
        {
            
            List<SimpleReportViewModel> lstModel = GetHistoricoPeso();

            byte[] fileContents;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Put whatever you want here in the sheet
                // For example, for cell on row1 col1
                int i = 0;
                int totalRows = lstModel.Count();
                worksheet.Cells[1, 1].Value = "Data";
                worksheet.Cells[1, 2].Value = "Peso";

                for (int row = 2; row <= totalRows + 1; row++)
                {
                    worksheet.Cells[row, 1].Value = lstModel[i].DimensionOne;
                    worksheet.Cells[row, 2].Value = lstModel[i].Quantity;
                    i++;
                }

                // So many things you can try but you got the idea.

                // Finally when you're done, export it to byte array.
                fileContents = package.GetAsByteArray();
            }

            if (fileContents == null || fileContents.Length == 0)
            {
                return RedirectToAction(nameof(ConsultarHistoricoPeso));
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "HistoricoPesoHealthUp.xlsx"
            );
        }
        private List<SimpleReportViewModel> GetHistoricoPeso()
        {
            return _context.Socios.SingleOrDefault(x => x.NumCC == HttpContext.Session.GetString("UserId")).GetHistoricoPeso(_context);
        }

        #endregion

        #region ConsultarInfoPT
        public IActionResult ConsultarInfoPT()
        {
            var x = _context.Socios.Include(s => s.NumProfessorNavigation).ThenInclude(s => s.NumProfessorNavigation).SingleOrDefault(x => x.NumCC == HttpContext.Session.GetString("UserId")).NumProfessorNavigation.NumProfessorNavigation;
            return View(x);
        }

        
        public IActionResult DeixarSerAluno(string NumProf)
        {
            var prof = _context.Professores.SingleOrDefault(x => x.NumCC == NumProf);
            var socio = _context.Socios.SingleOrDefault(x => x.NumCC == HttpContext.Session.GetString("UserId"));

            socio.NumProfessor = null;
            socio.NumProfessorNavigation = null;

            prof.Socio.Remove(socio);

            _context.Socios.Update(socio);
            _context.Professores.Update(prof);

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region HistoricoAulas
#warning "CODIGO AINDA NAO TESTADO"
        public IActionResult HistoricoAulas()
        {
            var socio=_context.Socios.SingleOrDefault(s => s.NumCC == HttpContext.Session.GetString("UserId"));
            // load de todas as properties usadas por esta collection
            _context.Entry(socio).Collection(p => p.Inscreve).Load();
            return View(socio.Inscreve);
        }
        #endregion

        #region ConsultarPlanoTreino
#warning "CODIGO AINDA NAO TESTADO"

        public IActionResult ConsultarPlanoTreino()
        {
            var socio = _context.Socios.SingleOrDefault(s => s.NumCC == HttpContext.Session.GetString("UserId"));
            // load de todas as properties usadas por esta collection
            _context.Entry(socio).Collection(p => p.PlanoTreino).Load();
            if (socio.PlanoTreino.Any()==false)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(socio.PlanoTreino);

        }
        #endregion

        #region Inscrever Aula

        public IActionResult ListarAulas()
        {
            return View();
        }

        public IActionResult ListarAulasByData(DateTime data)
        {
            int dia = (int)data.DayOfWeek;
            var lista = _context.Aulas.Include(x => x.AulaGrupoNavigation).Include(x=>x.Inscreve);
            lista.Where(x => x.AulaGrupoNavigation.Aula.DiaSemana == dia && x.ValidoAte>data && x.ValidoDe<data );
            ViewBag.Socio = _context.Socios.FirstOrDefault(x => x.NumCC == HttpContext.Session.GetString("UserId")).NumCC;
            return PartialView(nameof(ListarAulasByData),lista.ToList());

        }

        public IActionResult Inscrever(int aula)
        {
            Inscreve i = new Inscreve();
            i.IdAula = aula;
            i.NumSocio = HttpContext.Session.GetString("UserId");
            _context.Inscricoes.Add(i);
            _context.SaveChanges();
            return View(nameof(ListarAulas));

        }

        public IActionResult Desinscrever(int aula)
        {
            Inscreve i = new Inscreve();
            i.IdAula = aula;
            i.NumSocio = HttpContext.Session.GetString("UserId");
            _context.Inscricoes.Remove(i);
            _context.SaveChanges();
            return View(nameof(ListarAulas));

        }
        #endregion
    }
}

    
