using Microsoft.EntityFrameworkCore;
using Moment3MVC.Models;

namespace Moment3MVC.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Loans> Loans { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-many relationship between User and Loans
            modelBuilder.Entity<Loans>()
                .HasOne<User>()
                .WithMany(u => u.Loans)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-many relationship between Book and Loans (without navigation property in Book)
            modelBuilder.Entity<Loans>()
                .HasOne<Book>()
                .WithMany()
                .HasForeignKey(l => l.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}