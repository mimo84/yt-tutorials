using FoodDiary.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Data.Contexts;

public partial class FoodDiaryDbContext : IdentityDbContext<AppUser>
{
    public FoodDiaryDbContext()
    {
    }

    public FoodDiaryDbContext(DbContextOptions<FoodDiaryDbContext> options)
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
                .IsRequired()
                .HasColumnType("character varying")
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
                .HasComment("This is to help the user to know what kind of \"amount\" it is, is it a serving, is it based on weight.")
                .HasColumnType("character varying")
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
                .HasColumnType("character varying")
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

            entity.Property(e => e.FoodMealId)
                .HasComment("There can be multiple times the same food and food amount within the same meal")
                .HasColumnName("food_meal_id");
            entity.Property(e => e.ConsumedAmount)
                .HasPrecision(10, 4)
                .HasColumnName("consumed_amount");
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

            entity.HasOne(d => d.Meal).WithMany(p => p.FoodMeals)
                .HasForeignKey(d => d.MealId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_food_meal_meal");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("pk_meal");

            entity.ToTable("meal");

            entity.Property(e => e.MealId).HasColumnName("meal_id");
            entity.Property(e => e.DiaryId).HasColumnName("diary_id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");

            entity.HasOne(d => d.Diary).WithMany(p => p.Meals)
                .HasForeignKey(d => d.DiaryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_meal_diary");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
