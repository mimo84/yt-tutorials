using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Infrastructure;

namespace FoodDiary.Core.Extensions;

public static class FoodExtensions
{
    public static FoodWithNutritionInfoDto AsDto(this Food food)
    {
        var nutritionInfo = food.FoodAmounts.First();
        var protein = nutritionInfo.Protein ?? 0;
        var fat = nutritionInfo.Fat ?? 0;
        var carbohydrates = nutritionInfo.Carbohydrates ?? 0;
        var calories = Helpers.CalculateCalories(protein, carbohydrates, fat);
        return new FoodWithNutritionInfoDto(
          food.FoodId,
          food.Name,
          nutritionInfo.Amount,
          protein,
          fat,
          carbohydrates,
          calories
        );
    }
}
