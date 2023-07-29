using FoodDiary.Core.Mappers;
using FoodDiary.Core.Messages;
using FoodDiary.Core.Models;
using FoodDiary.Core.Repositories;
using MediatR;

namespace FoodDiary.Core.Handlers.Diaries;

public class GetAllDiariesHandler : IRequestHandler<GetAllDiaries, DiaryEnvelope<DiariesResponse>>
{
    private readonly IDiaryRepository diaryRepository;

    public GetAllDiariesHandler(IDiaryRepository _diaryRepository)
    {
        diaryRepository = _diaryRepository;
    }

    public async Task<DiaryEnvelope<DiariesResponse>> Handle(
        GetAllDiaries request,
        CancellationToken cancellationToken
    )
    {
        var diary = await diaryRepository.GetAllDiaries(request.User, cancellationToken);
        var diaryResponse = DiariesMapper.MapFromDiariesEntity(diary);
        var result = new DiaryEnvelope<DiariesResponse>(diaryResponse);
        return result;
    }
}
