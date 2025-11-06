using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class PatioResponseSample : IExamplesProvider<PatioDTO>
    {
        public PatioDTO GetExamples()
        {
            return new PatioDTO
            {
                Id = 1,
                IdFilial = 1,
                Nome = "Pátio Central SP",
                AreaM2 = 500,
                Capacidade = 50,
                Ativo = "S"
            };
        }
    }
}
