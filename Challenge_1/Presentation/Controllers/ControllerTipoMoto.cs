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
    public class ControllerTipoMoto : ControllerBase
    {
        private readonly InterfaceTipoMoto _service;

        public ControllerTipoMoto(InterfaceTipoMoto service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os tipos de moto", Description = "Retorna todos os tipos de moto cadastrados com suporte a paginação.")]
        [SwaggerResponse(statusCode: 200, description: "Lista de tipos de moto retornada com sucesso", type: typeof(IEnumerable<TipoMotoDTO>))]
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
        [SwaggerOperation(Summary = "Busca tipo de moto por ID", Description = "Retorna um tipo de moto específico pelo seu identificador único.")]
        [SwaggerResponse(statusCode: 200, description: "Tipo de moto encontrado", type: typeof(TipoMotoDTO))]
        [SwaggerResponse(statusCode: 404, description: "Tipo de moto não encontrado")]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound($"Tipo de moto com ID {id} não encontrado.");

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
        [SwaggerOperation(Summary = "Busca tipo de moto por descrição", Description = "Retorna todos os tipos de moto que correspondem à descrição informada.")]
        [SwaggerResponse(statusCode: 200, description: "Tipos de moto encontrados", type: typeof(IEnumerable<TipoMotoDTO>))]
        [SwaggerResponse(statusCode: 404, description: "Nenhum tipo de moto encontrado com a descrição informada")]
        public async Task<IActionResult> GetByDescricao([FromQuery] string descricao)
        {
            var result = await _service.GetByDescricaoAsync(descricao);
            if (!result.Any())
                return NotFound($"Nenhum tipo de moto encontrado com descrição '{descricao}'.");

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra um novo tipo de moto", Description = "Cria um novo registro de tipo de moto no sistema.")]
        [SwaggerRequestExample(typeof(TipoMotoDTO), typeof(TipoMotoRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Tipo de moto criado com sucesso", type: typeof(TipoMotoDTO))]
        [SwaggerResponseExample(statusCode: 201, typeof(TipoMotoResponseSample))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos para criação do tipo de moto")]
        public async Task<IActionResult> Create([FromBody] TipoMotoDTO dto)
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
        [SwaggerOperation(Summary = "Atualiza um tipo de moto", Description = "Atualiza os dados de um tipo de moto já cadastrado.")]
        [SwaggerResponse(statusCode: 200, description: "Tipo de moto atualizado com sucesso", type: typeof(TipoMotoDTO))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponse(statusCode: 404, description: "Tipo de moto não encontrado")]
        public async Task<IActionResult> Update(long id, [FromBody] TipoMotoDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não bate com o objeto.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Tipo de moto com ID {id} não encontrado.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um tipo de moto", Description = "Remove um tipo de moto do sistema pelo seu ID.")]
        [SwaggerResponse(statusCode: 204, description: "Tipo de moto excluído com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Tipo de moto não encontrado")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Tipo de moto com ID {id} não encontrado.");

            return NoContent();
        }
    }
}
