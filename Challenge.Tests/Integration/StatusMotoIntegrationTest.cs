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
    public class StatusMotoIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public StatusMotoIntegrationTest(WebApplicationFactory<Program> factory)
        {
            // ✅ Cria um banco em memória único para cada execução
            var dbName = $"StatusMotoTestDB_{Guid.NewGuid()}";

            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove contexto real
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Adiciona contexto InMemory com nome único
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase(dbName));

                    // Popula o banco
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();

                    // 🔹 Insere um registro inicial (Id manual diferente)
                    db.StatusMotos.Add(new StatusMotoEntity
                    {
                        Id = 101, // ID único
                        Descricao = "Disponível",
                        Disponivel = "S"
                    });

                    db.SaveChanges();
                });
            });

            _client = customFactory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/v1/ControllerStatusMoto deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/ControllerStatusMoto");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/ControllerStatusMoto deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var dto = new StatusMotoDTO
            {
                Descricao = "Em Manutenção",
                Disponivel = "N"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/ControllerStatusMoto", dto);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}