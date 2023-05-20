using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
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
    public async Task<bool> AddDiary(DiaryIngressDto diaryEntryDto, CancellationToken cancellationToken)
    {
        await diaryHandler.CreateFullDiaryAsync(diaryEntryDto, cancellationToken);

        return true;
    }
}
