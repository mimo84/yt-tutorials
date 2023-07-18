namespace FoodDiary.Core.Dto;

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

public record FoodDto(string Name);

public record FoodWithAmountDto(string Name, FoodAmountDto FoodAmount);

public record FoodWithNutritionInfoDto(
    int FoodId,
    string FoodName,
    decimal Amount,
    decimal Protein,
    decimal Fat,
    decimal Carbohydrates,
    decimal Calories
);
