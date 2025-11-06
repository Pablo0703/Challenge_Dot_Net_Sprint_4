using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MOTTU_NF")]
    public class NotaFiscalEntity
    {
        [Key]
        [Column("ID_NF")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [Column("NUMERO")]
        [MaxLength(20)]
        public string Numero { get; set; }

        [Required]
        [Column("SERIE")]
        [MaxLength(5)]
        public string Serie { get; set; }

        [Required]
        [Column("MODELO")]
        [MaxLength(2)]
        public string Modelo { get; set; }

        [Column("CHAVE_ACESSO")]
        [MaxLength(44)]
        public string? ChaveAcesso { get; set; }

        [Required]
        [Column("DATA_EMISSAO")]
        public DateTime DataEmissao { get; set; }

        [Required]
        [Column("VALOR_TOTAL")]
        public decimal ValorTotal { get; set; }

        [Required]
        [Column("FORNECEDOR")]
        [MaxLength(100)]
        public string Fornecedor { get; set; }

        [Required]
        [Column("CNPJ_FORNECEDOR")]
        [MaxLength(14)]
        public string CnpjFornecedor { get; set; }
    }
}
