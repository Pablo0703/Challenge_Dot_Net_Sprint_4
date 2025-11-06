using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Challenge_1.Doc.Samples
{
    public class LocalizacaoMotoRequestSample : IExamplesProvider<LocalizacaoMotoDTO>
    {
        public LocalizacaoMotoDTO GetExamples()
        {
            return new LocalizacaoMotoDTO
            {
                IdMoto = 1,
                IdZona = 2,
                DataHoraEntrada = DateTime.Parse("2025-05-20T21:19:55"),
                DataHoraSaida = null
            };
        }
    }
}