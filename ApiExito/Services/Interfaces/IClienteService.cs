using ApiExito.Model;

namespace ApiExito.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente> GetByIdAsync(int id);
        Task<Cliente> AddAsync(Cliente cliente);
        Task<bool> UpdateAsync(int id, Cliente cliente);
        Task<bool> DeleteAsync(int id);
        Task<Cliente> GetByCedulaAsync(int? cedula);
        Task<Cliente> GetByNitAsync(string nit);
        bool Verify(int id);
    }
}
