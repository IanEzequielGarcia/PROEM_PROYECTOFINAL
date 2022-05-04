using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROEM_PROYECTOFINALMVC.Migrations
{
    public partial class itemFactura : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Factura_FacturasNumero",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_FacturasNumero",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "FacturasNumero",
                table: "Item");

            migrationBuilder.AddColumn<int>(
                name: "IdFactura",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdFactura",
                table: "Item");

            migrationBuilder.AddColumn<int>(
                name: "FacturasNumero",
                table: "Item",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_FacturasNumero",
                table: "Item",
                column: "FacturasNumero");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Factura_FacturasNumero",
                table: "Item",
                column: "FacturasNumero",
                principalTable: "Factura",
                principalColumn: "Numero");
        }
    }
}
