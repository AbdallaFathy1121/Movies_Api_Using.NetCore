using Microsoft.AspNetCore.Http;
using MoviesApi.Models;
using MoviesApi.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IGenreService _genreService;
        public GenresController(ApplicationDbContext context, IGenreService genreService)
        {
            _context = context;
            _genreService = genreService;
        }

        [HttpGet]
        public IActionResult GetAllGenres()
        {
            return Ok(_genreService.GetAllGenres());
        }

        [HttpPost]
        public async Task<IActionResult> CreateGenreAsync(GenreDto dto)
        {
            var genre = await _genreService.GetByNameAsync(dto.Name);
            if (genre != null)
                return BadRequest("Name Of Genre is Already exists");

            Genre result = new() { Name = dto.Name };

            try
            {
                var res = await _genreService.AddAsync(result);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGenreAsync(byte id, [FromBody] GenreDto dto)
        {
            var genre = await _genreService.GetByIdAsync(id);
            if(genre == null)
                return NotFound($"No Genre was Found with ID: {id}");

            genre.Name = dto.Name;
            
            try
            {
                await _genreService.UpdateAsync(genre);
                return Ok(genre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenreAsync(byte id)
        {
            var genre = await _genreService.GetByIdAsync(id);
            if (genre == null)
                return NotFound($"No Genre was Found with ID: {id}");

            try
            {
                await _genreService.DeleteAsync(genre);
                return Ok(genre);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
