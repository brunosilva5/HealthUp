﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HealthUp.Models
{
    public partial class Socio
    {
        public Socio()
        {
            Inscreve = new HashSet<Inscreve>();
            PlanoTreino = new HashSet<PlanoTreino>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Tem de possuir 8 caracteres!")]
        [Remote("IsValidNumCC", "Validation", HttpMethod = "POST", ErrorMessage = "Número de cartão de cidadão inválido!")]
        [Display(Name = "Número de cartão de cidadão")]
        public string NumCC { get; set; }

        public string NumProfessor { get; set; }
        public string NumAdmin { get; set; }
        public string ID_Solicitacao { get; set; }
        [StringLength(3)]
        public string Altura { get; set; }
        public int Peso { get; set; }
        [DataType(DataType.Date)]
        public DateTime DataRegisto { get; set; }
        [StringLength(200)]
        public string Motivo { get; set; }
        public DateTime? DataSuspensao { get; set; }


        [ForeignKey(nameof(ID_Solicitacao))]
        [InverseProperty(nameof(SolicitacaoProfessor.Socio))]
        public virtual SolicitacaoProfessor IdSolicitacaoNavigation { get; set; }

        [ForeignKey(nameof(NumAdmin))]
        [InverseProperty(nameof(Admin.SociosSuspensos))]
        public virtual Admin NumAdminNavigation { get; set; }

        [ForeignKey(nameof(NumProfessor))]
        [InverseProperty(nameof(Professor.Socio))]
        public virtual Professor NumProfessorNavigation { get; set; }

        [ForeignKey(nameof(NumCC))]
        [InverseProperty(nameof(Pessoa.Socio))]
        public virtual Pessoa NumSocioNavigation { get; set; }

        [InverseProperty("NumSocioNavigation")]
        public virtual ICollection<Inscreve> Inscreve { get; set; }

        [InverseProperty("NumSocioNavigation")]
        public virtual ICollection<PlanoTreino> PlanoTreino { get; set; }
    }
}
