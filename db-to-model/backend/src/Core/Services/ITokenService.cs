using FoodDiary.Core.Entities;

namespace FoodDiary.Core.Services;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
