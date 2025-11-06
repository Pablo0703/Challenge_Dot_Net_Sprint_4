using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Challenge_1.Doc.Samples
{
    public class MotociclistaRequestSample : IExamplesProvider<MotociclistaDTO>
    {
        public MotociclistaDTO GetExamples()
        {
            return new MotociclistaDTO
            {
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