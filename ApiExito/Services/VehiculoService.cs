using ApiExito.Data;
using ApiExito.Model;
using ApiExito.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiExito.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly ApplicationDbContext _context;

        public VehiculoService(ApplicationDbContext Context)
        {
            _context = Context;
        }

        public async Task<IEnumerable<Vehiculo>> GetAllAsync()
        {
            return await _context.Vehiculo.ToListAsync();
        }

        public async Task<Vehiculo> GetByIdAsync(int id)
        {
            return await _context.Vehiculo.FindAsync(id);
        }

        public async Task<Vehiculo> AddAsync(Vehiculo vehiculo)
        {
            _context.Vehiculo.Add(vehiculo);
            await _context.SaveChangesAsync();
            return vehiculo;
        }

        public async Task<bool> UpdateAsync(int id, Vehiculo vehiculo)
        {
            var Update = await _context.Vehiculo.FindAsync(id);
            if (Update == null)
            {
                return false;
            }

            _context.Entry(Update).CurrentValues.SetValues(vehiculo);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (Verify(id)) return false;

            var objeto = await _context.Vehiculo.FindAsync(id);
            if (objeto == null)
            {
                return false;
            }
            _context.Vehiculo.Remove(objeto);

            await _context.SaveChangesAsync();
            return true;
        }

        public bool Verify(int id)
        {
            return _context.ControlVehiculo.Any(p => p.Vehiculoid == id);
        }
    }
}
