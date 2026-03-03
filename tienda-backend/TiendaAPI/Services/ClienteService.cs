using TiendaAPI.Models;
using TiendaAPI.Repositories;

namespace TiendaAPI.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repo;

        public ClienteService(IClienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            await _repo.AddAsync(cliente);
            return cliente;
        }

        public async Task<bool> UpdateAsync(int id, Cliente cliente)
        {
            var existente = await _repo.GetByIdAsync(id);
            if (existente == null) return false;

            existente.Nombre = cliente.Nombre;
            existente.Email = cliente.Email;
            existente.Telefono = cliente.Telefono;

            await _repo.UpdateAsync(existente);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existente = await _repo.GetByIdAsync(id);
            if (existente == null) return false;

            await _repo.DeleteAsync(existente);
            return true;
        }
    }
}