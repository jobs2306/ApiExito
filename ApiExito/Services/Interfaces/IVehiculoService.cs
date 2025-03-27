using ApiExito.Model;

namespace ApiExito.Services.Interfaces
{
    public interface IVehiculoService
    {
        Task<IEnumerable<Vehiculo>> GetAllAsync();
        Task<Vehiculo> GetByIdAsync(int id);
        Task<Vehiculo> AddAsync(Vehiculo Vehiculo);
        Task<bool> UpdateAsync(int id, Vehiculo Vehiculo);
        Task<bool> DeleteAsync(int id);
        bool Verify(int id);
    }
}
