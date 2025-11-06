using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface InterfaceMoto
    {
        Task<IEnumerable<MotoDTO>> GetAllAsync();
        Task<MotoDTO> GetByIdAsync(long id);
        Task<MotoDTO> CreateAsync(MotoDTO dto);
        Task<MotoDTO> UpdateAsync(long id, MotoDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<MotoDTO>> GetByPlacaAsync(string placa);
    }
}
