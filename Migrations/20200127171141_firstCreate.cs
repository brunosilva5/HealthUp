using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthUp.Migrations
{
    public partial class firstCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    NumCC = table.Column<string>(maxLength: 8, nullable: false),
                    NumAdmin = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(maxLength: 30, nullable: false),
                    Sexo = table.Column<string>(maxLength: 3, nullable: false),
                    Fotografia = table.Column<string>(maxLength: 100, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Telemovel = table.Column<string>(maxLength: 13, nullable: false),
                    Nacionalidade = table.Column<string>(maxLength: 20, nullable: false),
                    Username = table.Column<string>(maxLength: 20, nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.NumCC);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    NumCC = table.Column<string>(maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.NumCC);
                    table.ForeignKey(
                        name: "FK_Admins_Pessoas_NumCC",
                        column: x => x.NumCC,
                        principalTable: "Pessoas",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mensagens",
                columns: table => new
                {
                    IdMensagem = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPessoaReceiver = table.Column<string>(nullable: true),
                    IdPessoaSender = table.Column<string>(nullable: true),
                    DataEnvio = table.Column<DateTime>(nullable: true),
                    Lida = table.Column<bool>(nullable: false),
                    Arquivada_Receiver = table.Column<bool>(nullable: false),
                    Arquivada_Sender = table.Column<bool>(nullable: false),
                    Conteudo = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mensagens", x => x.IdMensagem);
                    table.ForeignKey(
                        name: "FK_Mensagens_Pessoas_IdPessoaReceiver",
                        column: x => x.IdPessoaReceiver,
                        principalTable: "Pessoas",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mensagens_Pessoas_IdPessoaSender",
                        column: x => x.IdPessoaSender,
                        principalTable: "Pessoas",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Exercicios",
                columns: table => new
                {
                    IdExercicio = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumAdmin = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(maxLength: 50, nullable: false),
                    Descricao = table.Column<string>(maxLength: 500, nullable: false),
                    Video = table.Column<string>(maxLength: 100, nullable: false),
                    Fotografia = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercicios", x => x.IdExercicio);
                    table.ForeignKey(
                        name: "FK_Exercicios_Admins_NumAdmin",
                        column: x => x.NumAdmin,
                        principalTable: "Admins",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ginasios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumAdmin = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(maxLength: 30, nullable: false),
                    Endereco = table.Column<string>(maxLength: 200, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Hora_Abertura = table.Column<TimeSpan>(nullable: false),
                    Hora_Fecho = table.Column<TimeSpan>(nullable: false),
                    Telemovel = table.Column<string>(maxLength: 13, nullable: false),
                    LocalizacaoGps = table.Column<string>(maxLength: 200, nullable: false),
                    NumAdminNavigationNumCC = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ginasios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ginasios_Admins_NumAdminNavigationNumCC",
                        column: x => x.NumAdminNavigationNumCC,
                        principalTable: "Admins",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PedidosSocios",
                columns: table => new
                {
                    NumCC = table.Column<string>(maxLength: 8, nullable: false),
                    NumAdmin = table.Column<string>(nullable: true),
                    Nome = table.Column<string>(maxLength: 30, nullable: false),
                    Sexo = table.Column<string>(maxLength: 3, nullable: false),
                    Fotografia = table.Column<string>(maxLength: 100, nullable: false),
                    DataNascimento = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Telemovel = table.Column<string>(nullable: false),
                    Nacionalidade = table.Column<string>(maxLength: 20, nullable: false),
                    Username = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidosSocios", x => x.NumCC);
                    table.ForeignKey(
                        name: "FK_PedidosSocios_Admins_NumAdmin",
                        column: x => x.NumAdmin,
                        principalTable: "Admins",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolicitacaoProfessores",
                columns: table => new
                {
                    IdSolicitacao = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumAdmin = table.Column<string>(nullable: true),
                    Data = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacaoProfessores", x => x.IdSolicitacao);
                    table.ForeignKey(
                        name: "FK_SolicitacaoProfessores_Admins_NumAdmin",
                        column: x => x.NumAdmin,
                        principalTable: "Admins",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Professores",
                columns: table => new
                {
                    NumCC = table.Column<string>(maxLength: 8, nullable: false),
                    NumAdmin = table.Column<string>(nullable: true),
                    IdSolicitacao = table.Column<int>(nullable: true),
                    Motivo = table.Column<string>(maxLength: 200, nullable: true),
                    DataSuspensao = table.Column<DateTime>(nullable: true),
                    Especialidade = table.Column<string>(maxLength: 30, nullable: true),
                    IdSolicitacaoNavigationIdSolicitacao = table.Column<int>(nullable: true),
                    NumAdminNavigationNumCC = table.Column<string>(nullable: true),
                    RegistoPesos = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professores", x => x.NumCC);
                    table.ForeignKey(
                        name: "FK_Professores_SolicitacaoProfessores_IdSolicitacaoNavigationIdSolicitacao",
                        column: x => x.IdSolicitacaoNavigationIdSolicitacao,
                        principalTable: "SolicitacaoProfessores",
                        principalColumn: "IdSolicitacao",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Professores_Admins_NumAdminNavigationNumCC",
                        column: x => x.NumAdminNavigationNumCC,
                        principalTable: "Admins",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Professores_Pessoas_NumCC",
                        column: x => x.NumCC,
                        principalTable: "Pessoas",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aulas",
                columns: table => new
                {
                    IdAula = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(maxLength: 30, nullable: false),
                    NumProfessor = table.Column<string>(nullable: false),
                    NumAdmin = table.Column<string>(nullable: false),
                    ValidoDe = table.Column<DateTime>(nullable: false),
                    ValidoAte = table.Column<DateTime>(nullable: false),
                    Lotacao = table.Column<int>(nullable: false),
                    HoraInicio = table.Column<TimeSpan>(nullable: false),
                    DiaSemana = table.Column<int>(nullable: false),
                    FotografiaDivulgacao = table.Column<string>(maxLength: 100, nullable: false),
                    VideoDivulgacao = table.Column<string>(maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aulas", x => x.IdAula);
                    table.ForeignKey(
                        name: "FK_Aulas_Admins_NumAdmin",
                        column: x => x.NumAdmin,
                        principalTable: "Admins",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Aulas_Professores_NumProfessor",
                        column: x => x.NumProfessor,
                        principalTable: "Professores",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Socios",
                columns: table => new
                {
                    NumCC = table.Column<string>(maxLength: 8, nullable: false),
                    NumAdmin = table.Column<string>(nullable: true),
                    ID_Solicitacao = table.Column<int>(nullable: true),
                    Altura = table.Column<string>(maxLength: 3, nullable: true),
                    Peso = table.Column<double>(nullable: true),
                    DataRegisto_Peso = table.Column<DateTime>(nullable: true),
                    Motivo = table.Column<string>(maxLength: 200, nullable: true),
                    DataSuspensao = table.Column<DateTime>(nullable: true),
                    NumProfessor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socios", x => x.NumCC);
                    table.ForeignKey(
                        name: "FK_Socios_SolicitacaoProfessores_ID_Solicitacao",
                        column: x => x.ID_Solicitacao,
                        principalTable: "SolicitacaoProfessores",
                        principalColumn: "IdSolicitacao",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Socios_Admins_NumAdmin",
                        column: x => x.NumAdmin,
                        principalTable: "Admins",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Socios_Pessoas_NumCC",
                        column: x => x.NumCC,
                        principalTable: "Pessoas",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Socios_Professores_NumProfessor",
                        column: x => x.NumProfessor,
                        principalTable: "Professores",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cota",
                columns: table => new
                {
                    IdCota = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumSocio = table.Column<string>(nullable: true),
                    DataRegisto = table.Column<DateTime>(nullable: true),
                    NumeroCotasPagas = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cota", x => x.IdCota);
                    table.ForeignKey(
                        name: "FK_Cota_Socios_NumSocio",
                        column: x => x.NumSocio,
                        principalTable: "Socios",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inscricoes",
                columns: table => new
                {
                    NumSocio = table.Column<string>(nullable: false),
                    IdAula = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inscricoes", x => new { x.NumSocio, x.IdAula });
                    table.ForeignKey(
                        name: "FK_Inscricoes_Aulas_IdAula",
                        column: x => x.IdAula,
                        principalTable: "Aulas",
                        principalColumn: "IdAula",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inscricoes_Socios_NumSocio",
                        column: x => x.NumSocio,
                        principalTable: "Socios",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PlanosTreino",
                columns: table => new
                {
                    IdPlano = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumSocio = table.Column<string>(nullable: true),
                    NumProfessor = table.Column<string>(nullable: true),
                    Ativo = table.Column<bool>(nullable: false),
                    Descricao = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanosTreino", x => x.IdPlano);
                    table.ForeignKey(
                        name: "FK_PlanosTreino_Professores_NumProfessor",
                        column: x => x.NumProfessor,
                        principalTable: "Professores",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanosTreino_Socios_NumSocio",
                        column: x => x.NumSocio,
                        principalTable: "Socios",
                        principalColumn: "NumCC",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contem",
                columns: table => new
                {
                    IdPlano = table.Column<int>(nullable: false),
                    IdExercicio = table.Column<int>(nullable: false),
                    NumRepeticoes = table.Column<int>(nullable: false),
                    PeriodoDescanso = table.Column<int>(nullable: false),
                    QuantidadeSeries = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contem", x => new { x.IdPlano, x.IdExercicio });
                    table.ForeignKey(
                        name: "FK_Contem_Exercicios_IdExercicio",
                        column: x => x.IdExercicio,
                        principalTable: "Exercicios",
                        principalColumn: "IdExercicio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contem_PlanosTreino_IdPlano",
                        column: x => x.IdPlano,
                        principalTable: "PlanosTreino",
                        principalColumn: "IdPlano",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aulas_NumAdmin",
                table: "Aulas",
                column: "NumAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_Aulas_NumProfessor",
                table: "Aulas",
                column: "NumProfessor");

            migrationBuilder.CreateIndex(
                name: "IX_Contem_IdExercicio",
                table: "Contem",
                column: "IdExercicio");

            migrationBuilder.CreateIndex(
                name: "IX_Cota_NumSocio",
                table: "Cota",
                column: "NumSocio",
                unique: true,
                filter: "[NumSocio] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Exercicios_NumAdmin",
                table: "Exercicios",
                column: "NumAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_Ginasios_NumAdminNavigationNumCC",
                table: "Ginasios",
                column: "NumAdminNavigationNumCC");

            migrationBuilder.CreateIndex(
                name: "IX_Inscricoes_IdAula",
                table: "Inscricoes",
                column: "IdAula");

            migrationBuilder.CreateIndex(
                name: "IX_Mensagens_IdPessoaReceiver",
                table: "Mensagens",
                column: "IdPessoaReceiver");

            migrationBuilder.CreateIndex(
                name: "IX_Mensagens_IdPessoaSender",
                table: "Mensagens",
                column: "IdPessoaSender");

            migrationBuilder.CreateIndex(
                name: "IX_PedidosSocios_NumAdmin",
                table: "PedidosSocios",
                column: "NumAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_PlanosTreino_NumProfessor",
                table: "PlanosTreino",
                column: "NumProfessor");

            migrationBuilder.CreateIndex(
                name: "IX_PlanosTreino_NumSocio",
                table: "PlanosTreino",
                column: "NumSocio");

            migrationBuilder.CreateIndex(
                name: "IX_Professores_IdSolicitacaoNavigationIdSolicitacao",
                table: "Professores",
                column: "IdSolicitacaoNavigationIdSolicitacao");

            migrationBuilder.CreateIndex(
                name: "IX_Professores_NumAdminNavigationNumCC",
                table: "Professores",
                column: "NumAdminNavigationNumCC");

            migrationBuilder.CreateIndex(
                name: "IX_Socios_ID_Solicitacao",
                table: "Socios",
                column: "ID_Solicitacao");

            migrationBuilder.CreateIndex(
                name: "IX_Socios_NumAdmin",
                table: "Socios",
                column: "NumAdmin");

            migrationBuilder.CreateIndex(
                name: "IX_Socios_NumProfessor",
                table: "Socios",
                column: "NumProfessor");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacaoProfessores_NumAdmin",
                table: "SolicitacaoProfessores",
                column: "NumAdmin");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contem");

            migrationBuilder.DropTable(
                name: "Cota");

            migrationBuilder.DropTable(
                name: "Ginasios");

            migrationBuilder.DropTable(
                name: "Inscricoes");

            migrationBuilder.DropTable(
                name: "Mensagens");

            migrationBuilder.DropTable(
                name: "PedidosSocios");

            migrationBuilder.DropTable(
                name: "Exercicios");

            migrationBuilder.DropTable(
                name: "PlanosTreino");

            migrationBuilder.DropTable(
                name: "Aulas");

            migrationBuilder.DropTable(
                name: "Socios");

            migrationBuilder.DropTable(
                name: "Professores");

            migrationBuilder.DropTable(
                name: "SolicitacaoProfessores");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Pessoas");
        }
    }
}
