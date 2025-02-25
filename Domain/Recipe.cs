using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Recipe : BaseEntity
{
    public string RecipeName { get; set; } = default!;
    
    public int ServingSize { get; set; }
    
    [MinLength(1)]
    [MaxLength(1000)]
    public string RecipeDescription { get; set; } = default!;
    
    public int TimeToServe { get; set; }
    
    public ICollection<RecipeProduct>? RecipeProducts { get; set; }
    
}