namespace MoviesApi.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;
        public GenreService(ApplicationDbContext context)
        {
            _context = context;
        }

        public object GetAllGenres()
        {
            var query = from g in _context.Genres
                        .OrderBy(g => g.Name)
                        select new
                        {
                            g.Id,
                            g.Name,
                            g.Movies
                        };
            
            return query;
        }

        public async Task<Genre> GetByNameAsync(string name)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Name == name);
            return genre;
        }

        public async Task<Genre> GetByIdAsync(byte id)
        {
            var genre = await _context.Genres.FindAsync(id);
            return genre;
        }

        public async Task<Genre> AddAsync(Genre genre)
        {
            await _context.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<Genre> UpdateAsync(Genre genre)
        {
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<Genre> DeleteAsync(Genre genre)
        {
            _context.Remove(genre);
            await _context.SaveChangesAsync();
            return genre;
        }
    }
}
