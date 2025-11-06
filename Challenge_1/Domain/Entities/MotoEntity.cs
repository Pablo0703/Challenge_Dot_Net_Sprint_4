using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MOTTU_MOTO")]
    public class MotoEntity
    {
        [Key]
        [Column("ID_MOTO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public long Id { get; set; }

        [Required]
        [Column("ID_TIPO")]
        public long IdTipo { get; set; }

        [Required]
        [Column("ID_STATUS")]
        public long IdStatus { get; set; }

        [Required]
        [Column("PLACA")]
        [MaxLength(10)]
        public string Placa { get; set; }

        [Required]
        [Column("MODELO")]
        [MaxLength(50)]
        public string Modelo { get; set; }

        [Required]
        [Column("ANO_FABRICACAO")]
        public int AnoFabricacao { get; set; }

        [Required]
        [Column("ANO_MODELO")]
        public int AnoModelo { get; set; }

        [Required]
        [Column("CHASSI")]
        [MaxLength(17)]
        public string Chassi { get; set; }

        [Required]
        [Column("CILINDRADA")]
        public int Cilindrada { get; set; }

        [Required]
        [Column("COR")]
        [MaxLength(20)]
        public string Cor { get; set; }

        [Required]
        [Column("DATA_AQUISICAO")]
        public DateTime DataAquisicao { get; set; }

        [Required]
        [Column("VALOR_AQUISICAO")]
        public decimal ValorAquisicao { get; set; }

        [Column("ID_NF")]
        public long? IdNotaFiscal { get; set; }
    }
}
