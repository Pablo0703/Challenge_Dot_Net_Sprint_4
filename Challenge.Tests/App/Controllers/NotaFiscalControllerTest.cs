using Application.Interfaces;
using Challenge_1.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Presentation.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests.Controllers
{
    public class NotaFiscalControllerTest
    {
        private readonly Mock<InterfaceNotaFiscal> _serviceMock;
        private readonly ControllerNotaFiscal _controller;

        public NotaFiscalControllerTest()
        {
            _serviceMock = new Mock<InterfaceNotaFiscal>();
            _controller = new ControllerNotaFiscal(_serviceMock.Object);

            // 🔧 Simula o contexto HTTP
            var httpContext = new DefaultHttpContext();
            var routeData = new RouteData();
            var actionDescriptor = new ControllerActionDescriptor();
            var actionContext = new ActionContext(httpContext, routeData, actionDescriptor);

            _controller.ControllerContext = new ControllerContext(actionContext);

            // ✅ Cria um UrlHelper funcional sem depender do LinkGenerator real
            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns((UrlActionContext ctx) =>
                    $"http://localhost/api/v1/ControllerNotaFiscal/{ctx.Values}");

            _controller.Url = urlHelperMock.Object;
        }


        [Fact(DisplayName = "GET deve retornar lista paginada de notas fiscais com sucesso")]
        public async Task GetAll_DeveRetornarListaDeNotas()
        {
            var lista = new List<NotaFiscalDTO>
            {
                new NotaFiscalDTO { Id = 1, Numero = "NF001" },
                new NotaFiscalDTO { Id = 2, Numero = "NF002" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(lista);

            var result = await _controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as IEnumerable<NotaFiscalDTO>;

            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact(DisplayName = "GET por ID deve retornar nota existente")]
        public async Task GetById_DeveRetornarNota()
        {
            var nota = new NotaFiscalDTO { Id = 1, Numero = "NF999" };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(nota);

            var result = await _controller.GetById(1);
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as NotaFiscalDTO;

            Assert.NotNull(data);
            Assert.Equal("NF999", data.Numero);
        }

        [Fact(DisplayName = "POST deve retornar Created quando nota for criada")]
        public async Task Create_DeveRetornarCreated()
        {
            var dto = new NotaFiscalDTO { Id = 1, Numero = "NF123" };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<NotaFiscalDTO>())).ReturnsAsync(dto);

            var result = await _controller.Create(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);

            var response = created.Value!;
            var prop = response.GetType().GetProperty("Data");
            var data = prop?.GetValue(response) as NotaFiscalDTO;

            Assert.NotNull(data);
            Assert.Equal("NF123", data.Numero);
        }
    }
}
