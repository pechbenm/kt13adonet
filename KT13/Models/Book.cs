using System.ComponentModel.DataAnnotations;

namespace KT13.Models;

public class Book
{
    public int Id { get; set; }

    [Display(Name = "Название")]
    [Required(ErrorMessage = "Укажите название")]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Год издания")]
    [Range(1, 2100, ErrorMessage = "Некорректный год")]
    public int PublishedYear { get; set; }

    [Display(Name = "Жанр")]
    [StringLength(100)]
    public string? Genre { get; set; }

    // Внешний ключ и навигационное свойство к автору
    [Display(Name = "Автор")]
    public int AuthorId { get; set; }

    // nullable, чтобы MVC не требовал заполнения Author при отправке формы
    public Author? Author { get; set; }
}
