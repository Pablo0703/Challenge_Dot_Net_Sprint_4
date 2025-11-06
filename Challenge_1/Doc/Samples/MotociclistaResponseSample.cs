using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class MotociclistaResponseSample : IExamplesProvider<MotociclistaDTO>
    {
        public MotociclistaDTO GetExamples()
        {
            return new MotociclistaDTO
            {
                Id = 1,
                Nome = "João Silva",
                Cpf = "12345678901",
                Cnh = "SP12345678",
                DataValidadeCnh = DateTime.Parse("2030-01-01"),
                Telefone = "(11) 98765-4321",
                Email = "joao@email.com",
                DataCadastro = DateTime.UtcNow,
                Ativo = "S",
                IdEndereco = 2
            };
        }
    }
}
