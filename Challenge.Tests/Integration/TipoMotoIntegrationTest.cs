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
    public class TipoMotoIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TipoMotoIntegrationTest(WebApplicationFactory<Program> factory)
        {
            var dbName = $"TipoMotoTestDB_{Guid.NewGuid()}";

            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // remove o DbContext real
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    if (descriptor != null)
                        services.Remove(descriptor);

                    // adiciona InMemoryDatabase
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase(dbName));

                    // popula dados iniciais
                    var sp = services.BuildServiceProvider();
                    using var scope = sp.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.EnsureCreated();

                    db.TiposMoto.Add(new TipoMotoEntity
                    {
                        Id = 301,
                        Descricao = "Adventure 1000cc",
                        Categoria = "Big Trail"
                    });

                    db.SaveChanges();
                });
            });

            _client = customFactory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/v1/ControllerTipoMoto deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            var response = await _client.GetAsync("/api/v1/ControllerTipoMoto");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/ControllerTipoMoto deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            var dto = new TipoMotoDTO
            {
                Descricao = "Custom 900cc",
                Categoria = "Premium"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/ControllerTipoMoto", dto);
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine(body);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
