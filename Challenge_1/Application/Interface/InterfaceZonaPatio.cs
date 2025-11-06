using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface InterfaceZonaPatio
    {
        Task<IEnumerable<ZonaPatioDTO>> GetAllAsync();
        Task<ZonaPatioDTO> GetByIdAsync(long id);
        Task<ZonaPatioDTO> CreateAsync(ZonaPatioDTO dto);
        Task<ZonaPatioDTO> UpdateAsync(long id, ZonaPatioDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<ZonaPatioDTO>> GetByNomeZonaAsync(string nomeZona);
    }
}
