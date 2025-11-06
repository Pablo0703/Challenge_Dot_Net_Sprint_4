using Application.Interfaces;
using Challenge_1.Doc.Samples;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTOs;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Challenge_1.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ControllerPatio : ControllerBase
    {
        private readonly InterfacePatio _service;

        public ControllerPatio(InterfacePatio service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os pátios", Description = "Retorna todos os pátios cadastrados com suporte a paginação.")]
        [SwaggerResponse(statusCode: 200, description: "Lista de pátios retornada com sucesso", type: typeof(IEnumerable<PatioDTO>))]
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
        [SwaggerOperation(Summary = "Busca pátio por ID", Description = "Retorna um pátio específico pelo seu identificador único.")]
        [SwaggerResponse(statusCode: 200, description: "Pátio encontrado", type: typeof(PatioDTO))]
        [SwaggerResponse(statusCode: 404, description: "Pátio não encontrado")]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound($"Pátio com ID {id} não encontrado.");

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
        [SwaggerOperation(Summary = "Busca pátio por nome", Description = "Retorna todos os pátios que correspondem ao nome informado.")]
        [SwaggerResponse(statusCode: 200, description: "Pátios encontrados", type: typeof(IEnumerable<PatioDTO>))]
        [SwaggerResponse(statusCode: 404, description: "Nenhum pátio encontrado com o nome informado")]
        public async Task<IActionResult> GetByNome([FromQuery] string nome)
        {
            var result = await _service.GetByNomeAsync(nome);
            if (!result.Any())
                return NotFound($"Nenhum pátio encontrado com nome '{nome}'.");

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra um novo pátio", Description = "Cria um novo registro de pátio no sistema.")]
        [SwaggerRequestExample(typeof(PatioDTO), typeof(PatioRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Pátio criado com sucesso", type: typeof(PatioDTO))]
        [SwaggerResponseExample(statusCode: 201, typeof(PatioResponseSample))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos para criação do pátio")]
        public async Task<IActionResult> Create([FromBody] PatioDTO dto)

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
        [SwaggerOperation(Summary = "Atualiza um pátio", Description = "Atualiza os dados de um pátio já cadastrado.")]
        [SwaggerResponse(statusCode: 200, description: "Pátio atualizado com sucesso", type: typeof(PatioDTO))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponse(statusCode: 404, description: "Pátio não encontrado")]
        public async Task<IActionResult> Update(long id, [FromBody] PatioDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não bate com o objeto.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"Pátio com ID {id} não encontrado.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um pátio", Description = "Remove um pátio do sistema pelo seu ID.")]
        [SwaggerResponse(statusCode: 204, description: "Pátio excluído com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Pátio não encontrado")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"Pátio com ID {id} não encontrado.");

            return NoContent();
        }
    }
}
