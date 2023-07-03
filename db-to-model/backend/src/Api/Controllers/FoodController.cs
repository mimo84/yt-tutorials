using FoodDiary.Api.Models;
using FoodDiary.Core.Dto;
using FoodDiary.Core.Messages;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FoodController : ControllerBase
{
    private readonly IMediator mediator;

    public FoodController(IMediator _mediator)
    {
        mediator = _mediator;
    }

    [HttpPost("new", Name = "NewFood")]
    public async Task<FoodEnvelope<bool>> CreateAsync(
        RequestEnvelope<FoodEnvelope<FoodWithAmountDto>> request,
        CancellationToken cancellationToken)
    {
        var message = new AddFoodWithAmounts(request.Body);
        await mediator.Send(message, cancellationToken);
        return new FoodEnvelope<bool>(true);
    }

    [HttpGet("find")]
    public async Task<ActionResult<FoodEnvelope<List<FoodWithNutritionInfoDto>>>> FindFoodByName([FromQuery] GetFoodsFromQuery query, CancellationToken cancellationToken)
    {
        var message = new GetFoodsFromQuery(query.Name);
        return await mediator.Send(message, cancellationToken);
    }

    [HttpGet("all")]
    public async Task<ActionResult<FoodEnvelope<List<FoodWithNutritionInfoDto>>>> GetAllFoods(CancellationToken cancellationToken)
    {
        var message = new GetAllFoods();
        return await mediator.Send(message, cancellationToken);
    }

}

