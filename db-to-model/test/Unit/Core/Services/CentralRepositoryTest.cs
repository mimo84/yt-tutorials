using FluentAssertions;
using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Services;
using Moq;
using Xunit;

namespace Unit.Core.Services;

public class CentralRepositoryTest
{

    public async void AddFoodWithAmountsAsyncTest()
    {
        var foodAmounts = new FoodAmountDto(Amount: 100M, Protein: 3.4M, 4M, 2.4M, 3.4M, 3.4M, 3.4M, 3.4M, 3.4M, 3.4M, 3.4M, 3.4M, 3.4M, "Mock Test", "Amount per 100");

        var foodWithAmounts = new FoodWithAmountDto("This is food", foodAmounts);

        var mockRepo = new Mock<IFoodHandler>();
        mockRepo
            .Setup(repo =>
                repo.AddFoodWithAmountsAsync(
                    It.IsAny<FoodWithAmountDto>(), It.IsAny<CancellationToken>()
                )
            ).Returns(Task.CompletedTask);

        await mockRepo.Object.AddFoodWithAmountsAsync(foodWithAmounts, CancellationToken.None);
    }
}
