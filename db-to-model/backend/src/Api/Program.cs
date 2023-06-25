using FoodDiary.Api.Extensions;
using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Handlers;
using FoodDiary.Core.Services;
using FoodDiary.Data.Contexts;
using FoodDiary.Data.Services;
using FoodDiary.Infrastructure.Extensions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddIdentityServices(builder.Configuration);
// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails(options =>
{
    // Only include exception details in a development environment. There's really no need
    // to set this as it's the default behavior. It's just included here for completeness :)
    options.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment();


    // This will map NotImplementedException to the 501 Not Implemented status code.
    options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);

    // You can configure the middleware to re-throw certain types of exceptions, all exceptions or based on a predicate.
    // This is useful if you have upstream middleware that  needs to do additional handling of exceptions.
    options.Rethrow<NotSupportedException>();

    // You can configure the middleware to ingore any exceptions of the specified type.
    // This is useful if you have upstream middleware that  needs to do additional handling of exceptions.
    // Note that unlike Rethrow, additional information will not be added to the exception.
    options.Ignore<DivideByZeroException>();

    // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
    // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
});
builder.Services.ConfigureOptions<ProblemDetailsLogging>();


builder.Services.AddDbContext<FoodDiaryDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
});

builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(
                    policy =>
                    {
                        policy
                            .WithOrigins(config.GetValue<string>("Cors:Origins").Split(','))
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

builder.Services.AddScoped<ICentralRepository, CentralRepository>();
builder.Services.AddScoped<IDiaryHandler, DiaryHandler>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<IUserHandler, UserHandler>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetFoodsFromQueryHandler>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();
app.UseProblemDetails();
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<FoodDiaryDbContext>();
    var csvImporter = new CsvImporter();
    List<FoodWithAmountDto> foods = csvImporter.ReadCsv("/Users/mimo/work/github_repos/yt-tutorials/db-to-model/backend/test/Unit/TestData/file input.csv");
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await Seed.SeedData(context, foods, userManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
public partial class Program { }
