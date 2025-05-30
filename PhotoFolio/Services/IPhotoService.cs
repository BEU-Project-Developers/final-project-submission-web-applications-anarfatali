using PhotoFolio.Models;

namespace PhotoFolio.Services;

public interface IPhotoService
{
    Task<List<Photo>> GetAllPhotosAsync();
    Task<List<Photo>> GetPhotosByCategoryAsync(string? category);
    Task<Photo> GetPhotoByIdAsync(int id);
    Task AddPhotoAsync(Photo photo);
    Task DeletePhotoAsync(int id);
}
