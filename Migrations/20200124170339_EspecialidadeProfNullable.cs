using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthUp.Migrations
{
    public partial class EspecialidadeProfNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Especialidade",
                table: "Professores",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Especialidade",
                table: "Professores",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
