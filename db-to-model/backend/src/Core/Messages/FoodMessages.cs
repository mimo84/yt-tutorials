using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using MediatR;

namespace FoodDiary.Core.Messages;

public record GetFoodsFromQuery(string Name) : IRequest<FoodEnvelope<List<Food>>>;
public record GetAllFoods() : IRequest<FoodEnvelope<List<FoodWithNutritionInfoDto>>>;