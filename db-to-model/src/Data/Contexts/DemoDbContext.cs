using FoodDiary.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Data.Contexts;

public partial class DemoDbContext : DbContext
{
    public DemoDbContext()
    {
    }

    public DemoDbContext(DbContextOptions<DemoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Diary> Diaries { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<FoodAmount> FoodAmounts { get; set; }

    public virtual DbSet<FoodMeal> FoodMeals { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:Database");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Diary>(entity =>
        {
            entity.HasKey(e => e.DiaryId).HasName("pk_diary");

            entity.ToTable("diary");

            entity.Property(e => e.DiaryId).HasColumnName("diary_id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("date");
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("pk_food");

            entity.ToTable("food");

            entity.Property(e => e.FoodId).HasColumnName("food_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<FoodAmount>(entity =>
        {
            entity.HasKey(e => e.FoodAmountId).HasName("pk_food_amount");

            entity.ToTable("food_amount");

            entity.Property(e => e.FoodAmountId).HasColumnName("food_amount_id");
            entity.Property(e => e.Alcohol)
                .HasPrecision(10, 4)
                .HasColumnName("alcohol");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("1")
                .HasColumnName("amount");
            entity.Property(e => e.AmountName)
                .HasMaxLength(100)
                .HasComment("This is to help the user to know what kind of \"amount\" it is, is it a serving, is it based on weight.")
                .HasColumnName("amount_name");
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

            entity.HasOne(d => d.Food).WithMany(p => p.FoodAmounts)
                .HasForeignKey(d => d.FoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_food_amount_food");
        });

        modelBuilder.Entity<FoodMeal>(entity =>
        {
            entity.HasKey(e => e.FoodMealId).HasName("pk_food_meal");

            entity.ToTable("food_meal");

            entity.HasIndex(e => e.MealId, "unq_food_meal_meal_id").IsUnique();

            entity.Property(e => e.FoodMealId)
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

            entity.HasOne(d => d.Food).WithMany(p => p.FoodMeals)
                .HasForeignKey(d => d.FoodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_food_meal_food");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("pk_meal");

            entity.ToTable("meal");

            entity.HasIndex(e => e.DiaryId, "unq_meal_diary_id").IsUnique();

            entity.Property(e => e.MealId)
                .ValueGeneratedOnAdd()
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
