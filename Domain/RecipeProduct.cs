namespace Domain;

public class RecipeProduct : BaseEntity
{
    public int ProductAmount { get; set; }
    
    
    public int ProductId { get; set; }
    
    public Product? Product { get; set; }
    
    public int RecipeId { get; set; }
    
    public Recipe? User { get; set; }
}