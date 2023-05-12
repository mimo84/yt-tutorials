using System.Linq;
using System.Threading.Tasks;
using FoodDiary.Core.Entities;
using FoodDiary.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DiaryController : ControllerBase
{
    private readonly DemoDbContext dbContext;
    public DiaryController(DemoDbContext _dbContext)
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
        var diaries = await dbContext.Diaries.Where(d => d.DiaryId == id).FirstOrDefaultAsync();
        return diaries;
    }
}
