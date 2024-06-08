using BooksManagementSystem.Areas.Identity.Data;
using BooksManagementSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BooksManagementSystem.Data;

public class BooksManagementSystemContext : IdentityDbContext<BooksManagementSystemUser>
{
    public BooksManagementSystemContext(DbContextOptions<BooksManagementSystemContext> options)
        : base(options)
    {
    }

    public DbSet<BookViewModel> Books { get; set; }
    public DbSet<AuthorViewModel> Authors { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<BookViewModel>()
           .HasOne(b => b.AuthorViewModel)
           .WithMany(a => a.Books)
           .HasForeignKey(b => b.AuthorId);
    }
}
