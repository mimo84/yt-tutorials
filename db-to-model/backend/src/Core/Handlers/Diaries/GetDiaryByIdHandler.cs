using FoodDiary.Core.Models;
using FoodDiary.Core.Repositories;
using MediatR;

namespace FoodDiary.Core.Handlers.Diaries;

public class GetDiaryByIdHandler : IRequest<DiaryEnvelope<DiaryResponse>>
{
    private readonly IDiaryRepository diaryHandler;
    public GetDiaryByIdHandler(IDiaryRepository _diaryHandler)
    {
        diaryHandler = _diaryHandler;
    }
}
