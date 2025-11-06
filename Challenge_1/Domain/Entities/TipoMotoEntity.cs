using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MOTTU_TIPO_MOTO")]
    public class TipoMotoEntity
    {
        [Key]
        [Column("ID_TIPO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [Column("DESCRICAO")]
        [MaxLength(60)]
        public string Descricao { get; set; }

        [Required]
        [Column("CATEGORIA")]
        [MaxLength(30)]
        public string Categoria { get; set; }
    }
}
