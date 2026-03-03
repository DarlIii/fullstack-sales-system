using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public List<Producto> Productos { get; set; } = new();
    }
}