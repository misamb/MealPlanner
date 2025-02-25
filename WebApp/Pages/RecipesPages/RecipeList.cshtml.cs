using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.RecipesPages;

public class RecipeList : PageModel
{
    
    private readonly DAL.AppDbContext _context;

    public RecipeList(DAL.AppDbContext context)
    {
        _context = context;
    }
    
    [BindProperty(SupportsGet = true)]
    public int UserId { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public string? RecipeIds { get; set; }
    
    
    [BindProperty(SupportsGet = true)]
    public string? Servings { get; set; }
    
    public List<Recipe>? RecipesList { get; set; }
    
    public async Task OnGetAsync()
    {
        var recipeIdsList = System.Text.Json.JsonSerializer.Deserialize<List<int>>(RecipeIds);

        if (recipeIdsList != null)
        {
            RecipesList = await _context.Recipes.Where(r => recipeIdsList.Contains(r.Id))
                .ToListAsync();
        }
        
        
    }
}