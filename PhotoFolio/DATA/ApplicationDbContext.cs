using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhotoFolio.Models;

namespace PhotoFolio.DATA;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<PhotographerRequest> PhotographerRequests { get; set; }
    public DbSet<Gallery> Galleries { get; set; }
    public DbSet<PhotoAlbumItem> PhotoAlbumItems { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PhotoTag> PhotoTags { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PhotoAlbumItem>().HasKey(x => new { x.GalleryId, x.PhotoId });
        builder.Entity<PhotoTag>().HasKey(x => new { x.PhotoId, x.TagId });
        builder.Entity<Favorite>().HasKey(x => new { x.PhotoId, x.UserId });
    }
}
