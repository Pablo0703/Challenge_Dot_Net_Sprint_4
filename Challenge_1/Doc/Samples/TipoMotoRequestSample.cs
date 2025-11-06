using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class TipoMotoRequestSample : IExamplesProvider<TipoMotoDTO>
    {
        public TipoMotoDTO GetExamples()
        {
            return new TipoMotoDTO
            {
                Descricao = "Mottu Sport",
                Categoria = "Esportiva"
            };
        }
    }
}
