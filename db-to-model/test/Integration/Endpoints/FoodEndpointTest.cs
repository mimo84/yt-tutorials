using System.Net;
using System.Text.Json;
using FluentAssertions;
using FoodDiary.Api.Models;
using FoodDiary.Core.Entities;
using FoodDiary.Data.Contexts;
using Integration.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static Integration.ConfigureWebApplicationFactory;

namespace Integration.Endpoints;

public class FoodEndpointTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly CustomWebApplicationFactory<Program> factory;
    private readonly FoodDiaryDbContext dbContext;
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public FoodEndpointTest(CustomWebApplicationFactory<Program> _factory)
    {

        factory = _factory;
        var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        dbContext = scopedServices.GetRequiredService<FoodDiaryDbContext>();
        DatabaseUtility.RestoreDatabase(dbContext);
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact(DisplayName = "Get Food By Name")]
    public async Task GetFoodNameTest()
    {
        List<string> searches = new() { "tasty", "woolworths", "abalone", "wpi", "bacon", "beef", "broccoli", "bRoccoli", "OniOn", "Oil Olive", "Olive", "Stock", "parsley", "cucumber" };

        foreach (var search in searches)
        {
            var response = await _httpClient.GetAsync($"/Food/find?Name={search}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var stringResult = await response.Content.ReadAsStringAsync();

            var foodJson = JsonSerializer.Deserialize<FoodEnvelope<List<Food>>>(stringResult, jsonSerializerOptions) ?? throw new Exception($"Food {search} was not found");

            foodJson.Food.Count.Should().BeLessThanOrEqualTo(10, "we return up to 10 foods");
            foreach (var food in foodJson.Food)
            {
                food.Name.ToLower().Should().Contain(search.ToLower());
                food.FoodId.Should().BePositive();
            }
        }
    }

}
