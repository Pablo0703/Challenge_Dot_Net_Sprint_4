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
    public class ControllerMotociclista : ControllerBase
    {
        private readonly InterfaceMotociclista _service;

        public ControllerMotociclista(InterfaceMotociclista service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os motociclistas", Description = "Retorna todos os motociclistas cadastrados com suporte a paginação.")]
        [SwaggerResponse(statusCode: 200, description: "Lista de motociclistas retornada com sucesso", type: typeof(IEnumerable<MotociclistaDTO>))]
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
        [SwaggerOperation(Summary = "Busca motociclista por ID", Description = "Retorna um motociclista específico pelo seu identificador único.")]
        [SwaggerResponse(statusCode: 200, description: "Motociclista encontrado", type: typeof(MotociclistaDTO))]
        [SwaggerResponse(statusCode: 404, description: "Motociclista não encontrado")]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound($"Motociclista com ID {id} não encontrado.");

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
        [SwaggerOperation(Summary = "Busca motociclistas por nome", Description = "Retorna todos os motociclistas que correspondem ao nome informado.")]
        [SwaggerResponse(statusCode: 200, description: "Motociclistas encontrados", type: typeof(IEnumerable<MotociclistaDTO>))]
        [SwaggerResponse(statusCode: 404, description: "Nenhum motociclista encontrado com o nome informado")]
        public async Task<IActionResult> GetByNome([FromQuery] string nome)
        {
            var result = await _service.GetByNomeAsync(nome);
            if (!result.Any())
                return NotFound($"Nenhum motociclista encontrado com nome '{nome}'.");

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra um novo motociclista", Description = "Cria um novo registro de motociclista no sistema.")]
        [SwaggerRequestExample(typeof(MotociclistaDTO), typeof(MotociclistaRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Motociclista criado com sucesso", type: typeof(MotociclistaDTO))]
        [SwaggerResponseExample(statusCode: 201, typeof(MotociclistaResponseSample))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos para criação do motociclista")]
        public async Task<IActionResult> Create([FromBody] MotociclistaDTO dto)
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
        [SwaggerOperation(Summary = "Atualiza um motociclista", Description = "Atualiza os dados de um motociclista já cadastrado.")]
        [SwaggerResponse(statusCode: 200, description: "Motociclista atualizado com sucesso", type: typeof(MotociclistaDTO))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponse(statusCode: 404, description: "Motociclista não encontrado")]
        public async Task<IActionResult> Update(long id, [FromBody] MotociclistaDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não bate com o objeto.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Motociclista com ID {id} não encontrado.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um motociclista", Description = "Remove um motociclista do sistema pelo seu ID.")]
        [SwaggerResponse(statusCode: 204, description: "Motociclista excluído com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Motociclista não encontrado")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Motociclista com ID {id} não encontrado.");

            return NoContent();
        }
    }
}
