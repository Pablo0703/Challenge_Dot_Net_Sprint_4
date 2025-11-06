using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class StatusMotoRequestSample : IExamplesProvider<StatusMotoDTO>
    {
        public StatusMotoDTO GetExamples()
        {
            return new StatusMotoDTO
            {
                Descricao = "Disponível",
                Disponivel = "S"
            };
        }
    }
}