namespace MoviesApi.Services
{
    public interface IGenreService
    {
        object GetAllGenres();
        Task<Genre> GetByNameAsync(string name);
        Task<Genre> GetByIdAsync(byte id);
        Task<Genre> AddAsync(Genre genre);
        Task<Genre> DeleteAsync(Genre genre);
        Task<Genre> UpdateAsync(Genre genre);
    }
}
