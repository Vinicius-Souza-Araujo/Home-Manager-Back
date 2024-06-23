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
        /// <response code="400">Dados de entrada inválidos.</response>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCategoriaAsync([FromBody] CreateCategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Categoria categoria = new Categoria 
            { 
                Nome = categoriaDto.Nome,
                Grupo = categoriaDto.Grupo
            };

           await _context.Categorias.AddAsync(categoria);
           await _context.SaveChangesAsync();

           return CreatedAtAction(nameof(GetByCategoriaAsync), new { id = categoria.Id} ,categoria);
        }

        /// <summary>
        /// Obtém uma lista de categorias.
        /// </summary>
        /// <param name="skip">Número de registros para pular.</param>
        /// <param name="take">Número de registros para retornar.</param>
        /// <returns>Uma lista de <see cref="ReadCategoriaDto"/> contendo as categorias.</returns>
        /// <response code="200">Caso a operação seja bem-sucedida.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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


        /// <summary>
        /// Obtém uma categoria pelo seu ID.
        /// </summary>
        /// <param name="id">ID da categoria.</param>
        /// <returns>Um objeto <see cref="IActionResult"/> contendo a categoria.</returns>
        /// <response code="200">Caso a operação seja bem-sucedida.</response>
        /// <response code="404">Caso a categoria não seja encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Atualiza uma categoria existente.
        /// </summary>
        /// <param name="id">ID da categoria a ser atualizada.</param>
        /// <param name="categoriaDto">Objeto com os campos necessários para a atualização da categoria.</param>
        /// <returns>Um objeto <see cref="IActionResult"/> indicando o resultado da operação.</returns>
        /// <response code="204">Caso a atualização seja bem-sucedida.</response>
        /// <response code="404">Caso a categoria não seja encontrada.</response>
        /// <response code="400">Dados de entrada inválidos.</response>

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateCategoriaAsync(int id, [FromBody] UpdateCategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoria = await _context.Categorias.FirstOrDefaultAsync(categoria => categoria.Id == id);
            
            if (categoria == null) return NotFound();
            
            categoria.Nome = categoriaDto.Nome;
            categoria.Grupo = categoriaDto.Grupo;

            await _context.SaveChangesAsync();

            return NoContent();

        }

        /// <summary>
        /// Atualiza parcialmente uma categoria existente.
        /// </summary>
        /// <param name="id">ID da categoria a ser atualizada.</param>
        /// <param name="patch">Objeto <see cref="JsonPatchDocument"/> contendo as alterações a serem aplicadas.</param>
        /// <returns>Um objeto <see cref="IActionResult"/> indicando o resultado da operação.</returns>
        /// <response code="204">Caso a atualização seja bem-sucedida.</response>
        /// <response code="404">Caso a categoria não seja encontrada.</response>
        /// <response code="400">Dados de entrada inválidos.</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Deleta uma categoria existente.
        /// </summary>
        /// <param name="id">ID da categoria a ser deletada.</param>
        /// <returns>Um objeto <see cref="IActionResult"/> indicando o resultado da operação.</returns>
        /// <response code="204">Caso a exclusão seja bem-sucedida.</response>
        /// <response code="404">Caso a categoria não seja encontrada.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
