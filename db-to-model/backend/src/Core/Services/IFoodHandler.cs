using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;

namespace FoodDiary.Core.Services;

public interface IFoodHandler
{
    Task AddFoodWithAmountsAsync(FoodWithAmountDto foodAmountDto, CancellationToken cancellationToken);
    Task<List<Food>> FindFood(string name, CancellationToken cancellationToken);
}