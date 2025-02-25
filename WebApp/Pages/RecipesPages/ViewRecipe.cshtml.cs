using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.RecipesPages;

public class ViewRecipe : PageModel
{
    
    private readonly DAL.AppDbContext _context;

    public ViewRecipe(DAL.AppDbContext context)
    {
        _context = context;
    }
    
    [BindProperty(SupportsGet = true)]
    public int UserId { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public int RecipeId { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public int Servings { get; set; }
    
    public double Multiplier { get; set; }
    public Recipe? Recipe { get; set; }
    
    public List<RecipeProduct>? RecipeProducts { get; set; }
    
    
    public async Task OnGetAsync()
    {
        Recipe = await _context.Recipes.Include(r => r.RecipeProducts)
            .FirstOrDefaultAsync(r => r.Id == RecipeId);

        Multiplier = Servings / (double)Recipe!.ServingSize;
        
        RecipeProducts = await _context.RecipeProducts.Where(r => r.RecipeId == RecipeId)
            .Include(rp => rp.Product)
            .ToListAsync();
    }
}