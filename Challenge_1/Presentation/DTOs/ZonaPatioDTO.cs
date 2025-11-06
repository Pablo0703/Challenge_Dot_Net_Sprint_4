using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs
{
    public class ZonaPatioDTO
    {
        public long Id { get; set; }

        [Required]
        public long IdPatio { get; set; }

        [Required]
        [MaxLength(30)]
        public string NomeZona { get; set; }

        [Required]
        [MaxLength(20)]
        public string TipoZona { get; set; }

        [Required]
        public long Capacidade { get; set; }
    }
}
