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
    public class ControllerStatusOperacao : ControllerBase
    {
        private readonly InterfaceStatusOperacao _service;

        public ControllerStatusOperacao(InterfaceStatusOperacao service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os status de operação", Description = "Retorna todos os status de operação cadastrados com suporte a paginação.")]
        [SwaggerResponse(statusCode: 200, description: "Lista de status de operação retornada com sucesso", type: typeof(IEnumerable<StatusOperacaoDTO>))]
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
        [SwaggerOperation(Summary = "Busca status de operação por ID", Description = "Retorna um status de operação específico pelo seu identificador único.")]
        [SwaggerResponse(statusCode: 200, description: "Status de operação encontrado", type: typeof(StatusOperacaoDTO))]
        [SwaggerResponse(statusCode: 404, description: "Status de operação não encontrado")]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound($"StatusOperacao com ID {id} não encontrado.");

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

        [HttpGet("porDescricao")]
        [SwaggerOperation(Summary = "Busca status de operação por descrição", Description = "Retorna todos os status de operação que correspondem à descrição informada.")]
        [SwaggerResponse(statusCode: 200, description: "Status de operação encontrados", type: typeof(IEnumerable<StatusOperacaoDTO>))]
        [SwaggerResponse(statusCode: 404, description: "Nenhum status de operação encontrado com a descrição informada")]
        public async Task<IActionResult> GetByDescricao([FromQuery] string descricao)
        {
            var result = await _service.GetByDescricaoAsync(descricao);
            if (!result.Any())
                return NotFound($"Nenhum status de operação encontrado com descrição '{descricao}'.");

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra um novo status de operação", Description = "Cria um novo registro de status de operação no sistema.")]
        [SwaggerRequestExample(typeof(StatusOperacaoDTO), typeof(StatusOperacaoRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Status de operação criado com sucesso", type: typeof(StatusOperacaoDTO))]
        [SwaggerResponseExample(statusCode: 201, typeof(StatusOperacaoResponseSample))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos para criação do status de operação")]
        public async Task<IActionResult> Create([FromBody] StatusOperacaoDTO dto)
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
        [SwaggerOperation(Summary = "Atualiza um status de operação", Description = "Atualiza os dados de um status de operação já cadastrado.")]
        [SwaggerResponse(statusCode: 200, description: "Status de operação atualizado com sucesso", type: typeof(StatusOperacaoDTO))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponse(statusCode: 404, description: "Status de operação não encontrado")]
        public async Task<IActionResult> Update(long id, [FromBody] StatusOperacaoDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não bate com o objeto.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"StatusOperacao com ID {id} não encontrado.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um status de operação", Description = "Remove um status de operação do sistema pelo seu ID.")]
        [SwaggerResponse(statusCode: 204, description: "Status de operação excluído com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Status de operação não encontrado")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"StatusOperacao com ID {id} não encontrado.");

            return NoContent();
        }
    }
}
