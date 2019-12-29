﻿// <auto-generated />
using System;
using HealthUp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HealthUp.Migrations
{
    [DbContext(typeof(HealthUpContext))]
    [Migration("20191229154648_fix")]
    partial class fix
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HealthUp.Models.Admin", b =>
                {
                    b.Property<string>("NumCC")
                        .HasColumnType("nvarchar(8)")
                        .HasMaxLength(8);

                    b.HasKey("NumCC");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("HealthUp.Models.Aula", b =>
                {
                    b.Property<int>("IdAula")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DiaSemana")
                        .HasColumnType("int");

                    b.Property<TimeSpan?>("HoraInicio")
                        .HasColumnType("time");

                    b.Property<int>("Lotacao")
                        .HasColumnType("int");

                    b.Property<string>("NumAdmin")
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("NumProfessor")
                        .HasColumnType("nvarchar(8)");

                    b.Property<DateTime?>("ValidoAte")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ValidoDe")
                        .HasColumnType("datetime2");

                    b.HasKey("IdAula");

                    b.HasIndex("NumAdmin");

                    b.HasIndex("NumProfessor");

                    b.ToTable("Aulas");
                });

            modelBuilder.Entity("HealthUp.Models.AulaGrupo", b =>
                {
                    b.Property<int>("IdAula")
                        .HasColumnType("int");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("FotografiaDivulgacao")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.HasKey("IdAula");

                    b.ToTable("AulasGrupo");
                });

            modelBuilder.Entity("HealthUp.Models.Contem", b =>
                {
                    b.Property<int>("IdPlano")
                        .HasColumnType("int");

                    b.Property<int>("IdExercicio")
                        .HasColumnType("int");

                    b.Property<int>("NumRepeticoes")
                        .HasColumnType("int");

                    b.Property<int>("PeriodoDescanso")
                        .HasColumnType("int");

                    b.Property<int>("QuantidadeSeries")
                        .HasColumnType("int");

                    b.HasKey("IdPlano", "IdExercicio");

                    b.HasIndex("IdExercicio");

                    b.ToTable("Contem");
                });

            modelBuilder.Entity("HealthUp.Models.Exercicio", b =>
                {
                    b.Property<int>("IdExercicio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("Fotografia")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("NumAdmin")
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Video")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("IdExercicio");

                    b.HasIndex("NumAdmin");

                    b.ToTable("Exercicios");
                });

            modelBuilder.Entity("HealthUp.Models.Ginasio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Endereco")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("LocalizacaoGps")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("NumAdmin")
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Telemovel")
                        .IsRequired()
                        .HasColumnType("nvarchar(13)")
                        .HasMaxLength(13);

                    b.HasKey("Id");

                    b.HasIndex("NumAdmin");

                    b.ToTable("Ginasios");
                });

            modelBuilder.Entity("HealthUp.Models.Inscreve", b =>
                {
                    b.Property<string>("NumSocio")
                        .HasColumnType("nvarchar(8)");

                    b.Property<int>("IdAula")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Data")
                        .HasColumnType("datetime2");

                    b.HasKey("NumSocio", "IdAula");

                    b.HasIndex("IdAula");

                    b.ToTable("Inscricoes");
                });

            modelBuilder.Entity("HealthUp.Models.Mensagem", b =>
                {
                    b.Property<string>("IdMensagem")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Arquivada")
                        .HasColumnType("bit");

                    b.Property<string>("Conteudo")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<DateTime?>("DataEnvio")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdPessoa")
                        .HasColumnType("nvarchar(8)");

                    b.Property<bool>("Lida")
                        .HasColumnType("bit");

                    b.HasKey("IdMensagem");

                    b.HasIndex("IdPessoa");

                    b.ToTable("Mensagens");
                });

            modelBuilder.Entity("HealthUp.Models.PedidoSocio", b =>
                {
                    b.Property<string>("NumCC")
                        .HasColumnType("nvarchar(8)")
                        .HasMaxLength(8);

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Fotografia")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Nacionalidade")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("NumAdmin")
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Sexo")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Telemovel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("NumCC");

                    b.HasIndex("NumAdmin");

                    b.ToTable("PedidosSocios");
                });

            modelBuilder.Entity("HealthUp.Models.Pessoa", b =>
                {
                    b.Property<string>("NumCC")
                        .HasColumnType("nvarchar(8)")
                        .HasMaxLength(8);

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Fotografia")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<bool>("IsNotified")
                        .HasColumnType("bit");

                    b.Property<string>("Nacionalidade")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("NumAdmin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Sexo")
                        .IsRequired()
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("Telemovel")
                        .IsRequired()
                        .HasColumnType("nvarchar(13)")
                        .HasMaxLength(13);

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("NumCC");

                    b.ToTable("Pessoas");
                });

            modelBuilder.Entity("HealthUp.Models.PlanoTreino", b =>
                {
                    b.Property<int>("IdPlano")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Ativo")
                        .HasColumnType("bit");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("NumProfessor")
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("NumSocio")
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("IdPlano");

                    b.HasIndex("NumProfessor");

                    b.HasIndex("NumSocio");

                    b.ToTable("PlanosTreino");
                });

            modelBuilder.Entity("HealthUp.Models.Professor", b =>
                {
                    b.Property<string>("NumCC")
                        .HasColumnType("nvarchar(8)")
                        .HasMaxLength(8);

                    b.Property<DateTime?>("DataSuspensao")
                        .HasColumnType("datetime2");

                    b.Property<string>("Especialidade")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("IdSolicitacao")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Motivo")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("NumAdmin")
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("NumCC");

                    b.HasIndex("IdSolicitacao");

                    b.HasIndex("NumAdmin");

                    b.ToTable("Professores");
                });

            modelBuilder.Entity("HealthUp.Models.Socio", b =>
                {
                    b.Property<string>("NumCC")
                        .HasColumnType("nvarchar(8)")
                        .HasMaxLength(8);

                    b.Property<string>("Altura")
                        .HasColumnType("nvarchar(3)")
                        .HasMaxLength(3);

                    b.Property<DateTime>("DataRegisto")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DataSuspensao")
                        .HasColumnType("datetime2");

                    b.Property<string>("ID_Solicitacao")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Motivo")
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("NumAdmin")
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("NumProfessor")
                        .HasColumnType("nvarchar(8)");

                    b.Property<int>("Peso")
                        .HasColumnType("int");

                    b.HasKey("NumCC");

                    b.HasIndex("ID_Solicitacao");

                    b.HasIndex("NumAdmin");

                    b.HasIndex("NumProfessor");

                    b.ToTable("Socios");
                });

            modelBuilder.Entity("HealthUp.Models.SolicitacaoProfessor", b =>
                {
                    b.Property<string>("IdSolicitacao")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("Data")
                        .HasColumnType("datetime2");

                    b.Property<string>("NumAdmin")
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("IdSolicitacao");

                    b.HasIndex("NumAdmin");

                    b.ToTable("SolicitacaoProfessores");
                });

            modelBuilder.Entity("HealthUp.Models.Admin", b =>
                {
                    b.HasOne("HealthUp.Models.Pessoa", "NumAdminNavigation")
                        .WithOne("Admin")
                        .HasForeignKey("HealthUp.Models.Admin", "NumCC")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HealthUp.Models.Aula", b =>
                {
                    b.HasOne("HealthUp.Models.Admin", "NumAdminNavigation")
                        .WithMany("Aula")
                        .HasForeignKey("NumAdmin");

                    b.HasOne("HealthUp.Models.Professor", "NumProfessorNavigation")
                        .WithMany("Aula")
                        .HasForeignKey("NumProfessor");
                });

            modelBuilder.Entity("HealthUp.Models.AulaGrupo", b =>
                {
                    b.HasOne("HealthUp.Models.Aula", "IdAulaNavigation")
                        .WithOne("AulaGrupo")
                        .HasForeignKey("HealthUp.Models.AulaGrupo", "IdAula")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HealthUp.Models.Contem", b =>
                {
                    b.HasOne("HealthUp.Models.Exercicio", "IdExercicioNavigation")
                        .WithMany("Contem")
                        .HasForeignKey("IdExercicio")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthUp.Models.PlanoTreino", "IdPlanoNavigation")
                        .WithMany("Contem")
                        .HasForeignKey("IdPlano")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HealthUp.Models.Exercicio", b =>
                {
                    b.HasOne("HealthUp.Models.Admin", "NumAdminNavigation")
                        .WithMany("Exercicio")
                        .HasForeignKey("NumAdmin");
                });

            modelBuilder.Entity("HealthUp.Models.Ginasio", b =>
                {
                    b.HasOne("HealthUp.Models.Admin", "NumAdminNavigation")
                        .WithMany("Ginasio")
                        .HasForeignKey("NumAdmin");
                });

            modelBuilder.Entity("HealthUp.Models.Inscreve", b =>
                {
                    b.HasOne("HealthUp.Models.Aula", "IdAulaNavigation")
                        .WithMany("Inscreve")
                        .HasForeignKey("IdAula")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthUp.Models.Socio", "NumSocioNavigation")
                        .WithMany("Inscreve")
                        .HasForeignKey("NumSocio")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HealthUp.Models.Mensagem", b =>
                {
                    b.HasOne("HealthUp.Models.Pessoa", "IdNavigation")
                        .WithMany("Mensagem")
                        .HasForeignKey("IdPessoa");
                });

            modelBuilder.Entity("HealthUp.Models.PedidoSocio", b =>
                {
                    b.HasOne("HealthUp.Models.Admin", "NumAdminNavigation")
                        .WithMany("PedidosSocio")
                        .HasForeignKey("NumAdmin");
                });

            modelBuilder.Entity("HealthUp.Models.PlanoTreino", b =>
                {
                    b.HasOne("HealthUp.Models.Professor", "NumProfessorNavigation")
                        .WithMany("PlanoTreino")
                        .HasForeignKey("NumProfessor");

                    b.HasOne("HealthUp.Models.Socio", "NumSocioNavigation")
                        .WithMany("PlanoTreino")
                        .HasForeignKey("NumSocio");
                });

            modelBuilder.Entity("HealthUp.Models.Professor", b =>
                {
                    b.HasOne("HealthUp.Models.SolicitacaoProfessor", "IdSolicitacaoNavigation")
                        .WithMany("Professor")
                        .HasForeignKey("IdSolicitacao");

                    b.HasOne("HealthUp.Models.Admin", "NumAdminNavigation")
                        .WithMany("ProfessoresSuspensos")
                        .HasForeignKey("NumAdmin");

                    b.HasOne("HealthUp.Models.Pessoa", "NumProfessorNavigation")
                        .WithOne("Professor")
                        .HasForeignKey("HealthUp.Models.Professor", "NumCC")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HealthUp.Models.Socio", b =>
                {
                    b.HasOne("HealthUp.Models.SolicitacaoProfessor", "IdSolicitacaoNavigation")
                        .WithMany("Socio")
                        .HasForeignKey("ID_Solicitacao");

                    b.HasOne("HealthUp.Models.Admin", "NumAdminNavigation")
                        .WithMany("SociosSuspensos")
                        .HasForeignKey("NumAdmin");

                    b.HasOne("HealthUp.Models.Pessoa", "NumSocioNavigation")
                        .WithOne("Socio")
                        .HasForeignKey("HealthUp.Models.Socio", "NumCC")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthUp.Models.Professor", "NumProfessorNavigation")
                        .WithMany("Socio")
                        .HasForeignKey("NumProfessor");
                });

            modelBuilder.Entity("HealthUp.Models.SolicitacaoProfessor", b =>
                {
                    b.HasOne("HealthUp.Models.Admin", "NumAdminNavigation")
                        .WithMany("SolicitacaoProfessor")
                        .HasForeignKey("NumAdmin");
                });
#pragma warning restore 612, 618
        }
    }
}
