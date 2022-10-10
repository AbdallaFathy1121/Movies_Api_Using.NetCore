using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;
using MoviesApi.Services;
using System.Linq;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private new List<string> _allowedExtenstions = new() { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576; // 1MB

        private readonly ApplicationDbContext _context;
        private IMovieService _movieService;
        public MoviesController(ApplicationDbContext context, IMovieService movieService)
        {
            _context = context;
            _movieService = movieService;
        }

        [HttpGet]
        public IActionResult GetAllMovie()
        {
            try
            {
                var movies = _movieService.GetAllMovies();

                return Ok(movies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetMovieByGenreId/{genreId}")]
        public IActionResult GetMovieByGenreId(byte genreId)
        {
            var movies = _movieService.GetMovieByGenreId(genreId);

            if (movies == null)
                return NotFound($"No Movie was found with Genre ID: {genreId}");

            return Ok(movies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovieAsync([FromForm] MovieDto dto)
        {
            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB");

            bool isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest($"Invalid Genre With ID: {dto.GenreId}");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            Movie movie = new()
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Poster = dataStream.ToArray(),
                Rate = dto.Rate,
                StoryLine = dto.StoryLine,
                Year = dto.Year
            };

            try
            {
                await _movieService.AddAsync(movie);
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovieAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);    
            if(movie == null)
                return NotFound($"No Movie was found with ID: {id}");

            bool isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);
            if (!isValidGenre)
                return BadRequest($"Invalid Genre With ID: {dto.GenreId}");

            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);

            movie.Title = dto.Title;    
            movie.Year = dto.Year;
            movie.StoryLine = dto.StoryLine;
            movie.Rate = dto.Rate;
            movie.GenreId = dto.GenreId;
            movie.Poster = dataStream.ToArray();

            try
            {
                _context.SaveChanges();
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound($"No Movie was Found with ID: {id}");

            try
            {
                await _movieService.DeleteAsync(movie);
                return Ok(movie);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
