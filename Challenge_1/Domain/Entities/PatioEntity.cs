using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MOTTU_PATIO")]
    public class PatioEntity
    {
        [Key]
        [Column("ID_PATIO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [Column("ID_FILIAL")]
        public long IdFilial { get; set; }

        [Required]
        [Column("NOME")]
        [MaxLength(50)]
        public string Nome { get; set; }

        [Column("AREA_M2")]
        public decimal? AreaM2 { get; set; }

        [Required]
        [Column("CAPACIDADE")]
        public long Capacidade { get; set; }

        [Column("ATIVO")]
        [MaxLength(1)]
        public string? Ativo { get; set; }
    }
}
