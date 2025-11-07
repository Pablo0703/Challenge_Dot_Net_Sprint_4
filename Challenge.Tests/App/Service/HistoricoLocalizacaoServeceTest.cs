using Application.Services;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests.Services
{
    public class HistoricoLocalizacaoServiceTest
    {
        private readonly AppDbContext _context;
        private readonly ServiceHistoricoLocalizacao _service;

        public HistoricoLocalizacaoServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("HistoricoServiceTestDB")
                .Options;

            _context = new AppDbContext(options);
            _service = new ServiceHistoricoLocalizacao(_context);
        }

        [Fact(DisplayName = "Deve criar histórico de localização com sucesso")]
        public async Task DeveCriarHistorico()
        {
            var dto = new HistoricoLocalizacaoDTO
            {
                IdMoto = 1,
                IdMotociclista = 2,
                IdZonaPatio = 3,
                KmRodados = 120
            };

            var result = await _service.CreateAsync(dto);
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "Deve retornar lista de históricos")]
        public async Task DeveRetornarListaDeHistoricos()
        {
            await _service.CreateAsync(new HistoricoLocalizacaoDTO
            {
                IdMoto = 1,
                IdMotociclista = 2,
                IdZonaPatio = 3,
                KmRodados = 200
            });

            var lista = await _service.GetAllAsync();
            Assert.NotEmpty(lista);
        }
    }
}
