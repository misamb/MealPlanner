using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.ProductManagement
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserProduct UserProduct { get; set; } = default!;
        
        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? UserProductId)
        {
            if (UserProductId == null)
            {
                return NotFound();
            }

            var userproduct = await _context.UserProducts
                .Include(up => up.Product)
                .FirstOrDefaultAsync(m => m.Id == UserProductId);

            if (userproduct is not null)
            {
                UserProduct = userproduct;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? UserProductId, int? UserId)
        {
            if (UserProductId == null)
            {
                return NotFound();
            }

            var userproduct = await _context.UserProducts.FindAsync(UserProductId);
            if (userproduct != null)
            {
                UserProduct = userproduct;
                _context.UserProducts.Remove(UserProduct);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new {UserId});
        }
    }
}
