using System.ComponentModel.DataAnnotations;

namespace PROEM_PROYECTOFINALMVC.Models
{
    public class Items
    {
        public Items()
        {
        }

        public Items(string descripcion, int cantidad, float precio,int idFactura)
        {
            Descripcion = descripcion;
            Cantidad = cantidad;
            Precio = precio;
            IdFactura = idFactura;
        }

        [Key]
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public float Precio { get; set; }
        public int IdFactura { get; set; }
        public Facturas Facturas { get; set; }

    }
}
