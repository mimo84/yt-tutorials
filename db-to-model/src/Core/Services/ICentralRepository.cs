using FoodDiary.Core.Dto;

namespace FoodDiary.Core.Services;

public interface ICentralRepository
{
    public Task AddFoodWithAmountsAsync(FoodWithAmountDto foodAmountDto, CancellationToken cancellationToken);
}
