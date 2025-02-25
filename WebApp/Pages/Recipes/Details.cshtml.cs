using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_Recipes
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public Recipe Recipe { get; set; } = default!;
        
        public List<RecipeProduct>? RecipeProducts { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FirstOrDefaultAsync(m => m.Id == id);
            
            RecipeProducts = await _context.RecipeProducts.Where(r => r.RecipeId == id)
                .Include(rp => rp.Product)
                .ToListAsync();

            if (recipe is not null)
            {
                Recipe = recipe;

                return Page();
            }

            return NotFound();
        }
    }
}
