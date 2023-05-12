namespace FoodDiary.Core.Entities;

public partial class Food
{
    public int FoodId { get; set; }

    public string Name { get; set; }

    public virtual FoodAmount FoodAmount { get; set; }

    public virtual FoodMeal FoodMeal { get; set; }
}
