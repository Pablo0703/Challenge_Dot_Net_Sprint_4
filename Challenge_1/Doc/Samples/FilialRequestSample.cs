using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class FilialRequestSample : IExamplesProvider<FilialDTO>
    {
        public FilialDTO GetExamples()
        {
            return new FilialDTO
            {
                Nome = "Mottu São Paulo",
                Cnpj = "98765432000199",
                Telefone = "(11) 9999-8888",
                Email = "sp@mottu.com.br",
                Ativo = "S",
                IdEndereco = 1
            };
        }
    }
}