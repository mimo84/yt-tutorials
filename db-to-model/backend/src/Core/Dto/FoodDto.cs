namespace FoodDiary.Core.Dto;

public record FoodDto(string Name);

public record FoodWithAmountDto(string Name, FoodAmountDto FoodAmount);

public record FoodQuery(string Name);