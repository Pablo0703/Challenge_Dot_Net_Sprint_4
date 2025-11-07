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
    public class TipoMotoControllerTest
    {
        private readonly Mock<InterfaceTipoMoto> _serviceMock;
        private readonly ControllerTipoMoto _controller;

        public TipoMotoControllerTest()
        {
            _serviceMock = new Mock<InterfaceTipoMoto>();

            // ✅ Cria o controlador com mock de serviço
            _controller = new ControllerTipoMoto(_serviceMock.Object);

            // ✅ Configura ActionContext e UrlHelperFactory
            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            _controller.ControllerContext = new ControllerContext(actionContext);

            // ✅ Mock do IUrlHelperFactory e do IUrlHelper (sem usar IRouter)
            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/api/fake-link");

            var urlHelperFactoryMock = new Mock<IUrlHelperFactory>();
            urlHelperFactoryMock
                .Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(urlHelperMock.Object);

            // ✅ Injeta o UrlHelper falso no controller
            _controller.Url = urlHelperMock.Object;
        }

        [Fact(DisplayName = "GET deve retornar lista de tipos de moto")]
        public async Task GetAll_DeveRetornarLista()
        {
            var lista = new List<TipoMotoDTO>
            {
                new TipoMotoDTO { Id = 1, Descricao = "Street 150cc", Categoria = "Urbana" },
                new TipoMotoDTO { Id = 2, Descricao = "Trail 300cc", Categoria = "Off-road" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(lista);

            var result = await _controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as IEnumerable<TipoMotoDTO>;

            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact(DisplayName = "GET por ID deve retornar tipo de moto existente")]
        public async Task GetById_DeveRetornarTipoMoto()
        {
            var dto = new TipoMotoDTO { Id = 10, Descricao = "Custom 900cc", Categoria = "Premium" };
            _serviceMock.Setup(s => s.GetByIdAsync(10)).ReturnsAsync(dto);

            var result = await _controller.GetById(10);
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as TipoMotoDTO;

            Assert.NotNull(data);
            Assert.Equal("Custom 900cc", data.Descricao);
            Assert.Equal("Premium", data.Categoria);
        }

        [Fact(DisplayName = "POST deve retornar Created quando tipo de moto for criado")]
        public async Task Create_DeveRetornarCreated()
        {
            var dto = new TipoMotoDTO { Id = 1, Descricao = "Adventure 1000cc", Categoria = "Big Trail" };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<TipoMotoDTO>())).ReturnsAsync(dto);

            var result = await _controller.Create(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);

            var response = created.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as TipoMotoDTO;

            Assert.NotNull(data);
            Assert.Equal("Adventure 1000cc", data.Descricao);
            Assert.Equal("Big Trail", data.Categoria);
        }
    }
}