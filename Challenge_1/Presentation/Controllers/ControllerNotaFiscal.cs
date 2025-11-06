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
    public class ControllerNotaFiscal : ControllerBase
    {
        private readonly InterfaceNotaFiscal _service;

        public ControllerNotaFiscal(InterfaceNotaFiscal service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todas as notas fiscais", Description = "Retorna todas as notas fiscais cadastradas com suporte a paginação.")]
        [SwaggerResponse(statusCode: 200, description: "Lista de notas fiscais retornada com sucesso", type: typeof(IEnumerable<NotaFiscalDTO>))]
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
        [SwaggerOperation(Summary = "Busca nota fiscal por ID", Description = "Retorna uma nota fiscal específica pelo seu identificador único.")]
        [SwaggerResponse(statusCode: 200, description: "Nota fiscal encontrada", type: typeof(NotaFiscalDTO))]
        [SwaggerResponse(statusCode: 404, description: "Nota fiscal não encontrada")]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound($"Nota fiscal com ID {id} não encontrada.");

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

        [HttpGet("porNumero")]
        [SwaggerOperation(Summary = "Busca nota fiscal por número", Description = "Retorna todas as notas fiscais que correspondem ao número informado.")]
        [SwaggerResponse(statusCode: 200, description: "Notas fiscais encontradas", type: typeof(IEnumerable<NotaFiscalDTO>))]
        [SwaggerResponse(statusCode: 404, description: "Nenhuma nota fiscal encontrada com o número informado")]
        public async Task<IActionResult> GetByNumero([FromQuery] string numero)
        {
            var result = await _service.GetByNumeroAsync(numero);
            if (!result.Any())
                return NotFound($"Nenhuma nota fiscal encontrada com número '{numero}'.");

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra uma nova nota fiscal", Description = "Cria um novo registro de nota fiscal no sistema.")]
        [SwaggerRequestExample(typeof(NotaFiscalDTO), typeof(NotaFiscalRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Nota fiscal criada com sucesso", type: typeof(NotaFiscalDTO))]
        [SwaggerResponseExample(statusCode: 201, typeof(NotaFiscalResponseSample))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos para criação da nota fiscal")]
        public async Task<IActionResult> Create([FromBody] NotaFiscalDTO dto)
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
        [SwaggerOperation(Summary = "Atualiza uma nota fiscal", Description = "Atualiza os dados de uma nota fiscal já cadastrada.")]
        [SwaggerResponse(statusCode: 200, description: "Nota fiscal atualizada com sucesso", type: typeof(NotaFiscalDTO))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponse(statusCode: 404, description: "Nota fiscal não encontrada")]
        public async Task<IActionResult> Update(long id, [FromBody] NotaFiscalDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não bate com o objeto.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Nota fiscal com ID {id} não encontrada.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui uma nota fiscal", Description = "Remove uma nota fiscal do sistema pelo seu ID.")]
        [SwaggerResponse(statusCode: 204, description: "Nota fiscal excluída com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Nota fiscal não encontrada")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Nota fiscal com ID {id} não encontrada.");

            return NoContent();
        }
    }
}
