using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Helpers;
using HealthUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Controllers
{
    [MyRoleFilter(Perfil = "Professor, Socio")]
    public class MensagensController : BaseController
    {
        private readonly HealthUpContext _context;

        public MensagensController(HealthUpContext context)
        {
            _context = context;
        }

        #region Caixa de Entrada
        public async Task<IActionResult> CaixaEntrada(bool? arquivadas = false)
        {

            if (arquivadas == true)
            {
                ViewBag.Arquivadas = true;
            }
            IOrderedQueryable<Mensagem> mensagens = _context.Mensagens.Include(m => m.IdPessoaReceiverNavigation).Include(m => m.IdPessoaSenderNavigation).Where(x => x.IdPessoaReceiver == HttpContext.Session.GetString("UserId") && x.Arquivada_Receiver == arquivadas).OrderByDescending(x => x.DataEnvio);

            return View(await mensagens.ToListAsync());
        }
        #endregion
        #region Caixa de Saída
        public async Task<IActionResult> CaixaSaida(bool? arquivadas = false)
        {
            if (arquivadas == true)
            {
                ViewBag.Arquivadas = true;
            }
            IOrderedQueryable<Mensagem> mensagens = _context.Mensagens.Include(m => m.IdPessoaReceiverNavigation).Include(m => m.IdPessoaSenderNavigation).Where(x => x.IdPessoaSender == HttpContext.Session.GetString("UserId") && x.Arquivada_Sender == arquivadas).OrderByDescending(x => x.DataEnvio);
            return View(await mensagens.ToListAsync());
        }
        #endregion
        #region Ler Mensagem
        public IActionResult LerMensagem(int IdMensagem, bool? arquivadas = false)
        {
            if (arquivadas == true)
            {
                ViewBag.Arquivadas = true;
            }

            Mensagem Msg = _context.Mensagens.Include(m => m.IdPessoaSenderNavigation).Include(m => m.IdPessoaReceiverNavigation).SingleOrDefault(x => x.IdMensagem == IdMensagem);
            if (Msg.Lida == false && Msg.IdPessoaReceiver == HttpContext.Session.GetString("UserId"))
            {
                Msg.Lida = true;
                _context.Update(Msg);
                _context.SaveChanges();
            }

            return View(Msg);
        }
        #endregion

        #region Ver Mensagem
        public IActionResult VerMensagem(int IdMensagem)
        {
            Mensagem Message = _context.Mensagens.Include(x => x.IdPessoaReceiverNavigation).Include(x => x.IdPessoaSenderNavigation).SingleOrDefault(x => x.IdMensagem == IdMensagem);

            return View(Message);
        }
        #endregion
        #region Responder Mensagem
        [AjaxOnly]
        public IActionResult Responder(int Destino)
        {
            ViewBag.Destino = Destino;
            return View();
        }

        [AjaxOnly]
        [HttpPost]
        public IActionResult Responder(string Conteudo, string Destino)
        {

            if (ModelState.IsValid)
            {
                Pessoa Receiver = _context.Pessoas.SingleOrDefault(x => x.NumCC == Destino);
                Pessoa Sender = _context.Pessoas.SingleOrDefault(x => x.NumCC == HttpContext.Session.GetString("UserId"));

                DateTime date = DateTime.Now;
                date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);
                Mensagem Msg = new Mensagem
                {
                    DataEnvio = date,
                    IdPessoaSender = Sender.NumCC,
                    IdPessoaReceiver = Receiver.NumCC,
                    Conteudo = Conteudo,
                };
                Receiver.MensagensEntrada.Add(Msg);
                _context.Mensagens.Add(Msg);

                _context.Pessoas.Update(Receiver);

                _context.SaveChanges();

                return View("RespostaEnviada");
            }
            return View();
        }
        #endregion

        #region Enviar Mensagem

        [Socios_PTs_Filter(Pessoa = "Socio, Professor", DeixarAcederSeTiver = true)]
        public IActionResult EnviarMensagem()
        {
            // verificar se é sócio 
            if (HelperFunctions.IsSocio(_context, HttpContext.Session.GetString("UserId")))
            {
                Professor personal_trainer = _context.Socios.Include(s => s.NumProfessorNavigation).ThenInclude(p => p.NumProfessorNavigation).SingleOrDefault(s => s.NumCC == HttpContext.Session.GetString("UserId")).NumProfessorNavigation;

                ViewBag.Professor = new List<SelectListItem>(){
                    (new SelectListItem(personal_trainer.NumProfessorNavigation.Nome + " | Especialidade: " + personal_trainer.Especialidade, personal_trainer.NumCC))
                };


            }
            // verificar se é professor
            if (HelperFunctions.IsProfessor(_context, HttpContext.Session.GetString("UserId")))
            {
                List<Socio> ListaSocios = _context.Professores.Include(s => s.Socio).ThenInclude(p => p.NumSocioNavigation).SingleOrDefault(s => s.NumCC == HttpContext.Session.GetString("UserId")).Socio.ToList();
                ViewBag.Socios = ListaSocios.Select(s => new SelectListItem()
                {
                    Text = "CC: " + s.NumCC + " | " + s.NumSocioNavigation.Nome,
                    Value = s.NumCC
                });
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Socios_PTs_Filter(Pessoa = "Socio, Professor", DeixarAcederSeTiver = true)]
        public IActionResult EnviarMensagem(IFormCollection Dados)
        {

            if (ModelState.IsValid)
            {
                Pessoa Receiver = _context.Pessoas.Include(p => p.MensagensEntrada).Include(p => p.MensagensSaida).SingleOrDefault(p => p.NumCC == Dados["Destino"].ToString());
                Pessoa Sender = _context.Pessoas.Include(p => p.MensagensEntrada).Include(p => p.MensagensSaida).SingleOrDefault(p => p.NumCC == HttpContext.Session.GetString("UserId"));
                Mensagem Msg = new Mensagem()
                {
                    DataEnvio = DateTime.Now,
                    Conteudo = Dados["Conteudo"],
                    IdPessoaReceiver = Receiver.NumCC,
                    IdPessoaSender = Sender.NumCC
                };
                Receiver.MensagensEntrada.Add(Msg);
                Sender.MensagensSaida.Add(Msg);

                _context.Pessoas.Update(Receiver);
                _context.Pessoas.Update(Sender);
                _context.Mensagens.Add(Msg);
                _context.SaveChanges();
                return RedirectToAction(nameof(CaixaSaida));

            }
            // verificar se é sócio 
            if (HelperFunctions.IsSocio(_context, HttpContext.Session.GetString("UserId")))
            {
                Professor personal_trainer = _context.Socios.Include(s => s.NumProfessorNavigation).ThenInclude(p => p.NumProfessorNavigation).SingleOrDefault(s => s.NumCC == HttpContext.Session.GetString("UserId")).NumProfessorNavigation;

                ViewBag.Professor = new List<SelectListItem>(){
                    (new SelectListItem(personal_trainer.NumProfessorNavigation.Nome + " | Especialidade: " + personal_trainer.Especialidade, personal_trainer.NumCC))
                };


            }
            // verificar se é professor
            if (HelperFunctions.IsProfessor(_context, HttpContext.Session.GetString("UserId")))
            {
                List<Socio> ListaSocios = _context.Professores.Include(s => s.Socio).ThenInclude(p => p.NumSocioNavigation).SingleOrDefault(s => s.NumCC == HttpContext.Session.GetString("UserId")).Socio.ToList();
                ViewBag.Socios = ListaSocios.Select(s => new SelectListItem()
                {
                    Text = "CC: " + s.NumCC + " | " + s.NumSocioNavigation.Nome,
                    Value = s.NumCC
                });
            }

            return View();
        }
        #endregion

        #region Arquivar Mensagens
        public IActionResult ArquivarMensagensEntrada()
        {
            IEnumerable<Mensagem> Msgs = _context.Pessoas.Include(p => p.MensagensEntrada).SingleOrDefault(p => p.NumCC == HttpContext.Session.GetString("UserId")).MensagensEntrada.Where(m => m.Arquivada_Receiver == false);
            foreach (Mensagem msg in Msgs)
            {
                msg.ArquivarMensagem_Receiver();
                _context.Mensagens.Update(msg);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(CaixaEntrada));
        }

        public IActionResult ArquivarMensagensSaida()
        {
            IEnumerable<Mensagem> Msgs = _context.Pessoas.Include(p => p.MensagensSaida).SingleOrDefault(p => p.NumCC == HttpContext.Session.GetString("UserId")).MensagensSaida.Where(m => m.Arquivada_Sender == false);
            foreach (Mensagem msg in Msgs)
            {
                msg.ArquivarMensagem_Sender();
                _context.Mensagens.Update(msg);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(CaixaSaida));
        }


        #endregion

        #region Ver Mensagens Arquivadas
        public IActionResult VerMensagensEntradaArquivadas()
        {
            return RedirectToAction(nameof(CaixaEntrada), new { arquivadas = true });
        }
        public IActionResult VerMensagensSaidaArquivadas()
        {
            return RedirectToAction(nameof(CaixaSaida), new { arquivadas = true });
        }

        #endregion
    }
}
