using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CelestineMovies.Data;
using CelestineMovies.Models;

namespace CelestineMovies.Controllers;

public class MoviesController : Controller
{
    private readonly CelestineMoviesContext _context;

    public MoviesController(CelestineMoviesContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string movieGenre, string searchString, int? searchYear)
    {
        if (_context.Movie == null)
        {
            return Problem("Entity set 'CelestineMoviesContext.Movie' is null.");
        }

        IQueryable<string> genreQuery = from m in _context.Movie
                                        orderby m.Genre
                                        select m.Genre;

        var movies = from m in _context.Movie
                     select m;

        if (!string.IsNullOrEmpty(searchString))
        {
            movies = movies.Where(s => s.Title!.Contains(searchString));
        }

        if (!string.IsNullOrEmpty(movieGenre))
        {
            movies = movies.Where(x => x.Genre == movieGenre);
        }

        if (searchYear.HasValue)
        {
            movies = movies.Where(x => x.ReleaseDate.Year >= searchYear.Value);
        }

        var movieGenreVM = new MovieGenreViewModel
        {
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
            Movies = await movies.ToListAsync()
        };

        return View(movieGenreVM);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return NotFound();

        return View(movie);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (ModelState.IsValid)
        {
            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(movie);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var movie = await _context.Movie.FindAsync(id);

        if (movie == null) return NotFound();

        return View(movie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (id != movie.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(movie);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var movie = await _context.Movie.FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return NotFound();

        return View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var movie = await _context.Movie.FindAsync(id);

        if (movie != null)
        {
            _context.Movie.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}