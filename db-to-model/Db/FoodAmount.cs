namespace db_to_model.Db;

public partial class FoodAmount
{
    public int FoodId { get; set; }

    public int FoodAmountId { get; set; }

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

    /// <summary>
    /// This is to help the user to know what kind of &quot;amount&quot; it is, is it a serving, is it based on weight.
    /// </summary>
    public string AmountName { get; set; }

    public virtual Food Food { get; set; }

    public virtual ICollection<FoodMeal> FoodMeals { get; set; } = new List<FoodMeal>();
}
