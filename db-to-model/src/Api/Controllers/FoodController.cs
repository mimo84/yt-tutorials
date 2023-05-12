using FoodDiary.Api.Models;
using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Data.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FoodController : ControllerBase
{
    private readonly FoodDiaryDbContext dbContext;
    public FoodController(FoodDiaryDbContext _dbContext)
    {
        dbContext = _dbContext;
    }

    [HttpPost("new", Name = "NewFood")]
    public async Task<FoodEnvelope<bool>> CreateAsync(
        RequestEnvelope<FoodEnvelope<FoodWithAmountDto>> request,
        CancellationToken cancellationToken)
    {
        Food food = new();
        food.Name = request.Body.Food.Name;

        var foodAmount = new FoodAmount()
        {
            Amount = request.Body.Food.FoodAmount.Amount,
            Protein = request.Body.Food.FoodAmount.Protein,
            Fat = request.Body.Food.FoodAmount.Fat,
            Carbohydrates = request.Body.Food.FoodAmount.Carbohydrates,
            Fiber = request.Body.Food.FoodAmount.Fiber,
            Alcohol = request.Body.Food.FoodAmount.Alcohol,
            Sugar = request.Body.Food.FoodAmount.Sugar,
            SaturatedFats = request.Body.Food.FoodAmount.SaturatedFats,
            Sodium = request.Body.Food.FoodAmount.Sodium,
            Cholesterol = request.Body.Food.FoodAmount.Cholesterol,
            Potassium = request.Body.Food.FoodAmount.Potassium,
            Iron = request.Body.Food.FoodAmount.Iron,
            Calcium = request.Body.Food.FoodAmount.Calcium,
            Source = request.Body.Food.FoodAmount.Source,
            AmountName = request.Body.Food.FoodAmount.AmountName,
            Food = food,
        };
        await dbContext.FoodAmounts.AddAsync(foodAmount, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new FoodEnvelope<bool>(true);
    }
}
