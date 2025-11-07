using Xunit;
using Moq;
using Domain.Entities;
using Application.Services;
using Infrastructure.Data;
using Presentation.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Challenge.Tests.App.Service
{
    public class MotoServiceTest
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly ServiceMoto _service;

        public MotoServiceTest()
        {
            // Mocka o DbContext
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            var context = new AppDbContext(options);
            _mockContext = new Mock<AppDbContext>(options);
            _service = new ServiceMoto(context);
        }

        [Fact(DisplayName = "Deve criar moto com sucesso")]
        public async Task DeveCriarMotoComSucesso()
        {
            // Arrange
            var dto = new MotoDTO
            {
                IdTipo = 1,
                IdStatus = 1,
                Placa = "ABC1234",
                Modelo = "Honda CG",
                AnoFabricacao = 2022,
                AnoModelo = 2023,
                Chassi = "CH123",
                Cilindrada = 150,
                Cor = "Vermelha",
                ValorAquisicao = 10000
            };

            // Act
            var result = await _service.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Honda CG", result.Modelo);
        }

        [Fact(DisplayName = "Deve retornar nulo ao buscar moto inexistente")]
        public async Task DeveRetornarNullQuandoNaoEncontrarMoto()
        {
            // Act
            var result = await _service.GetByIdAsync(99999);

            // Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "Deve retornar lista de motos (vazia se não houver dados)")]
        public async Task DeveRetornarListaDeMotos()
        {
            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<MotoDTO>>(result);
        }
    }
}
