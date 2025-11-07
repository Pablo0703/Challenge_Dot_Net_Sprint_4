using Application.Interfaces;
using Challenge_1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Presentation.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests.App.Controllers
{
    public class MotoControllerTest
    {
        private readonly Mock<InterfaceMoto> _serviceMock;
        private readonly ControllerMoto _controller;

        public MotoControllerTest()
        {
            _serviceMock = new Mock<InterfaceMoto>();
            _controller = new ControllerMoto(_serviceMock.Object);

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/api/v1/moto/1");

            _controller.Url = urlHelperMock.Object;

        }


        [Fact(DisplayName = "GET deve retornar lista paginada de motos com sucesso")]
        public async Task GetAll_DeveRetornarListaDeMotos()
        {
            // Arrange
            var motos = new List<MotoDTO> { new MotoDTO { Id = 1, Modelo = "CG 160", Placa = "ABC1234" } };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(motos);

            // Act
            var result = await _controller.GetAll();
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Serializa o valor em JSON e reinterpreta como dicionário
            var json = System.Text.Json.JsonSerializer.Serialize(okResult.Value);
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var root = doc.RootElement;

            // Assert
            Assert.True(root.TryGetProperty("Data", out var data));
            Assert.True(data.GetArrayLength() > 0);

        }


        [Fact(DisplayName = "GET por ID deve retornar moto existente")]
        public async Task GetById_DeveRetornarMoto()
        {
            // Arrange
            var moto = new MotoDTO { Id = 1, Placa = "CCC9999", Modelo = "Honda Titan", Cor = "Preta" };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(moto);

            // Act
            var result = await _controller.GetById(1);
            var okResult = Assert.IsType<OkObjectResult>(result);

            // Extrai o objeto anônimo
            var json = System.Text.Json.JsonSerializer.Serialize(okResult.Value);
            using var doc = System.Text.Json.JsonDocument.Parse(json);

            // Assert
            Assert.True(doc.RootElement.TryGetProperty("Data", out var data));
            Assert.True(data.TryGetProperty("Modelo", out var modelo));
            Assert.Equal("Honda Titan", modelo.GetString());
        }

        [Fact(DisplayName = "POST deve retornar Created quando moto for criada")]
        public async Task Create_DeveRetornarCreated()
        {
            // Arrange
    var novaMoto = new MotoDTO
    {
        Id = 1,
        Placa = "XYZ1234",
        Modelo = "Suzuki Yes",
        Cor = "Prata"
    };

            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<MotoDTO>())).ReturnsAsync(novaMoto);

            // Act
            var result = await _controller.Create(novaMoto);
            var created = Assert.IsType<CreatedAtActionResult>(result);

            // Serializa o objeto anônimo retornado
            var json = System.Text.Json.JsonSerializer.Serialize(created.Value);
            using var doc = System.Text.Json.JsonDocument.Parse(json);

            // Assert
            Assert.True(doc.RootElement.TryGetProperty("Data", out var data));
            Assert.True(data.TryGetProperty("Modelo", out var modelo));
            Assert.Equal("Suzuki Yes", modelo.GetString());
        }

        [Fact(DisplayName = "DELETE deve retornar NoContent se moto for excluída")]
        public async Task Delete_DeveRetornarNoContent()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "DELETE deve retornar NotFound se moto não existir")]
        public async Task Delete_DeveRetornarNotFound()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
