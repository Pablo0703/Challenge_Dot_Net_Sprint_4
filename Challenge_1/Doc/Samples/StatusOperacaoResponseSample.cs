using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class StatusOperacaoResponseSample : IExamplesProvider<StatusOperacaoDTO>
    {
        public StatusOperacaoDTO GetExamples()
        {
            return new StatusOperacaoDTO
            {
                Id = 1,
                Descricao = "Locação",
                TipoMovimentacao = "SAIDA"
            };
        }
    }
}