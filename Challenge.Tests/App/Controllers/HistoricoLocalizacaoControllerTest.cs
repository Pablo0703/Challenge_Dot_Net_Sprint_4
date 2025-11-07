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
    public class HistoricoLocalizacaoControllerTest
    {
        private readonly Mock<InterfaceHistoricoLocalizacao> _serviceMock;
        private readonly ControllerHistoricoLocalizacao _controller;

        public HistoricoLocalizacaoControllerTest()
        {
            _serviceMock = new Mock<InterfaceHistoricoLocalizacao>();
            _controller = new ControllerHistoricoLocalizacao(_serviceMock.Object)
            {
                Url = Mock.Of<IUrlHelper>() // ✅ simula helper vazio e seguro
            };
        }


        [Fact(DisplayName = "GET deve retornar lista paginada de históricos com sucesso")]
        public async Task GetAll_DeveRetornarListaDeHistoricos()
        {
            var historicos = new List<HistoricoLocalizacaoDTO>
            {
                new HistoricoLocalizacaoDTO { Id = 1, IdMoto = 10, KmRodados = 100 }
            };

            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(historicos);

            var result = await _controller.GetAll();
            var ok = Assert.IsType<OkObjectResult>(result);

            var response = ok.Value!;
            var dataProperty = response.GetType().GetProperty("Data");
            var dataValue = dataProperty?.GetValue(response) as IEnumerable<HistoricoLocalizacaoDTO>;

            Assert.NotNull(dataValue);
            Assert.True(dataValue.Any());
        }

        [Fact(DisplayName = "GET por ID deve retornar histórico existente")]
        public async Task GetById_DeveRetornarHistorico()
        {
            var historico = new HistoricoLocalizacaoDTO
            {
                Id = 1,
                IdMoto = 10,
                KmRodados = 150
            };

            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(historico);

            var result = await _controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            var response = ok.Value!;

            // acessa a propriedade Data dinamicamente
            var dataProperty = response.GetType().GetProperty("Data");
            var dataValue = dataProperty?.GetValue(response) as HistoricoLocalizacaoDTO;

            Assert.NotNull(dataValue);
            Assert.Equal(10, dataValue.IdMoto); //  verificação real
        }


        [Fact(DisplayName = "POST deve retornar Created quando histórico for criado")]
        public async Task Create_DeveRetornarCreated()
        {
            var dto = new HistoricoLocalizacaoDTO { IdMoto = 10, KmRodados = 200 };
            _serviceMock.Setup(s => s.CreateAsync(It.IsAny<HistoricoLocalizacaoDTO>())).ReturnsAsync(dto);

            var result = await _controller.Create(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, created.StatusCode);
        }
    }
}
