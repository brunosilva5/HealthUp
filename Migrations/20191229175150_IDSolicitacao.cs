using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthUp.Migrations
{
    public partial class IDSolicitacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professores_SolicitacaoProfessores_IdSolicitacao",
                table: "Professores");

            migrationBuilder.DropForeignKey(
                name: "FK_Socios_SolicitacaoProfessores_ID_Solicitacao",
                table: "Socios");

            migrationBuilder.AlterColumn<int>(
                name: "ID_Solicitacao",
                table: "Socios",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "IdSolicitacao",
                table: "Professores",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Professores_SolicitacaoProfessores_IdSolicitacao",
                table: "Professores",
                column: "IdSolicitacao",
                principalTable: "SolicitacaoProfessores",
                principalColumn: "IdSolicitacao",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Socios_SolicitacaoProfessores_ID_Solicitacao",
                table: "Socios",
                column: "ID_Solicitacao",
                principalTable: "SolicitacaoProfessores",
                principalColumn: "IdSolicitacao",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Professores_SolicitacaoProfessores_IdSolicitacao",
                table: "Professores");

            migrationBuilder.DropForeignKey(
                name: "FK_Socios_SolicitacaoProfessores_ID_Solicitacao",
                table: "Socios");

            migrationBuilder.AlterColumn<int>(
                name: "ID_Solicitacao",
                table: "Socios",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "IdSolicitacao",
                table: "Professores",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Professores_SolicitacaoProfessores_IdSolicitacao",
                table: "Professores",
                column: "IdSolicitacao",
                principalTable: "SolicitacaoProfessores",
                principalColumn: "IdSolicitacao",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Socios_SolicitacaoProfessores_ID_Solicitacao",
                table: "Socios",
                column: "ID_Solicitacao",
                principalTable: "SolicitacaoProfessores",
                principalColumn: "IdSolicitacao",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
