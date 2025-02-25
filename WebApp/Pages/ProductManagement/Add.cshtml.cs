using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Pages.ProductManagement;

public class Add : PageModel
{
    
    private readonly DAL.AppDbContext _context;

    public Add(DAL.AppDbContext context)
    {
        _context = context;
    }
    
    [BindProperty]
    public UserProduct UserProduct { get; set; } = default!;
    
    [BindProperty(SupportsGet = true)]
    public int UserId { get; set; }
    
    public SelectList ProductSelectList { get; set; } = default!;
    
    public void OnGet()
    {
        ProductSelectList = new SelectList(_context.Products.ToList(), "Id", "ProductName");
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.UserProducts.Add(UserProduct);
        await _context.SaveChangesAsync();

        return RedirectToPage("./Index", new {UserId});
    }
}