using FoodDiary.Core.Entities;
using FoodDiary.Data.Contexts;

namespace FoodDiary.Api.Extensions;

public static class IdentityServicesExtensions
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

        return services;
    }
}

