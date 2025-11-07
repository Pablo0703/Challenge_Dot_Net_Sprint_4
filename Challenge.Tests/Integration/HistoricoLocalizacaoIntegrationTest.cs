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
    public class HistoricoLocalizacaoIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public HistoricoLocalizacaoIntegrationTest(WebApplicationFactory<Program> factory)
        {
            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase("HistoricoIntegrationTestDB"));

                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();

                    db.HistoricosLocalizacao.Add(new HistoricoLocalizacaoEntity
                    {
                        IdMoto = 1,
                        IdMotociclista = 2,
                        IdZonaPatio = 3,
                        DataHoraSaida = DateTime.UtcNow,
                        DataHoraEntrada = null,
                        KmRodados = 150,
                        IdStatusOperacao = 1
                    });

                    db.SaveChanges();
                });
            });

            _client = customFactory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/v1/ControllerHistoricoLocalizacao deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/ControllerHistoricoLocalizacao");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/ControllerHistoricoLocalizacao deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var novo = new HistoricoLocalizacaoDTO
            {
                IdMoto = 5,
                IdMotociclista = 7,
                IdZonaPatio = 2,
                DataHoraSaida = DateTime.UtcNow,
                KmRodados = 300,
                IdStatusOperacao = 2
            };

            var response = await _client.PostAsJsonAsync("/api/v1/ControllerHistoricoLocalizacao", novo);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
