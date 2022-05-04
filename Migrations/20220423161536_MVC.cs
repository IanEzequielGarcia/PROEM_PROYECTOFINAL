using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PROEM_PROYECTOFINALMVC.Migrations
{
    public partial class MVC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Codigo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<float>(type: "real", nullable: false),
                    FacturasNumero = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Codigo);
                    table.ForeignKey(
                        name: "FK_Item_Factura_FacturasNumero",
                        column: x => x.FacturasNumero,
                        principalTable: "Factura",
                        principalColumn: "Numero");
                });
            migrationBuilder.CreateIndex(
                name: "IX_Item_FacturasNumero",
                table: "Item",
                column: "FacturasNumero");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Pago");

            migrationBuilder.DropTable(
                name: "Factura");

            migrationBuilder.DropTable(
                name: "Cliente");
        }
    }
}
