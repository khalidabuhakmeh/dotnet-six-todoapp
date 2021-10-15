using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Pages;

public class Lists : PageModel
{
    private readonly Database _database;

    [BindProperty(SupportsGet = true)] public int Id { get; set; }

    public TodoList List { get; set; } = null!;

    public IEnumerable<TodoItem> Todos =>
        List?.Items.ToList() ?? Enumerable.Empty<TodoItem>();

    [TempData]
    public string AddedTaskText { get; set; } = string.Empty;

    public Lists(Database database)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGet()
    {
        var list = await _database
            .Lists
            .Include(l => l.Items)
            .FirstOrDefaultAsync(l => l.Id == Id);

        if (list is null)
        {
            return NotFound();
        }

        List = list;

        return Request.IsHtmx()
            ? Partial("_Todos", this)
            : Page();
    }

    public async Task<IActionResult> OnPostDelete()
    {
        var list = await _database
            .Lists
            .Include(l => l.Items)
            .FirstOrDefaultAsync(l => l.Id == Id);

        if (list is not null)
        {
            // remove it
            _database.Items.RemoveRange(list.Items);
            _database.Lists.Remove(list);
            await _database.SaveChangesAsync();
        }

        return StatusCode(Status202Accepted);
    }
}