using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Product : BaseEntity
{
    [MinLength(1)]
    [MaxLength(128)]
    public string ProductName { get; set; } = default!;
    
    [MinLength(1)]
    [MaxLength(10)]
    public string ProductMeasurment { get; set; } = default!;
    
    public ICollection<UserProduct>? UserProducts { get; set; }
    
    public ICollection<RecipeProduct>? RecipeProducts { get; set; }
}