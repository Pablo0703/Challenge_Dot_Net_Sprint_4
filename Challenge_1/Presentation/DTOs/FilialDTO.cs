using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs
{
    public class FilialDTO
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(14)]
        public string Cnpj { get; set; }

        [MaxLength(20)]
        public string? Telefone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [MaxLength(1)]
        public string? Ativo { get; set; }

        [Required]
        public long IdEndereco { get; set; }
    }
}
