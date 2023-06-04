using FoodDiary.Api.Models;
using FoodDiary.Core.Entities;

namespace FoodDiary.Api.Mappers;

public static class MealsMapper
{
    public static MealResponse MapFromMealEntity(Meal meal)
    {
        var result = new MealResponse(
          MealId: meal.MealId,
          MealName: meal.Name
        );
        return result;
    }
}
