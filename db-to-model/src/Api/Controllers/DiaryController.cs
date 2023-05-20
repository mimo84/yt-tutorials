using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DiaryController : ControllerBase
{
    private readonly FoodDiaryDbContext dbContext;
    public DiaryController(FoodDiaryDbContext _dbContext)
    {
        dbContext = _dbContext;
    }

    [HttpGet("get", Name = "diaries")]
    public async Task<Diary> Get()
    {
        var diaries = await dbContext.Diaries.FirstOrDefaultAsync();
        return diaries;
    }

    [HttpGet("get/{id:int}", Name = "diary_by_id")]
    public async Task<Diary> Get(int id)
    {
        var diaries = await dbContext.Diaries.Where(d => d.DiaryId == id).SingleOrDefaultAsync();
        return diaries;
    }

    [HttpPost(Name = "add_diary")]
    public async Task<bool> AddDiary(DiaryIngressDto diaryEntryDto)
    {
        DateOnly diaryDate = new(diaryEntryDto.Date.Year, diaryEntryDto.Date.Month, diaryEntryDto.Date.Day);
        Diary diary = await dbContext.Diaries.Where(d => d.Date == diaryDate).SingleOrDefaultAsync();

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
                var food = await dbContext.Foods.Where(dbf => dbf.FoodId == f.FoodId).SingleOrDefaultAsync();
                var foodAmount = await dbContext.FoodAmounts.Where(dbfa => dbfa.FoodAmountId == f.FoodAmountId).SingleOrDefaultAsync();

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

        await dbContext.SaveChangesAsync();


        return true;
    }
}
