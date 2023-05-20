using System.Linq;
using System.Threading.Tasks;
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
    public async Task<Diary> AddDiary(DiaryIngressDto diaryEntryDto)
    {
        DateOnly diaryDate = new(diaryEntryDto.Date.Year, diaryEntryDto.Date.Month, diaryEntryDto.Date.Day);
        Diary diary = await dbContext.Diaries.Where(d => d.Date == diaryDate).SingleOrDefaultAsync();

        if (diary == null)
        {
            diary = new Diary()
            {
                Date = diaryDate,
                Meal = new List<Meal>()
            };
            dbContext.Add(diary);
        }

        await dbContext.SaveChangesAsync();


        return diary;
    }
}
