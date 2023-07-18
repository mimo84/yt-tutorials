using FoodDiary.Core.Messages;
using FoodDiary.Core.Repositories;
using MediatR;

namespace FoodDiary.Core.Handlers.Diaries;

public class AddNewDiaryHandler : IRequestHandler<AddNewDiary, bool>
{
    private readonly IDiaryRepository diaryRepository;
    public AddNewDiaryHandler(IDiaryRepository _diaryRepository)
    {
        diaryRepository = _diaryRepository;
    }

    public async Task<bool> Handle(AddNewDiary request, CancellationToken cancellationToken)
    {
        await diaryRepository.CreateFullDiaryAsync(request.Request, request.User, cancellationToken);

        return true;
    }
}
