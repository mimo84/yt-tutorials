using FoodDiary.Core.Dto;

namespace FoodDiary.Core.Services;

public interface IFoodRepository
{
    Task AddFoodWithAmountsAsync(FoodWithAmountDto foodAmountDto, CancellationToken cancellationToken);
    Task<List<FoodWithNutritionInfoDto>> FindFood(string name, CancellationToken cancellationToken);

    Task<List<FoodWithNutritionInfoDto>> GetAllFoods(CancellationToken cancellationToken);
}