namespace TodoApp.Models;

public class TodoList
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<TodoItem> Items { get; set; } = new List<TodoItem>();
}