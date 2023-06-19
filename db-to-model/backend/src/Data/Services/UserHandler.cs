using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace FoodDiary.Data.Services;

public class UserHandler : IUserHandler
{
    private readonly ICentralRepository repository;
    private readonly UserManager<AppUser> userManager;

    public UserHandler(ICentralRepository _repository, UserManager<AppUser> _userManager)
    {
        repository = _repository;
        userManager = _userManager;
    }

    public async Task<UserDto> CreateAsync(NewUserDto newUser, CancellationToken cancellationToken)
    {
        await repository.AddUserAsync(newUser, cancellationToken);
        var token = "this should be a signed token";
        return new UserDto(DisplayName: newUser.DisplayName, Token: token, Bio: string.Empty, FirstName: newUser.FirstName, FamilyName: newUser.FamilyName, Address: newUser.Address);
    }

    public async Task<UserDto> LoginAsync(LoginUserDto login, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByEmailAsync(login.Email, cancellationToken);
        await userManager.CheckPasswordAsync(user, login.Password);
        var token = "this should be a generated token";
        return new UserDto(DisplayName: user.DisplayName, Token: token, Bio: user.Bio, FirstName: user.FirstName, FamilyName: user.FamilyName, Address: user.Address);
    }

    public async Task<UserDto> GetAsync(string email, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByEmailAsync(email, cancellationToken);
        var token = "this should be a generated token";
        return new UserDto(DisplayName: user.DisplayName, Token: token, Bio: user.Bio, FirstName: user.FirstName, FamilyName: user.FamilyName, Address: user.Address);
    }
}
