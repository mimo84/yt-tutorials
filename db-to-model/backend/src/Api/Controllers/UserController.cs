using System.Security.Claims;
using FoodDiary.Core.Dto;
using FoodDiary.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDiary.Api.Controllers;


[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ICentralRepository centralRepository;
    private readonly IUserRepository userHandler;
    public UserController(ICentralRepository _centralRepository, IUserRepository _userHandler)
    {
        centralRepository = _centralRepository;
        userHandler = _userHandler;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginUserDto loginDto, CancellationToken cancellationToken)
    {
        var user = await centralRepository.GetUserByEmailAsync(loginDto.Email, cancellationToken);

        if (user == null) return Unauthorized();

        await centralRepository.CheckUserAsync(user, loginDto.Password);

        return await userHandler.LoginAsync(loginDto, cancellationToken);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(NewUserDto registerDto, CancellationToken cancellationToken)
    {
        if (await centralRepository.GetUserByUsernameAsync(registerDto.UserName, cancellationToken) != default)
        {
            ModelState.AddModelError("username", "Username taken");
            return ValidationProblem();
        }

        if (await centralRepository.GetUserByEmailAsync(registerDto.Email, cancellationToken) != default)
        {
            ModelState.AddModelError("email", "Email taken");
            return ValidationProblem();
        }

        await centralRepository.AddUserAsync(registerDto, cancellationToken);
        var loginDto = new LoginUserDto(Email: registerDto.Email, Password: registerDto.Password);
        return await userHandler.LoginAsync(loginDto, cancellationToken);
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken cancellationToken)
    {
        var user = await userHandler.GetAsync(User.FindFirstValue(ClaimTypes.Email), cancellationToken);
        return user;
    }
}
