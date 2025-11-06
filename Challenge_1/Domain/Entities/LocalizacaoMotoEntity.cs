using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MOTTU_LOCALIZACAO_MOTO")]
    public class LocalizacaoMotoEntity
    {
        [Key]
        [Column("ID_LOCALIZACAO")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [Required]
        [Column("ID_MOTO")]
        public long IdMoto { get; set; }

        [Required]
        [Column("ID_ZONA")]
        public long IdZona { get; set; }

        [Required]
        [Column("DATA_HORA_ENTRADA")]
        public DateTime DataHoraEntrada { get; set; }

        [Column("DATA_HORA_SAIDA")]
        public DateTime? DataHoraSaida { get; set; }
    }
}
