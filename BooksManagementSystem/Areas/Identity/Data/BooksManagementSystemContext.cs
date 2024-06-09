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

    public DbSet<BorrowingViewModel> Borrowings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<BookViewModel>()
           .HasOne(b => b.AuthorViewModel)
           .WithMany(a => a.Books)
           .HasForeignKey(b => b.AuthorId);

        builder.Entity<BookViewModel>()
         .HasIndex(b => b.Title)
         .IsClustered(false);

        builder.Entity<AuthorViewModel>()
           .HasIndex(b => b.AuthorSecondname)
           .IsClustered(false);


        builder.Entity<BorrowingViewModel>()
         .HasOne(b => b.BooksManagementSystemUser)
         .WithMany(a => a.Borrowings)
         .HasForeignKey(b => b.UserId);


    }
}
