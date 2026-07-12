using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Programacion_III.Migrations
{
    /// <inheritdoc />
    public partial class EstadoServico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstadoServicos",
                table: "Servicos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Servicos",
                keyColumn: "ServicosId",
                keyValue: 1,
                column: "EstadoServicos",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Servicos",
                keyColumn: "ServicosId",
                keyValue: 2,
                column: "EstadoServicos",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Servicos",
                keyColumn: "ServicosId",
                keyValue: 3,
                column: "EstadoServicos",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Servicos",
                keyColumn: "ServicosId",
                keyValue: 4,
                column: "EstadoServicos",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Servicos",
                keyColumn: "ServicosId",
                keyValue: 5,
                column: "EstadoServicos",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoServicos",
                table: "Servicos");
        }
    }
}
