namespace FoodDiary.Core.Dto;

/* Ingress */
public record DiaryIngressDto(DateTime Date, IEnumerable<DiaryMealIngress> MealEntries);

public record DiaryMealIngress(string Name, IEnumerable<DiaryFoodIngress> FoodEntries);

public record DiaryFoodIngress(int FoodId, int FoodAmountId, decimal ConsumedAmount);

/* Egress */
public record DiaryEntryDto(DateTime Date, IEnumerable<DiaryMealEgress> DiaryMealDtos);

public record DiaryMealEgress(string MealName, IEnumerable<DiaryMealFoodEgress> FoodDtos);

public record DiaryMealFoodEgress(FoodDto Food, FoodAmountDto FoodAmount, decimal ConsumedAmount);
