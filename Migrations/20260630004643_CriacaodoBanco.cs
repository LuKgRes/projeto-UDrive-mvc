using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Programacion_III.Migrations
{
    /// <inheritdoc />
    public partial class CriacaodoBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Servicoss_ServicosId",
                table: "Agendamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Servicoss",
                table: "Servicoss");

            migrationBuilder.RenameTable(
                name: "Servicoss",
                newName: "Servicos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Servicos",
                table: "Servicos",
                column: "ServicosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Servicos_ServicosId",
                table: "Agendamentos",
                column: "ServicosId",
                principalTable: "Servicos",
                principalColumn: "ServicosId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agendamentos_Servicos_ServicosId",
                table: "Agendamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Servicos",
                table: "Servicos");

            migrationBuilder.RenameTable(
                name: "Servicos",
                newName: "Servicoss");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Servicoss",
                table: "Servicoss",
                column: "ServicosId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agendamentos_Servicoss_ServicosId",
                table: "Agendamentos",
                column: "ServicosId",
                principalTable: "Servicoss",
                principalColumn: "ServicosId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
