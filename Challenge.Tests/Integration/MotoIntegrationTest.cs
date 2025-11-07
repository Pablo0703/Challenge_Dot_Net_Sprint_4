using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.DTOs;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests.Integration
{
    public class MotoIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public MotoIntegrationTest(WebApplicationFactory<Program> factory)
        {
            // 🔧 Cria uma factory customizada com banco isolado
            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove o contexto original
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // ✅ Usa um nome de banco único pra cada execução
                    var dbName = $"TestDb_{Guid.NewGuid()}";

                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase(dbName));

                    // Cria escopo para popular dados iniciais
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    // Insere moto inicial
                    db.Motos.Add(new MotoEntity
                    {
                        Id = 1,
                        IdTipo = 1,
                        IdStatus = 1,
                        Placa = "ABC1D23",
                        Modelo = "Mottu Sport",
                        AnoFabricacao = 2023,
                        AnoModelo = 2023,
                        Chassi = "9BWZZZ377VT004251",
                        Cilindrada = 300,
                        Cor = "Vermelha",
                        DataAquisicao = DateTime.UtcNow,
                        ValorAquisicao = 25000,
                        IdNotaFiscal = 1
                    });

                    db.SaveChanges();
                });
            });

            _client = customFactory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/v1/ControllerMoto deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/ControllerMoto");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/ControllerMoto deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var novaMoto = new MotoDTO
            {
                IdTipo = 1,
                IdStatus = 1,
                Placa = "XYZ9876",
                Modelo = "Yamaha Factor",
                AnoFabricacao = 2022,
                AnoModelo = 2023,
                Chassi = "CHS987654",
                Cilindrada = 150,
                Cor = "Preta",
                DataAquisicao = DateTime.UtcNow,
                ValorAquisicao = 17000,
                IdNotaFiscal = 2
            };

            var response = await _client.PostAsJsonAsync("/api/v1/ControllerMoto", novaMoto);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
