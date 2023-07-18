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
        => optionsBuilder.UseSqlite("DataSource=/Users/mimo/work/github_repos/yt-tutorials/db-to-model/backend/src/Data/Database/database.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Diary>(entity =>
        {
            entity.ToTable("diary");

            entity.Property(e => e.DiaryId).HasColumnName("diary_id");
            entity.Property(e => e.Date)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.AppUserId)
                .IsRequired()
                .HasColumnName("user_id");

            entity.HasOne(d => d.AppUser).WithMany(p => p.Diaries)
                .HasForeignKey(d => d.AppUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.ToTable("food");

            entity.Property(e => e.FoodId).HasColumnName("food_id");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnType("varchar")
                .HasColumnName("name");
        });

        modelBuilder.Entity<FoodAmount>(entity =>
        {
            entity.ToTable("food_amount");

            entity.Property(e => e.FoodAmountId).HasColumnName("food_amount_id");
            entity.Property(e => e.Alcohol)
                .HasColumnType("numeric(10,4)")
                .HasColumnName("alcohol");
            entity.Property(e => e.Amount)
                .IsRequired()
                .HasDefaultValueSql("1")
                .HasColumnType("numeric(10,2)")
                .HasColumnName("amount");
            entity.Property(e => e.AmountName)
                .HasColumnType("varchar")
                .HasColumnName("amount_name");
            entity.Property(e => e.Calcium)
                .HasColumnType("numeric(10,4)")
                .HasColumnName("calcium");
            entity.Property(e => e.Carbohydrates)
                .HasColumnType("numeric(10,4)")
                .HasColumnName("carbohydrates");
            entity.Property(e => e.Cholesterol)
                .HasColumnType("numeric(10,4)")
                .HasColumnName("cholesterol");
            entity.Property(e => e.Fat)
                .HasColumnType("numeric(10,4)")
                .HasColumnName("fat");
            entity.Property(e => e.Fiber)
                .HasColumnType("numeric(10,4)")
                .HasColumnName("fiber");
            entity.Property(e => e.FoodId).HasColumnName("food_id");
            entity.Property(e => e.Iron)
                .HasColumnType("numeric(10,4)")
                .HasColumnName("iron");
            entity.Property(e => e.Potassium)
                .HasColumnType("numeric(10,4)")
                .HasColumnName("potassium");
            entity.Property(e => e.Protein)
                .HasColumnType("numeric(10,4)")
                .HasColumnName("protein");
            entity.Property(e => e.SaturatedFats)
                .HasColumnType("numeric(10,4)")
                .HasColumnName("saturated_fats");
            entity.Property(e => e.Sodium)
                .HasColumnType("numeric(10,4)")
                .HasColumnName("sodium");
            entity.Property(e => e.Source)
                .HasColumnType("varchar")
                .HasColumnName("source");
            entity.Property(e => e.Sugar)
                .HasColumnType("numeric(10,4)")
                .HasColumnName("sugar");

            entity.HasOne(d => d.Food).WithMany(p => p.FoodAmounts)
                .HasForeignKey(d => d.FoodId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<FoodMeal>(entity =>
        {
            entity.ToTable("food_meal");

            entity.Property(e => e.FoodMealId).HasColumnName("food_meal_id");
            entity.Property(e => e.ConsumedAmount)
                .IsRequired()
                .HasColumnType("numeric(10,4)")
                .HasColumnName("consumed_amount");
            entity.Property(e => e.FoodAmountId).HasColumnName("food_amount_id");
            entity.Property(e => e.FoodId).HasColumnName("food_id");
            entity.Property(e => e.MealId).HasColumnName("meal_id");

            entity.HasOne(d => d.FoodAmount).WithMany(p => p.FoodMeals)
                .HasForeignKey(d => d.FoodAmountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Food).WithMany(p => p.FoodMeals)
                .HasForeignKey(d => d.FoodId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Meal).WithMany(p => p.FoodMeals)
                .HasForeignKey(d => d.MealId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.ToTable("meal");

            entity.Property(e => e.MealId).HasColumnName("meal_id");
            entity.Property(e => e.DiaryId).HasColumnName("diary_id");
            entity.Property(e => e.Name)
                .HasColumnType("varchar")
                .HasColumnName("name");

            entity.HasOne(d => d.Diary).WithMany(p => p.Meals)
                .HasForeignKey(d => d.DiaryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        base.OnModelCreating(modelBuilder);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
