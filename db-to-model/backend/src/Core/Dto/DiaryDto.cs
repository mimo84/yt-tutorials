namespace FoodDiary.Core.Dto;

/* Ingress */
public record DiaryIngressDto(DateTime Date, IList<DiaryMealIngress> MealEntries);

public record DiaryMealIngress(string Name, IList<DiaryFoodIngress> FoodEntries);

public record DiaryFoodIngress(int FoodId, int FoodAmountId, decimal ConsumedAmount);

/* Ingress With Food Names */
public record DiaryIngressWithFoodNamesDto(DateTime Date, IList<DiaryMealsWithNames> MealEntries);

public record DiaryMealsWithNames(string Name, IList<DiaryFoodWithNames> FoodEntries);

public record DiaryFoodWithNames(string FoodName, decimal ConsumedAmount);

/* Egress */
public record DiaryEntryDto(DateTime Date, IList<DiaryMealEgress> DiaryMealDtos);

public record DiaryMealEgress(string MealName, IList<DiaryMealFoodEgress> FoodDtos);

public record DiaryMealFoodEgress(FoodDto Food, FoodAmountDto FoodAmount, decimal ConsumedAmount);
