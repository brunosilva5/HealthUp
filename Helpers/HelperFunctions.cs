using HealthUp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HealthUp.Helpers
{
    public static class HelperFunctions
    {

        // Retira todos os espaços em branco extra
        public static string NormalizeWhiteSpace(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            int current = 0;
            char[] output = new char[input.Length];
            bool skipped = false;

            foreach (char c in input.ToCharArray())
            {
                if (char.IsWhiteSpace(c))
                {
                    if (!skipped)
                    {
                        if (current > 0)
                            output[current++] = ' ';

                        skipped = true;
                    }
                }
                else
                {
                    skipped = false;
                    output[current++] = c;
                }
            }

            return new string(output, 0, current);
        }

        public static bool IsJustNumbers(string str)
        {
            Match match = Regex.Match(str, @"^(\d+)$", RegexOptions.IgnoreCase);// verificar se a string apenas contem numeros

            if (match.Success) return true;

            return false;
        }

        public static bool estaAutenticado(HttpContext contexto)
        {
            if (contexto.Session.GetString("UserId") != null)
                return true;
            else
                return false;
        }
        public static bool IsValidPassword(string str)
        {
            Match match = Regex.Match(str, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");// verificar se contem 1 maiuscula, 1 minuscula e 1 numero

            if (match.Success)
                return true;
            return false;

        }

        public static bool IsSocio(HealthUpContext healthUpContext, string Pessoa_id)
        {
            if (healthUpContext.Pessoas.Include(p=>p.Socio).SingleOrDefault(p=>p.Socio.NumCC==Pessoa_id)!=null)
                return true;
            else
                return false;
        }

        public static bool IsProfessor(HealthUpContext healthUpContext, string Pessoa_id)
        {
            if (healthUpContext.Pessoas.Include(p => p.Professor).SingleOrDefault(p => p.Professor.NumCC == Pessoa_id) != null)
                return true;
            else
                return false;
        }

        public static bool IsAdmin(HealthUpContext healthUpContext, string Pessoa_id)
        {
            if (healthUpContext.Pessoas.Include(p => p.Admin).SingleOrDefault(p => p.Admin.NumCC == Pessoa_id) != null)
                return true;
            else
                return false;
        }

        public static DateTime GetMonday(DateTime time)
        {
            if (time.DayOfWeek != DayOfWeek.Monday)
                return time.Subtract(new TimeSpan((int)time.DayOfWeek - 1, 0, 0, 0)).Date;

            return time.Date;
        }

        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            int start = (int)from.DayOfWeek;
            int target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start).Date;
        }

        public static int GetDay(string dia)
        {
            switch(dia)
            {
                case "Segunda-Feira": return 1;
                case "Terça-Feira": return 2;
                case "Quarta-Feira": return 3;
                case "Quinta-Feira": return 4;
                case "Sexta-Feira": return 5;
                case "Sábado": return 6;
                case "Domingo": return 7;
                default: return 1;
            }
        }


    }
}
