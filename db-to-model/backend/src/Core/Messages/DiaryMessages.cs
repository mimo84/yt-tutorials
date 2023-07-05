using FoodDiary.Core.Models;
using MediatR;

namespace FoodDiary.Core.Messages;

public record GetDiaryById(int Id) : IRequest<DiaryEnvelope<DiaryResponse>>;