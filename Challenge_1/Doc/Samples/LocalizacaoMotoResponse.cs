using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Doc.Samples
{
    public class LocalizacaoMotoResponseSample : IExamplesProvider<LocalizacaoMotoDTO>
    {
        public LocalizacaoMotoDTO GetExamples()
        {
            return new LocalizacaoMotoDTO
            {
                Id = 1,
                IdMoto = 1,
                IdZona = 2,
                DataHoraEntrada = DateTime.Parse("2025-05-20T21:19:55"),
                DataHoraSaida = null
            };
        }
    }
}