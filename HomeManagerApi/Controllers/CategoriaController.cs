using Azure;
using HomeManagerApi.Data;
using HomeManagerApi.Data.Dtos;
using HomeManagerApi.Enums;
using HomeManagerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HomeManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {

        private HomeManagerContext _context;

        public CategoriaController(HomeManagerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adiciona uma nova categoria.
        /// </summary>
        /// <param name="categoriaDto">Objeto com os campos necessários para a criação de uma categoria.</param>
        /// <returns>Um objeto <see cref="IActionResult"/> indicando o resultado da operação.</returns>
        /// <response code="201">Caso a inserção seja feita com sucesso.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> AddCategoriaAsync([FromBody] CreateCategoriaDto categoriaDto)
        {
            Categoria categoria = new Categoria 
            { 
                Nome = categoriaDto.Nome,
                Grupo = categoriaDto.Grupo
            };

           await _context.Categorias.AddAsync(categoria);
           await _context.SaveChangesAsync();

           return CreatedAtAction(nameof(GetByCategoriaAsync), new { id = categoria.Id} ,categoria);
        }

        [HttpGet]
        public async Task<IEnumerable<ReadCategoriaDto>> GetAllCategoriasAsync([FromQuery]int skip = 0, [FromQuery]int take = 10)
        {
           //return _context.Categorias.Skip(skip).Take(take);

            var categorias = await _context.Categorias
                 .Skip(skip)
                 .Take(take)
                 .Select(categoria => new ReadCategoriaDto
                 {
                     Id = categoria.Id,
                     Nome = categoria.Nome,
                     Grupo = categoria.Grupo
                 })
                 .ToListAsync();

            return categorias;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByCategoriaAsync(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == id);
            if (categoria == null) return NotFound();

            var categoriaDto = new ReadCategoriaDto
            {
                Id = categoria.Id,
                Nome = categoria.Nome,
                Grupo = categoria.Grupo
            };

            return Ok(categoriaDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategoriaAsync(int id, [FromBody] UpdateCategoriaDto categoriaDto)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == id);
            
            if (categoria == null) return NotFound();
            
            categoria.Nome = categoriaDto.Nome;
            categoria.Grupo = categoriaDto.Grupo;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialUpdateCategoriaAsync(int id, [FromBody] JsonPatchDocument<UpdateCategoriaDto> patch)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == id);

            if (categoria == null) return NotFound();

            var categoriaParaAtualizar = new UpdateCategoriaDto
            {
                Nome = categoria.Nome,
                Grupo = categoria.Grupo
            };

            patch.ApplyTo(categoriaParaAtualizar, ModelState);

            if (!TryValidateModel(categoriaParaAtualizar))
            {
                return ValidationProblem(ModelState);
            }

            categoria.Nome = categoriaParaAtualizar.Nome;
            categoria.Grupo = categoriaParaAtualizar.Grupo;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoriaAsync(int id)
        {
            var categoria = await _context.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == id);

            if (categoria == null) return NotFound();

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
