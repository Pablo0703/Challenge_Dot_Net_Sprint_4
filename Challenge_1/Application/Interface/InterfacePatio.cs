using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface InterfacePatio
    {
        Task<IEnumerable<PatioDTO>> GetAllAsync();
        Task<PatioDTO> GetByIdAsync(long id);
        Task<PatioDTO> CreateAsync(PatioDTO dto);
        Task<PatioDTO> UpdateAsync(long id, PatioDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<PatioDTO>> GetByNomeAsync(string nome);
    }
}
