using Microsoft.EntityFrameworkCore;
using TiendaAPI.Models;
using TiendaAPI.Repositories;

namespace TiendaAPI.Services
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepo;
        private readonly TiendaContext _context;

        public VentaService(IVentaRepository ventaRepo, TiendaContext context)
        {
            _ventaRepo = ventaRepo;
            _context = context;
        }

        public async Task<IEnumerable<Venta>> GetAllAsync()
        {
            return await _ventaRepo.GetAllAsync();
        }

        public async Task<Venta?> GetByIdAsync(int id)
        {
            return await _ventaRepo.GetByIdAsync(id);
        }

        public async Task<Venta> CreateAsync(Venta venta)
        {
            // Validar cliente
            var clienteExiste = await _context.Clientes.AnyAsync(c => c.Id == venta.ClienteId);
            if (!clienteExiste)
                throw new InvalidOperationException("El cliente no existe.");

            if (venta.Detalles == null || venta.Detalles.Count == 0)
                throw new InvalidOperationException("La venta debe tener al menos un producto.");

            decimal total = 0m;

            foreach (var d in venta.Detalles)
            {
                var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == d.ProductoId);
                if (producto == null)
                    throw new InvalidOperationException($"Producto {d.ProductoId} no existe.");

                if (d.Cantidad <= 0)
                    throw new InvalidOperationException("La cantidad debe ser mayor que 0.");

                if (producto.Stock < d.Cantidad)
                    throw new InvalidOperationException($"Stock insuficiente para el producto {producto.Nombre}.");

                // Precio y subtotal
                d.PrecioUnitario = producto.Precio;
                d.Subtotal = d.PrecioUnitario * d.Cantidad;

                // Descontar stock
                producto.Stock -= d.Cantidad;

                total += d.Subtotal;
            }

            venta.Total = total;
            venta.Fecha = DateTime.Now;

            await _ventaRepo.AddAsync(venta);
            return venta;
        }
    }
}