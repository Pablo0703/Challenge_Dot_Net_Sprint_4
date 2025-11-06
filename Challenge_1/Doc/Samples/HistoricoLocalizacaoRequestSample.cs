using Presentation.DTOs;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Challenge_1.Doc.Samples
{
    public class HistoricoLocalizacaoRequestSample : IExamplesProvider<HistoricoLocalizacaoDTO>
    {
        public HistoricoLocalizacaoDTO GetExamples()
        {
            return new HistoricoLocalizacaoDTO
            {
                IdMoto = 1,
                IdMotociclista = 4,
                IdZonaPatio = 1,
                DataHoraSaida = DateTime.Parse("2025-05-15T21:19:56"),
                DataHoraEntrada = DateTime.Parse("2025-05-17T21:19:56"),
                KmRodados = 15.5M,
                IdStatusOperacao = 1
            };
        }
    }
}