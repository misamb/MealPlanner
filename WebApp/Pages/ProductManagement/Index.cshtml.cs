using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.ProductManagement;

public class IndexModel : PageModel
{
    private readonly DAL.AppDbContext _context;

    public IndexModel(DAL.AppDbContext context)
    {
        _context = context;
    }
    
    [BindProperty(SupportsGet = true)]
    public int UserId { get; set; }

    public IList<UserProduct> UserProduct { get;set; } = default!;

    public async Task OnGetAsync()
    {
        UserProduct = await _context.UserProducts
            .Where(up => up.UserId == UserId)
            .Include(up => up.Product).ToListAsync();
    }
}
