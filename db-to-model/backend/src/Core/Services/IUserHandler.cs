using FoodDiary.Core.Dto;

namespace FoodDiary.Core.Services;

public interface IUserHandler
{
    public Task<UserDto> CreateAsync(NewUserDto newUser, CancellationToken cancellationToken);

    public Task<UserDto> LoginAsync(LoginUserDto login, CancellationToken cancellationToken);

    public Task<UserDto> GetAsync(string email, CancellationToken cancellationToken);
}
