using FoodDiary.Core.Dto;
using FoodDiary.Core.Messages;
using FoodDiary.Core.Repositories;
using MediatR;

namespace FoodDiary.Core.Handlers.Foods;

public class GetAllFoodsHandler
    : IRequestHandler<GetAllFoods, FoodEnvelope<List<FoodWithNutritionInfoDto>>>
{
    private readonly IFoodRepository foodRepository;

    public GetAllFoodsHandler(IFoodRepository _foodRepository)
    {
        foodRepository = _foodRepository;
    }

    public async Task<FoodEnvelope<List<FoodWithNutritionInfoDto>>> Handle(
        GetAllFoods request,
        CancellationToken cancellationToken
    )
    {
        var foods = await foodRepository.GetAllFoods(cancellationToken);
        var result = new FoodEnvelope<List<FoodWithNutritionInfoDto>>(foods);
        return result;
    }
}
