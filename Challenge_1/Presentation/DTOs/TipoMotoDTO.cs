using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs
{
    public class TipoMotoDTO
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Descricao { get; set; }

        [Required]
        [MaxLength(30)]
        public string Categoria { get; set; }
    }
}
