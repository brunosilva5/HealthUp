using HealthUp.Data;
using HealthUp.Filters;
using HealthUp.Helpers;
using HealthUp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HealthUp.Controllers
{
    public class UtilizadoresController : BaseController
    {
        private readonly HealthUpContext _context;
        public UtilizadoresController(HealthUpContext contexto)
        {
            _context = contexto;

        }
        #region PedidoSocio
        [NaoAutenticado]
        public IActionResult PedidoSocio()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NaoAutenticado]
        public IActionResult PedidoSocio(IFormCollection data)
        {
            Microsoft.Extensions.Primitives.StringValues indicativo = data["Indicativo"];
            string telemovel = HelperFunctions.NormalizeWhiteSpace(data["Telemovel"]);
            string nome = HelperFunctions.NormalizeWhiteSpace(data["Nome"]);


            if (ModelState.IsValid)
            {
                PedidoSocio p = new PedidoSocio()
                {
                    DataNascimento = DateTime.Parse(data["DataNascimento"]),
                    Email = data["Email"],
                    Fotografia = data["Fotografia"],
                    Nacionalidade = data["Nacionalidade"],
                    Nome = nome,
                    Sexo = data["sexo"],
                    Username = data["Username"],
                    Telemovel = new string("+" + indicativo + telemovel),
                    NumCC = data["NumCC"]

                };
                _context.PedidosSocios.Add(p);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        #endregion

        #region Login
        [NaoAutenticado]
        public IActionResult Login()
        {
            return View();
        }

        [NaoAutenticado]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(IFormCollection data)
        {
            string Password = data["Password"];
            string Username = data["Username"];

            Pessoa p = _context.Pessoas.Include(p => p.Admin).Include(p => p.Professor).Include(p => p.Socio).SingleOrDefault(p => p.Username == Username);

            if (ModelState.IsValid)
            {
                // Definir a password (primeiro login)
                if (p.Password == null)
                {
                    p.Password = Password;
                    _context.Pessoas.Update(p);
                    _context.SaveChanges();
                }

                HttpContext.Session.SetString("Nome", p.Nome);
                HttpContext.Session.SetString("UserId", p.NumCC);

                if (p.Admin != null)
                {
                    HttpContext.Session.SetString("Role", "Admin");
                }

                if (p.Socio != null)
                {
                    HttpContext.Session.SetString("Role", "Socio");

                    if (p.Socio.NumProfessor != null)
                    {
                        HttpContext.Session.SetString("ExistePT", "Sim");
                    }
                    else
                    {
                        HttpContext.Session.SetString("ExistePT", "Nao");
                    }
                }
                if (p.Professor != null)
                {
                    HttpContext.Session.SetString("Role", "Professor");
                }
                return LocalRedirect("/");
            }
            else
            {
                return View();
            }
        }
        #endregion


        #region Logout
        [Autenticado]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("CookieSessao");
            return LocalRedirect("/");
        }
        #endregion


        #region Completar Perfil
        [Autenticado]
        public IActionResult CompletarPerfil()
        {
            return View();
        }

        [Autenticado]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompletarPerfil(IFormCollection dados)
        {
            if (ModelState.IsValid)
            {
                if (HelperFunctions.IsCurrentUserProfessor(HttpContext))
                {
                    Professor prof = _context.Professores.SingleOrDefault(p => p.NumCC == HttpContext.Session.GetString("UserId"));
                    prof.Especialidade = dados["Professor.Especialidade"].ToString();
                    _context.Professores.Update(prof);
                    _context.SaveChanges();
                }

                if (HelperFunctions.IsCurrentUserSocio(HttpContext))
                {
                    Socio socio = _context.Socios.SingleOrDefault(s => s.NumCC == HttpContext.Session.GetString("UserId"));
                    socio.Peso = double.Parse(dados["Socio.Peso"].ToString(), CultureInfo.InvariantCulture);
                    socio.Altura = dados["Socio.Altura"].ToString();
                    socio.DataRegisto_Peso = DateTime.Now;
                    _context.Socios.Update(socio);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }


            return View();
        }

        #endregion

        #region VerificarNovoUser
        [AjaxOnly]
        public JsonResult IsNewUser(string Username)
        {
            Pessoa pessoa = _context.Pessoas.SingleOrDefault(p => p.Username == Username);

            // your logic
            if (pessoa != null && pessoa.Password == null)
            {
                return Json(true);

            }
            return Json(false);
        }
        #endregion

        #region Recuperar Password
        [NaoAutenticado]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NaoAutenticado]
        public IActionResult ForgotPassword(string Email)
        {
            if (ModelState.IsValid)
            {
                Pessoa pessoa = _context.Pessoas.SingleOrDefault(x => x.Email == Email);

                string callbackUrl = Url.Action("ForgotPassword_Confirm", "Utilizadores", new { IdPessoa = pessoa.NumCC, code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(pessoa.Email)), valid = DateTime.Now.AddHours(24) }, Request.Scheme);

                HelperFunctions.SendEmailReposicaoPassword(Email, HttpContext, callbackUrl);

                ViewBag.Message = "success, Foi enviado para o seu email um link para mudar a sua password.";
                return View();
            }

            return View();
        }
        [NaoAutenticado]
        public IActionResult ForgotPassword_Confirm(string IdPessoa, string code, DateTime valid)
        {
            //Verifica a diferença de tempo entre a data enviada e a data atual
            TimeSpan valido = valid - DateTime.Now;
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            if (code == _context.Pessoas.FirstOrDefault(x => x.NumCC == IdPessoa).Email && valido.TotalHours < 24 && valido.TotalHours >= 0)
            {
                ViewBag.IdPessoa = IdPessoa;
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NaoAutenticado]
        public IActionResult ForgotPasswordChange_Confirm([Bind("Password1, Password2, IdPessoa")] ForgotPasswordModel forgotPass)
        {


            if (ModelState.IsValid)
            {
                Pessoa pessoa = _context.Pessoas.FirstOrDefault(x => x.NumCC == forgotPass.IdPessoa);

                pessoa.Password = SecurePasswordHasher.Hash(forgotPass.Password1);


                _context.Update(pessoa);
                _context.SaveChanges();

                ViewBag.Result = $"Password alterada com sucesso. \n Clique <a href = \"/Utilizadores/Login\">Aqui</a> para efetuar o login";
                return View();
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion
    }


}