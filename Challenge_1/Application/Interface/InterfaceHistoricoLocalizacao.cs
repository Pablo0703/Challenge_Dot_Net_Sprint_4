using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface InterfaceHistoricoLocalizacao
    {
        Task<IEnumerable<HistoricoLocalizacaoDTO>> GetAllAsync();
        Task<HistoricoLocalizacaoDTO> GetByIdAsync(long id);
        Task<HistoricoLocalizacaoDTO> CreateAsync(HistoricoLocalizacaoDTO dto);
        Task<HistoricoLocalizacaoDTO> UpdateAsync(long id, HistoricoLocalizacaoDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<HistoricoLocalizacaoDTO>> GetByIdMotoAsync(long idMoto);
    }
}
