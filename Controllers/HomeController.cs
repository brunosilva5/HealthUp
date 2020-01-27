using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HealthUp.Models;
using HealthUp.Filters;
using HealthUp.Data;
using System.Globalization;
using HealthUp.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace HealthUp.Controllers
{
    [ApagarAulasAntigas]
    [PerfilCompleto]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HealthUpContext _context;
        public HomeController(ILogger<HomeController> logger, HealthUpContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var gym = _context.Ginasios.SingleOrDefault();

            string[] coordenadas = gym.LocalizacaoGps.Split(',');
            ViewBag.Latitude = coordenadas[0];
            ViewBag.Longitude = coordenadas[1];
            return View(gym);
        }

        public IActionResult Sobrenos()
        {
            return View();
        }
        
        public IActionResult Contactos()
        {
            var gym = _context.Ginasios.SingleOrDefault();

            string[] coordenadas = gym.LocalizacaoGps.Split(',');
            ViewBag.Latitude = coordenadas[0];
            ViewBag.Longitude = coordenadas[1];

            return View(gym);
        }

        public IActionResult MapaAulas()
        {
            var gym=_context.Ginasios.SingleOrDefault();
            var numero_aulas_diarias= Convert.ToInt32(((gym.Hora_Fecho.TotalMinutes - gym.Hora_Abertura.TotalMinutes) ) / 50);
            // Listas de aulas diarias
            List<Aula> ListaAulas_SegundaFeira = new List<Aula>(numero_aulas_diarias);
            List<Aula> ListaAulas_TercaFeira = new List<Aula>(numero_aulas_diarias);
            List<Aula> ListaAulas_QuartaFeira = new List<Aula>(numero_aulas_diarias);
            List<Aula> ListaAulas_QuintaFeira = new List<Aula>(numero_aulas_diarias);
            List<Aula> ListaAulas_SextaFeira = new List<Aula>(numero_aulas_diarias);
            List<Aula> ListaAulas_Sabado = new List<Aula>(numero_aulas_diarias);
            List<Aula> ListaAulas_Domingo = new List<Aula>(numero_aulas_diarias);

            
            foreach (var item in _context.Aulas.ToList())
            {
                // verificar se a aula vai ocorrer esta semana na segunda feira
                if (item.IsAulaInCurrentWeek() && item.GetDiaSemana()=="Segunda-Feira")
                {
                    ListaAulas_SegundaFeira.Add(item); // se sim, adicionar a lista
                }
                // verificar se a aula vai ocorrer esta semana na terca feira
                if (item.IsAulaInCurrentWeek() && item.GetDiaSemana() == "Terça-Feira")
                {
                    ListaAulas_TercaFeira.Add(item); // se sim, adicionar a lista
                }
                // verificar se a aula vai ocorrer esta semana na quarta feira
                if (item.IsAulaInCurrentWeek() && item.GetDiaSemana() == "Quarta-Feira")
                {
                    ListaAulas_QuartaFeira.Add(item); // se sim, adicionar a lista
                }
                // verificar se a aula vai ocorrer esta semana na quinta feira
                if (item.IsAulaInCurrentWeek() && item.GetDiaSemana() == "Quinta-Feira")
                {
                    ListaAulas_QuintaFeira.Add(item); // se sim, adicionar a lista
                }
                // verificar se a aula vai ocorrer esta semana na sexta feira
                if (item.IsAulaInCurrentWeek() && item.GetDiaSemana() == "Sexta-Feira")
                {
                    ListaAulas_SextaFeira.Add(item); // se sim, adicionar a lista
                }
                // verificar se a aula vai ocorrer esta semana no sabado
                if (item.IsAulaInCurrentWeek() && item.GetDiaSemana() == "Sábado")
                {
                    ListaAulas_Sabado.Add(item); // se sim, adicionar a lista
                }
                // verificar se a aula vai ocorrer esta semana no domingo
                if (item.IsAulaInCurrentWeek() && item.GetDiaSemana() == "Domingo")
                {
                    ListaAulas_Domingo.Add(item); // se sim, adicionar a lista
                }
            }
            for (int i = ListaAulas_SegundaFeira.Count + 1; i <= ListaAulas_SegundaFeira.Capacity; i++)
                ListaAulas_SegundaFeira.Add(null);

            for (int i = ListaAulas_TercaFeira.Count + 1; i <= ListaAulas_SegundaFeira.Capacity; i++)
                ListaAulas_TercaFeira.Add(null);

            for (int i = ListaAulas_QuartaFeira.Count + 1; i <= ListaAulas_QuartaFeira.Capacity; i++)
                ListaAulas_QuartaFeira.Add(null);

            for (int i = ListaAulas_QuintaFeira.Count + 1; i <= ListaAulas_QuintaFeira.Capacity; i++)
                ListaAulas_QuintaFeira.Add(null);

            for (int i = ListaAulas_SextaFeira.Count + 1; i <= ListaAulas_SextaFeira.Capacity; i++)
                ListaAulas_SextaFeira.Add(null);

            for (int i = ListaAulas_Sabado.Count + 1; i <= ListaAulas_Sabado.Capacity; i++)
                ListaAulas_Sabado.Add(null);

            for (int i = ListaAulas_Domingo.Count + 1; i <= ListaAulas_Domingo.Capacity; i++)
                ListaAulas_Domingo.Add(null);


            ViewBag.SegundaFeira = OrganizarLista(ListaAulas_SegundaFeira);
            ViewBag.TercaFeira = OrganizarLista(ListaAulas_TercaFeira);
            ViewBag.QuartaFeira = OrganizarLista(ListaAulas_QuartaFeira);
            ViewBag.QuintaFeira = OrganizarLista(ListaAulas_QuintaFeira);
            ViewBag.SextaFeira = OrganizarLista(ListaAulas_SextaFeira);
            ViewBag.Sabado = OrganizarLista(ListaAulas_Sabado);
            ViewBag.Domingo = OrganizarLista(ListaAulas_Domingo);

            return View();
        }
        private List<Aula> OrganizarLista(List<Aula> Lista)
        {
            // Reorganizar lista por ordem certa
            List<Aula> NewLista = new List<Aula>(Lista);
            var cont = HttpContext.RequestServices.GetRequiredService<HealthUpContext>().Ginasios.SingleOrDefault().Hora_Abertura.TotalMinutes;
            foreach (var item in Lista)
            {
                // se é null, continuar
                if (item == null)
                {
                    continue;
                }
                // primeiro caso, esta certo
                if (item.HoraInicio.TotalMinutes == cont)
                {
                    NewLista[0] = item;
                }
                cont += 50;
                if (item.HoraInicio.TotalMinutes == cont)
                {
                    var ordem = (cont - HttpContext.RequestServices.GetRequiredService<HealthUpContext>().Ginasios.SingleOrDefault().Hora_Abertura.TotalMinutes) / 50;
                    var index = NewLista.FindIndex(p => p.IdAula == item.IdAula);
                    NewLista[index] = null;
                    NewLista[(int)ordem] = item;
                }
            }
            return NewLista;
        }
        
        
    }
}
