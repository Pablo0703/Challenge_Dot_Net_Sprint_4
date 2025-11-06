using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface InterfaceLocalizacaoMoto
    {
        Task<IEnumerable<LocalizacaoMotoDTO>> GetAllAsync();
        Task<LocalizacaoMotoDTO> GetByIdAsync(long id);
        Task<LocalizacaoMotoDTO> CreateAsync(LocalizacaoMotoDTO dto);
        Task<LocalizacaoMotoDTO> UpdateAsync(long id, LocalizacaoMotoDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<LocalizacaoMotoDTO>> GetByIdMotoAsync(long idMoto);
    }
}
