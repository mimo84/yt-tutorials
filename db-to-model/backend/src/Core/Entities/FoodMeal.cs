namespace FoodDiary.Core.Entities;

public partial class FoodMeal
{
    /// <summary>
    /// There can be multiple times the same food and food amount within the same meal
    /// </summary>
    public int FoodMealId { get; set; }

    public int FoodId { get; set; }

    public int MealId { get; set; }

    public int FoodAmountId { get; set; }

    public decimal ConsumedAmount { get; set; }

    public virtual Food Food { get; set; }

    public virtual FoodAmount FoodAmount { get; set; }

    public virtual Meal Meal { get; set; }
}
