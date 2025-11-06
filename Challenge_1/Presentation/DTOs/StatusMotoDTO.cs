using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs
{
    public class StatusMotoDTO
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Descricao { get; set; }

        [MaxLength(1)]
        public string? Disponivel { get; set; }
    }
}
