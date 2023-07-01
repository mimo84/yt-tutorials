using Microsoft.AspNetCore.Identity;

namespace FoodDiary.Core.Entities;

public partial class AppUser : IdentityUser
{
    public string DisplayName { get; set; }

    public string Bio { get; set; }

    public string FirstName { get; set; }

    public string FamilyName { get; set; }

    public string Address { get; set; }

    public ICollection<Diary> Diaries { get; set; }

}
