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
    public class ControllerFilial : ControllerBase
    {
        private readonly InterfaceFilial _service;

        public ControllerFilial(InterfaceFilial service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todas as filiais", Description = "Retorna todas as filiais cadastradas com suporte a paginação.")]
        [SwaggerResponse(statusCode: 200, description: "Lista de filiais retornada com sucesso", type: typeof(IEnumerable<FilialDTO>))]
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
        [SwaggerOperation(Summary = "Busca filial por ID", Description = "Retorna uma filial específica pelo seu identificador único.")]
        [SwaggerResponse(statusCode: 200, description: "Filial encontrada", type: typeof(FilialDTO))]
        [SwaggerResponse(statusCode: 404, description: "Filial não encontrada")]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound($"Filial com ID {id} não encontrada.");

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

        [HttpGet("porNome")]
        [SwaggerOperation(Summary = "Busca filial por nome", Description = "Retorna todas as filiais que correspondem ao nome informado.")]
        [SwaggerResponse(statusCode: 200, description: "Filiais encontradas", type: typeof(IEnumerable<FilialDTO>))]
        [SwaggerResponse(statusCode: 404, description: "Nenhuma filial encontrada com o nome informado")]
        public async Task<IActionResult> GetByNome([FromQuery] string nome)
        {
            var result = await _service.GetByNomeAsync(nome);
            if (!result.Any())
                return NotFound($"Nenhuma filial encontrada com nome '{nome}'.");

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra uma nova filial", Description = "Cria um novo registro de filial no sistema.")]
        [SwaggerRequestExample(typeof(FilialDTO), typeof(FilialRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Filial criada com sucesso", type: typeof(FilialDTO))]
        [SwaggerResponseExample(statusCode: 201, typeof(FilialResponseSample))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos para criação da filial")]
        public async Task<IActionResult> Create([FromBody] FilialDTO dto)
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
        [SwaggerOperation(Summary = "Atualiza uma filial", Description = "Atualiza os dados de uma filial já cadastrada.")]
        [SwaggerResponse(statusCode: 200, description: "Filial atualizada com sucesso", type: typeof(FilialDTO))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponse(statusCode: 404, description: "Filial não encontrada")]
        public async Task<IActionResult> Update(long id, [FromBody] FilialDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não bate com o objeto.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Filial com ID {id} não encontrada.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui uma filial", Description = "Remove uma filial do sistema pelo seu ID.")]
        [SwaggerResponse(statusCode: 204, description: "Filial excluída com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Filial não encontrada")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Filial com ID {id} não encontrada.");

            return NoContent();
        }
    }
}
