using System;
using System.Collections.Generic;

namespace db_to_model.Db;

public partial class Food
{
    public int FoodId { get; set; }

    public string? Name { get; set; }

    public virtual FoodAmount? FoodAmount { get; set; }
}
