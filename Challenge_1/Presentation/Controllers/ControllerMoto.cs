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
    public class ControllerMoto : ControllerBase
    {
        private readonly InterfaceMoto _service;

        public ControllerMoto(InterfaceMoto service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todas as motos", Description = "Retorna todas as motos cadastradas com suporte a paginação.")]
        [SwaggerResponse(statusCode: 200, description: "Lista de motos retornada com sucesso", type: typeof(IEnumerable<MotoDTO>))]
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
        [SwaggerOperation(Summary = "Busca moto por ID", Description = "Retorna uma moto específica pelo seu identificador único.")]
        [SwaggerResponse(statusCode: 200, description: "Moto encontrada", type: typeof(MotoDTO))]
        [SwaggerResponse(statusCode: 404, description: "Moto não encontrada")]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound($"Moto com ID {id} não encontrada.");

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

        [HttpGet("porPlaca")]
        [SwaggerOperation(Summary = "Busca moto por placa", Description = "Retorna todas as motos que correspondem à placa informada.")]
        [SwaggerResponse(statusCode: 200, description: "Motos encontradas", type: typeof(IEnumerable<MotoDTO>))]
        [SwaggerResponse(statusCode: 404, description: "Nenhuma moto encontrada com a placa informada")]
        public async Task<IActionResult> GetByPlaca([FromQuery] string placa)
        {
            var result = await _service.GetByPlacaAsync(placa);
            if (!result.Any())
                return NotFound($"Nenhuma moto encontrada com placa '{placa}'.");

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra uma nova moto", Description = "Cria um novo registro de moto no sistema.")]
        [SwaggerRequestExample(typeof(MotoDTO), typeof(MotoRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Moto criada com sucesso", type: typeof(MotoDTO))]
        [SwaggerResponseExample(statusCode: 201, typeof(MotoResponseSample))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos para criação da moto")]
        public async Task<IActionResult> Create([FromBody] MotoDTO dto)
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
        [SwaggerOperation(Summary = "Atualiza uma moto", Description = "Atualiza os dados de uma moto já cadastrada.")]
        [SwaggerResponse(statusCode: 200, description: "Moto atualizada com sucesso", type: typeof(MotoDTO))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponse(statusCode: 404, description: "Moto não encontrada")]
        public async Task<IActionResult> Update(long id, [FromBody] MotoDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não bate com o objeto.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Moto com ID {id} não encontrada.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui uma moto", Description = "Remove uma moto do sistema pelo seu ID.")]
        [SwaggerResponse(statusCode: 204, description: "Moto excluída com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Moto não encontrada")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Moto com ID {id} não encontrada.");

            return NoContent();
        }
    }
}
