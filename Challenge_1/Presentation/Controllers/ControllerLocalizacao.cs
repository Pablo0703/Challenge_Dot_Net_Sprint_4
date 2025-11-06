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
    public class ControllerLocalizacaoMoto : ControllerBase
    {
        private readonly InterfaceLocalizacaoMoto _service;

        public ControllerLocalizacaoMoto(InterfaceLocalizacaoMoto service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todas as localizações de motos", Description = "Retorna todas as localizações cadastradas com suporte a paginação.")]
        [SwaggerResponse(statusCode: 200, description: "Lista de localizações retornada com sucesso", type: typeof(IEnumerable<LocalizacaoMotoDTO>))]
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
        [SwaggerOperation(Summary = "Busca localização por ID", Description = "Retorna uma localização específica pelo seu identificador único.")]
        [SwaggerResponse(statusCode: 200, description: "Localização encontrada", type: typeof(LocalizacaoMotoDTO))]
        [SwaggerResponse(statusCode: 404, description: "Localização não encontrada")]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound($"Localização de moto com ID {id} não encontrada.");

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
        [SwaggerOperation(Summary = "Busca localização por ID da moto", Description = "Retorna todas as localizações associadas a uma moto específica.")]
        [SwaggerResponse(statusCode: 200, description: "Localizações encontradas", type: typeof(IEnumerable<LocalizacaoMotoDTO>))]
        [SwaggerResponse(statusCode: 404, description: "Nenhuma localização encontrada para a moto informada")]
        public async Task<IActionResult> GetByIdMoto([FromQuery] long idMoto)
        {
            var result = await _service.GetByIdMotoAsync(idMoto);
            if (!result.Any())
                return NotFound($"Nenhuma localização encontrada para a moto com ID {idMoto}.");

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Registra nova localização da moto", Description = "Cria um novo registro de localização de uma moto em um pátio/zona.")]
        [SwaggerRequestExample(typeof(LocalizacaoMotoDTO), typeof(LocalizacaoMotoRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Localização criada com sucesso", type: typeof(LocalizacaoMotoDTO))]
        [SwaggerResponseExample(statusCode: 201, typeof(LocalizacaoMotoResponseSample))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos para criação da localização")]
        public async Task<IActionResult> Create([FromBody] LocalizacaoMotoDTO dto)
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
        [SwaggerOperation(Summary = "Atualiza uma localização de moto", Description = "Atualiza os dados de uma localização já cadastrada.")]
        [SwaggerResponse(statusCode: 200, description: "Localização atualizada com sucesso", type: typeof(LocalizacaoMotoDTO))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponse(statusCode: 404, description: "Localização não encontrada")]
        public async Task<IActionResult> Update(long id, [FromBody] LocalizacaoMotoDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não bate com o objeto.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Localização de moto com ID {id} não encontrada.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui uma localização de moto", Description = "Remove uma localização do sistema pelo seu ID.")]
        [SwaggerResponse(statusCode: 204, description: "Localização excluída com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Localização não encontrada")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Localização de moto com ID {id} não encontrada.");

            return NoContent();
        }
    }
}
