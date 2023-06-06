namespace FoodDiary.Api.Models;

public record FoodEnvelope<T>(T Food);

public record FoodInMealResponse(
  int FoodId,
  string FoodName,
  decimal ConsumedAmount,
  decimal Fat,
  decimal Protein,
  decimal Carbohydrates,
  decimal Calories
);