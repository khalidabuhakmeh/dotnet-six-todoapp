using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly Database _database;
    
    [BindProperty]
    public string? Name { get; set; }

    public IndexModel(ILogger<IndexModel> logger, Database database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task OnGet()
    {
        Lists = await _database
            .Lists
            .Select(l =>
                new ListViewModel
                {
                    Id = l.Id,
                    Name = l.Name,
                    TotalItemsCount = l.Items.Count,
                    TotalItemsCompleted = l.Items.Count(i => i.IsDone),
                    CreatedAt = l.CreatedAt
                }).ToListAsync();
    }

    public async Task<IActionResult> OnPostNew()
    {
        var list = new TodoList {Name = Name ?? "Untitled List"};
        _database.Lists.Add(list);
        await _database.SaveChangesAsync();

        return RedirectToPage(nameof(Pages.Lists), new {id = list.Id});
    }

    public IEnumerable<ListViewModel> Lists { get; set; }
        = Enumerable.Empty<ListViewModel>();
}

public class ListViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int TotalItemsCompleted { get; set; }
    public int TotalItemsCount { get; set; }
    public DateTime CreatedAt { get; set; }

    public int Progress =>
        TotalItemsCount > 0
            ? (TotalItemsCompleted / TotalItemsCount) * 100
            : 0;
}