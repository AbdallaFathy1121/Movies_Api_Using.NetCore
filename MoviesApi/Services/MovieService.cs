namespace MoviesApi.Services
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;
        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }

        public object GetAllMovies()
        {
            var query = from m in _context.Movies
                        .OrderByDescending(m => m.Rate)
                        select new
                        {
                            m.Id,
                            m.Title,
                            m.Year,
                            m.Rate,
                            m.StoryLine,
                            m.Genre,
                            m.Poster
                        };

            return query;
        }

        public object GetMovieByGenreId(byte genreId)
        {
            var query = from m in _context.Movies
                        .OrderByDescending(m => m.Rate)
                        where m.GenreId == genreId
                        select new
                        {
                            m.Id,
                            m.Title,
                            m.Year,
                            m.Rate,
                            m.StoryLine,
                            m.Genre,
                            m.Poster
                        };

            return query;
        }

        public async Task<Movie> AddAsync(Movie movie)
        {
            await _context.AddAsync(movie);
            _context.SaveChanges();
            return movie;
        }

        public async Task<Movie> DeleteAsync(Movie movie)
        {
            _context.Remove(movie);
            await _context.SaveChangesAsync();
            return movie;
        }
    }
}
