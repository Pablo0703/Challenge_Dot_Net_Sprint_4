using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface InterfaceStatusOperacao
    {
        Task<IEnumerable<StatusOperacaoDTO>> GetAllAsync();
        Task<StatusOperacaoDTO> GetByIdAsync(long id);
        Task<StatusOperacaoDTO> CreateAsync(StatusOperacaoDTO dto);
        Task<StatusOperacaoDTO> UpdateAsync(long id, StatusOperacaoDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<StatusOperacaoDTO>> GetByDescricaoAsync(string descricao);
    }
}
