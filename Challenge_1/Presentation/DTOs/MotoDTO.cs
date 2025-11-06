using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.DTOs
{
    public class MotoDTO
    {
        public long Id { get; set; }

        [Required]
        public long IdTipo { get; set; }

        [Required]
        public long IdStatus { get; set; }

        [Required]
        [MaxLength(10)]
        public string Placa { get; set; }

        [Required]
        [MaxLength(50)]
        public string Modelo { get; set; }

        [Required]
        public int AnoFabricacao { get; set; }

        [Required]
        public int AnoModelo { get; set; }

        [Required]
        [MaxLength(17)]
        public string Chassi { get; set; }

        [Required]
        public int Cilindrada { get; set; }

        [Required]
        [MaxLength(20)]
        public string Cor { get; set; }

        [Required]
        public DateTime DataAquisicao { get; set; }

        [Required]
        public decimal ValorAquisicao { get; set; }

        public long? IdNotaFiscal { get; set; }
    }
}
