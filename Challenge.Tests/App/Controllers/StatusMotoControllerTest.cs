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
    public class StatusMotoControllerTest
    {
        private readonly Mock<InterfaceStatusMoto> _serviceMock;
        private readonly ControllerStatusMoto _controller;

        public StatusMotoControllerTest()
        {
            _serviceMock = new Mock<InterfaceStatusMoto>();
            _controller = new ControllerStatusMoto(_serviceMock.Object);

            // 🔧 Configura contexto simulado para o controller
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            _controller.ControllerContext = new ControllerContext(actionContext);

            // ✅ Injeta um mock de IUrlHelper para evitar o erro do IRouter
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/fake-url");

            _controller.Url = mockUrlHelper.Object;
        }

        [Fact(DisplayName = "GET deve retornar lista de status de moto")]
        public async Task GetAll_DeveRetornarLista()
        {
            var lista = new List<StatusMotoDTO>
            {
                new StatusMotoDTO { Id = 1, Descricao = "Disponível" },
                new StatusMotoDTO { Id = 2, Descricao = "Em Manutenção" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(lista);

            var result = await _controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as IEnumerable<StatusMotoDTO>;

            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
            Assert.Contains(data, s => s.Descricao == "Disponível");
        }

        [Fact(DisplayName = "GET por ID deve retornar status existente")]
        public async Task GetById_DeveRetornarStatus()
        {
            var dto = new StatusMotoDTO { Id = 1, Descricao = "Disponível" };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

            var result = await _controller.GetById(1);
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as StatusMotoDTO;

            Assert.NotNull(data);
            Assert.Equal("Disponível", data.Descricao);
        }

        [Fact(DisplayName = "POST deve retornar Created quando status for criado")]
        public async Task Create_DeveRetornarCreated()
        {
            var dto = new StatusMotoDTO { Id = 1, Descricao = "Em Reparos" };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<StatusMotoDTO>())).ReturnsAsync(dto);

            var result = await _controller.Create(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);

            var response = created.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as StatusMotoDTO;

            Assert.NotNull(data);
            Assert.Equal("Em Reparos", data.Descricao);
        }
    }
}