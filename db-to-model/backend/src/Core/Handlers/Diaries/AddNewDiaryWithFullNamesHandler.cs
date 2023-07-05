using FoodDiary.Core.Messages;
using FoodDiary.Core.Repositories;
using MediatR;

namespace FoodDiary.Core.Handlers.Diaries;

public class AddNewDiaryWithFullNamesHandler : IRequestHandler<AddNewDiaryWithFoodNames, bool>
{
    private readonly IDiaryRepository diaryRepository;
    public AddNewDiaryWithFullNamesHandler(IDiaryRepository _diaryRepository)
    {
        diaryRepository = _diaryRepository;
    }

    public async Task<bool> Handle(AddNewDiaryWithFoodNames request, CancellationToken cancellationToken)
    {
        await diaryRepository.CreateFullDiaryWithNamesAsync(request.Request, cancellationToken);

        return true;
    }
}
