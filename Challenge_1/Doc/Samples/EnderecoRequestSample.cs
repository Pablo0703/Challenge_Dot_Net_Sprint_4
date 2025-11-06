using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class EnderecoRequestSample : IExamplesProvider<EnderecoDTO>
    {
        public EnderecoDTO GetExamples()
        {
            return new EnderecoDTO
            {
                Logradouro = "Avenida Paulista",
                Numero = "1000",
                Complemento = "Conjunto 101",
                Bairro = "Bela Vista",
                Cep = "01310000",
                Cidade = "São Paulo",
                Estado = "SP",
                Pais = "Brasil"
            };
        }
    }
}