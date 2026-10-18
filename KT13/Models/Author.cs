using System.ComponentModel.DataAnnotations;

namespace KT13.Models;

// Атрибуты здесь — только для подписей и валидации в UI.
// Вся конфигурация БД (ключи, длины, связи, каскад) задана через Fluent API в LibraryContext.
public class Author
{
    public int Id { get; set; }

    [Display(Name = "Имя")]
    [Required(ErrorMessage = "Укажите имя")]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Display(Name = "Фамилия")]
    [Required(ErrorMessage = "Укажите фамилию")]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Дата рождения")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? BirthDate { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    // Навигационное свойство: один автор — много книг
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
