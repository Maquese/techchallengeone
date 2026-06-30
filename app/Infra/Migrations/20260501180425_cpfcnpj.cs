using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class cpfcnpj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cpf",
                table: "Cliente",
                newName: "Documento");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_Cpf",
                table: "Cliente",
                newName: "IX_Cliente_Documento");

            migrationBuilder.AlterColumn<string>(
                name: "Documento",
                table: "Cliente",
                type: "varchar(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(11)",
                oldMaxLength: 11);

            migrationBuilder.AddColumn<string>(
                name: "TipoDocumento",
                table: "Cliente",
                type: "varchar(4)",
                maxLength: 4,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoDocumento",
                table: "Cliente");

            migrationBuilder.RenameColumn(
                name: "Documento",
                table: "Cliente",
                newName: "Cpf");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_Documento",
                table: "Cliente",
                newName: "IX_Cliente_Cpf");

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Cliente",
                type: "varchar(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(14)",
                oldMaxLength: 14);
        }
    }
}
