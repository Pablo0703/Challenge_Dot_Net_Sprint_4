using Application.Interfaces;
using Challenge_1.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Presentation.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests.App.Controllers
{
    public class EnderecoControllerTest
    {
        private readonly Mock<InterfaceEndereco> _serviceMock;
        private readonly ControllerEndereco _controller;

        public EnderecoControllerTest()
        {
            _serviceMock = new Mock<InterfaceEndereco>();
            _controller = new ControllerEndereco(_serviceMock.Object);

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/api/v1/ControllerEndereco/1");
            _controller.Url = urlHelperMock.Object;
        }

        [Fact(DisplayName = "GET deve retornar lista paginada de endereços com sucesso")]
        public async Task GetAll_DeveRetornarListaDeEnderecos()
        {
            var enderecos = new List<EnderecoDTO>
            {
                new EnderecoDTO { Id = 1, Logradouro = "Rua A", Numero = "123", Cidade = "São Paulo" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(enderecos);

            var result = await _controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result);
            // ✅ Use isso:
            var response = ok.Value!;
            var dataProperty = response.GetType().GetProperty("Data");
            var dataValue = dataProperty?.GetValue(response) as IEnumerable<EnderecoDTO>;

            Assert.NotNull(dataValue);
            Assert.True(dataValue.Any());
            Assert.NotNull(response);
            
        }
        [Fact(DisplayName = "GET por ID deve retornar endereço existente")]
        public async Task GetById_DeveRetornarEndereco()
        {
            var dto = new EnderecoDTO
            {
                Id = 1,
                Logradouro = "Rua B",
                Numero = "100",
                Cidade = "Campinas"
            };

            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

            var result = await _controller.GetById(1);
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var dataProp = response.GetType().GetProperty("Data");
            var dataValue = dataProp?.GetValue(response) as EnderecoDTO;

            Assert.NotNull(dataValue);
            Assert.Equal("Rua B", dataValue.Logradouro);
            Assert.Equal("Campinas", dataValue.Cidade);
        }


        [Fact(DisplayName = "POST deve retornar Created quando endereço for criado")]
        public async Task Create_DeveRetornarCreated()
        {
            // Arrange
            var novoEndereco = new EnderecoDTO
            {
                Id = 1,
                Logradouro = "Rua C",
                Numero = "50",
                Cidade = "São Paulo"
            };

            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<EnderecoDTO>()))
                        .ReturnsAsync(novoEndereco);

            // Act
            var result = await _controller.Create(novoEndereco);
            var created = Assert.IsType<CreatedAtActionResult>(result);

            // Corrigido: acessa o objeto dentro de "Data"
            var response = created.Value!;
            var dataProp = response.GetType().GetProperty("Data");
            var dataValue = dataProp?.GetValue(response) as EnderecoDTO;

            // Assert
            Assert.NotNull(dataValue);
            Assert.Equal("Rua C", dataValue.Logradouro);
            Assert.Equal("São Paulo", dataValue.Cidade);
        }

        [Fact(DisplayName = "DELETE deve retornar NoContent se endereço for excluído")]
        public async Task Delete_DeveRetornarNoContent()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);
            var result = await _controller.Delete(1);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact(DisplayName = "DELETE deve retornar NotFound se endereço não existir")]
        public async Task Delete_DeveRetornarNotFound()
        {
            _serviceMock.Setup(s => s.DeleteAsync(99)).ReturnsAsync(false);
            var result = await _controller.Delete(99);
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
