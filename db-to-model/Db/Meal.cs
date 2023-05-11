using System;
using System.Collections.Generic;

namespace db_to_model.Db;

public partial class Meal
{
    public int MealId { get; set; }

    public string? Name { get; set; }

    public int DiaryId { get; set; }

    public virtual Diary Diary { get; set; } = null!;

    public virtual FoodMeal MealNavigation { get; set; } = null!;
}
