using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;

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
            var Socio = _context.Socios.SingleOrDefault(s => s.NumCC == HttpContext.Session.GetString("UserId"));
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

        private List<SimpleReportViewModel> GetHistoricoPeso()
        {
            List<Socio> Lista = new List<Socio>();
            var lstModel = new List<SimpleReportViewModel>();
            foreach (var professor in _context.Professores.Include(x => x.Socio))
            {
                foreach (var SocioRegistado in professor.Socio)
                {
                    if (SocioRegistado.NumCC == HttpContext.Session.GetString("UserId"))
                    {
                        Lista.Add(SocioRegistado);
                        
                    }
                }
            }
            Lista.OrderByDescending(x => x.DataRegisto_Peso);
            foreach (var item in Lista)
            {
                lstModel.Add(new SimpleReportViewModel()
                {
                    DimensionOne = item.DataRegisto_Peso.Date.ToString(),
                    Quantity = item.Peso
                });
            }
            
            return lstModel;
        }
        public IActionResult HistoricoPesoParaExcel()
        {
            List<SimpleReportViewModel> lstModel = GetHistoricoPeso();
            
            var socio = _context.Pessoas.SingleOrDefault(s => s.NumCC == HttpContext.Session.GetString("UserId"));
            string rootFolder = _he.ContentRootPath;
            string fileName = @"HistoricoPeso_HealthUp.xls";

            FileInfo file = new FileInfo(Path.Combine(rootFolder, fileName));

            using (ExcelPackage package = new ExcelPackage(file))
            {


                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("HistoricoPeso_"+socio.Nome);
                int totalRows = lstModel.Count();

                worksheet.Cells[1, 1].Value = "Data";
                worksheet.Cells[1, 2].Value = "Peso";
                    
                int i = 0;
                for (int row = 2; row <= totalRows + 1; row++)
                {
                    worksheet.Cells[row, 1].Value = lstModel[i].DimensionOne;
                    worksheet.Cells[row, 2].Value = lstModel[i].Quantity;
                    i++;
                }

                package.Save();

            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(file.FullName);
            return File(fileBytes, "application/force-download", fileName);

        }
        #endregion
    }
}