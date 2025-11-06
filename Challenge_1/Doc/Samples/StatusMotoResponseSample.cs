using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class StatusMotoResponseSample : IExamplesProvider<StatusMotoDTO>
    {
        public StatusMotoDTO GetExamples()
        {
            return new StatusMotoDTO
            {
                Id = 1,
                Descricao = "Disponível",
                Disponivel = "S"
            };
        }
    }
}