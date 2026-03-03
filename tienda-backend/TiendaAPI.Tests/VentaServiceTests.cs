using Microsoft.EntityFrameworkCore;
using Moq;
using TiendaAPI.Models;
using TiendaAPI.Repositories;
using TiendaAPI.Services;
using Xunit;

namespace TiendaAPI.Tests
{
    public class VentaServiceTests
    {
        private TiendaContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TiendaContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new TiendaContext(options);
        }

        [Fact]
        public async Task CreateAsync_CalculaTotalYDescuentaStock()
        {
            // Arrange
            using var context = CreateInMemoryContext();

            var cliente = new Cliente { Id = 1, Nombre = "Juan", Email = "juan@test.com", Telefono = "809" };
            var producto = new Producto { Id = 1, Nombre = "Laptop", Descripcion = "X", Precio = 100m, Stock = 5, CategoriaId = 1 };

            context.Clientes.Add(cliente);
            context.Productos.Add(producto);
            await context.SaveChangesAsync();

            var ventaRepoMock = new Mock<IVentaRepository>();

            // Capturamos la venta que el service manda al repo para validarla
            Venta? ventaGuardada = null;
            ventaRepoMock
                .Setup(r => r.AddAsync(It.IsAny<Venta>()))
                .Callback<Venta>(v => ventaGuardada = v)
                .Returns(Task.CompletedTask);

            var service = new VentaService(ventaRepoMock.Object, context);

            var venta = new Venta
            {
                ClienteId = 1,
                Detalles = new List<VentaDetalle>
                {
                    new VentaDetalle { ProductoId = 1, Cantidad = 2 }
                }
            };

            // Act
            var result = await service.CreateAsync(venta);

            // Assert
            Assert.NotNull(ventaGuardada);
            Assert.Equal(200m, result.Total);          // 100 * 2
            Assert.Single(result.Detalles);
            Assert.Equal(100m, result.Detalles[0].PrecioUnitario);
            Assert.Equal(200m, result.Detalles[0].Subtotal);

            var productoActualizado = await context.Productos.FindAsync(1);
            Assert.NotNull(productoActualizado);
            Assert.Equal(3, productoActualizado!.Stock); // 5 - 2
        }
    }
}