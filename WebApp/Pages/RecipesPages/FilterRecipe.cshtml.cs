using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WebApp.Pages.RecipesPages;

public class FilterRecipe : PageModel
{
    private readonly DAL.AppDbContext _context;

    public FilterRecipe(DAL.AppDbContext context)
    {
        _context = context;
    }
    
    [BindProperty(SupportsGet = true)]
    public int UserId { get; set; }
    
    
    public List<Product>? IncludedProducts { get; set; }

    
    public List<Product>? ExcludedProducts { get; set; }
    
    
    [BindProperty]
    public List<ProductSelect>? IncludeProductSelectList { get; set; }
    
    [BindProperty]
    public List<ProductSelect>? ExcludeProductSelectList { get; set; }
    
    [BindProperty]
    public int MaxMinutes { get; set; }
    
    [BindProperty]
    public int Servings { get; set; }
    
    public async Task OnGetAsync()
    {
        
        
        var userProductList = await _context.UserProducts
            .Where(up => up.UserId == UserId)
            .Include(up => up.Product)
            .Select(up => up.Product)
            .ToListAsync();

        IncludeProductSelectList =
            userProductList.Select(p => new ProductSelect { Product = p, IsSelected = false }).ToList();
        
        ExcludeProductSelectList = 
            userProductList.Select(p => new ProductSelect { Product = p, IsSelected = false }).ToList();
    }


    public async Task<RedirectToPageResult> OnPostAsync()
    {
        IncludedProducts = IncludeProductSelectList.Where(vp => vp.IsSelected)
            .Select(vp => vp.Product!).ToList();
        
        ExcludedProducts = ExcludeProductSelectList.Where(vp => vp.IsSelected)
            .Select(vp => vp.Product!).ToList();
        
        var includedIds = IncludedProducts.Select(p => p.Id).ToList();
        
        var excludedIds = ExcludedProducts.Select(p => p.Id).ToList();
        
        var userProducts = await _context.UserProducts
            .Where(up => up.UserId == UserId)
            .ToListAsync();

        
        var recipes = await _context.Recipes
            .Include(r => r.RecipeProducts)
            .ToListAsync();
        
        // Filter recipes to include only those that have at least one included product
        var okRecipesList = recipes.Where(r => r.RecipeProducts
            .Any(rp => includedIds.Contains(rp.ProductId))).ToList();
        
        //remove excluded
        okRecipesList = okRecipesList.Where(r => !r.RecipeProducts
            .Any(rp => excludedIds.Contains(rp.ProductId))).ToList();
        
        //remove over max minutes
        okRecipesList = okRecipesList.Where(r => r.TimeToServe <= MaxMinutes).ToList();
        
        var cookableRecipes = okRecipesList
            .Where(r => r.RecipeProducts.All(rp =>
            {
                var servingMultiplier = Servings / (double)r.ServingSize;
                var userProduct = userProducts.FirstOrDefault(up => up.ProductId == rp.ProductId);
                return userProduct != null && userProduct.ProductAmount >= rp.ProductAmount * servingMultiplier;
            }))
            .ToList();
        
        var cookableRecipeIds = cookableRecipes.Select(r => r.Id).ToList();
        
        var JsonRecipeIds = System.Text.Json.JsonSerializer.Serialize(cookableRecipeIds);
        
        return RedirectToPage("./RecipeList", new { RecipeIds = JsonRecipeIds, UserId, Servings});
        
    }
}