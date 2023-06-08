﻿namespace FoodDiary.Core.Entities;

public partial class Diary
{
    public int DiaryId { get; set; }

    public DateOnly Date { get; set; }

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
}
