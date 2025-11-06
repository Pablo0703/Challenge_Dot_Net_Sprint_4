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
    public class ControllerStatusMoto : ControllerBase
    {
        private readonly InterfaceStatusMoto _service;

        public ControllerStatusMoto(InterfaceStatusMoto service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Lista todos os status de moto", Description = "Retorna todos os status cadastrados com suporte a paginação.")]
        [SwaggerResponse(statusCode: 200, description: "Lista de status retornada com sucesso", type: typeof(IEnumerable<StatusMotoDTO>))]
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
        [SwaggerOperation(Summary = "Busca status de moto por ID", Description = "Retorna um status específico pelo seu identificador único.")]
        [SwaggerResponse(statusCode: 200, description: "Status encontrado", type: typeof(StatusMotoDTO))]
        [SwaggerResponse(statusCode: 404, description: "Status não encontrado")]
        public async Task<IActionResult> GetById(long id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
                return NotFound($"StatusMoto com ID {id} não encontrado.");

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
        [SwaggerOperation(Summary = "Busca status de moto por descrição", Description = "Retorna todos os status que correspondem à descrição informada.")]
        [SwaggerResponse(statusCode: 200, description: "Status encontrados", type: typeof(IEnumerable<StatusMotoDTO>))]
        [SwaggerResponse(statusCode: 404, description: "Nenhum status encontrado com a descrição informada")]
        public async Task<IActionResult> GetByNome([FromQuery] string nome)
        {
            var result = await _service.GetByNomeAsync(nome);
            if (!result.Any())
                return NotFound($"Nenhum status de moto encontrado com nome '{nome}'.");

            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Cadastra um novo status de moto", Description = "Cria um novo registro de status no sistema.")]
        [SwaggerRequestExample(typeof(StatusMotoDTO), typeof(StatusMotoRequestSample))]
        [SwaggerResponse(statusCode: 201, description: "Status criado com sucesso", type: typeof(StatusMotoDTO))]
        [SwaggerResponseExample(statusCode: 201, typeof(StatusMotoResponseSample))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos para criação do status")]
        public async Task<IActionResult> Create([FromBody] StatusMotoDTO dto)
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
        [SwaggerOperation(Summary = "Atualiza um status de moto", Description = "Atualiza os dados de um status já cadastrado.")]
        [SwaggerResponse(statusCode: 200, description: "Status atualizado com sucesso", type: typeof(StatusMotoDTO))]
        [SwaggerResponse(statusCode: 400, description: "Dados inválidos")]
        [SwaggerResponse(statusCode: 404, description: "Status não encontrado")]
        public async Task<IActionResult> Update(long id, [FromBody] StatusMotoDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não bate com o objeto.");

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"StatusMoto com ID {id} não encontrado.");

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Exclui um status de moto", Description = "Remove um status do sistema pelo seu ID.")]
        [SwaggerResponse(statusCode: 204, description: "Status excluído com sucesso")]
        [SwaggerResponse(statusCode: 404, description: "Status não encontrado")]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"StatusMoto com ID {id} não encontrado.");

            return NoContent();
        }
    }
}
