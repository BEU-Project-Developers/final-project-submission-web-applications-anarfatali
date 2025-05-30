using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhotoFolio.DATA;
using PhotoFolio.Models;
using PhotoFolio.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IGalleryService, GalleryService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

// 2) Add Identity
// builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
//     .AddEntityFrameworkStores<ApplicationDbContext>()
//     .AddDefaultTokenProviders();

// 3) Add MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 4) Seed Roles
// using (var scope = app.Services.CreateScope())
// {
//     var services = scope.ServiceProvider;
//     var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//     string[] roles = { "User", "Photographer", "Admin" };
//     foreach (var role in roles)
//     {
//         if (!await roleManager.RoleExistsAsync(role))
//             await roleManager.CreateAsync(new IdentityRole(role));
//     }
// }

// 5) Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Admin}/{action=AdminPanel}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);

app.Run();
