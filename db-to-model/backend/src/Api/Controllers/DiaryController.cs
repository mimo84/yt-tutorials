using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Mappers;
using FoodDiary.Core.Models;
using FoodDiary.Core.Services;
using FoodDiary.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DiaryController : ControllerBase
{
    private readonly FoodDiaryDbContext dbContext;
    private readonly IDiaryHandler diaryHandler;
    public DiaryController(FoodDiaryDbContext _dbContext, IDiaryHandler _diaryHandler)
    {
        dbContext = _dbContext;
        diaryHandler = _diaryHandler;
    }

    [HttpGet("get", Name = "diaries")]
    public async Task<ActionResult<DiariesResponse>> Get(CancellationToken cancellationToken)
    {
        var diaries = await dbContext.
            Diaries.OrderByDescending(d => d.Date)
                .Include(d => d.Meals)
                .ThenInclude(m => m.FoodMeals)
                .ThenInclude(f => f.Food)
                .ThenInclude(f => f.FoodAmounts)
                .ToListAsync(cancellationToken);

        var result = DiariesMapper.MapFromDiariesEntity(diaries);
        return result;
    }

    [HttpGet("get/{id:int}", Name = "diary_by_id")]
    public async Task<Diary> Get(int id)
    {
        var diaries = await dbContext.Diaries.Where(d => d.DiaryId == id).SingleOrDefaultAsync();
        return diaries;
    }

    [HttpPost(Name = "add_diary")]
    public async Task<bool> AddDiary(DiaryIngressDto diaryEntryDto, CancellationToken cancellationToken)
    {
        await diaryHandler.CreateFullDiaryAsync(diaryEntryDto, cancellationToken);

        return true;
    }

    [HttpPost("by_food_name", Name = "add_diary_food_names")]
    public async Task<bool> AddDiaryByFoodName(DiaryIngressWithFoodNamesDto diaryEntryDto, CancellationToken cancellationToken)
    {
        await diaryHandler.CreateFullDiaryWithNamesAsync(diaryEntryDto, cancellationToken);

        return true;
    }
}
