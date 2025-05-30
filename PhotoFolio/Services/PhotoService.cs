using Microsoft.EntityFrameworkCore;
using PhotoFolio.DATA;
using PhotoFolio.Models;

namespace PhotoFolio.Services;

public class PhotoService : IPhotoService
{
    private readonly ApplicationDbContext _context;

    public PhotoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Photo>> GetAllPhotosAsync()
    {
        return await _context.Photos.ToListAsync();
    }
    
    public async Task<List<Photo>> GetPhotosByCategoryAsync(string? categoryName)
    { 
        IQueryable<Photo> query = _context.Photos.Include(p => p.Category);
        if (!string.IsNullOrEmpty(categoryName) && categoryName != "All")
        {
            query = query.Where(p => p.Category != null && p.Category.Name == categoryName);
        }
        return await query.ToListAsync();
    }

    public async Task<Photo> GetPhotoByIdAsync(int id)
    {
        return await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddPhotoAsync(Photo photo)
    {
        _context.Photos.Add(photo);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePhotoAsync(int id)
    {
        var photo = await _context.Photos.FindAsync(id);
        if (photo != null)
        {
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
        }
    }
}
