using Azure;
using HomeManagerApi.Data;
using HomeManagerApi.Data.Dtos;
using HomeManagerApi.Enums;
using HomeManagerApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult AddCategoria([FromBody] CreateCategoriaDto categoriaDto)
        {
            Categoria categoria = new Categoria 
            { 
                Nome = categoriaDto.Nome,
                Grupo = categoriaDto.Grupo
            };

           _context.Categorias.Add(categoria);
            _context.SaveChanges();
           return CreatedAtAction(nameof(GetByCategoria), new { id = categoria.Id} ,categoria);
        }

        [HttpGet]
        public IEnumerable<ReadCategoriaDto> GetAllCategorias([FromQuery]int skip = 0, [FromQuery]int take = 10)
        {
           //return _context.Categorias.Skip(skip).Take(take);

            var categorias = _context.Categorias
                 .Skip(skip)
                 .Take(take)
                 .Select(categoria => new ReadCategoriaDto
                 {
                     Id = categoria.Id,
                     Nome = categoria.Nome,
                     Grupo = categoria.Grupo
                 })
                 .ToList();

            return categorias;
        }

        [HttpGet("{id}")]
        public IActionResult GetByCategoria(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.Id == id);
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
        public IActionResult UpdateCategoria(int id, [FromBody] UpdateCategoriaDto categoriaDto)
        {
            var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.Id == id);
            
            if (categoria == null) return NotFound();
            
            categoria.Nome = categoriaDto.Nome;
            categoria.Grupo = categoriaDto.Grupo;

            _context.SaveChanges();

            return NoContent();

        }

        [HttpPatch("{id}")]
        public IActionResult PartialUpdateCategoria(int id, [FromBody] JsonPatchDocument<UpdateCategoriaDto> patch)
        {
            var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.Id == id);

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

            _context.SaveChanges();

            return NoContent();

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategoria(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(categoria => categoria.Id == id);

            if (categoria == null) return NotFound();

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
