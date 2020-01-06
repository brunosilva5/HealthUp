using Microsoft.EntityFrameworkCore.Migrations;

namespace HealthUp.Migrations
{
    public partial class fixname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contem_Exercicios_IdExercicio",
                table: "Contem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercicios",
                table: "Exercicios");

            migrationBuilder.DropColumn(
                name: "IdExericio",
                table: "Exercicios");

            migrationBuilder.AddColumn<int>(
                name: "IdExercicio",
                table: "Exercicios",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercicios",
                table: "Exercicios",
                column: "IdExercicio");

            migrationBuilder.AddForeignKey(
                name: "FK_Contem_Exercicios_IdExercicio",
                table: "Contem",
                column: "IdExercicio",
                principalTable: "Exercicios",
                principalColumn: "IdExercicio",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contem_Exercicios_IdExercicio",
                table: "Contem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercicios",
                table: "Exercicios");

            migrationBuilder.DropColumn(
                name: "IdExercicio",
                table: "Exercicios");

            migrationBuilder.AddColumn<int>(
                name: "IdExericio",
                table: "Exercicios",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercicios",
                table: "Exercicios",
                column: "IdExericio");

            migrationBuilder.AddForeignKey(
                name: "FK_Contem_Exercicios_IdExercicio",
                table: "Contem",
                column: "IdExercicio",
                principalTable: "Exercicios",
                principalColumn: "IdExericio",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
