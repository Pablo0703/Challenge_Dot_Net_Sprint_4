using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MOTTU_ZONA_PATIO")]
    public class ZonaPatioEntity
    {
        [Key]
        [Column("ID_ZONA")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [Column("ID_PATIO")]
        public long IdPatio { get; set; }

        [Required]
        [Column("NOME_ZONA")]
        [MaxLength(30)]
        public string NomeZona { get; set; }

        [Required]
        [Column("TIPO_ZONA")]
        [MaxLength(20)]
        public string TipoZona { get; set; }

        [Required]
        [Column("CAPACIDADE")]
        public long Capacidade { get; set; }
    }
}
