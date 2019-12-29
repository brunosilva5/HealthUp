using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HealthUp.Models;

namespace HealthUp.Data
{
    public class HealthUpContext : DbContext
    {
        public HealthUpContext (DbContextOptions<HealthUpContext> options)
            : base(options)
        {
        }
        // change  
        public DbSet<HealthUp.Models.Admin> Admins { get; set; }
        public DbSet<HealthUp.Models.Aula> Aulas { get; set; }
        public DbSet<HealthUp.Models.AulaGrupo> AulasGrupo { get; set; }
        public DbSet<HealthUp.Models.Contem> Contem { get; set; }
        public DbSet<HealthUp.Models.Exercicio> Exercicios { get; set; }
        public DbSet<HealthUp.Models.Ginasio> Ginasios { get; set; }
        public DbSet<HealthUp.Models.Inscreve> Inscricoes { get; set; }
        public DbSet<HealthUp.Models.Mensagem> Mensagens { get; set; }
        public DbSet<HealthUp.Models.PedidoSocio> PedidosSocios { get; set; }
        public DbSet<HealthUp.Models.Pessoa> Pessoas { get; set; }
        public DbSet<HealthUp.Models.PlanoTreino> PlanosTreino { get; set; }
        public DbSet<HealthUp.Models.Professor> Professores { get; set; }
        public DbSet<HealthUp.Models.Socio> Socios { get; set; }
        public DbSet<HealthUp.Models.SolicitacaoProfessor> SolicitacaoProfessores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contem>()
                .HasKey(c => new { c.IdPlano, c.IdExercicio });

            modelBuilder.Entity<Inscreve>()
                .HasKey(c => new { c.NumSocio, c.IdAula });
        }

    }
}
