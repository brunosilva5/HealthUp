using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HealthUp.Data;
using Microsoft.AspNetCore.Mvc;

namespace HealthUp.Controllers
{

    public class Validation_RegisterController : Controller
    {
        private readonly HealthUpContext _context;


        public Validation_RegisterController(HealthUpContext context)
        {
            _context = context;
        }

        [HttpPost]
        public JsonResult IsValidUsername(string Username)
        {
            var P = _context.Pessoas.FirstOrDefault(p => p.Username == Username);
            if (P == null)
            {
                return Json(true);
            }
            else
            {
                return Json(new string("Este username já se encontra em utilização!"));
            }

        }

        [HttpPost]
        public JsonResult DoesExerciceExist(string Nome)
        {
            var Ex = _context.Exercicios.SingleOrDefault(x => x.Nome == Nome);
            if (Ex == null)
                return Json(true);
            return Json(new string("Este exercício já existe!"));

        }

        [HttpPost]
        public JsonResult IsValidDateOfBirth(string DataNascimento)
        {
            var min = DateTime.Now.AddYears(-18);
            var max = DateTime.Now.AddYears(-110);
            var msg = string.Format("Por favor insira uma data entre {0:MM/dd/yyyy} e {1:MM/dd/yyyy}", max, min);
            try
            {
                var date = DateTime.Parse(DataNascimento);
                if (date > min || date < max)
                    return Json(msg);
                else
                    return Json(true);
            }
            catch (Exception)
            {
                return Json(msg);
            }
        }

        [HttpPost]
        public JsonResult IsJpg(string Fotografia)
        {
            try
            {
                if (Path.GetExtension(Fotografia) != ".jpg")
                {
                    return Json("A fotografia tem de possuir o formato .jpg!");
                }
                else
                {
                    return Json(true);
                }

            }
            catch (Exception)
            {
                return Json("A fotografia tem de possuir o formato .jpg!");
            }
        }

        [HttpPost]
        public JsonResult IsValidVideo(string Video)
        {
            try
            {
                if (Path.GetExtension(Video) != ".mp4")
                {
                    return Json("O vídeo tem de possuir o formato .mp4!");
                }
                else
                {
                    return Json(true);
                }

            }
            catch (Exception)
            {
                return Json("O vídeo tem de possuir o formato .mp4!");
            }
        }

        public JsonResult IsValidPhoneNumber(string telemovel)
        {
            if (telemovel == null)
            {
                return Json(new string("O número inserido não é valido!"));
            }
            Match match = Regex.Match(telemovel, @"^(\d+)$", RegexOptions.IgnoreCase);// verificar se a string apenas contem numeros

            if (match.Success) return Json(true);
            return Json(new string("O número inserido não é valido!"));

        }
        [HttpPost]
        public JsonResult IsValidPassword(string Password)
        {
            if (Password == null)
            {
                return Json(new string("O número inserido não é valido!"));
            }


            Match match = Regex.Match(Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", RegexOptions.IgnoreCase);// verificar se contem 1 maiuscula, 1 minuscula e 1 numero

            try
            {
                if (match.Success)
                {
                    return Json("A Password tem de possuir 8 carateres, um carater maiúsculo, um mínusculo e um número!");
                }
                else
                {
                    return Json(true);
                }

            }
            catch (Exception)
            {
                return Json("A Password tem de possuir 8 carateres, um carater maiúsculo, um mínusculo e um número!");
            }

        }
        [HttpPost]
        public JsonResult IsValidEmail(string Email)
        {
            var P = _context.Pessoas.FirstOrDefault(p => p.Email == Email);
            if (P == null)
            {
                return Json(true);
            }
            return Json(new string("Este email já se encontra em utilização!"));
        }

        [HttpPost]
        public JsonResult IsValidNumCC(int numCC)
        {
            var P = _context.Pessoas.FirstOrDefault(p => p.NumCC == numCC.ToString());
            if (P == null)
            {
                return Json(true);
            }
            return Json(new string("Este número de cartão de cidadão já se encontra em utilização!"));
        }

        [HttpPost]
        public JsonResult IsValidCoordinates(string LocalizacaoGps)
        {
            try
            {
                string[] coordenadas = LocalizacaoGps.Split(',');
                float latitude = float.Parse(coordenadas[0]);
                float longitude = float.Parse(coordenadas[1]);

                if (latitude <= 90 && latitude >= -90 && longitude <= 180 && longitude > -180)
                {
                    return Json(true);
                }
                return Json(new string("As coordenadas não tem o formato correto é: latitude,longitude"));
            }
            catch (Exception)
            {
                return Json(new string("As coordenadas não tem o formato correto é: latitude,longitude"));
            }


        }
    }
}