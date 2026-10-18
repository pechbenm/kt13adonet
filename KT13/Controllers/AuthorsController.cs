using KT13.Data;
using KT13.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KT13.Controllers;

public class AuthorsController : Controller
{
    private readonly LibraryContext _context;

    public AuthorsController(LibraryContext context) => _context = context;

    // READ (список)
    public async Task<IActionResult> Index() =>
        View(await _context.Authors.Include(a => a.Books)
            .OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync());

    // READ (один автор + его книги)
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
        return author == null ? NotFound() : View(author);
    }

    // CREATE
    public IActionResult Create() => View(new Author());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,BirthDate")] Author author)
    {
        if (!ModelState.IsValid) return View(author);
        _context.Add(author);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // UPDATE
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var author = await _context.Authors.FindAsync(id);
        return author == null ? NotFound() : View(author);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,BirthDate")] Author author)
    {
        if (id != author.Id) return NotFound();
        if (!ModelState.IsValid) return View(author);

        try
        {
            _context.Update(author);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Authors.AnyAsync(a => a.Id == id)) return NotFound();
            throw;
        }
        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
        return author == null ? NotFound() : View(author);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author != null)
        {
            // Книги автора удалятся каскадно (OnDelete(DeleteBehavior.Cascade))
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
