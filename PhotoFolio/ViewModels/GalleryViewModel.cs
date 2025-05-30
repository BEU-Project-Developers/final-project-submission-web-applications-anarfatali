using PhotoFolio.Models;

namespace PhotoFolio.ViewModels;

public class GalleryViewModel
{
    public IEnumerable<Category> Categories { get; set; } = new List<Category>
    {
        new Category { Id = 1, Name = "Nature" },
        new Category { Id = 2, Name = "People" },
        new Category { Id = 3, Name = "Architecture" },
        new Category { Id = 4, Name = "Animals" },
        new Category { Id = 5, Name = "Sports" },
        new Category { Id = 6, Name = "Travel" },
    };

    public string? SelectedCategory { get; set; }

    public IEnumerable<Photo> Photos { get; set; } = new List<Photo>();
}
