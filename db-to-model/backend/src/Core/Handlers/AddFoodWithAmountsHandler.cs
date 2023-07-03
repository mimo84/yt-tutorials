using FoodDiary.Core.Messages;
using FoodDiary.Core.Services;
using MediatR;

namespace FoodDiary.Core.Handlers;

public class AddFoodWithAmountsHandler : IRequestHandler<AddFoodWithAmounts, Task>
{
    private readonly IFoodRepository foodRepository;
    public AddFoodWithAmountsHandler(IFoodRepository _foodRepository)
    {
        foodRepository = _foodRepository;
    }

    public async Task<Task> Handle(AddFoodWithAmounts request, CancellationToken cancellationToken)
    {
        await foodRepository.AddFoodWithAmountsAsync(request.FoodWithAmount.Food, cancellationToken);
        return Task.CompletedTask;

    }
}

