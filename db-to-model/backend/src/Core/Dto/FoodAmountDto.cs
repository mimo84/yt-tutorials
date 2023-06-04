namespace FoodDiary.Core.Dto;

public record FoodAmountDto(
    decimal Amount,
    decimal Protein,
    decimal Fat,
    decimal Carbohydrates,
    decimal Fiber,
    decimal Alcohol,
    decimal Sugar,
    decimal SaturatedFats,
    decimal Sodium,
    decimal Cholesterol,
    decimal Potassium,
    decimal Iron,
    decimal Calcium,
    string Source,
    string AmountName
);