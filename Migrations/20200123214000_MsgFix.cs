using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthUp.Migrations
{
    public partial class MsgFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdPessoaSender",
                table: "Mensagens",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Mensagens_IdPessoaSender",
                table: "Mensagens",
                column: "IdPessoaSender");

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagens_Pessoas_IdPessoaSender",
                table: "Mensagens",
                column: "IdPessoaSender",
                principalTable: "Pessoas",
                principalColumn: "NumCC",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mensagens_Pessoas_IdPessoaSender",
                table: "Mensagens");

            migrationBuilder.DropIndex(
                name: "IX_Mensagens_IdPessoaSender",
                table: "Mensagens");

            migrationBuilder.AlterColumn<string>(
                name: "IdPessoaSender",
                table: "Mensagens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
