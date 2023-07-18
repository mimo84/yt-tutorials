using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Repositories;
using FoodDiary.Data.Contexts;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Data.Repositories;

public class CentralRepository : ICentralRepository
{
    private readonly FoodDiaryDbContext dbContext;

    private readonly UserManager<AppUser> userManager;

    public CentralRepository(FoodDiaryDbContext _dbContext, UserManager<AppUser> _userManager)
    {
        dbContext = _dbContext;
        userManager = _userManager;
    }

    public async Task AddUserAsync(NewUserDto newUserDto, CancellationToken cancellationToken)
    {
        if (
            await dbContext.Users.AnyAsync(
                x => x.UserName == newUserDto.UserName,
                cancellationToken
            )
        )
        {
            throw new ProblemDetailsException(
                new ValidationProblemDetails
                {
                    Status = 422,
                    Detail = "Cannot register user",
                    Errors =
                    {
                        new KeyValuePair<string, string[]>(
                            "UserName",
                            new[] { "UserName not available" }
                        )
                    }
                }
            );
        }

        if (await dbContext.Users.AnyAsync(x => x.Email == newUserDto.Email, cancellationToken))
        {
            throw new ProblemDetailsException(
                new ValidationProblemDetails
                {
                    Status = 422,
                    Detail = "Cannot register user",
                    Errors =
                    {
                        new KeyValuePair<string, string[]>(
                            "Email",
                            new[] { "Email address already in use" }
                        )
                    }
                }
            );
        }

        var user = new AppUser()
        {
            UserName = newUserDto.UserName,
            Email = newUserDto.Email,
            DisplayName = newUserDto.DisplayName,
            Address = newUserDto.Address,
            Bio = newUserDto.Bio,
            FamilyName = newUserDto.FamilyName,
            FirstName = newUserDto.FirstName,
        };
        var result = await userManager.CreateAsync(user, newUserDto.Password);
        if (result.Errors.Any())
        {
            var errorDetail = string.Join("", result.Errors.Select(e => e.Description).ToArray());
            throw new ProblemDetailsException(
                new ProblemDetails { Status = 422, Detail = $"Cannot register user: {errorDetail}" }
            );
        }
        return;
    }

    public async Task<AppUser> GetUserByEmailAsync(
        string email,
        CancellationToken cancellationToken
    )
    {
        return await userManager.Users.FirstOrDefaultAsync(
            x => x.Email == email,
            cancellationToken
        );
    }

    public Task<AppUser> GetUserByUsernameAsync(
        string username,
        CancellationToken cancellationToken
    )
    {
        return userManager.Users.FirstOrDefaultAsync(
            x => x.UserName == username,
            cancellationToken
        );
    }

    public async Task CheckUserAsync(AppUser user, string password)
    {
        var checkedUser = await userManager.CheckPasswordAsync(user, password);
        if (!checkedUser)
        {
            throw new ProblemDetailsException(
                new ValidationProblemDetails
                {
                    Status = 422,
                    Detail = "Incorrect Credentials",
                    Errors =
                    {
                        new KeyValuePair<string, string[]>(
                            "Credentials",
                            new[] { "incorrect credentials" }
                        )
                    }
                }
            );
        }
    }
}
