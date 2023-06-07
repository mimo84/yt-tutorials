using FoodDiary.Api.Models;
using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;

namespace FoodDiary.Api.Mappers;

public static class MealsMapper
{
    public static MealResponse MapFromMealEntity(Meal meal)
    {
        var foodInMeal = meal.FoodMeals.Select(f => MapFromFoodMealEntity(f)).ToList();
        var totalCaloriesInMeal = foodInMeal.Sum(f => f.Calories);
        return new MealResponse(
          MealId: meal.MealId,
          MealName: meal.Name,
          FoodInMealResponse: foodInMeal,
          CaloriesInMeal: totalCaloriesInMeal
        );
    }

    public static FoodInMealResponse MapFromFoodMealEntity(FoodMeal food)
    {
        var amount = food.FoodAmount.Amount;
        var ratio = Decimal.Divide(food.ConsumedAmount, amount);

        var protein = ratio * food.FoodAmount.Protein;
        var fat = ratio * food.FoodAmount.Fat;
        var carbohydrates = ratio * food.FoodAmount.Carbohydrates;
        var calories = 4 * protein + 8 * fat + 4 * carbohydrates;

        return new FoodInMealResponse(
          FoodId: food.FoodId,
          FoodName: food.Food.Name,
          ConsumedAmount: food.ConsumedAmount,
          Fat: fat ?? 0,
          Protein: protein ?? 0,
          Carbohydrates: carbohydrates ?? 0,
          Calories: calories ?? 0
        );
    }
}