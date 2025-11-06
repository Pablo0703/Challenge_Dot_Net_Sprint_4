using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs
{
    public class LocalizacaoMotoDTO
    {
        public long Id { get; set; }

        [Required]
        public long IdMoto { get; set; }

        [Required]
        public long IdZona { get; set; }

        [Required]
        public DateTime DataHoraEntrada { get; set; }

        public DateTime? DataHoraSaida { get; set; }
    }
}
