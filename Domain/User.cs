using System.ComponentModel.DataAnnotations;

namespace Domain;

public class User : BaseEntity
{
    
    [MinLength(1)]
    [MaxLength(128)]
    public string Username { get; set; } = default!;

    public ICollection<UserProduct>? UserProducts { get; set; }
}