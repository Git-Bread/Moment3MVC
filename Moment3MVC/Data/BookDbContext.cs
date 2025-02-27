﻿using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Loans>()
                .HasOne(l => l.Book)
                .WithMany()
                .HasForeignKey(l => l.BookId);
        }
    }
}
