using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<EnderecoEntity> Enderecos { get; set; }
        public DbSet<NotaFiscalEntity> NotasFiscais { get; set; }
        public DbSet<StatusMotoEntity> StatusMotos { get; set; }
        public DbSet<StatusOperacaoEntity> StatusOperacoes { get; set; }
        public DbSet<TipoMotoEntity> TiposMoto { get; set; }
        public DbSet<FilialEntity> Filiais { get; set; }
        public DbSet<PatioEntity> Patios { get; set; }
        public DbSet<ZonaPatioEntity> ZonasPatio { get; set; }
        public DbSet<MotoEntity> Motos { get; set; }
        public DbSet<MotociclistaEntity> Motociclistas { get; set; }
        public DbSet<LocalizacaoMotoEntity> LocalizacoesMoto { get; set; }
        public DbSet<HistoricoLocalizacaoEntity> HistoricosLocalizacao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.HasSequence<long>("SEQ_MOTTU_MOTO", schema: "RM556834");

            modelBuilder.Entity<MotoEntity>(entity =>
            {
                entity.ToTable("MOTTU_MOTO");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("ID_MOTO")
                      .ValueGeneratedOnAdd() 
                      .HasDefaultValueSql("RM556834.SEQ_MOTTU_MOTO.NEXTVAL"); 
            });
        }
    }
}
