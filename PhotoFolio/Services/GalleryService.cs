using Microsoft.EntityFrameworkCore;
using PhotoFolio.DATA;
using PhotoFolio.Models;

namespace PhotoFolio.Services;

public class GalleryService : IGalleryService
{
    private readonly ApplicationDbContext _context;

    public GalleryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Photo>> GetGalleryItemsAsync()
    {
        return await _context.Photos.ToListAsync();
    }
}
