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

            return Created($"/api/Categoria/{marca.Id}", new { id = marca.Id });

        }

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
