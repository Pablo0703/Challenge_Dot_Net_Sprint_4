using Application.Interfaces;
using Challenge_1.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
    public class ZonaPatioControllerTest
    {
        private readonly Mock<InterfaceZonaPatio> _serviceMock;
        private readonly ControllerZonaPatio _controller;

        public ZonaPatioControllerTest()
        {
            _serviceMock = new Mock<InterfaceZonaPatio>();
            _controller = new ControllerZonaPatio(_serviceMock.Object);

            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            _controller.ControllerContext = new ControllerContext(actionContext);

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.Setup(u => u.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/api/fake-link");

            _controller.Url = urlHelperMock.Object;
        }

        [Fact(DisplayName = "GET deve retornar lista de zonas de pátio")]
        public async Task GetAll_DeveRetornarLista()
        {
            var lista = new List<ZonaPatioDTO>
            {
                new ZonaPatioDTO { Id = 1, NomeZona = "Zona A", TipoZona = "Coberta", Capacidade = 10 },
                new ZonaPatioDTO { Id = 2, NomeZona = "Zona B", TipoZona = "Aberta", Capacidade = 15 }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(lista);

            var result = await _controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as IEnumerable<ZonaPatioDTO>;

            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact(DisplayName = "GET por ID deve retornar zona existente")]
        public async Task GetById_DeveRetornarZonaPatio()
        {
            var dto = new ZonaPatioDTO { Id = 10, NomeZona = "Zona C", TipoZona = "Aberta", Capacidade = 25 };
            _serviceMock.Setup(s => s.GetByIdAsync(10)).ReturnsAsync(dto);

            var result = await _controller.GetById(10);
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as ZonaPatioDTO;

            Assert.NotNull(data);
            Assert.Equal("Zona C", data.NomeZona);
        }

        [Fact(DisplayName = "POST deve retornar Created quando zona for criada")]
        public async Task Create_DeveRetornarCreated()
        {
            var dto = new ZonaPatioDTO { Id = 1, NomeZona = "Zona D", TipoZona = "Coberta", Capacidade = 12 };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<ZonaPatioDTO>())).ReturnsAsync(dto);

            var result = await _controller.Create(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);

            var response = created.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as ZonaPatioDTO;

            Assert.NotNull(data);
            Assert.Equal("Zona D", data.NomeZona);
        }
    }
}