namespace MyRecipeBook.Domain.Entities;

public class EntityBase
{
    public long Id { get; set; }
    public bool Active { get; set; } =  true;
    public DateTime CreateOn { get; set; } = DateTime.UtcNow;
}