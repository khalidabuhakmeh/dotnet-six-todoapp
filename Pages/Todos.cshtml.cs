using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Pages;

public class Todos : PageModel
{
    private readonly Database _database;

    [BindProperty] public int? Id { get; set; }
    [BindProperty] public int ListId { get; set; }

    public Todos(Database database)
    {
        _database = database;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost([FromForm] TodoItemModel request)
    {
        var list = await _database
            .Lists.Include(l => l.Items)
            .FirstOrDefaultAsync(l => l.Id == ListId);

        if (list == null) return NotFound();

        var item = new TodoItem {Text = request.Text, IsDone = request.IsDone};
        
        list.Items.Add(item);
        await _database.SaveChangesAsync();

        TempData[nameof(Lists.AddedTaskText)] = item.Text;
        return RedirectToPage(nameof(Lists), new {id = list.Id});
    }

    /// <summary>
    /// HTMX Powered
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPostDelete()
    {
        var item = await _database.Items.Where(i => i.ListId == ListId && i.Id == Id).FirstOrDefaultAsync();
        if (item is not null)
        {
            _database.Items.Remove(item);
            await _database.SaveChangesAsync();
        }

        return StatusCode(Status202Accepted);
    }

    public async Task<IActionResult> OnPostUpdate([FromForm] bool isDone)
    {
        var item = await _database.Items.Where(i => i.ListId == ListId && i.Id == Id).FirstOrDefaultAsync();
        if (item is not null)
        {
            item.IsDone = isDone;
            item.LastUpdatedAt = DateTime.UtcNow;
            await _database.SaveChangesAsync();
        }

        return Partial("_TodoRow", item);
    }
}

public class TodoItemModel
{
    [Required] public string Text { get; set; } = string.Empty;
    public bool IsDone { get; set; }
}

public class TodoItemViewModel : TodoItemModel
{
    public int ListId { get; set; }
}