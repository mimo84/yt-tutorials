using FoodDiary.Core.Dto;

namespace FoodDiary.Core.Services;

public interface IDiaryHandler
{
    public Task<bool> CreateFullDiaryAsync(DiaryIngressDto diaryEntryDto, CancellationToken cancellationToken);
}
