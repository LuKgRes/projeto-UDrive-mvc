using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Programacion_III.Migrations
{
    /// <inheritdoc />
    public partial class FixRenameFechaHoraParaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
    name: "FechaHora",
    table: "Agendamentos",
    newName: "Data");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
       name: "Data",
       table: "Agendamentos",
       newName: "FechaHora");
        }
    }
}
