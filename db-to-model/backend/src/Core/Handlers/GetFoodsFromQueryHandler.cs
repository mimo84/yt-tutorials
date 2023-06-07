using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Messages;
using FoodDiary.Core.Services;
using MediatR;

namespace FoodDiary.Core.Handlers;

public class GetFoodsFromQueryHandler : IRequestHandler<GetFoodsFromQuery, FoodEnvelope<List<Food>>>
{
    private readonly IFoodRepository foodRepository;
    public GetFoodsFromQueryHandler(IFoodRepository _foodRepository)
    {
        foodRepository = _foodRepository;
    }

    public async Task<FoodEnvelope<List<Food>>> Handle(GetFoodsFromQuery request, CancellationToken cancellationToken)
    {
        var foods = await foodRepository.FindFood(request.Name, cancellationToken);
        var result = new FoodEnvelope<List<Food>>(foods);
        return result;
    }
}
