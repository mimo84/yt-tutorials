namespace FoodDiary.Api.Models;

public record MealEnvelope<T>(T Meal);

public record MealResponse(
  int MealId,
  string MealName,
  List<FoodInMealResponse> FoodInMealResponse,
  decimal CaloriesInMeal
);
