using ApiExito.Model;

namespace ApiExito.Services.Interfaces
{
    public interface IControlVehiculoService
    {
        Task<IEnumerable<ControlVehiculo>> GetAllAsync();
        Task<IEnumerable<ControlVehiculo>> GetAllByVehiculoAsync(int vehiculoId);
        Task<ControlVehiculo> GetVehiculoTaller(int vehiculoId);
        Task<IEnumerable<ControlVehiculo>> GetVehiculosTaller();
        Task<ControlVehiculo> GetByIdAsync(int id);
        Task<ControlVehiculo> AddAsync(ControlVehiculo ControlVehiculo);
        Task<bool> UpdateAsync(int id, ControlVehiculo ControlVehiculo);
        Task<bool> DeleteAsync(int id);
    }
}
