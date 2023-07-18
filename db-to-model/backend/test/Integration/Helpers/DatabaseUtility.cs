using FoodDiary.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Integration.Helpers;

public static class DatabaseUtility
{
    public static void RestoreDatabase(FoodDiaryDbContext db)
    {
        db.Database.ExecuteSqlRaw("DELETE FROM food_meal;");
        db.Database.ExecuteSqlRaw("DELETE FROM meal;");
        db.Database.ExecuteSqlRaw("DELETE FROM diary;");
    }
}
