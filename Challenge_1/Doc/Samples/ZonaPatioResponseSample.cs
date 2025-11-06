using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class ZonaPatioResponseSample : IExamplesProvider<ZonaPatioDTO>
    {
        public ZonaPatioDTO GetExamples()
        {
            return new ZonaPatioDTO
            {
                Id = 1,
                IdPatio = 1,
                NomeZona = "Zona A",
                TipoZona = "Estacionamento",
                Capacidade = 50
            };
        }
    }
}
