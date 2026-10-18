using KT13.Models;
using Microsoft.EntityFrameworkCore;

namespace KT13.Data;

public class LibraryContext : DbContext
{
    public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("Authors");
            entity.HasKey(a => a.Id);

            entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);
            entity.Ignore(a => a.FullName);

            entity.HasData(
                new Author { Id = 1, FirstName = "Лев", LastName = "Толстой", BirthDate = new DateTime(1828, 9, 9) },
                new Author { Id = 2, FirstName = "Фёдор", LastName = "Достоевский", BirthDate = new DateTime(1821, 11, 11) });
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("Books");
            entity.HasKey(b => b.Id);

            entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
            entity.Property(b => b.Genre).HasMaxLength(100);
            entity.Property(b => b.PublishedYear).IsRequired();
            entity.HasIndex(b => b.Title);

            // Один автор — много книг; при удалении автора его книги удаляются каскадно
            entity.HasOne(b => b.Author)
                  .WithMany(a => a.Books)
                  .HasForeignKey(b => b.AuthorId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasData(
                new Book { Id = 1, Title = "Война и мир", PublishedYear = 1869, Genre = "Роман", AuthorId = 1 },
                new Book { Id = 2, Title = "Анна Каренина", PublishedYear = 1877, Genre = "Роман", AuthorId = 1 },
                new Book { Id = 3, Title = "Преступление и наказание", PublishedYear = 1866, Genre = "Роман", AuthorId = 2 });
        });
    }
}
