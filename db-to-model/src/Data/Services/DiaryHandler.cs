using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Services;
using FoodDiary.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Data.Services;

public class DiaryHandler : IDiaryHandler
{
    private readonly FoodDiaryDbContext dbContext;
    public DiaryHandler(FoodDiaryDbContext _dbContext)
    {
        dbContext = _dbContext;
    }

    public async Task<bool> CreateFullDiaryAsync(DiaryIngressDto diaryEntryDto, CancellationToken cancellationToken)
    {
        DateOnly diaryDate = new(diaryEntryDto.Date.Year, diaryEntryDto.Date.Month, diaryEntryDto.Date.Day);
        Diary diary = await dbContext.Diaries.Where(d => d.Date == diaryDate).Include(d => d.Meals).SingleOrDefaultAsync(cancellationToken);

        if (diary == null)
        {
            diary = new Diary()
            {
                Date = diaryDate,
                Meals = new List<Meal>()
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
            Meal newMeal = diary.Meals.Where(diaryMeal => diaryMeal.Name == m.Name).FirstOrDefault();
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
                var food = await dbContext.Foods.Where(dbf => dbf.FoodId == f.FoodId).SingleOrDefaultAsync(cancellationToken);
                var foodAmount = await dbContext.FoodAmounts.Where(dbfa => dbfa.FoodAmountId == f.FoodAmountId).SingleOrDefaultAsync(cancellationToken);

                FoodMeal foodMeal = new()
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

    public async Task<bool> CreateFullDiaryWithNamesAsync(DiaryIngressWithFoodNamesDto diaryEntryDto, CancellationToken cancellationToken)
    {
        DateOnly diaryDate = new(diaryEntryDto.Date.Year, diaryEntryDto.Date.Month, diaryEntryDto.Date.Day);
        Diary diary = await dbContext.Diaries.Where(d => d.Date == diaryDate).Include(d => d.Meals).SingleOrDefaultAsync(cancellationToken);

        if (diary == null)
        {
            Console.WriteLine("No diary was found, creating a new one");
            diary = new Diary()
            {
                Date = diaryDate,
                Meals = new List<Meal>()
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
            Meal newMeal = diary.Meals.Where(diaryMeal => diaryMeal.Name == m.Name).FirstOrDefault();
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
                var food = await dbContext.Foods.Where(dbf => dbf.Name == f.FoodName).SingleOrDefaultAsync(cancellationToken);
                Console.WriteLine($"Looking for food: {food.Name}");
                var foodAmount = await dbContext.FoodAmounts.Where(dbfa => dbfa.FoodId == food.FoodId).SingleOrDefaultAsync(cancellationToken);

                FoodMeal foodMeal = new()
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
