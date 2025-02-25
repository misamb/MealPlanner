using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class UserHome : PageModel
{
    private readonly DAL.AppDbContext _context;

    public UserHome(DAL.AppDbContext context)
    {
        _context = context;
    }
    
    [BindProperty(SupportsGet = true)]
    public int UserId { get; set; }
    
    public List<UserProduct>? ProductsList { get; set; }
    
    public List<Recipe>? AvailableRecipesList { get; set; }
    
    public async Task OnGetAsync()
    {
        ProductsList = await _context.UserProducts
            .Where(up => up.UserId == UserId)
            .Include(up => up.Product).ToListAsync();

        AvailableRecipesList = await GetCookableRecipesAsync(UserId);
    }
    
    public async Task<List<Recipe>> GetCookableRecipesAsync(int userId)
    {
        // Get the user's products with their amounts
        var userProducts = await _context.UserProducts
            .Where(up => up.UserId == userId)
            .ToListAsync();

        // Get all recipes
        var recipes = await _context.Recipes
            .Include(r => r.RecipeProducts)
            .ToListAsync();

        // Filter recipes that can be cooked with the available products and amounts
        var cookableRecipes = recipes
            .Where(r => r.RecipeProducts.All(rp =>
            {
                Console.WriteLine(r.RecipeName);
                var userProduct = userProducts.FirstOrDefault(up => up.ProductId == rp.ProductId);
                Console.WriteLine(userProduct?.Product?.ProductName);
                Console.WriteLine(userProduct != null && userProduct.ProductAmount >= rp.ProductAmount);
                return userProduct != null && userProduct.ProductAmount >= rp.ProductAmount;
            }))
            .ToList();

        return cookableRecipes;
    }
}