using HealthUp.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthUp.Models
{

    public partial class Aula
    {
        public Aula()
        {
            Inscreve = new HashSet<Inscreve>();
        }

        [Key]
        [Display(Name = "Id da aula")]
        public int IdAula { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(30)]
        [Display(Name = "Aula")]
        [Remote("IsValidNomeAula", "Validation_Register", HttpMethod = "POST", ErrorMessage = "Esta aula já existe!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [Display(Name = "Número de professor")]
        public string NumProfessor { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [Display(Name = "Número de administrador")]
        public string NumAdmin { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [DataType(DataType.Date)]
        [Display(Name = "Válido de")]
        [Remote("IsValidDataDe", "Validation_Register", HttpMethod = "GET", ErrorMessage = "Esta data não é válida!")]
        public DateTime ValidoDe { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Válido até")]
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [Remote("IsValidDataAte", "Validation_Register", HttpMethod = "GET", ErrorMessage = "Esta data não é válida!", AdditionalFields = "ValidoDe")]
        public DateTime ValidoAte { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [Display(Name = "Lotação")]
        [Range(0, int.MaxValue)]
        public int? Lotacao { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [Display(Name = "Hora de início")]
        public TimeSpan HoraInicio { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [Display(Name = "Dia da semana")]
        [Range(1, 7)]
        public int DiaSemana { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.Aula))]
        [Display(Name = "Número de navegação do administrador")]
        public virtual Admin NumAdminNavigation { get; set; }

        [ForeignKey(nameof(NumProfessor))]
        [InverseProperty(nameof(Professor.Aula))]
        [Display(Name = "Número de navegação do professor")]
        public virtual Professor NumProfessorNavigation { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(100)]
        [Remote("IsValidFotografiaDivulgacao", "Validation_Files", HttpMethod = "POST", ErrorMessage = "A fotografia tem de ser no formato .jpg")]
        [Display(Name = "Fotografia")]
        public string FotografiaDivulgacao { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(100)]
        [Remote("IsValidVideoDivulgacao", "Validation_Files", HttpMethod = "POST", ErrorMessage = "O video tem de ser no formato .mp4")]
        [Display(Name = "Video")]
        public string VideoDivulgacao { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório!")]
        [StringLength(500)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }


        [InverseProperty("IdAulaNavigation")]
        public virtual ICollection<Inscreve> Inscreve { get; set; }

        public bool IsAulaInCurrentWeek()
        {
            List<DateTime> ListaDatas = new List<DateTime>();
            if (GetDiaSemana() == "Domingo")
            {
                ListaDatas = HelperFunctions.GetDatesBetween(ValidoDe, ValidoAte, DayOfWeek.Sunday);
            }
            if (GetDiaSemana() == "Segunda-Feira")
            {
                ListaDatas = HelperFunctions.GetDatesBetween(ValidoDe, ValidoAte, DayOfWeek.Monday);
            }
            if (GetDiaSemana() == "Terça-Feira")
            {
                ListaDatas = HelperFunctions.GetDatesBetween(ValidoDe, ValidoAte, DayOfWeek.Tuesday);
            }
            if (GetDiaSemana() == "Quarta-Feira")
            {
                ListaDatas = HelperFunctions.GetDatesBetween(ValidoDe, ValidoAte, DayOfWeek.Wednesday);
            }
            if (GetDiaSemana() == "Quinta-Feira")
            {
                ListaDatas = HelperFunctions.GetDatesBetween(ValidoDe, ValidoAte, DayOfWeek.Thursday);
            }
            if (GetDiaSemana() == "Sexta-Feira")
            {
                ListaDatas = HelperFunctions.GetDatesBetween(ValidoDe, ValidoAte, DayOfWeek.Friday);
            }
            if (GetDiaSemana() == "Sábado")
            {
                ListaDatas = HelperFunctions.GetDatesBetween(ValidoDe, ValidoAte, DayOfWeek.Saturday);
            }

            // vai ser criada uma lista de datas de todos os dias da semana ( ex: quarta-feira) entre as datas valido de e valido ate
            // depois abaixo vamos excluir todas as datas que nao se encontrem na semana atual
            List<DateTime> NewListaDatas = new List<DateTime>();
            foreach (DateTime item in ListaDatas)
            {
                if (HelperFunctions.GetWeekOfTheYear(DateTime.Now) == HelperFunctions.GetWeekOfTheYear(item))
                {
                    NewListaDatas.Add(item);
                }
            }
            return NewListaDatas.Count != 0 ? true : false;
        }
        public string GetDiaSemana()
        {
            return DiaSemana switch
            {
                1 => "Segunda-Feira",
                2 => "Terça-Feira",
                3 => "Quarta-Feira",
                4 => "Quinta-Feira",
                5 => "Sexta-Feira",
                6 => "Sábado",
                7 => "Domingo",
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public bool VerificarValidade(DateTime segunda,
                                      DateTime domingo)
        {
            DateTime DataDiaSemana = segunda.AddDays(DiaSemana - 1);
            if (DataDiaSemana >= ValidoDe && DataDiaSemana <= ValidoAte)
            {
                return true;
            }
            return false;
        }
    }
}