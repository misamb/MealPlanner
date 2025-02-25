
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<Recipe> Recipes { get; set; } = default!;
    public DbSet<RecipeProduct> RecipeProducts  { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<UserProduct> UserProducts { get; set; } = default!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
}