using FoodDiary.Core.Dto;
using FoodDiary.Core.Handlers;
using FoodDiary.Core.Services;
using FoodDiary.Data.Contexts;
using FoodDiary.Data.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .AddEnvironmentVariables()
    .Build();

// Add services to the container.
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();
app.UseCors();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<FoodDiaryDbContext>();
    var csvImporter = new CsvImporter();
    List<FoodWithAmountDto> foods = csvImporter.ReadCsv("/Users/mimo/work/github_repos/yt-tutorials/db-to-model/backend/test/Unit/TestData/file input.csv");
    await Seed.SeedData(context, foods);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
public partial class Program { }
