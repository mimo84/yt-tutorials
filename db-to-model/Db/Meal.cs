namespace db_to_model.Db;

public partial class Meal
{
    public int MealId { get; set; }

    public string Name { get; set; }

    public int DiaryId { get; set; }

    public virtual Diary Diary { get; set; }

    public virtual FoodMeal MealNavigation { get; set; }
}
