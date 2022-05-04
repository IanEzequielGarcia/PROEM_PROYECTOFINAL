using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROEM_PROYECTOFINALMVC.Migrations
{
    public partial class SacandoSaldoCliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Saldo",
                table: "Cliente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Saldo",
                table: "Cliente",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
