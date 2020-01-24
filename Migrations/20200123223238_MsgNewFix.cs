using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthUp.Migrations
{
    public partial class MsgNewFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mensagens_Pessoas_IdPessoa",
                table: "Mensagens");

            migrationBuilder.DropIndex(
                name: "IX_Mensagens_IdPessoa",
                table: "Mensagens");

            migrationBuilder.DropColumn(
                name: "IdPessoa",
                table: "Mensagens");

            migrationBuilder.AddColumn<string>(
                name: "IdPessoaReceiver",
                table: "Mensagens",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mensagens_IdPessoaReceiver",
                table: "Mensagens",
                column: "IdPessoaReceiver");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagens_Pessoas_IdPessoaReceiver",
                table: "Mensagens",
                column: "IdPessoaReceiver",
                principalTable: "Pessoas",
                principalColumn: "NumCC",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mensagens_Pessoas_IdPessoaReceiver",
                table: "Mensagens");

            migrationBuilder.DropIndex(
                name: "IX_Mensagens_IdPessoaReceiver",
                table: "Mensagens");

            migrationBuilder.DropColumn(
                name: "IdPessoaReceiver",
                table: "Mensagens");

            migrationBuilder.AddColumn<string>(
                name: "IdPessoa",
                table: "Mensagens",
                type: "nvarchar(8)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mensagens_IdPessoa",
                table: "Mensagens",
                column: "IdPessoa");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagens_Pessoas_IdPessoa",
                table: "Mensagens",
                column: "IdPessoa",
                principalTable: "Pessoas",
                principalColumn: "NumCC",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
