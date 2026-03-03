using TiendaAPI.Models;

namespace TiendaAPI.Services
{
    public interface IVentaService
    {
        Task<IEnumerable<Venta>> GetAllAsync();
        Task<Venta?> GetByIdAsync(int id);

        // Registrar una venta (calcula total y descuenta stock)
        Task<Venta> CreateAsync(Venta venta);
    }
}