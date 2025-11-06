using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class ZonaPatioRequestSample : IExamplesProvider<ZonaPatioDTO>
    {
        public ZonaPatioDTO GetExamples()
        {
            return new ZonaPatioDTO
            {
                IdPatio = 1,
                NomeZona = "Zona Teste",
                TipoZona = "ESTACIONAMENTO",
                Capacidade = 20
            };
        }
    }
}
