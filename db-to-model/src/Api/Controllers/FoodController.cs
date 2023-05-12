using FoodDiary.Api.Models;
using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Services;
using FoodDiary.Data.Contexts;
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
}
