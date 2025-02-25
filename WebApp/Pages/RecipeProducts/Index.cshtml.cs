using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages_RecipeProducts
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<RecipeProduct> RecipeProduct { get;set; } = default!;

        public async Task OnGetAsync()
        {
            RecipeProduct = await _context.RecipeProducts
                .Include(r => r.Product)
                .Include(r => r.User).ToListAsync();
        }
    }
}
