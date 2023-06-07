using FoodDiary.Api.Models;
using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FoodController : ControllerBase
{
    private readonly IMediator mediator;

    private readonly IFoodRepository foodRepository;
    public FoodController(IFoodRepository _FoodRepository, IMediator _mediator)
    {
        foodRepository = _FoodRepository;
        mediator = _mediator;
    }

    [HttpPost("new", Name = "NewFood")]
    public async Task<FoodEnvelope<bool>> CreateAsync(
        RequestEnvelope<FoodEnvelope<FoodWithAmountDto>> request,
        CancellationToken cancellationToken)
    {
        await foodRepository.AddFoodWithAmountsAsync(request.Body.Food, cancellationToken);
        return new FoodEnvelope<bool>(true);
    }

    [HttpGet("find")]
    public async Task<ActionResult<FoodEnvelope<List<Food>>>> FindFood([FromQuery] GetFoodsFromQuery query, CancellationToken cancellationToken)
    {
        return await mediator.Send(query, cancellationToken);
    }
}

