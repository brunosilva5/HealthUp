using HealthUp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        public static bool EstaAutenticado(HttpContext context)
        {
            if (context.Session.GetString("UserId") != null)
                return true;
            else
                return false;
        }
        public static bool IsCurrentUserAdmin(HttpContext context)
        {
            if (EstaAutenticado(context))
            {
                if (context.Session.GetString("Role") == "Admin")
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsCurrentUserSocio(HttpContext context)
        {
            if (EstaAutenticado(context))
            {
                if (context.Session.GetString("Role") == "Socio")
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsCurrentUserProfessor(HttpContext context)
        {
            if (EstaAutenticado(context))
            {
                if (context.Session.GetString("Role") == "Professor")
                {
                    return true;
                }
            }
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
                target += 6;
            return from.AddDays(target - start).Date;
        }

        public static int GetDay(string dia)
        {
            return dia switch
            {
                "Segunda-Feira" => 1,
                "Terça-Feira" => 2,
                "Quarta-Feira" => 3,
                "Quinta-Feira" => 4,
                "Sexta-Feira" => 5,
                "Sábado" => 6,
                "Domingo" => 7,
                _ => 1,
            };
        }

        public static DateTime GetData(string data)
        {
            string[] dados = data.Split('-', 'W');
            return GetDateFromWeekNumberAndDayOfWeek(int.Parse(dados[0]), int.Parse(dados[2]));
        }

        public static DateTime GetDateFromWeekNumberAndDayOfWeek(int year, int weekNumber, int dayOfWeek=1)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Tuesday - jan1.DayOfWeek;

            DateTime firstMonday = jan1.AddDays(daysOffset);

            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(jan1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekNumber;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }

            var result = firstMonday.AddDays(weekNum * 7 + dayOfWeek);
            return result;
        }

        public static string JSONSerialize(this NameValueCollection _nvc)
        {
            return JsonConvert.SerializeObject(_nvc.AllKeys.ToDictionary(k => k, k => _nvc.GetValues(k)));
        }
        public static NameValueCollection JSONDeserialize( string _serializedString)
        {
            NameValueCollection _nvc = new NameValueCollection();
            if (_serializedString==null)
            {
                return _nvc;
            }
            var deserializedobject = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(_serializedString);
            foreach (var strCol in deserializedobject.Values)
                foreach (var str in strCol)
                    _nvc.Add(deserializedobject.FirstOrDefault(x => x.Value.Contains(str)).Key, str);

            return _nvc;
        }

        public static string SendEmailConfirmacao(bool aceite, string Email)
        {
            string mensagem;
            if (aceite)
            {
                mensagem = "O presente email serve para confirmar que o seu pedido de adesão ao HealthUp foi aceite com sucesso!\n" +
                    "Poderá agora aceder à sua conta através da página Login, definindo a sua password.";
            }
            else
            {
                mensagem = "O presente email serve para confirmar que o seu pedido de adesão ao HealthUp foi rejeitado. Lamentamos!";
            }
            try
            {
                
                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("healthupgym@gmail.com"),
                    Subject = "Confirmação do pedido sócio",
                    Body = mensagem
                };
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(Email));
                // Smtp client

                var client = new SmtpClient
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = new NetworkCredential("healthupgym@gmail.com", "healthup2019")
                };

                client.Send(mail);
                return "Email Sent Successfully!";
            }
            catch (System.Exception e)
            {
                return e.Message;
            }

        }

        public static IEnumerable<T> Add<T>(this IEnumerable<T> e, T value)
        {
            foreach (var cur in e)
            {
                yield return cur;
            }
            yield return value;
        }

        public static bool DoesProfHaveStudents(HttpContext context)
        {
            var db = context.RequestServices.GetRequiredService<HealthUpContext>();
            var prof = db.Professores.Include(x=>x.Socio).SingleOrDefault(p => p.NumCC == context.Session.GetString("UserId"));
            if (prof!=null && prof.Socio.Count>0)
            {
                return true;
            }
            return false;
        }

        public static bool DoesSocioHavePT(HttpContext context)
        {
            var db = context.RequestServices.GetRequiredService<HealthUpContext>();
            var socio = db.Socios.Include(x => x.NumProfessorNavigation).SingleOrDefault(p => p.NumCC == context.Session.GetString("UserId"));
            if (socio != null && socio.NumProfessorNavigation != null)
            {
                return true;
            }
            return false;
        }

        public static int GetWeekOfTheYear(DateTime data)
        {
            CultureInfo myCI = new CultureInfo("pt-PT");
            Calendar myCal = myCI.Calendar;

            // Gets the DTFI properties required by GetWeekOfYear.
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;

            return myCal.GetWeekOfYear(data, myCWR, myFirstDOW);
        }

        public static List<DateTime> GetDatesBetween(DateTime start, DateTime end, params DayOfWeek[] weekdays)
        {
            bool allDays = weekdays == null || !weekdays.Any();

            var dates = Enumerable.Range(0, 1 + end.Subtract(start).Days)
                                  .Select(offset => start.AddDays(offset))
                                  .Where(d => allDays || weekdays.Contains(d.DayOfWeek))
                                  .ToList();
            return dates;
        }


    }
}
