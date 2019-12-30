using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthUp.Migrations
{
    public partial class fix1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoDivulgacao",
                table: "AulasGrupo",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoDivulgacao",
                table: "AulasGrupo");
        }
    }
}
