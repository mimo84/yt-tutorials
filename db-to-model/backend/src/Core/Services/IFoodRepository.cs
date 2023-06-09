using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;

namespace FoodDiary.Core.Services;

public interface IFoodRepository
{
    Task AddFoodWithAmountsAsync(FoodWithAmountDto foodAmountDto, CancellationToken cancellationToken);
    Task<List<Food>> FindFood(string name, CancellationToken cancellationToken);

    Task<List<FoodWithNutritionInfoDto>> GetAllFoods(CancellationToken cancellationToken);
}