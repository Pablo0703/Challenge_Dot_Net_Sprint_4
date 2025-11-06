using Challenge_1.Infrastructure.ML;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Challenge_1.Presentation.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PredicaoController : ControllerBase
    {
        private static ConsumoPredictor _predictor = new ConsumoPredictor();

        [HttpPost("consumo")]
        [SwaggerOperation(Summary = "Prediz o consumo médio (km/L)", Description = "Utiliza ML.NET para prever o consumo baseado em cilindrada, peso e velocidade média.")]
        public IActionResult PredizerConsumo([FromBody] ConsumoInput input)
        {
            var resultado = _predictor.PreverConsumo(input.Cilindrada, input.Peso, input.VelocidadeMedia);
            return Ok(new
            {
                input,
                ConsumoPrevisto = Math.Round(resultado, 2),
                Observacao = "Previsão gerada via modelo de regressão linear (ML.NET)"
            });
        }

        [HttpPost("treinar")]
        [SwaggerOperation(Summary = "Treina o modelo com novos dados", Description = "Permite adicionar um novo exemplo de treino (com consumo real) e atualizar o modelo ML.NET.")]
        public IActionResult TreinarModelo([FromBody] ConsumoInput novoDado)
        {
            _predictor.AdicionarDadosTreino(novoDado);

            return Ok(new
            {
                Mensagem = "Modelo atualizado com sucesso!",
                DadosAdicionados = novoDado
            });
        }
    }
}
