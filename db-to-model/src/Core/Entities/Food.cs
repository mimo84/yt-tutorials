using System;
using System.Collections.Generic;

namespace FoodDiary.Core.Entities;

public partial class Food
{
    public int FoodId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<FoodAmount> FoodAmounts { get; set; } = new List<FoodAmount>();

    public virtual ICollection<FoodMeal> FoodMeals { get; set; } = new List<FoodMeal>();
}
