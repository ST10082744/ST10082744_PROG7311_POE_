using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ST10082744_PROG7311_POE_.Areas.Identity.Data;
using ST10082744_PROG7311_POE_.Models;

namespace ST10082744_PROG7311_POE_.Data;

public class ST10082744_PROG7311_POE_Context : IdentityDbContext<ST10082744_PROG7311_POE_User>
{
    public ST10082744_PROG7311_POE_Context(DbContextOptions<ST10082744_PROG7311_POE_Context> options)
        : base(options)
    {
    }
    /// <summary>
    /// collection of products in database
    /// </summary>
    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Product>()
           .HasOne(p => p.User)
           .WithMany()
           .HasForeignKey(p => p.UserId)
           .OnDelete(DeleteBehavior.Cascade);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

}
