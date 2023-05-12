using System;

namespace FoodDiary.Core.Entities;

public partial class Diary
{
    public int DiaryId { get; set; }

    public DateOnly Date { get; set; }

    public virtual Meal Meal { get; set; }
}
