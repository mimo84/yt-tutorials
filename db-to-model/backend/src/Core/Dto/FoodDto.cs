using FoodDiary.Core.Entities;
using MediatR;

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

public record GetFoodsFromQuery(string Name) : IRequest<FoodEnvelope<List<Food>>>;