using FoodDiary.Core.Messages;
using FoodDiary.Core.Repositories;
using MediatR;

namespace FoodDiary.Core.Handlers.Diaries;

public class DeleteFoodFromMealHandler : IRequestHandler<DeleteFoodFromMeal, bool>
{
    private readonly IDiaryRepository diaryRepository;

    public DeleteFoodFromMealHandler(IDiaryRepository _diaryRepository)
    {
        diaryRepository = _diaryRepository;
    }

    public async Task<bool> Handle(DeleteFoodFromMeal request, CancellationToken cancellationToken)
    {
        await diaryRepository.DeleteFoodFromDiary(
            request.foodMealId,
            request.User,
            cancellationToken
        );

        return true;
    }
}
