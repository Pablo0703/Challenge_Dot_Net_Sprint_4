using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface InterfaceStatusMoto
    {
        Task<IEnumerable<StatusMotoDTO>> GetAllAsync();
        Task<StatusMotoDTO> GetByIdAsync(long id);
        Task<StatusMotoDTO> CreateAsync(StatusMotoDTO dto);
        Task<StatusMotoDTO> UpdateAsync(long id, StatusMotoDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<StatusMotoDTO>> GetByNomeAsync(string nome);
    }
}
