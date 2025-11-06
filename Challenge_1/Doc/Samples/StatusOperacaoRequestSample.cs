using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class StatusOperacaoRequestSample : IExamplesProvider<StatusOperacaoDTO>
    {
        public StatusOperacaoDTO GetExamples()
        {
            return new StatusOperacaoDTO
            {
                Descricao = "Locação",
                TipoMovimentacao = "SAIDA"
            };
        }
    }
}