﻿namespace FoodDiary.Core.Entities;

public partial class Meal
{
    public int MealId { get; set; }

    public int DiaryId { get; set; }

    public string Name { get; set; }

    public virtual Diary Diary { get; set; }

    public virtual ICollection<FoodMeal> FoodMeals { get; set; }
}
