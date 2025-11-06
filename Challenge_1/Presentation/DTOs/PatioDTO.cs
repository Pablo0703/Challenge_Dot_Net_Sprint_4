using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs
{
    public class PatioDTO
    {
        public long Id { get; set; }

        [Required]
        public long IdFilial { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nome { get; set; }

        public decimal? AreaM2 { get; set; }

        [Required]
        public long Capacidade { get; set; }

        [MaxLength(1)]
        public string? Ativo { get; set; }
    }
}
