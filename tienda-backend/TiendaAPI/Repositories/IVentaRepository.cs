using TiendaAPI.Models;

namespace TiendaAPI.Repositories
{
    public interface IVentaRepository
    {
        Task<IEnumerable<Venta>> GetAllAsync();
        Task<Venta?> GetByIdAsync(int id);
        Task AddAsync(Venta venta);
    }
}