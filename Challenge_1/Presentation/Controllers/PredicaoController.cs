using Challenge_1.Infrastructure.ML;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Challenge_1.Presentation.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [SwaggerTag("Predição de Consumo - ML.NET")]
    public class PredicaoController : ControllerBase
    {
        private static readonly ConsumoPredictor _predictor = new ConsumoPredictor();

        [HttpPost("consumo")]
        [SwaggerOperation(
            Summary = "Prediz o consumo médio (km/L)",
            Description = "Utiliza ML.NET para prever o consumo médio de uma moto com base em cilindrada, peso e velocidade média.")]
        [SwaggerResponse(statusCode: 200, description: "Previsão de consumo gerada com sucesso.")]
        [SwaggerResponse(statusCode: 400, description: "Dados de entrada inválidos.")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno ao processar a predição.")]
        public IActionResult PredizerConsumo([FromBody] ConsumoInput input)
        {
            if (input == null)
                return BadRequest(new { message = "Entrada inválida. Forneça os parâmetros de cilindrada, peso e velocidade média." });

            var resultado = _predictor.PreverConsumo(input.Cilindrada, input.Peso, input.VelocidadeMedia);

            return Ok(new
            {
                input,
                ConsumoPrevisto = Math.Round(resultado, 2),
                Observacao = "Previsão gerada via modelo de regressão linear (ML.NET)"
            });
        }

        [HttpPost("treinar")]
        [SwaggerOperation(
            Summary = "Treina o modelo com novos dados",
            Description = "Adiciona um novo exemplo com consumo real para atualizar o modelo ML.NET e melhorar a precisão das previsões.")]
        [SwaggerResponse(statusCode: 200, description: "Modelo atualizado com sucesso.")]
        [SwaggerResponse(statusCode: 400, description: "Dados de treino inválidos.")]
        [SwaggerResponse(statusCode: 500, description: "Erro interno ao atualizar o modelo.")]
        public IActionResult TreinarModelo([FromBody] ConsumoInput novoDado)
        {
            if (novoDado == null)
                return BadRequest(new { message = "Dados de treino inválidos. Forneça valores válidos para o modelo." });

            _predictor.AdicionarDadosTreino(novoDado);

            return Ok(new
            {
                Mensagem = "Modelo atualizado com sucesso!",
                DadosAdicionados = novoDado
            });
        }
    }
}
