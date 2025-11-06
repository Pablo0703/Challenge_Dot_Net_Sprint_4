using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface InterfaceNotaFiscal
    {
        Task<IEnumerable<NotaFiscalDTO>> GetAllAsync();
        Task<NotaFiscalDTO> GetByIdAsync(long id);
        Task<NotaFiscalDTO> CreateAsync(NotaFiscalDTO dto);
        Task<NotaFiscalDTO> UpdateAsync(long id, NotaFiscalDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<NotaFiscalDTO>> GetByNumeroAsync(string numero);
    }
}
