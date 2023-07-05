using FoodDiary.Core.Dto;
using FoodDiary.Core.Models;
using MediatR;

namespace FoodDiary.Core.Messages;

public record GetDiaryById(int Id) : IRequest<DiaryEnvelope<DiaryResponse>>;

public record GetAllDiaries() : IRequest<DiaryEnvelope<DiariesResponse>>;

public record AddNewDiary(DiaryIngressDto Request) : IRequest<bool>;

public record AddNewDiaryWithFoodNames(DiaryIngressWithFoodNamesDto Request) : IRequest<bool>;