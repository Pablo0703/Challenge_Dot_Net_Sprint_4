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
    public class LocalizacaoMotoIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public LocalizacaoMotoIntegrationTest(WebApplicationFactory<Program> factory)
        {
            // 🧩 Usa um nome de banco único a cada execução para evitar conflito de IDs
            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove o contexto real (registrado no projeto principal)
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // Cria novo contexto InMemory com banco único
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase(Guid.NewGuid().ToString())); // ✅ nome aleatório

                    // Popula o banco com dados iniciais
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();

                    //Adiciona apenas um registro inicial
                    db.LocalizacoesMoto.Add(new LocalizacaoMotoEntity
                    {
                        IdMoto = 123,
                        IdZona = 7
                    });

                    db.SaveChanges();
                });
            });

            _client = customFactory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/v1/ControllerLocalizacaoMoto deve retornar lista de localizações")]
        public async Task GetAll_DeveRetornarListaDeLocalizacoes()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/ControllerLocalizacaoMoto");
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine(body); // log útil se houver erro

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/ControllerLocalizacaoMoto deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            // Arrange
            var novaLocalizacao = new LocalizacaoMotoDTO
            {
                IdMoto = 456,
                IdZona = 9
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/ControllerLocalizacaoMoto", novaLocalizacao);
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine(body); //se erro interno ocorrer

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
