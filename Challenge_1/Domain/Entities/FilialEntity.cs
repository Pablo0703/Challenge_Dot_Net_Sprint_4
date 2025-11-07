using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MOTTU_FILIAL")]
    public class FilialEntity
    {
        [Key]
        [Column("ID_FILIAL")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("NOME")]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [Column("CNPJ")]
        [MaxLength(14)]
        public string Cnpj { get; set; }

        [Column("TELEFONE")]
        [MaxLength(20)]
        public string? Telefone { get; set; }

        [Column("EMAIL")]
        [MaxLength(100)]
        public string? Email { get; set; }

        [Column("ATIVO")]
        [MaxLength(1)]
        public string? Ativo { get; set; }

        [Required]
        [Column("ID_ENDERECO")]
        public long IdEndereco { get; set; }
    }
}
