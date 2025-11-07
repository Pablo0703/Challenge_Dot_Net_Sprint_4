using Challenge_1;
using Microsoft.AspNetCore.Mvc.Testing;
using Presentation.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests.Integration
{
    public class PatioIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PatioIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact(DisplayName = "GET /api/v1/ControllerPatio")]
        public async Task GetAll_DeveRetornarLista()
        {
            var response = await _client.GetAsync("/api/v1/ControllerPatio");
            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }


        [Fact(DisplayName = "POST /api/v1/ControllerPatio deve retornar 201 Created")]
        public async Task Post_DeveCriarPatio()
        {
            var dto = new PatioDTO
            {
                IdFilial = 1,
                Nome = "Pátio Novo",
                AreaM2 = 300,
                Capacidade = 30,
                Ativo = "S"
            };

            var response = await _client.PostAsJsonAsync("/api/v1/ControllerPatio", dto);
            var body = await response.Content.ReadAsStringAsync();

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}