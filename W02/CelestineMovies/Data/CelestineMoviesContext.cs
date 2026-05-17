using Microsoft.EntityFrameworkCore;
using CelestineMovies.Models;

namespace CelestineMovies.Data;

public class CelestineMoviesContext : DbContext
{
    public CelestineMoviesContext(DbContextOptions<CelestineMoviesContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movie { get; set; } = default!;
}
