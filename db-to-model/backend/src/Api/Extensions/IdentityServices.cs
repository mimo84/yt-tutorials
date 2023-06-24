using FoodDiary.Core.Entities;
using FoodDiary.Core.Services;
using FoodDiary.Data.Contexts;
using FoodDiary.Data.Services;

namespace FoodDiary.Api.Extensions;

public static class IdentityServices
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services,
        IConfiguration config)
    {
        // AddIdentity would add redirection behaviours which can create issues
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<FoodDiaryDbContext>();

        services.AddAuthentication();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}

