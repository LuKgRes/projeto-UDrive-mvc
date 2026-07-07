using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_Programacion_III.Migrations
{
    /// <inheritdoc />
    public partial class ColunaDataServico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duracao",
                table: "Servicos");

            migrationBuilder.AddColumn<DateTime>(
                name: "Data",
                table: "Servicos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Servicos");

            migrationBuilder.AddColumn<int>(
                name: "Duracao",
                table: "Servicos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
