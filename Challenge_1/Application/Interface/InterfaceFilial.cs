using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface InterfaceFilial
    {
        Task<IEnumerable<FilialDTO>> GetAllAsync();
        Task<FilialDTO> GetByIdAsync(long id);
        Task<FilialDTO> CreateAsync(FilialDTO dto);
        Task<FilialDTO> UpdateAsync(long id, FilialDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<FilialDTO>> GetByNomeAsync(string nome);
    }
}
