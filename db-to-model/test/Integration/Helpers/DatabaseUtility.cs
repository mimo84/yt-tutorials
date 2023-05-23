using FoodDiary.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Integration.Helpers;

public static class DatabaseUtility
{
    public static void RestoreDatabase(FoodDiaryDbContext db)
    {
        db.Database.ExecuteSqlRaw("TRUNCATE TABLE public.food_meal RESTART IDENTITY CASCADE;");
        db.Database.ExecuteSqlRaw("TRUNCATE TABLE public.meal RESTART IDENTITY CASCADE;");
        db.Database.ExecuteSqlRaw("TRUNCATE TABLE public.diary RESTART IDENTITY CASCADE;");
    }
}
