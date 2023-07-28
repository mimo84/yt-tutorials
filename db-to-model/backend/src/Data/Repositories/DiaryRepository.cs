using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Repositories;
using FoodDiary.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Data.Repositories;

public class DiaryRepository : IDiaryRepository
{
    private readonly FoodDiaryDbContext dbContext;

    public DiaryRepository(FoodDiaryDbContext _dbContext)
    {
        dbContext = _dbContext;
    }

    public async Task<Diary> GetDiaryById(int id, CancellationToken cancellationToken)
    {
        var diary = await dbContext.Diaries
            .Where(d => d.DiaryId == id)
            .SingleAsync(cancellationToken);
        return diary;
    }

    public async Task<List<Diary>> GetAllDiaries(CancellationToken cancellationToken)
    {
        var diaries = await dbContext.Diaries
            .OrderByDescending(d => d.Date)
            .Include(d => d.Meals)
            .ThenInclude(m => m.FoodMeals)
            .ThenInclude(f => f.Food)
            .ThenInclude(f => f.FoodAmounts)
            .ToListAsync(cancellationToken);

        return diaries;
    }

    public async Task<bool> DeleteFoodFromDiary(
        int foodMealId,
        AppUser user,
        CancellationToken cancellationToken
    )
    {
        var foodMeal = await dbContext.FoodMeals
            .Where(f => f.FoodMealId == foodMealId)
            .Include(fm => fm.Meal)
            .ThenInclude(m => m.Diary)
            .SingleAsync(cancellationToken);
        var owner = foodMeal.Meal.Diary.AppUserId;
        if (owner != user.Id)
        {
            return false;
        }
        dbContext.Remove(foodMeal);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> CreateFullDiaryAsync(
        DiaryIngressDto diaryEntryDto,
        AppUser user,
        CancellationToken cancellationToken
    )
    {
        DateTime diaryDate =
            new(diaryEntryDto.Date.Year, diaryEntryDto.Date.Month, diaryEntryDto.Date.Day);
        Diary diary = await dbContext.Diaries
            .Where(d => d.Date == diaryDate && d.AppUser == user)
            .Include(d => d.Meals)
            .SingleOrDefaultAsync(cancellationToken);

        if (diary == null)
        {
            diary = new Diary()
            {
                Date = diaryDate,
                Meals = new List<Meal>(),
                AppUser = user
            };
            dbContext.Diaries.Add(diary);
        }

        if (diary.Meals == null)
        {
            diary.Meals = new List<Meal>();
            dbContext.Meals.AddRange(diary.Meals);
        }

        foreach (var m in diaryEntryDto.MealEntries)
        {
            Meal newMeal = diary.Meals
                .Where(diaryMeal => diaryMeal.Name == m.Name)
                .FirstOrDefault();
            if (newMeal == null)
            {
                newMeal = new()
                {
                    Name = m.Name,
                    Diary = diary,
                    FoodMeals = new List<FoodMeal>(),
                };
                dbContext.Meals.Add(newMeal);
            }

            foreach (var f in m.FoodEntries)
            {
                var food = await dbContext.Foods
                    .Where(dbf => dbf.FoodId == f.FoodId)
                    .SingleOrDefaultAsync(cancellationToken);
                var foodAmount = await dbContext.FoodAmounts
                    .Where(dbfa => dbfa.FoodAmountId == f.FoodAmountId)
                    .SingleOrDefaultAsync(cancellationToken);

                FoodMeal foodMeal =
                    new()
                    {
                        ConsumedAmount = f.ConsumedAmount,
                        Food = food,
                        FoodAmount = foodAmount,
                        Meal = newMeal,
                    };
                dbContext.FoodMeals.Add(foodMeal);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> CreateFullDiaryWithNamesAsync(
        DiaryIngressWithFoodNamesDto diaryEntryDto,
        AppUser user,
        CancellationToken cancellationToken
    )
    {
        DateTime diaryDate =
            new(diaryEntryDto.Date.Year, diaryEntryDto.Date.Month, diaryEntryDto.Date.Day);
        Diary diary = await dbContext.Diaries
            .Where(d => d.Date == diaryDate && d.AppUser == user)
            .Include(d => d.Meals)
            .SingleOrDefaultAsync(cancellationToken);

        if (diary == null)
        {
            Console.WriteLine("No diary was found, creating a new one");
            diary = new Diary()
            {
                Date = diaryDate,
                Meals = new List<Meal>(),
                AppUser = user
            };
            dbContext.Diaries.Add(diary);
        }

        if (diary.Meals == null)
        {
            Console.WriteLine("No meals where associated with this diary.");
            diary.Meals = new List<Meal>();
            dbContext.Meals.AddRange(diary.Meals);
        }

        foreach (var m in diaryEntryDto.MealEntries)
        {
            Meal newMeal = diary.Meals
                .Where(diaryMeal => diaryMeal.Name == m.Name)
                .FirstOrDefault();
            if (newMeal == null)
            {
                newMeal = new()
                {
                    Name = m.Name,
                    Diary = diary,
                    FoodMeals = new List<FoodMeal>(),
                };
                dbContext.Meals.Add(newMeal);
            }

            foreach (var f in m.FoodEntries)
            {
                Console.WriteLine($"This is what we're looking for: {f.FoodName}");
                var food = await dbContext.Foods
                    .Where(dbf => dbf.Name == f.FoodName)
                    .SingleOrDefaultAsync(cancellationToken);
                Console.WriteLine($"Looking for food: {food.Name}");
                var foodAmount = await dbContext.FoodAmounts
                    .Where(dbfa => dbfa.FoodId == food.FoodId)
                    .SingleOrDefaultAsync(cancellationToken);

                FoodMeal foodMeal =
                    new()
                    {
                        ConsumedAmount = f.ConsumedAmount,
                        Food = food,
                        FoodAmount = foodAmount,
                        Meal = newMeal,
                    };
                dbContext.FoodMeals.Add(foodMeal);
            }
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}
