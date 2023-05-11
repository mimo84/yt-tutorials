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

    public virtual DbSet<Diary> Diaries { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<FoodAmount> FoodAmounts { get; set; }

    public virtual DbSet<FoodMeal> FoodMeals { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

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

        modelBuilder.Entity<Diary>(entity =>
        {
            entity.HasKey(e => e.DiaryId).HasName("pk_diary");

            entity.ToTable("diary");

            entity.Property(e => e.DiaryId)
                .ValueGeneratedNever()
                .HasColumnName("diary_id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("date");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("pk_food");

            entity.ToTable("food");

            entity.Property(e => e.FoodId)
                .ValueGeneratedNever()
                .HasColumnName("food_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<FoodAmount>(entity =>
        {
            entity.HasKey(e => e.FoodAmountId).HasName("pk_food_amount");

            entity.ToTable("food_amount");

            entity.HasIndex(e => e.FoodId, "unq_food_amount_food_id").IsUnique();

            entity.Property(e => e.FoodAmountId)
                .ValueGeneratedNever()
                .HasColumnName("food_amount_id");
            entity.Property(e => e.Alcohol)
                .HasPrecision(10, 4)
                .HasColumnName("alcohol");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("1")
                .HasColumnName("amount");
            entity.Property(e => e.Calcium)
                .HasPrecision(10, 4)
                .HasColumnName("calcium");
            entity.Property(e => e.Carbohydrates)
                .HasPrecision(10, 4)
                .HasColumnName("carbohydrates");
            entity.Property(e => e.Cholesterol)
                .HasPrecision(10, 4)
                .HasColumnName("cholesterol");
            entity.Property(e => e.Fat)
                .HasPrecision(10, 4)
                .HasColumnName("fat");
            entity.Property(e => e.Fiber)
                .HasPrecision(10, 4)
                .HasColumnName("fiber");
            entity.Property(e => e.FoodId).HasColumnName("food_id");
            entity.Property(e => e.Iron)
                .HasPrecision(10, 4)
                .HasColumnName("iron");
            entity.Property(e => e.Potassium)
                .HasPrecision(10, 4)
                .HasColumnName("potassium");
            entity.Property(e => e.Protein)
                .HasPrecision(10, 4)
                .HasColumnName("protein");
            entity.Property(e => e.SaturatedFats)
                .HasPrecision(10, 4)
                .HasColumnName("saturated_fats");
            entity.Property(e => e.Sodium)
                .HasPrecision(10, 4)
                .HasColumnName("sodium");
            entity.Property(e => e.Source)
                .HasMaxLength(100)
                .HasColumnName("source");
            entity.Property(e => e.Sugar)
                .HasPrecision(10, 4)
                .HasColumnName("sugar");

            entity.HasOne(d => d.Food).WithOne(p => p.FoodAmount)
                .HasForeignKey<FoodAmount>(d => d.FoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_food_amount_food");
        });

        modelBuilder.Entity<FoodMeal>(entity =>
        {
            entity.HasKey(e => e.FoodMealId).HasName("pk_food_meal");

            entity.ToTable("food_meal");

            entity.HasIndex(e => e.FoodId, "unq_food_meal_food_id").IsUnique();

            entity.HasIndex(e => e.MealId, "unq_food_meal_meal_id").IsUnique();

            entity.Property(e => e.FoodMealId)
                .ValueGeneratedNever()
                .HasComment("There can be multiple times the same food and food amount within the same meal")
                .HasColumnName("food_meal_id");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 4)
                .HasColumnName("amount");
            entity.Property(e => e.FoodAmountId).HasColumnName("food_amount_id");
            entity.Property(e => e.FoodId).HasColumnName("food_id");
            entity.Property(e => e.MealId).HasColumnName("meal_id");

            entity.HasOne(d => d.FoodAmount).WithMany(p => p.FoodMeals)
                .HasForeignKey(d => d.FoodAmountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_food_meal_food_amount");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("pk_meal");

            entity.ToTable("meal");

            entity.HasIndex(e => e.DiaryId, "unq_meal_diary_id").IsUnique();

            entity.Property(e => e.MealId)
                .ValueGeneratedNever()
                .HasColumnName("meal_id");
            entity.Property(e => e.DiaryId).HasColumnName("diary_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasOne(d => d.Diary).WithOne(p => p.Meal)
                .HasForeignKey<Meal>(d => d.DiaryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_meal_diary");

            entity.HasOne(d => d.MealNavigation).WithOne(p => p.Meal)
                .HasPrincipalKey<FoodMeal>(p => p.MealId)
                .HasForeignKey<Meal>(d => d.MealId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_meal_food_meal");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
