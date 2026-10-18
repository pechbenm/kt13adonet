using KT13.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Создаёт БД при первом запуске. Для миграций замените на .Database.Migrate()
// (см. dotnet ef migrations add InitialCreate).
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<LibraryContext>().Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authors}/{action=Index}/{id?}");

app.Run();
