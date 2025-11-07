using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Presentation.DTOs;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests.Integration
{
    public class EnderecoIntegrationTest : IClassFixture<DelegatedWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public EnderecoIntegrationTest(DelegatedWebApplicationFactory factory)
        {
            // ✅ Cria um client para chamar a API
            _client = factory.CreateClient();

            // ✅ Popula o banco InMemory com um registro inicial
            using (var scope = factory.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Database.EnsureCreated();
                context.Enderecos.Add(new EnderecoEntity
                {
                    Id = 1,
                    Logradouro = "Rua Teste",
                    Numero = "10",
                    Bairro = "Centro",
                    Cidade = "São Paulo",
                    Estado = "SP",
                    Cep = "01000-000",
                    Pais = "Brasil"
                });

                context.SaveChanges();
            }
        }

        [Fact(DisplayName = "GET /api/v1/ControllerEndereco deve retornar 200 OK")]
        public async Task GetAll_DeveRetornar200()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/ControllerEndereco");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(DisplayName = "POST /api/v1/ControllerEndereco deve retornar 201 Created")]
        public async Task Post_DeveRetornar201()
        {
            // Arrange
            var dto = new EnderecoDTO
            {
                Logradouro = "Rua Nova",
                Numero = "50",
                Bairro = "Centro",
                Cidade = "Campinas",
                Estado = "SP",
                Cep = "13000-000",
                Pais = "Brasil"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/ControllerEndereco", dto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
