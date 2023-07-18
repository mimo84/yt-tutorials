using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;

namespace FoodDiary.Core.Repositories;

public interface IUserRepository
{
    public Task<UserDto> CreateAsync(NewUserDto newUser, CancellationToken cancellationToken);

    public Task<UserDto> LoginAsync(LoginUserDto login, CancellationToken cancellationToken);

    public Task<UserDto> GetAsync(string email, CancellationToken cancellationToken);

    public Task<AppUser> GetAppUserAsync(string email, CancellationToken cancellationToken);
}
