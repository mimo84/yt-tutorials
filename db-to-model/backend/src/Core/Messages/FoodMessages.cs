using FoodDiary.Core.Dto;
using MediatR;

namespace FoodDiary.Core.Messages;

public record GetFoodsFromQuery(string Name)
    : IRequest<FoodEnvelope<List<FoodWithNutritionInfoDto>>>;

public record GetAllFoods() : IRequest<FoodEnvelope<List<FoodWithNutritionInfoDto>>>;

public record AddFoodWithAmounts(FoodEnvelope<FoodWithAmountDto> FoodWithAmount) : IRequest<Task>;
