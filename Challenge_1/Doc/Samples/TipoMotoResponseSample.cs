using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class TipoMotoResponseSample : IExamplesProvider<TipoMotoDTO>
    {
        public TipoMotoDTO GetExamples()
        {
            return new TipoMotoDTO
            {
                Id = 1,
                Descricao = "Mottu Sport",
                Categoria = "Esportiva"
            };
        }
    }
}