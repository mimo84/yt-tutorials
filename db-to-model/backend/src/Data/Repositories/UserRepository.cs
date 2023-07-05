using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Repositories;
using FoodDiary.Core.Services;
using Microsoft.AspNetCore.Identity;

namespace FoodDiary.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ICentralRepository repository;
    private readonly UserManager<AppUser> userManager;

    private readonly ITokenService tokenService;

    public UserRepository(ICentralRepository _repository, UserManager<AppUser> _userManager, ITokenService _tokenService)
    {
        repository = _repository;
        userManager = _userManager;
        tokenService = _tokenService;
    }

    public async Task<UserDto> CreateAsync(NewUserDto newUser, CancellationToken cancellationToken)
    {
        await repository.AddUserAsync(newUser, cancellationToken);
        var user = await repository.GetUserByEmailAsync(newUser.Email, cancellationToken);
        var token = tokenService.CreateToken(user);
        return new UserDto(DisplayName: user.DisplayName, Token: token, Bio: string.Empty, FirstName: user.FirstName, FamilyName: user.FamilyName, Address: user.Address);
    }

    public async Task<UserDto> LoginAsync(LoginUserDto login, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByEmailAsync(login.Email, cancellationToken);
        await userManager.CheckPasswordAsync(user, login.Password);
        var token = tokenService.CreateToken(user);
        return new UserDto(DisplayName: user.DisplayName, Token: token, Bio: user.Bio, FirstName: user.FirstName, FamilyName: user.FamilyName, Address: user.Address);
    }

    public async Task<UserDto> GetAsync(string email, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByEmailAsync(email, cancellationToken);
        var token = tokenService.CreateToken(user);
        return new UserDto(DisplayName: user.DisplayName, Token: token, Bio: user.Bio, FirstName: user.FirstName, FamilyName: user.FamilyName, Address: user.Address);
    }
}
