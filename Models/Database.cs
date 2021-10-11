using Microsoft.EntityFrameworkCore;

namespace TodoApp.Models;

public class Database : DbContext
{
    public Database(DbContextOptions options)
        :base(options)
    {
    }

    public DbSet<TodoList> Lists { get; set; } = null!;
    public DbSet<TodoItem> Items { get; set; } = null!;
}