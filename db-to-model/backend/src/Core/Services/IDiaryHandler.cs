using FoodDiary.Core.Dto;

namespace FoodDiary.Core.Services;

public interface IDiaryHandler
{
    public Task<bool> CreateFullDiaryAsync(DiaryIngressDto diaryEntryDto, CancellationToken cancellationToken);

    public Task<bool> CreateFullDiaryWithNamesAsync(DiaryIngressWithFoodNamesDto diaryEntryDto, CancellationToken cancellationToken);
}
