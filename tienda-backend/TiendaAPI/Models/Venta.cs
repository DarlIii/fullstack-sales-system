using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Models
{
    public class Venta
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public int ClienteId { get; set; }

        public Cliente? Cliente { get; set; }

        public decimal Total { get; set; }

        public List<VentaDetalle> Detalles { get; set; } = new();
    }
}