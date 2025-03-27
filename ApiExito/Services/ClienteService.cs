using ApiExito.Services.Interfaces;
using ApiExito.Model;
using ApiExito.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiExito.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;

        public ClienteService(ApplicationDbContext Context)
        {
            _context = Context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Cliente.ToListAsync();
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            return await _context.Cliente.FindAsync(id);
        }

        public async Task<Cliente> AddAsync(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> UpdateAsync(int id,Cliente cliente)
        {
            var Update = await _context.Cliente.FindAsync(id);
            if (Update == null)
            {
                return false;
            }

            _context.Entry(Update).CurrentValues.SetValues(cliente);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (Verify(id)) return false;

            var objeto = await _context.Cliente.FindAsync(id);
            if (objeto == null)
            {
                return false;
            }
            _context.Cliente.Remove(objeto);

            await _context.SaveChangesAsync();
            return true;
        }

        public bool Verify(int id)
        {
            return _context.Vehiculo.Any(p => p.Clienteid == id);
        }
    }
}
