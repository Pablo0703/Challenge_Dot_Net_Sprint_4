using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MOTTU_MOTOCICLISTA")]
    public class MotociclistaEntity
    {
        [Key]
        [Column("ID_MOTOCICLISTA")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [Column("NOME")]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [Column("CPF")]
        [MaxLength(11)]
        public string Cpf { get; set; }

        [Required]
        [Column("CNH")]
        [MaxLength(20)]
        public string Cnh { get; set; }

        [Required]
        [Column("DATA_VALIDADE_CNH")]
        public DateTime DataValidadeCnh { get; set; }

        [Required]
        [Column("TELEFONE")]
        [MaxLength(20)]
        public string Telefone { get; set; }

        [Column("EMAIL")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Required]
        [Column("DATA_CADASTRO")]
        public DateTime DataCadastro { get; set; }

        [Column("ATIVO")]
        [MaxLength(1)]
        public string? Ativo { get; set; }

        [Column("ID_ENDERECO")]
        public long? IdEndereco { get; set; }
    }
}
