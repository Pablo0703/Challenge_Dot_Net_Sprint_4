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

namespace Challenge.Tests.Controllers
{
    public class MotociclistaControllerTest
    {
        private readonly Mock<InterfaceMotociclista> _serviceMock;
        private readonly ControllerMotociclista _controller;

        public MotociclistaControllerTest()
        {
            _serviceMock = new Mock<InterfaceMotociclista>();
            _controller = new ControllerMotociclista(_serviceMock.Object);

            // Inicializa contexto HTTP e UrlHelper
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            _controller.ControllerContext = new ControllerContext(actionContext);
            // Mock da URL Helper — evita erro de roteamento
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/api/v1/ControllerMotociclista/1");

            _controller.Url = mockUrlHelper.Object;

        }

        [Fact(DisplayName = "GET deve retornar lista paginada de motociclistas com sucesso")]
        public async Task GetAll_DeveRetornarListaDeMotociclistas()
        {
            var lista = new List<MotociclistaDTO>
            {
                new MotociclistaDTO { Id = 1, Nome = "João", Cpf = "11111111111" },
                new MotociclistaDTO { Id = 2, Nome = "Maria", Cpf = "22222222222" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(lista);

            var result = await _controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var dataProp = response.GetType().GetProperty("Data");
            var dataValue = dataProp?.GetValue(response) as IEnumerable<MotociclistaDTO>;

            Assert.NotNull(dataValue);
            Assert.Equal(2, dataValue.Count());
            Assert.Contains(dataValue, m => m.Nome == "João");
        }

        [Fact(DisplayName = "GET por ID deve retornar motociclista existente")]
        public async Task GetById_DeveRetornarMotociclista()
        {
            var motociclista = new MotociclistaDTO { Id = 1, Nome = "Pedro", Cpf = "33333333333" };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(motociclista);

            var result = await _controller.GetById(1);
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value!;
            var dataProp = response.GetType().GetProperty("Data");
            var dataValue = dataProp?.GetValue(response) as MotociclistaDTO;

            Assert.NotNull(dataValue);
            Assert.Equal("Pedro", dataValue.Nome);
        }

        [Fact(DisplayName = "POST deve retornar Created quando motociclista for criado")]
        public async Task Create_DeveRetornarCreated()
        {
            var dto = new MotociclistaDTO { Id = 1, Nome = "Ana", Cpf = "44444444444" };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<MotociclistaDTO>())).ReturnsAsync(dto);

            var result = await _controller.Create(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);

            var response = created.Value!;
            var dataProp = response.GetType().GetProperty("Data");
            var dataValue = dataProp?.GetValue(response) as MotociclistaDTO;

            Assert.NotNull(dataValue);
            Assert.Equal("Ana", dataValue.Nome);
        }

        [Fact(DisplayName = "PUT deve retornar Ok quando motociclista for atualizado")]
        public async Task Update_DeveRetornarOk()
        {
            var dto = new MotociclistaDTO { Id = 1, Nome = "Carlos", Cpf = "55555555555" };
            _serviceMock.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(dto);

            var result = await _controller.Update(1, dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var updated = Assert.IsAssignableFrom<MotociclistaDTO>(ok.Value);

            Assert.Equal("Carlos", updated.Nome);
        }

        [Fact(DisplayName = "DELETE deve retornar NoContent quando motociclista for removido")]
        public async Task Delete_DeveRetornarNoContent()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _controller.Delete(1);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
