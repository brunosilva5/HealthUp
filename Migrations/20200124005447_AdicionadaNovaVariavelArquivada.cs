using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthUp.Migrations
{
    public partial class AdicionadaNovaVariavelArquivada : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Arquivada",
                table: "Mensagens");

            migrationBuilder.AddColumn<bool>(
                name: "Arquivada_Receiver",
                table: "Mensagens",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Arquivada_Sender",
                table: "Mensagens",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Arquivada_Receiver",
                table: "Mensagens");

            migrationBuilder.DropColumn(
                name: "Arquivada_Sender",
                table: "Mensagens");

            migrationBuilder.AddColumn<bool>(
                name: "Arquivada",
                table: "Mensagens",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
