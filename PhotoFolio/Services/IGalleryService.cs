using PhotoFolio.Models;

namespace PhotoFolio.Services;

public interface IGalleryService
{
    
    Task<List<Photo>> GetGalleryItemsAsync();
}
