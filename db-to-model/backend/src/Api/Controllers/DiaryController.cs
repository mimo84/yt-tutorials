using System.Security.Claims;
using FoodDiary.Core.Dto;
using FoodDiary.Core.Messages;
using FoodDiary.Core.Models;
using FoodDiary.Core.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DiaryController : ControllerBase
{
    private readonly IMediator mediator;

    private readonly IUserRepository userRepository;

    public DiaryController(IMediator _mediator, IUserRepository _userRepository)
    {
        mediator = _mediator;
        userRepository = _userRepository;
    }

    [HttpGet("get", Name = "diaries")]
    public async Task<DiaryEnvelope<DiariesResponse>> Get(CancellationToken cancellationToken)
    {
        var message = new GetAllDiaries();
        var response = await mediator.Send(message, cancellationToken);
        return response;
    }

    [HttpGet("get/{id:int}", Name = "diary_by_id")]
    public async Task<DiaryEnvelope<DiaryResponse>> Get(int id, CancellationToken cancellationToken)
    {
        var message = new GetDiaryById(id);
        var response = await mediator.Send(message, cancellationToken);
        return response;
    }

    [HttpPost(Name = "add_diary")]
    public async Task<bool> AddDiary(
        DiaryIngressDto diaryEntryDto,
        CancellationToken cancellationToken
    )
    {
        var user = await userRepository.GetAppUserAsync(
            User.FindFirstValue(ClaimTypes.Email),
            cancellationToken
        );
        var message = new AddNewDiary(diaryEntryDto, user);
        return await mediator.Send(message, cancellationToken);
    }

    [HttpPost("by_food_name", Name = "add_diary_food_names")]
    public async Task<bool> AddDiaryByFoodName(
        DiaryIngressWithFoodNamesDto diaryEntryDto,
        CancellationToken cancellationToken
    )
    {
        var user = await userRepository.GetAppUserAsync(
            User.FindFirstValue(ClaimTypes.Email),
            cancellationToken
        );
        var message = new AddNewDiaryWithFoodNames(diaryEntryDto, user);
        return await mediator.Send(message, cancellationToken);
    }
}
