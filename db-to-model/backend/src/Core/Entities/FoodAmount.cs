namespace FoodDiary.Core.Entities;

public partial class FoodAmount
{
    public int FoodId { get; set; }

    public int FoodAmountId { get; set; }

    public string AmountName { get; set; }

    public decimal Amount { get; set; }

    public decimal? Protein { get; set; }

    public decimal? Fat { get; set; }

    public decimal? Carbohydrates { get; set; }

    public decimal? Fiber { get; set; }

    public decimal? Alcohol { get; set; }

    public decimal? Sugar { get; set; }

    public decimal? SaturatedFats { get; set; }

    public decimal? Sodium { get; set; }

    public decimal? Cholesterol { get; set; }

    public decimal? Potassium { get; set; }

    public decimal? Iron { get; set; }

    public decimal? Calcium { get; set; }

    public string Source { get; set; }

    public virtual Food Food { get; set; }

    public virtual ICollection<FoodMeal> FoodMeals { get; set; } = new List<FoodMeal>();
}
