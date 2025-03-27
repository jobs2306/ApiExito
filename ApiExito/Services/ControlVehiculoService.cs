using ApiExito.Data;
using ApiExito.Model;
using ApiExito.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ApiExito.Services
{
    public class ControlVehiculoService : IControlVehiculoService
    {
        private readonly ApplicationDbContext _context;

        public ControlVehiculoService(ApplicationDbContext Context)
        {
            _context = Context;
        }

        public async Task<IEnumerable<ControlVehiculo>> GetAllAsync()
        {
            return await _context.ControlVehiculo.ToListAsync();
        }

        public async Task<ControlVehiculo> GetByIdAsync(int id)
        {
            return await _context.ControlVehiculo.FindAsync(id);
        }

        public async Task<ControlVehiculo> AddAsync(ControlVehiculo control)
        {
            _context.ControlVehiculo.Add(control);
            await _context.SaveChangesAsync();
            return control;
        }

        public async Task<bool> UpdateAsync(int id,ControlVehiculo control)
        {
            var Update = await _context.ControlVehiculo.FindAsync(id);
            if (Update == null)
            {
                return false;
            }

            _context.Entry(Update).CurrentValues.SetValues(control);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var objeto = await _context.ControlVehiculo.FindAsync(id);
            if (objeto == null)
            {
                return false;
            }
            _context.ControlVehiculo.Remove(objeto);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
