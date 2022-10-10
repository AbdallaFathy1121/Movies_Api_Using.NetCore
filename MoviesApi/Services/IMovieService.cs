namespace MoviesApi.Services
{
    public interface IMovieService
    {
        object GetAllMovies();
        object GetMovieByGenreId(byte genreId);
        Task<Movie> AddAsync(Movie movie);
        Task<Movie> DeleteAsync(Movie movie);
    }
}
