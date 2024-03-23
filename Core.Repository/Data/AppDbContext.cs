using FinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<CartProducts> CartProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<CartProducts>()
            .HasOne(x => x.ApplicationUser)
            .WithMany(x => x.CartProducts)
            .HasForeignKey(x => x.ApplicationUserId)
            .IsRequired();
    }
}