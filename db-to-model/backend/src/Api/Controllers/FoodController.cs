using FoodDiary.Api.Models;
using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Messages;
using FoodDiary.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FoodController : ControllerBase
{
    private readonly IMediator mediator;

    private readonly IFoodHandler foodHandler;
    public FoodController(IFoodHandler _foodHandler, IMediator _mediator)
    {
        foodHandler = _foodHandler;
        mediator = _mediator;
    }

    [HttpPost("new", Name = "NewFood")]
    public async Task<FoodEnvelope<bool>> CreateAsync(
        RequestEnvelope<FoodEnvelope<FoodWithAmountDto>> request,
        CancellationToken cancellationToken)
    {
        await foodHandler.AddFoodWithAmountsAsync(request.Body.Food, cancellationToken);
        return new FoodEnvelope<bool>(true);
    }

    [HttpGet("find")]
    public async Task<ActionResult<FoodEnvelope<List<Food>>>> FindFood([FromQuery] FoodQuery query, CancellationToken cancellationToken)
    {
        var foods = await foodHandler.FindFood(query.Name, cancellationToken);
        var result = new FoodEnvelope<List<Food>>(foods);
        return result;
    }

    [HttpGet("ping")]
    public async Task<string> Ping(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new Ping(), cancellationToken);
        return result;
    }
}

