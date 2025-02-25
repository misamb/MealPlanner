namespace Domain;

public class UserProduct : BaseEntity
{
    public int ProductAmount { get; set; }
    
    public int ProductId { get; set; }
    
    public Product? Product { get; set; }
    
    public int UserId { get; set; }
    
    public User? User { get; set; }
}