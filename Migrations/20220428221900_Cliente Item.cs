using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROEM_PROYECTOFINALMVC.Migrations
{
    public partial class ClienteItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdCliente",
                table: "Pago",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FacturasNumero",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdCliente",
                table: "Factura",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Item_FacturasNumero",
                table: "Item",
                column: "FacturasNumero");

            migrationBuilder.AddForeignKey(
                name: "FK_Item_Factura_FacturasNumero",
                table: "Item",
                column: "FacturasNumero",
                principalTable: "Factura",
                principalColumn: "Numero",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Item_Factura_FacturasNumero",
                table: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Item_FacturasNumero",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "Pago");

            migrationBuilder.DropColumn(
                name: "FacturasNumero",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "IdCliente",
                table: "Factura");
        }
    }
}
