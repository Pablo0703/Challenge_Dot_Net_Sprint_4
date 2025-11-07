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
    public class MotociclistaIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public MotociclistaIntegrationTest(WebApplicationFactory<Program> factory)
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

                    // ✅ Usa banco InMemory com nome único por execução (evita conflito de IDs)
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase($"MotociclistaIntegrationTestDB_{Guid.NewGuid()}"));

                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    db.Database.EnsureCreated();

                    // Popula um motociclista inicial (sem definir Id)
                    db.Motociclistas.Add(new MotociclistaEntity
                    {
                        Nome = "Carlos Silva",
                        Cpf = "12345678900",
                        Cnh = "CNH123456",
                        Telefone = "(11) 91234-5678",
                        Email = "carlos@teste.com",
                        DataCadastro = DateTime.UtcNow,
                        Ativo = "S",
                        IdEndereco = 1
                    });

                    db.SaveChanges();
                });
            });

            _client = customFactory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/v1/ControllerMotociclista deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/ControllerMotociclista");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/ControllerMotociclista deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var novo = new MotociclistaDTO
            {
                Nome = "João Souza",
                Cpf = "98765432100",
                Cnh = "CNH654321",
                Telefone = "(11) 97777-8888",
                Email = "joao@teste.com",
                DataCadastro = DateTime.UtcNow,
                Ativo = "S",
                IdEndereco = 2
            };

            var response = await _client.PostAsJsonAsync("/api/v1/ControllerMotociclista", novo);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
