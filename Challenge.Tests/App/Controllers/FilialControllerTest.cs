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

namespace Challenge.Tests.Unit
{
    public class FilialControllerTest
    {
        private readonly Mock<InterfaceFilial> _serviceMock;
        private readonly ControllerFilial _controller;

        public FilialControllerTest()
        {
            _serviceMock = new Mock<InterfaceFilial>();
            _controller = new ControllerFilial(_serviceMock.Object);
        }

        [Fact(DisplayName = "GET deve retornar lista paginada de filiais com sucesso")]
        public async Task GetAll_DeveRetornarListaDeFiliais()
        {
            var filiais = new List<FilialDTO>
            {
                new FilialDTO { Id = 1, Nome = "Filial São Paulo", Cnpj = "1234567890001", Telefone = "1199999999" }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(filiais);

            var result = await _controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value!;
            var dataProperty = response.GetType().GetProperty("Data");
            var dataValue = dataProperty?.GetValue(response) as IEnumerable<FilialDTO>;

            Assert.NotNull(dataValue);
            Assert.True(dataValue.Any());
        }

        [Fact(DisplayName = "GET por ID deve retornar filial existente")]
        public async Task GetById_DeveRetornarFilial()
        {
            var dto = new FilialDTO { Id = 1, Nome = "Filial Rio", Cnpj = "9876543210001" };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(dto);

            // ✅ Mocka o UrlHelper
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/api/v1/ControllerFilial/1");
            _controller.Url = mockUrlHelper.Object;

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value!;
            var dataProperty = response.GetType().GetProperty("Data");
            var dataValue = dataProperty?.GetValue(response) as FilialDTO;

            Assert.NotNull(dataValue);
            Assert.Equal("Filial Rio", dataValue!.Nome);
        }


        [Fact(DisplayName = "POST deve retornar Created quando filial for criada")]
        public async Task Create_DeveRetornarCreated()
        {
            var dto = new FilialDTO { Id = 1, Nome = "Filial Campinas", Cnpj = "22334455667788" };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<FilialDTO>())).ReturnsAsync(dto);

            //  Mock do UrlHelper para evitar ArgumentNullException
            var mockUrlHelper = new Mock<IUrlHelper>();
            mockUrlHelper
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/api/v1/ControllerFilial/1");
            _controller.Url = mockUrlHelper.Object;

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var response = created.Value!;
            var dataProperty = response.GetType().GetProperty("Data");
            var dataValue = dataProperty?.GetValue(response) as FilialDTO;

            Assert.NotNull(dataValue);
            Assert.Equal("Filial Campinas", dataValue!.Nome);
        }

    }
}
