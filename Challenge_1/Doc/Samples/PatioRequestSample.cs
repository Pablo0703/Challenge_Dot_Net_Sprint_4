using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class PatioRequestSample : IExamplesProvider<PatioDTO>
    {
        public PatioDTO GetExamples()
        {
            return new PatioDTO
            {
                IdFilial = 1,
                Nome = "Pátio Central SP",
                AreaM2 = 500,
                Capacidade = 50,
                Ativo = "S"
            };
        }
    }
}
