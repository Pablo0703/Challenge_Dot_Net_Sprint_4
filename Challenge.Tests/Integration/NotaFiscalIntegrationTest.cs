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
    public class NotaFiscalIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public NotaFiscalIntegrationTest(WebApplicationFactory<Program> factory)
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

                    // Banco isolado (sem conflitos de ID)
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase($"NotaFiscalIntegrationTestDB_{Guid.NewGuid()}"));

                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    db.Database.EnsureCreated();

                    // Seed com uma nota fiscal
                    db.NotasFiscais.Add(new NotaFiscalEntity
                    {
                        Numero = "NF12345",
                        Serie = "A1",
                        Modelo = "55",
                        ChaveAcesso = "12345678901234567890123456789012345678901234",
                        DataEmissao = DateTime.UtcNow,
                        ValorTotal = 2500.75M,
                        Fornecedor = "Fornecedor Teste",
                        CnpjFornecedor = "12345678000199"
                    });
                    db.SaveChanges();
                });
            });

            _client = customFactory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/v1/ControllerNotaFiscal deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/ControllerNotaFiscal");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/ControllerNotaFiscal deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var novaNota = new NotaFiscalDTO
            {
                Numero = "NF67890",
                Serie = "B2",
                Modelo = "65",
                ChaveAcesso = "98765432109876543210987654321098765432109876",
                DataEmissao = DateTime.UtcNow,
                ValorTotal = 1800.50M,
                Fornecedor = "Fornecedor XYZ",
                CnpjFornecedor = "98765432000188"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/ControllerNotaFiscal", novaNota);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
