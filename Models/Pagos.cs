using System.ComponentModel.DataAnnotations;

namespace PROEM_PROYECTOFINALMVC.Models
{
    public class Pagos
    {
        public Pagos()
        {
        }

        public Pagos(DateTime fecha, float importe)
        {
            Fecha = fecha;
            Importe = importe;
        }

        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public Cliente Cliente { get; set; }
        public int IdCliente { get; set; }
        public float Importe { get; set; }
    }
}
