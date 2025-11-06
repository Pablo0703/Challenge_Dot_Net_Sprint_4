using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface InterfaceMotociclista
    {
        Task<IEnumerable<MotociclistaDTO>> GetAllAsync();
        Task<MotociclistaDTO> GetByIdAsync(long id);
        Task<MotociclistaDTO> CreateAsync(MotociclistaDTO dto);
        Task<MotociclistaDTO> UpdateAsync(long id, MotociclistaDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<MotociclistaDTO>> GetByNomeAsync(string nome);
    }
}
