using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROEM_PROYECTOFINALMVC.Migrations
{
    public partial class SaldoCuentaCorriente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "SaldoCuentaCorriente",
                table: "Cliente",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaldoCuentaCorriente",
                table: "Cliente");
        }
    }
}
