using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Programacion_III.Migrations
{
    /// <inheritdoc />
    public partial class AgendamentoPersonalizado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescricaoPersonalizada",
                table: "Agendamentos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescricaoPersonalizada",
                table: "Agendamentos");
        }
    }
}
