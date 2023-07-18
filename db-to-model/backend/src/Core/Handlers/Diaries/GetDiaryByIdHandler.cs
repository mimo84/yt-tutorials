using FoodDiary.Core.Mappers;
using FoodDiary.Core.Messages;
using FoodDiary.Core.Models;
using FoodDiary.Core.Repositories;
using MediatR;

namespace FoodDiary.Core.Handlers.Diaries;

public class GetDiaryByIdHandler : IRequestHandler<GetDiaryById, DiaryEnvelope<DiaryResponse>>
{
    private readonly IDiaryRepository diaryRepository;

    public GetDiaryByIdHandler(IDiaryRepository _diaryRepository)
    {
        diaryRepository = _diaryRepository;
    }

    public async Task<DiaryEnvelope<DiaryResponse>> Handle(
        GetDiaryById request,
        CancellationToken cancellationToken
    )
    {
        var diary = await diaryRepository.GetDiaryById(request.Id, cancellationToken);
        var diaryResponse = DiariesMapper.MapFromDiaryEntity(diary);
        var result = new DiaryEnvelope<DiaryResponse>(diaryResponse);
        return result;
    }
}
