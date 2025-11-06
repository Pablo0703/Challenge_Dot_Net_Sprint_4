using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs
{
    public class NotaFiscalDTO
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Numero { get; set; }

        [Required]
        [MaxLength(5)]
        public string Serie { get; set; }

        [Required]
        [MaxLength(2)]
        public string Modelo { get; set; }

        [MaxLength(44)]
        public string? ChaveAcesso { get; set; }

        [Required]
        public DateTime DataEmissao { get; set; }

        [Required]
        public decimal ValorTotal { get; set; }

        [Required]
        [MaxLength(100)]
        public string Fornecedor { get; set; }

        [Required]
        [MaxLength(14)]
        public string CnpjFornecedor { get; set; }
    }
}
