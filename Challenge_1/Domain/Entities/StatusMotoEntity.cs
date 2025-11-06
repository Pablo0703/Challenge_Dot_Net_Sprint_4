using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MOTTU_STATUS_MOTO")]
    public class StatusMotoEntity
    {
        [Key]
        [Column("ID_STATUS")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [Column("DESCRICAO")]
        [MaxLength(20)]
        public string Descricao { get; set; }

        [Column("DISPONIVEL")]
        [MaxLength(1)]
        public string? Disponivel { get; set; }
    }
}
