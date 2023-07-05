using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;

namespace FoodDiary.Core.Repositories;

public interface ICentralRepository
{
    public Task AddUserAsync(NewUserDto newUserDto, CancellationToken cancellationToken);

    public Task<AppUser> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

    public Task<AppUser> GetUserByUsernameAsync(string username, CancellationToken cancellationToken);

    public Task CheckUserAsync(AppUser user, string password);
}
