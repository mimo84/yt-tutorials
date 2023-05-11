using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace db_to_model.Db;

public partial class DemoDbContext : DbContext
{
    public DemoDbContext()
    {
    }

    public DemoDbContext(DbContextOptions<DemoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookAuthor> BookAuthors { get; set; }

    public virtual DbSet<BookCategory> BookCategories { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=demo_db;Username=user_demo;Password=pg_strong_password");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("pk_author");

            entity.ToTable("author");

            entity.Property(e => e.AuthorId)
                .ValueGeneratedNever()
                .HasColumnName("author_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .HasColumnName("surname");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("pk_book");

            entity.ToTable("book");

            entity.Property(e => e.BookId)
                .ValueGeneratedNever()
                .HasColumnName("book_id");
            entity.Property(e => e.Isbn)
                .HasMaxLength(100)
                .HasColumnName("isbn");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
        });

        modelBuilder.Entity<BookAuthor>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("book_author");

            entity.Property(e => e.AuthorId).HasColumnName("author_id");
            entity.Property(e => e.BookId).HasColumnName("book_id");

            entity.HasOne(d => d.Author).WithMany()
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_book_author_author");

            entity.HasOne(d => d.Book).WithMany()
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_book_author_book");
        });

        modelBuilder.Entity<BookCategory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("book_category");

            entity.Property(e => e.BookId).HasColumnName("book_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");

            entity.HasOne(d => d.Book).WithMany()
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_book_category_book");

            entity.HasOne(d => d.Category).WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_book_category_category");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("pk_category");

            entity.ToTable("category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("category_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Parent).HasColumnName("parent");

            entity.HasOne(d => d.ParentNavigation).WithMany(p => p.InverseParentNavigation)
                .HasForeignKey(d => d.Parent)
                .HasConstraintName("fk_category_category");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
