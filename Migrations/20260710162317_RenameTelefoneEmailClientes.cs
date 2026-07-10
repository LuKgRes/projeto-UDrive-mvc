using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Programacion_III.Migrations
{
    /// <inheritdoc />
    public partial class RenameTelefoneEmailClientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
     name: "Telefono",
     table: "Clientes",
     newName: "Telefone");

            migrationBuilder.RenameColumn(
       name: "Correo",
       table: "Clientes",
       newName: "Email");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
       name: "Telefone",
       table: "Clientes",
       newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Clientes",
                newName: "Correo");
        }
    }
}
