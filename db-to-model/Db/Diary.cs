namespace db_to_model.Db;

public partial class Diary
{
    public int DiaryId { get; set; }

    public DateOnly Date { get; set; }

    public virtual Meal Meal { get; set; }
}
