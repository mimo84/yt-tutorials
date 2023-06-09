using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;

namespace FoodDiary.Core.Mappers;

public static class FoodMapper
{
    public static FoodWithNutritionInfoDto MapToFoodWithNutritionalInfo(Food food)
    {
        var foodAmount = food.FoodAmounts.FirstOrDefault();

        var protein = foodAmount.Protein;
        var fat = foodAmount.Fat;
        var carbohydrates = foodAmount.Carbohydrates;
        var calories = 4 * protein + 8 * fat + 4 * carbohydrates;

        return new FoodWithNutritionInfoDto(
          FoodId: food.FoodId,
          FoodName: food.Name,
          Amount: foodAmount.Amount,
          Fat: fat ?? 0,
          Protein: protein ?? 0,
          Carbohydrates: carbohydrates ?? 0,
          Calories: calories ?? 0
        );
    }
}
