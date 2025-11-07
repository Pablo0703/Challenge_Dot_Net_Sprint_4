using Application.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests.App.Service
{
    public class EnderecoServiceTest
    {
        private readonly AppDbContext _context;
        private readonly ServiceEndereco _service;

        public EnderecoServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("EnderecoServiceTestDB")
                .Options;

            _context = new AppDbContext(options);
            _service = new ServiceEndereco(_context);
        }

        [Fact(DisplayName = "Deve criar endereço com sucesso")]
        public async Task DeveCriarEnderecoComSucesso()
        {
            var dto = new EnderecoDTO
            {
                Logradouro = "Rua das Palmeiras",
                Numero = "200",
                Bairro = "Centro",
                Cidade = "São Paulo",
                Estado = "SP",
                Cep = "01000-000",
                Pais = "Brasil"
            };

            var result = await _service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Rua das Palmeiras", result.Logradouro);
        }

        [Fact(DisplayName = "Deve retornar lista de endereços")]
        public async Task DeveRetornarListaDeEnderecos()
        {
            var lista = await _service.GetAllAsync();
            Assert.NotNull(lista);
        }

        [Fact(DisplayName = "Deve retornar nulo ao buscar ID inexistente")]
        public async Task DeveRetornarNuloSeIdNaoExistir()
        {
            var result = await _service.GetByIdAsync(999);
            Assert.Null(result);
        }
    }
}
