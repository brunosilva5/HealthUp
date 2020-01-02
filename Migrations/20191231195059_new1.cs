using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthUp.Migrations
{
    public partial class new1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AulasGrupo_Aulas_IdAula",
                table: "AulasGrupo");

            migrationBuilder.AlterColumn<int>(
                name: "IdAula",
                table: "AulasGrupo",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "IdAula",
                table: "Aulas",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Aulas_AulasGrupo_IdAula",
                table: "Aulas",
                column: "IdAula",
                principalTable: "AulasGrupo",
                principalColumn: "IdAula",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Aulas_AulasGrupo_IdAula",
                table: "Aulas");

            migrationBuilder.AlterColumn<int>(
                name: "IdAula",
                table: "AulasGrupo",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "IdAula",
                table: "Aulas",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_AulasGrupo_Aulas_IdAula",
                table: "AulasGrupo",
                column: "IdAula",
                principalTable: "Aulas",
                principalColumn: "IdAula",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
