using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDiary.Core.Entities;

public partial class Diary
{
    public int DiaryId { get; set; }

    public DateTime Date { get; set; }

    [Column("user_id")]
    public string AppUserId { get; set; }

    public AppUser AppUser { get; set; }

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
}
