namespace PhotoFolio.Models;

public class Favorite
{
    
    public int PhotoId { get; set; }
    public Photo? Photo { get; set; }

    public string UserId { get; set; } = "";
    public ApplicationUser? User { get; set; }

    public DateTime FavoritedAt { get; set; }
}
