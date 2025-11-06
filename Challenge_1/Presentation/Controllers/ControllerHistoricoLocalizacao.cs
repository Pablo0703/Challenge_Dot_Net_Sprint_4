using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Challenge_1.Doc.Samples;

namespace Challenge_1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ControllerHistoricoLocalizacao : ControllerBase
    {
        private readonly InterfaceHistoricoLocalizacao _service;

        public ControllerHistoricoLocalizacao(InterfaceHistoricoLocalizacao service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os históricos de localização", Description = "Retorna todos os históricos cadastrados com suporte a paginação.")]
        [SwaggerResponse(statusCode: 200, description: "Lista de históricos retornada com sucesso", type: typeof(IEnumerable<HistoricoLocalizacaoDTO>))]
        public async Task<IActionResult> GetAll([FromQuery] int limit = 10, [FromQuery] int offset = 0)
        {
            var result = await _service.GetAllAsync();
            var paged = result.Skip(offset).Take(limit).ToList();

            var response = new
            {
                Total = result.Count(),
                Limit = limit,
                Offset = offset,
                Data = paged
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Busca histórico por ID", Description = "Retorna um histórico específico pelo seu identificador único.")]
        [SwaggerResponse(statusCode: 200, description: "Histórico encontrado", type: typeof(HistoricoLocalizacaoDTO))]
        [SwaggerResponse(statusCode: 404, description: "Histórico não encontrado")]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound($"Histórico com ID {id} não encontrado.");

            var response = new
            {
                Data = dto,
                Links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetById), new { id }) },
                    new { rel = "update", href = Url.Action(nameof(Update), new { id }) },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id }) }
                }
            };

            return Ok(response);
        }

        [HttpGet("porIdMoto")]
        [SwaggerOperation(Summary = "Busca histórico por ID da moto", Description = "Retorna todos os históricos associados a uma moto específica.")]
        [SwaggerResponse(statusCode: 200, description: "Históricos encontrados", type: typeof(IEnumerable<HistoricoLocalizacaoDTO>))]
        [SwaggerResponse(statusCode: 404, description: "Nenhum histórico encontrado para a moto informada")]
        public async Task<IActionResult> GetByIdMoto([FromQuery] long idMoto)
        {
            var result = await _service.GetByIdMotoAsync(idMoto);
            if (!result.Any())
                return NotFound($"Nenhum histórico encontrado para a moto com ID {idMoto}.");

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Registra novo histórico de localização", Description = "Cria um novo registro de movimentação de moto no sistema.")]
        [SwaggerRequestExample(typeof(HistoricoLocalizacaoDTO), typeof(HistoricoLocalizacaoRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Histórico criado com sucesso", type: typeof(HistoricoLocalizacaoDTO))]
        [SwaggerResponseExample(statusCode: 201, typeof(HistoricoLocalizacaoResponseSample))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos para criação do histórico")]
        public async Task<IActionResult> Create([FromBody] HistoricoLocalizacaoDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.CreateAsync(dto);

            var response = new
            {
                Data = created,
                Links = new[]
                {
                    new { rel = "self", href = Url.Action(nameof(GetById), new { id = created.Id }) },
                    new { rel = "update", href = Url.Action(nameof(Update), new { id = created.Id }) },
                    new { rel = "delete", href = Url.Action(nameof(Delete), new { id = created.Id }) }
                }
            };

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Atualiza um histórico de localização", Description = "Atualiza os dados de um histórico já cadastrado.")]
        [SwaggerResponse(statusCode: 200, description: "Histórico atualizado com sucesso", type: typeof(HistoricoLocalizacaoDTO))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponse(statusCode: 404, description: "Histórico não encontrado")]
        public async Task<IActionResult> Update(long id, [FromBody] HistoricoLocalizacaoDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não bate com o objeto.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Histórico com ID {id} não encontrado.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um histórico de localização", Description = "Remove um histórico do sistema pelo seu ID.")]
        [SwaggerResponse(statusCode: 204, description: "Histórico excluído com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Histórico não encontrado")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Histórico com ID {id} não encontrado.");

            return NoContent();
        }
    }
}
