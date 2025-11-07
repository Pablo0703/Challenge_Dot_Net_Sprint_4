using Application.Interfaces;
using Challenge_1.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Moq;
using Presentation.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests.App.Controllers
{
    public class PatioControllerTest
    {
        private readonly Mock<InterfacePatio> _serviceMock;
        private readonly ControllerPatio _controller;

        public PatioControllerTest()
        {
            _serviceMock = new Mock<InterfacePatio>();
            _controller = new ControllerPatio(_serviceMock.Object);

            // Cria contexto HTTP simulado
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            _controller.ControllerContext = new ControllerContext(actionContext);

            // Mock do UrlHelper seguro (para evitar erro de roteamento)
            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock
                .Setup(u => u.Action(It.IsAny<UrlActionContext>()))
                .Returns("/fake/url");

            _controller.Url = urlHelperMock.Object;
        }

        [Fact(DisplayName = "GET deve retornar lista paginada de pátios com sucesso")]
        public async Task GetAll_DeveRetornarListaDePatios()
        {
            // Arrange
            var lista = new List<PatioDTO>
            {
                new PatioDTO { Id = 1, Nome = "Pátio Central" },
                new PatioDTO { Id = 2, Nome = "Pátio Norte" }
            };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(lista);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value!;
            var dataProp = response.GetType().GetProperty("Data");
            var data = dataProp?.GetValue(response) as IEnumerable<PatioDTO>;

            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact(DisplayName = "GET por ID deve retornar pátio existente")]
        public async Task GetById_DeveRetornarPatio()
        {
            // Arrange
            var patio = new PatioDTO { Id = 1, Nome = "Pátio Central" };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(patio);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as PatioDTO;

            Assert.NotNull(data);
            Assert.Equal("Pátio Central", data.Nome);
        }

        [Fact(DisplayName = "POST deve retornar Created quando pátio for criado")]
        public async Task Create_DeveRetornarCreated()
        {
            // Arrange
            var dto = new PatioDTO { Id = 1, Nome = "Pátio Novo" };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<PatioDTO>())).ReturnsAsync(dto);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var response = created.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as PatioDTO;

            Assert.NotNull(data);
            Assert.Equal("Pátio Novo", data.Nome);
        }

        [Fact(DisplayName = "PUT deve retornar Ok quando pátio for atualizado")]
        public async Task Update_DeveRetornarOk()
        {
            // Arrange
            var dto = new PatioDTO { Id = 1, Nome = "Pátio Atualizado" };
            _serviceMock.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(dto);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var updated = Assert.IsAssignableFrom<PatioDTO>(ok.Value);
            Assert.Equal("Pátio Atualizado", updated.Nome);
        }

        [Fact(DisplayName = "DELETE deve retornar NoContent quando pátio for removido")]
        public async Task Delete_DeveRetornarNoContent()
        {
            // Arrange
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}