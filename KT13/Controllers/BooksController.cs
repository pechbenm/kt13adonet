using KT13.Data;
using KT13.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KT13.Controllers;

public class BooksController : Controller
{
    private readonly LibraryContext _context;

    public BooksController(LibraryContext context) => _context = context;

    // READ
    public async Task<IActionResult> Index() =>
        View(await _context.Books.Include(b => b.Author).OrderBy(b => b.Title).ToListAsync());

    // CREATE (authorId — необязательно, чтобы заранее выбрать автора)
    public async Task<IActionResult> Create(int? authorId)
    {
        await LoadAuthors(authorId);
        return View(new Book { AuthorId = authorId ?? 0, PublishedYear = DateTime.Now.Year });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,PublishedYear,Genre,AuthorId")] Book book)
    {
        if (!await _context.Authors.AnyAsync(a => a.Id == book.AuthorId))
            ModelState.AddModelError(nameof(Book.AuthorId), "Выберите автора");

        if (!ModelState.IsValid)
        {
            await LoadAuthors(book.AuthorId);
            return View(book);
        }

        _context.Add(book);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // UPDATE
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var book = await _context.Books.FindAsync(id);
        if (book == null) return NotFound();
        await LoadAuthors(book.AuthorId);
        return View(book);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,PublishedYear,Genre,AuthorId")] Book book)
    {
        if (id != book.Id) return NotFound();

        if (!await _context.Authors.AnyAsync(a => a.Id == book.AuthorId))
            ModelState.AddModelError(nameof(Book.AuthorId), "Выберите автора");

        if (!ModelState.IsValid)
        {
            await LoadAuthors(book.AuthorId);
            return View(book);
        }

        try
        {
            _context.Update(book);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Books.AnyAsync(b => b.Id == id)) return NotFound();
            throw;
        }
        return RedirectToAction(nameof(Index));
    }

    // DELETE
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        return book == null ? NotFound() : View(book);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadAuthors(int? selected = null) =>
        ViewData["Authors"] = new SelectList(
            await _context.Authors.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ToListAsync(),
            "Id", "FullName", selected);
}
