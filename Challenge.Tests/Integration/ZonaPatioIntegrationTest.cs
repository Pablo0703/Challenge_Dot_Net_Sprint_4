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
    public class ZonaPatioIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ZonaPatioIntegrationTest(WebApplicationFactory<Program> factory)
        {
            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove o contexto real
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Usa um banco em memória novo a cada execução
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase($"ZonaPatioDB_{Guid.NewGuid()}"));

                    // Popula com dados iniciais
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();

                    db.ZonasPatio.Add(new ZonaPatioEntity
                    {
                        IdPatio = 10,
                        NomeZona = "Zona Inicial",
                        TipoZona = "Coberta",
                        Capacidade = 20
                    });

                    db.SaveChanges();
                });
            });

            _client = customFactory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/v1/ControllerZonaPatio deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/ControllerZonaPatio");
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/ControllerZonaPatio deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var novaZona = new ZonaPatioDTO
            {
                IdPatio = 20,
                NomeZona = "Zona Nova",
                TipoZona = "Aberta",
                Capacidade = 50
            };

            var response = await _client.PostAsJsonAsync("/api/v1/ControllerZonaPatio", novaZona);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}