using FoodDiary.Core.Dto;

namespace FoodDiary.Core.Repositories;

public interface IDiaryRepository
{
    public Task<bool> CreateFullDiaryAsync(DiaryIngressDto diaryEntryDto, CancellationToken cancellationToken);

    public Task<bool> CreateFullDiaryWithNamesAsync(DiaryIngressWithFoodNamesDto diaryEntryDto, CancellationToken cancellationToken);
}
