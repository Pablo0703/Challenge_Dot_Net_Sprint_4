using Application.Interfaces;
using Challenge_1.Doc.Samples;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ControllerEndereco : ControllerBase
    {
        private readonly InterfaceEndereco _service;

        public ControllerEndereco(InterfaceEndereco service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os endereços", Description = "Retorna todos os endereços cadastrados com suporte a paginação.")]
        [SwaggerResponse(statusCode: 200, description: "Lista de endereços retornada com sucesso", type: typeof(IEnumerable<EnderecoDTO>))]
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
        [SwaggerOperation(Summary = "Busca endereço por ID", Description = "Retorna um endereço específico pelo seu identificador único.")]
        [SwaggerResponse(statusCode: 200, description: "Endereço encontrado", type: typeof(EnderecoDTO))]
        [SwaggerResponse(statusCode: 404, description: "Endereço não encontrado")]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound($"Endereço com ID {id} não encontrado.");

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

        [HttpGet("porCidade")]
        [SwaggerOperation(Summary = "Busca endereço por cidade", Description = "Retorna todos os endereços que correspondem à cidade informada.")]
        [SwaggerResponse(statusCode: 200, description: "Endereços encontrados", type: typeof(IEnumerable<EnderecoDTO>))]
        [SwaggerResponse(statusCode: 404, description: "Nenhum endereço encontrado para a cidade informada")]
        public async Task<IActionResult> GetByCidade([FromQuery] string cidade)
        {
            var result = await _service.GetByCidadeAsync(cidade);
            if (!result.Any())
                return NotFound($"Nenhum endereço encontrado para a cidade '{cidade}'.");

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra um novo endereço", Description = "Cria um novo registro de endereço no sistema.")]
        [SwaggerRequestExample(typeof(EnderecoDTO), typeof(EnderecoRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Endereço criado com sucesso", type: typeof(EnderecoDTO))]
        [SwaggerResponseExample(statusCode: 201, typeof(EnderecoResponseSample))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos para criação do endereço")]
        public async Task<IActionResult> Create([FromBody] EnderecoDTO dto)
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
        [SwaggerOperation(Summary = "Atualiza um endereço", Description = "Atualiza os dados de um endereço já cadastrado.")]
        [SwaggerResponse(statusCode: 200, description: "Endereço atualizado com sucesso", type: typeof(EnderecoDTO))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponse(statusCode: 404, description: "Endereço não encontrado")]
        public async Task<IActionResult> Update(long id, [FromBody] EnderecoDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não bate com o objeto.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Endereço com ID {id} não encontrado.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize]
        [SwaggerOperation(Summary = "Exclui um endereço", Description = "Remove um endereço do sistema pelo seu ID.")]
        [SwaggerResponse(statusCode: 204, description: "Endereço excluído com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Endereço não encontrado")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Endereço com ID {id} não encontrado.");

            return NoContent();
        }
    }
}
