using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface InterfaceTipoMoto
    {
        Task<IEnumerable<TipoMotoDTO>> GetAllAsync();
        Task<TipoMotoDTO> GetByIdAsync(long id);
        Task<TipoMotoDTO> CreateAsync(TipoMotoDTO dto);
        Task<TipoMotoDTO> UpdateAsync(long id, TipoMotoDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<TipoMotoDTO>> GetByDescricaoAsync(string descricao);
    }
}
