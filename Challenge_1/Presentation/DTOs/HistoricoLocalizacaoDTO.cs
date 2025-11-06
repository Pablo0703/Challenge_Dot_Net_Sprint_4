using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs
{
    public class HistoricoLocalizacaoDTO
    {
        public long Id { get; set; }

        [Required]
        public long IdMoto { get; set; }

        public long? IdMotociclista { get; set; }

        public long? IdZonaPatio { get; set; }

        [Required]
        public DateTime DataHoraSaida { get; set; }

        public DateTime? DataHoraEntrada { get; set; }

        public decimal? KmRodados { get; set; }

        public long? IdStatusOperacao { get; set; }
    }
}
