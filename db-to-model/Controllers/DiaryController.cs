using db_to_model.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace db_to_model.Controllers;

[ApiController]
[Route("[controller]")]
public class DiaryController : ControllerBase
{
    DemoDbContext dbContext;
    public DiaryController(DemoDbContext _dbContext)
    {
        dbContext = _dbContext;
    }

    [HttpGet(Name = "Diaries")]
    public async Task<Diary> Get()
    {
        var diaries = await dbContext.Diaries.FirstOrDefaultAsync();
        return diaries;
    }
}
