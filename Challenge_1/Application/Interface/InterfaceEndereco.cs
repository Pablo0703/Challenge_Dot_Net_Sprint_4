using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface InterfaceEndereco
    {
        Task<IEnumerable<EnderecoDTO>> GetAllAsync();
        Task<EnderecoDTO> GetByIdAsync(long id);
        Task<EnderecoDTO> CreateAsync(EnderecoDTO dto);
        Task<EnderecoDTO> UpdateAsync(long id, EnderecoDTO dto);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<EnderecoDTO>> GetByCidadeAsync(string cidade);
    }
}
