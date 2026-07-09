using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Programacion_III.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Servicos",
                newName: "Tempo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tempo",
                table: "Servicos",
                newName: "Data");
        }
    }
}
