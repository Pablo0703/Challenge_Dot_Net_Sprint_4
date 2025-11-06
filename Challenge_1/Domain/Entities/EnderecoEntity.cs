using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MOTTU_ENDERECO")]
    public class EnderecoEntity
    {
        [Key]
        [Column("ID_ENDERECO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [Column("LOGRADOURO")]
        [MaxLength(50)]
        public string Logradouro { get; set; }

        [Required]
        [Column("NUMERO")]
        [MaxLength(10)]
        public string Numero { get; set; }

        [Column("COMPLEMENTO")]
        [MaxLength(50)]
        public string? Complemento { get; set; }

        [Required]
        [Column("BAIRRO")]
        [MaxLength(50)]
        public string Bairro { get; set; }

        [Required]
        [Column("CEP")]
        [MaxLength(10)]
        public string Cep { get; set; }

        [Required]
        [Column("CIDADE")]
        [MaxLength(50)]
        public string Cidade { get; set; }

        [Required]
        [Column("ESTADO")]
        [MaxLength(2)]
        public string Estado { get; set; }

        [Column("PAIS")]
        [MaxLength(50)]
        public string? Pais { get; set; }
    }
}
