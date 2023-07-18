using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;

namespace FoodDiary.Core.Repositories;

public interface IDiaryRepository
{

    public Task<Diary> GetDiaryById(int id, CancellationToken cancellationToken);

    public Task<List<Diary>> GetAllDiaries(CancellationToken cancellationToken);

    public Task<bool> CreateFullDiaryAsync(DiaryIngressDto diaryEntryDto, AppUser user, CancellationToken cancellationToken);

    public Task<bool> CreateFullDiaryWithNamesAsync(DiaryIngressWithFoodNamesDto diaryEntryDto, AppUser user, CancellationToken cancellationToken);
}
