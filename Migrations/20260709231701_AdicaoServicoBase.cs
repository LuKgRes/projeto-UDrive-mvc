using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Proyecto_Programacion_III.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoServicoBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Personalizado",
                table: "Servicos",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Servicos",
                columns: new[] { "ServicosId", "Descricao", "Estado", "Nome", "Personalizado", "Tempo", "Valor" },
                values: new object[,]
                {
                    { 1, "Troca de óleo do motor", 0, "Troca de óleo + filtros", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 235.00m },
                    { 2, "Troca das pastilhas de freio para manter a eficiência e a segurança da frenagem", 0, "Troca de pastilhas", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 320.00m },
                    { 3, "Ajuste da direção para manter as rodas alinhadas e evitar desgaste irregular", 0, "Alinhamento", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 140.00m },
                    { 4, "Balanceamento das rodas para reduzir vibrações e aumentar a estabilidade", 0, "Balanceamento", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 100.00m },
                    { 5, "Diagnóstico e reparo de componentes mecânicos do veículo", 0, "Geral", false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 320.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Servicos",
                keyColumn: "ServicosId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Servicos",
                keyColumn: "ServicosId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Servicos",
                keyColumn: "ServicosId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Servicos",
                keyColumn: "ServicosId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Servicos",
                keyColumn: "ServicosId",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Personalizado",
                table: "Servicos");
        }
    }
}
