using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages.ProductManagement
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
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
            if (userproduct == null)
            {
                return NotFound();
            }

            UserProduct = userproduct;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(UserProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProductExists(UserProduct.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new {UserId});
        }

        private bool UserProductExists(int id)
        {
            return _context.UserProducts.Any(e => e.Id == id);
        }
    }
}
