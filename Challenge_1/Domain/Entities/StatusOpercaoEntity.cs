using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MOTTU_STATUS_OPERACAO")]
    public class StatusOperacaoEntity
    {
        [Key]
        [Column("ID_STATUS_OPERACAO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [Column("DESCRICAO")]
        [MaxLength(50)]
        public string Descricao { get; set; }

        [Required]
        [Column("TIPO_MOVIMENTACAO")]
        [MaxLength(20)]
        public string TipoMovimentacao { get; set; }
    }
}
