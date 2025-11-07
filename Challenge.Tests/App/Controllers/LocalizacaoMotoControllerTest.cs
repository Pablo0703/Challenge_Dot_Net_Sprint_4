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

namespace Challenge.Tests.Controllers
{
    public class LocalizacaoMotoControllerTest
    {
        private readonly Mock<InterfaceLocalizacaoMoto> _serviceMock;
        private readonly ControllerLocalizacaoMoto _controller;

        public LocalizacaoMotoControllerTest()
        {
            _serviceMock = new Mock<InterfaceLocalizacaoMoto>();

            // ✅ Cria mock seguro para UrlHelper (evita erros de roteamento)
            var urlMock = new Mock<IUrlHelper>();
            urlMock
                .Setup(u => u.Action(It.IsAny<UrlActionContext>()))
                .Returns((UrlActionContext ctx) => $"/fake/{ctx.Action}");

            _controller = new ControllerLocalizacaoMoto(_serviceMock.Object)
            {
                Url = urlMock.Object
            };
        }

        // GET ALL
        [Fact(DisplayName = "GET deve retornar lista paginada de localizações com sucesso")]
        public async Task GetAll_DeveRetornarListaDeLocalizacoes()
        {
            // Arrange
            var lista = new List<LocalizacaoMotoDTO>
            {
                new LocalizacaoMotoDTO { Id = 1, IdMoto = 100, IdZona = 3 },
                new LocalizacaoMotoDTO { Id = 2, IdMoto = 200, IdZona = 4 }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(lista);

            // Act
            var result = await _controller.GetAll(limit: 10, offset: 0);
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var dataProp = response.GetType().GetProperty("Data");
            var dataValue = dataProp?.GetValue(response) as IEnumerable<LocalizacaoMotoDTO>;

            // Assert
            Assert.NotNull(dataValue);
            Assert.Equal(2, dataValue.Count());
            Assert.Equal(200, dataValue.Last().IdMoto);
        }

        // GET BY ID
        [Fact(DisplayName = "GET por ID deve retornar localização existente")]
        public async Task GetById_DeveRetornarLocalizacao()
        {
            // Arrange
            var localizacao = new LocalizacaoMotoDTO
            {
                Id = 1,
                IdMoto = 10,
                IdZona = 5
            };

            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(localizacao);

            // Act
            var result = await _controller.GetById(1);
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var dataProp = response.GetType().GetProperty("Data");
            var dataValue = dataProp?.GetValue(response) as LocalizacaoMotoDTO;

            // Assert
            Assert.NotNull(dataValue);
            Assert.Equal(10, dataValue.IdMoto);
            Assert.Equal(5, dataValue.IdZona);
        }

        // GET BY ID MOTO
        [Fact(DisplayName = "GET por IdMoto deve retornar lista de localizações da moto")]
        public async Task GetByIdMoto_DeveRetornarLocalizacoesDaMoto()
        {
            // Arrange
            var lista = new List<LocalizacaoMotoDTO>
            {
                new LocalizacaoMotoDTO { Id = 1, IdMoto = 99, IdZona = 8 }
            };

            _serviceMock.Setup(s => s.GetByIdMotoAsync(99)).ReturnsAsync(lista);

            // Act
            var result = await _controller.GetByIdMoto(99);
            var ok = Assert.IsType<OkObjectResult>(result);
            var data = Assert.IsAssignableFrom<IEnumerable<LocalizacaoMotoDTO>>(ok.Value);

            // Assert
            Assert.Single(data);
            Assert.Equal(99, data.First().IdMoto);
        }

        // POST
        [Fact(DisplayName = "POST deve retornar Created quando localização for criada")]
        public async Task Create_DeveRetornarCreated()
        {
            // Arrange
            var dto = new LocalizacaoMotoDTO { Id = 1, IdMoto = 5, IdZona = 2 };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<LocalizacaoMotoDTO>()))
                        .ReturnsAsync(dto);

            // Act
            var result = await _controller.Create(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);

            var response = created.Value!;
            var dataProp = response.GetType().GetProperty("Data");
            var dataValue = dataProp?.GetValue(response) as LocalizacaoMotoDTO;

            // Assert
            Assert.NotNull(dataValue);
            Assert.Equal(5, dataValue.IdMoto);
            Assert.Equal(2, dataValue.IdZona);
        }

        // PUT
        [Fact(DisplayName = "PUT deve retornar Ok quando localização for atualizada")]
        public async Task Update_DeveRetornarOk()
        {
            // Arrange
            var dto = new LocalizacaoMotoDTO { Id = 1, IdMoto = 12, IdZona = 4 };

            _serviceMock.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(dto);

            // Act
            var result = await _controller.Update(1, dto);
            var ok = Assert.IsType<OkObjectResult>(result);
            var updated = Assert.IsAssignableFrom<LocalizacaoMotoDTO>(ok.Value);

            // Assert
            Assert.Equal(12, updated.IdMoto);
            Assert.Equal(4, updated.IdZona);
        }

        // DELETE
        [Fact(DisplayName = "DELETE deve retornar NoContent quando localização for removida")]
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
