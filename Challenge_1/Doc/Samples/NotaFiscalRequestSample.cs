using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class NotaFiscalRequestSample : IExamplesProvider<NotaFiscalDTO>
    {
        public NotaFiscalDTO GetExamples()
        {
            return new NotaFiscalDTO
            {
                Numero = "12345",
                Serie = "1",
                Modelo = "55",
                ChaveAcesso = "35190304552144000125550010012345678901234567",
                DataEmissao = DateTime.Parse("2023-01-01"),
                ValorTotal = 25000,
                Fornecedor = "Mottu Motors",
                CnpjFornecedor = "12345678000199"
            };
        }
    }
}