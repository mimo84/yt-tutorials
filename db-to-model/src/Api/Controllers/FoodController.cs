using FoodDiary.Api.Models;
using FoodDiary.Core.Dto;
using FoodDiary.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FoodController : ControllerBase
{
    private readonly ICentralRepository centralRepository;
    public FoodController(ICentralRepository _centralRepository)
    {
        centralRepository = _centralRepository;
    }

    [HttpPost("new", Name = "NewFood")]
    public async Task<FoodEnvelope<bool>> CreateAsync(
        RequestEnvelope<FoodEnvelope<FoodWithAmountDto>> request,
        CancellationToken cancellationToken)
    {
        await centralRepository.AddFoodWithAmountsAsync(request.Body.Food, cancellationToken);
        return new FoodEnvelope<bool>(true);
    }

    [HttpPost("upload")]
    public IActionResult UploadFile([FromBody] IFormFile file)
    {
        var data = new byte[file.Length];

        using (var bstream = file.OpenReadStream())
        {
            while (bstream.CanRead)
            {
                bstream.Read(data);
            }
        }

        // etc

        return Ok();
    }
}
