using System.ComponentModel.DataAnnotations;

namespace PROEM_PROYECTOFINALMVC.Models
{
    public class Cliente
    {
        public Cliente()
        {
            SaldoCuentaCorriente = 0;
        }

        public Cliente(string nombre, string direccion):base()
        {
            Nombre = nombre;
            Direccion = direccion;
        }

        [Key]
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public float SaldoCuentaCorriente { get; set; }
    }
}
