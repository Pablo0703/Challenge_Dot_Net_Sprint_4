using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("MOTTU_HISTORICO_LOCALIZACAO")]
    public class HistoricoLocalizacaoEntity
    {
        [Key]
        [Column("ID_HISTORICO")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Column("ID_MOTO")]
        public long IdMoto { get; set; }

        [Column("ID_MOTOCICLISTA")]
        public long? IdMotociclista { get; set; }

        [Column("ID_ZONA_PATIO")]
        public long? IdZonaPatio { get; set; }

        [Required]
        [Column("DATA_HORA_SAIDA")]
        public DateTime DataHoraSaida { get; set; }

        [Column("DATA_HORA_ENTRADA")]
        public DateTime? DataHoraEntrada { get; set; }

        [Column("KM_RODADOS")]
        public decimal? KmRodados { get; set; }

        [Column("ID_STATUS_OPERACAO")]
        public long? IdStatusOperacao { get; set; }
    }
}
