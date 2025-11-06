using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs
{
    public class StatusOperacaoDTO
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Descricao { get; set; }

        [Required]
        [MaxLength(20)]
        public string TipoMovimentacao { get; set; }
    }
}
