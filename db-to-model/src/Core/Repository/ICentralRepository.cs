using FoodDiary.Core.Dto;

namespace FoodDiary.Core.Repository;

public interface ICentralRepository
{
    public Task AddFoodWithAmountsAsync(FoodAmountDto foodAmountDto);
}
