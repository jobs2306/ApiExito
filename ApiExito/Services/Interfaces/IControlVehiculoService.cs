using ApiExito.Model;

namespace ApiExito.Services.Interfaces
{
    public interface IControlVehiculoService
    {
        Task<IEnumerable<ControlVehiculo>> GetAllAsync();
        Task<ControlVehiculo> GetByIdAsync(int id);
        Task<ControlVehiculo> AddAsync(ControlVehiculo ControlVehiculo);
        Task<bool> UpdateAsync(int id, ControlVehiculo ControlVehiculo);
        Task<bool> DeleteAsync(int id);
    }
}
