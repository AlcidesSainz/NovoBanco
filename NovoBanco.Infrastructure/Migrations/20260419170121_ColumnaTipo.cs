using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovoBanco.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ColumnaTipo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Cuentas",
                newName: "Tipo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Cuentas",
                newName: "Type");
        }
    }
}
