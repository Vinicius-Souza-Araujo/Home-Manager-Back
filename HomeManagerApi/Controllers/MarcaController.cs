using HomeManagerApi.Data;
using HomeManagerApi.Data.Dtos;
using HomeManagerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaController : ControllerBase
    {
        private HomeManagerContext _context;

        public MarcaController(HomeManagerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém uma lista de marcas.
        /// </summary>
        /// <param name="skip">Número de registros para pular.</param>
        /// <param name="take">Número de registros para retornar.</param>
        /// <returns>Uma lista de <see cref="ReadMarcaDto"/> contendo as marcas.</returns>
        /// <response code="200">Caso a operação seja bem-sucedida.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<ReadMarcaDto>> GetAllMarcasAsync([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var marcas = await _context.Marcas.Skip(skip).Take(take).Select(marca => new ReadMarcaDto
            {
                Id = marca.Id,
                Nome = marca.Nome,
                Grupo = marca.Grupo
            })
            .ToListAsync();

            return marcas;
        }


        /// <summary>
        /// Obtém uma marca pelo seu ID.
        /// </summary>
        /// <param name="id">ID da marca.</param>
        /// <returns>Um objeto <see cref="IActionResult"/> contendo a marca.</returns>
        /// <response code="200">Caso a operação seja bem-sucedida.</response>
        /// <response code="404">Caso a marca não seja encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByMarcaAsync(int id)
        {
            var marca = await _context.Marcas.FirstOrDefaultAsync(marca => marca.Id == id);

            if (marca == null) return NotFound();

            var marcaDto = new ReadMarcaDto
            {
                Id = marca.Id,
                Nome = marca.Nome,
                Grupo = marca.Grupo
            };

            return Ok(marcaDto);
        }

        /// <summary>
        /// Adiciona uma nova marca.
        /// </summary>
        /// <param name="marcaDto">Objeto com os campos necessários para a criação de uma marca.</param>
        /// <returns>Um objeto <see cref="IActionResult"/> indicando o resultado da operação.</returns>
        /// <response code="201">Caso a inserção seja feita com sucesso.</response>
        /// <response code="400">Dados de entrada inválidos.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMarcaAsync([FromBody] CreateMarcaDto marcaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Marca marca = new Marca
            {
                Nome = marcaDto.Nome,
                Grupo = marcaDto.Grupo
            };

            await _context.Marcas.AddAsync(marca);
            await _context.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetByMarcaAsync), new { id = marca.Id }, marca);

            return Created($"/api/Marca/{marca.Id}", new { id = marca.Id });

        }

        /// <summary>
        /// Atualiza uma marca existente.
        /// </summary>
        /// <param name="id">ID da marca a ser atualizada.</param>
        /// <param name="marcaDto">Objeto com os campos necessários para a atualização da marca.</param>
        /// <returns>Um objeto <see cref="IActionResult"/> indicando o resultado da operação.</returns>
        /// <response code="204">Caso a atualização seja bem-sucedida.</response>
        /// <response code="404">Caso a marca não seja encontrada.</response>
        /// <response code="400">Dados de entrada inválidos.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMarcaAsync(int id, [FromBody] UpdateMarcaDto marcaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var marca = await _context.Marcas.FirstOrDefaultAsync(marca => marca.Id == id);

            if (marca == null) return NotFound();

            marca.Nome = marcaDto.Nome;
            marca.Grupo = marcaDto.Grupo;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        /// <summary>
        /// Atualiza parcialmente uma marca existente.
        /// </summary>
        /// <param name="id">ID da marca a ser atualizada.</param>
        /// <param name="patch">Objeto <see cref="JsonPatchDocument"/> contendo as alterações a serem aplicadas.</param>
        /// <returns>Um objeto <see cref="IActionResult"/> indicando o resultado da operação.</returns>
        /// <response code="204">Caso a atualização seja bem-sucedida.</response>
        /// <response code="404">Caso a marca não seja encontrada.</response>
        /// <response code="400">Dados de entrada inválidos.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PartialUpdateMarcaAsync(int id, [FromBody] JsonPatchDocument<UpdateMarcaDto> patch)
        {

            var marca = await _context.Marcas.FirstOrDefaultAsync(marca => marca.Id == id);

            if (marca == null) return NotFound();

            var marcaParaAtualizar = new UpdateMarcaDto
            {
                Nome = marca.Nome,
                Grupo = marca.Grupo
            };

            patch.ApplyTo(marcaParaAtualizar, ModelState);

            if (!TryValidateModel(marcaParaAtualizar))
            {
                return ValidationProblem(ModelState);
            }

            marca.Nome = marcaParaAtualizar.Nome;
            marca.Grupo = marcaParaAtualizar.Grupo;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        /// <summary>
        /// Deleta uma marca existente.
        /// </summary>
        /// <param name="id">ID da marca a ser deletada.</param>
        /// <returns>Um objeto <see cref="IActionResult"/> indicando o resultado da operação.</returns>
        /// <response code="204">Caso a exclusão seja bem-sucedida.</response>
        /// <response code="404">Caso a marca não seja encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMarcaAsync(int id)
        {
            var marca = await _context.Marcas.FirstOrDefaultAsync(marca => marca.Id == id);

            if (marca == null) return NotFound();

            _context.Marcas.Remove(marca);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
