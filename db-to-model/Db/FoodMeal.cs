﻿using System;
using System.Collections.Generic;

namespace db_to_model.Db;

public partial class FoodMeal
{
    public int FoodId { get; set; }

    public int MealId { get; set; }

    public int FoodAmountId { get; set; }

    public decimal Amount { get; set; }

    /// <summary>
    /// There can be multiple times the same food and food amount within the same meal
    /// </summary>
    public int FoodMealId { get; set; }

    public virtual FoodAmount FoodAmount { get; set; } = null!;

    public virtual Meal? Meal { get; set; }
}