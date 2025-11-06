using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs
{
    public class MotociclistaDTO
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(11)]
        public string Cpf { get; set; }

        [Required]
        [MaxLength(20)]
        public string Cnh { get; set; }

        [Required]
        public DateTime DataValidadeCnh { get; set; }

        [Required]
        [MaxLength(20)]
        public string Telefone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }

        [MaxLength(1)]
        public string? Ativo { get; set; }

        public long? IdEndereco { get; set; }
    }
}
