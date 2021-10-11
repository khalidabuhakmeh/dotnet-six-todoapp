namespace TodoApp.Models;

public class TodoItem
{
    public int Id { get; set; }
    public int ListId { get; set; }
    public TodoList List { get; set; } = null!;
    public string Text { get; set; } = string.Empty;
    public bool IsDone { get; set; }
    public int Order { get; set; } = int.MaxValue;
    public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
}