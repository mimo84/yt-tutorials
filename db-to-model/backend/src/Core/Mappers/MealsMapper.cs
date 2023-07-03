using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Models;
using FoodDiary.Infrastructure;

namespace FoodDiary.Core.Mappers;

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

        var protein = ratio * food.FoodAmount.Protein ?? 0;
        var fat = ratio * food.FoodAmount.Fat ?? 0;
        var carbohydrates = ratio * food.FoodAmount.Carbohydrates ?? 0;
        var calories = Helpers.CalculateCalories(protein, carbohydrates, fat);

        return new FoodInMealResponse(
          FoodId: food.FoodId,
          FoodName: food.Food.Name,
          ConsumedAmount: food.ConsumedAmount,
          Fat: fat,
          Protein: protein,
          Carbohydrates: carbohydrates,
          Calories: calories
        );
    }
}