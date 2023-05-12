using db_to_model.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace db_to_model.Controllers;

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
