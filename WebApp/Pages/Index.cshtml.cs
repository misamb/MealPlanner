using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    
    private readonly DAL.AppDbContext _context;

    public IndexModel(DAL.AppDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public string? Error { get; set; }

    [BindProperty]
    public string? UserName { get; set; }

    public void OnGet()
    {
    }
    
    public IActionResult OnPost()
    {
        UserName = UserName?.Trim();
        
        
        if (!string.IsNullOrWhiteSpace(UserName))
        {
            var User = _context.Users.FirstOrDefault(u => u.Username == UserName);
            if (User == default)
            {
                User = new User()
                {
                    Username = UserName
                };
                _context.Users.Add(User);
                _context.SaveChanges();
            }
            return RedirectToPage("./UserHome", new { UserId = User.Id });
        }
        
        Error = "Please enter a username.";
        
        return Page();
    }
}