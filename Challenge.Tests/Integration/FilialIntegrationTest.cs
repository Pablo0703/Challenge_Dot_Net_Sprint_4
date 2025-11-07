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
    public class FilialIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public FilialIntegrationTest(WebApplicationFactory<Program> factory)
        {
            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    var dbName = $"FilialTestDb_{Guid.NewGuid()}";
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase(dbName));

                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                    db.Filiais.Add(new FilialEntity
                    {
                        Id = 1,
                        Nome = "Filial Centro",
                        Cnpj = "12345678000199",
                        Telefone = "11999999999",
                        Email = "centro@empresa.com",
                        Ativo = "S",
                        IdEndereco = 1
                    });

                    db.SaveChanges();
                });
            });

            _client = customFactory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/v1/ControllerFilial deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/ControllerFilial");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/ControllerFilial deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var dto = new FilialDTO
            {
                Nome = "Filial Paulista",
                Cnpj = "99887766554433",
                Telefone = "1133334444",
                Email = "paulista@empresa.com",
                Ativo = "S",
                IdEndereco = 1
            };

            var response = await _client.PostAsJsonAsync("/api/v1/ControllerFilial", dto);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
