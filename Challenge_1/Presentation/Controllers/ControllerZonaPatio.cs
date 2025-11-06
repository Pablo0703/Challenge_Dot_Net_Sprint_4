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
    public class ControllerZonaPatio : ControllerBase
    {
        private readonly InterfaceZonaPatio _service;

        public ControllerZonaPatio(InterfaceZonaPatio service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todas as zonas de pátio", Description = "Retorna todas as zonas de pátio cadastradas com suporte a paginação.")]
        [SwaggerResponse(statusCode: 200, description: "Lista de zonas de pátio retornada com sucesso", type: typeof(IEnumerable<ZonaPatioDTO>))]
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
        [SwaggerOperation(Summary = "Busca zona de pátio por ID", Description = "Retorna uma zona de pátio específica pelo seu identificador único.")]
        [SwaggerResponse(statusCode: 200, description: "Zona de pátio encontrada", type: typeof(ZonaPatioDTO))]
        [SwaggerResponse(statusCode: 404, description: "Zona de pátio não encontrada")]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound($"Zona de pátio com ID {id} não encontrada.");

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
        [SwaggerOperation(Summary = "Busca zona de pátio por nome", Description = "Retorna todas as zonas de pátio que correspondem ao nome informado.")]
        [SwaggerResponse(statusCode: 200, description: "Zonas de pátio encontradas", type: typeof(IEnumerable<ZonaPatioDTO>))]
        [SwaggerResponse(statusCode: 404, description: "Nenhuma zona de pátio encontrada com o nome informado")]
        public async Task<IActionResult> GetByNomeZona([FromQuery] string nomeZona)
        {
            var result = await _service.GetByNomeZonaAsync(nomeZona);
            if (!result.Any())
                return NotFound($"Nenhuma zona encontrada com nome '{nomeZona}'.");

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra uma nova zona de pátio", Description = "Cria um novo registro de zona de pátio no sistema.")]
        [SwaggerRequestExample(typeof(ZonaPatioDTO), typeof(ZonaPatioRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Zona de pátio criada com sucesso", type: typeof(ZonaPatioDTO))]
        [SwaggerResponseExample(statusCode: 201, typeof(ZonaPatioResponseSample))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos para criação da zona de pátio")]
        public async Task<IActionResult> Create([FromBody] ZonaPatioDTO dto)
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
        [SwaggerOperation(Summary = "Atualiza uma zona de pátio", Description = "Atualiza os dados de uma zona de pátio já cadastrada.")]
        [SwaggerResponse(statusCode: 200, description: "Zona de pátio atualizada com sucesso", type: typeof(ZonaPatioDTO))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponse(statusCode: 404, description: "Zona de pátio não encontrada")]
        public async Task<IActionResult> Update(long id, [FromBody] ZonaPatioDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não bate com o objeto.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Zona de pátio com ID {id} não encontrada.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui uma zona de pátio", Description = "Remove uma zona de pátio do sistema pelo seu ID.")]
        [SwaggerResponse(statusCode: 204, description: "Zona de pátio excluída com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Zona de pátio não encontrada")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Zona de pátio com ID {id} não encontrada.");

            return NoContent();
        }
    }
}
