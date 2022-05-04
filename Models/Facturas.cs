using System.ComponentModel.DataAnnotations;

namespace PROEM_PROYECTOFINALMVC.Models
{
    public class Facturas
    {
        public Facturas()
        {
        }

        public Facturas(int numero, DateTime fecha):base()
        {
            Numero = numero;
            Fecha = fecha;
        }

        public Cliente Cliente { get; set; }
        public int IdCliente { get; set; }
        [Key]
        public int Numero { get; set; }
        public DateTime Fecha { get; set; }
    }
}
