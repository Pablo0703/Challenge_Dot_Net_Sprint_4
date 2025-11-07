using Application.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests.Unit
{
    public class FilialServiceTest
    {
        private readonly AppDbContext _context;
        private readonly ServiceFilial _service;

        public FilialServiceTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "FilialServiceTestDB")
                .Options;

            _context = new AppDbContext(options);
            _service = new ServiceFilial(_context);
        }

        [Fact(DisplayName = "Deve criar filial com sucesso")]
        public async Task DeveCriarFilial()
        {
            var dto = new FilialDTO
            {
                Nome = "Filial Santo André",
                Cnpj = "11122233344455",
                Telefone = "11223344",
                Email = "contato@filial.com",
                Ativo = "S",
                IdEndereco = 1
            };

            var result = await _service.CreateAsync(dto);
            Assert.NotNull(result);
            Assert.Equal("Filial Santo André", result.Nome);
        }

        [Fact(DisplayName = "Deve retornar lista de filiais")]
        public async Task DeveRetornarListaDeFiliais()
        {
            await _service.CreateAsync(new FilialDTO
            {
                Nome = "Filial Teste",
                Cnpj = "99988877766655",
                Ativo = "N"
            });

            var lista = await _service.GetAllAsync();
            Assert.NotEmpty(lista);
        }
    }
}
