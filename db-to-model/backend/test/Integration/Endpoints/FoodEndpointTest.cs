using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using FluentAssertions;
using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Data.Contexts;
using Integration.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using static Integration.ConfigureWebApplicationFactory;

namespace Integration.Endpoints;

public class FoodEndpointTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    private readonly ITestOutputHelper testOutputHelper;
    private readonly CustomWebApplicationFactory<Program> factory;
    private readonly FoodDiaryDbContext dbContext;
    private readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public FoodEndpointTest(CustomWebApplicationFactory<Program> _factory, ITestOutputHelper _testOutputHelper)
    {

        factory = _factory;
        testOutputHelper = _testOutputHelper;
        var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        dbContext = scopedServices.GetRequiredService<FoodDiaryDbContext>();
        DatabaseUtility.RestoreDatabase(dbContext);
        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Theory(DisplayName = "Get Food By Name")]
    [InlineData("tasty")]
    [InlineData("woolworths")]
    [InlineData("abalone")]
    [InlineData("wpi")]
    [InlineData("bacon")]
    [InlineData("beef")]
    [InlineData("broccoli")]
    [InlineData("bRoccoli")]
    [InlineData("OniOn")]
    [InlineData("Oil")]
    [InlineData("Olive")]
    [InlineData("Stock")]
    [InlineData("parsley")]
    [InlineData("cucumber")]
    public async Task GetFoodNameTest(string search)
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

    [Theory(DisplayName = "Get Food By Multiple Name Parts")]
    [InlineData("woolworths tasty", "Woolworths Light Tasty Shredded Cheese (100g)")]
    [InlineData("Banana cavendish", "Banana cavendish peeled raw")]
    [InlineData("Cavendish Banana", "Banana cavendish peeled raw")]
    [InlineData("97% Fat", "Bacon Short Cut 97% Fat Free (Aldi)")]
    [InlineData("Fat 97%", "Bacon Short Cut 97% Fat Free (Aldi)")]
    [InlineData("Free 97% Fat Short", "Bacon Short Cut 97% Fat Free (Aldi)")]
    [InlineData("Cut 97", "Bacon Short Cut 97% Fat Free (Aldi)")]
    [InlineData("Margarine olive oil", "Margarine spread olive oil blend 65% fat reduced salt sodium 360mg/100 g")]
    public async Task GetFood(string search, string expected)
    {
        NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

        queryString.Add("name", search);
        var response = await _httpClient.GetAsync($"/food/find?name={queryString}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var stringResult = await response.Content.ReadAsStringAsync();

        var foodJson = JsonSerializer.Deserialize<FoodEnvelope<List<Food>>>(stringResult, jsonSerializerOptions) ?? throw new Exception($"Food {search} was not found");

        foodJson.Food.Count.Should().BeLessThanOrEqualTo(10, "we return up to 10 foods");
        foreach (var food in foodJson.Food)
        {
            food.Name.ToLower().Should().Be(expected.ToLower());
            food.FoodId.Should().BePositive();
        }
    }

    [Fact]
    public async Task FindAllFoods()
    {
        var response = await _httpClient.GetAsync("/food/all");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var stringResult = await response.Content.ReadAsStringAsync();
        var i = 0;
        var foodJson = JsonSerializer.Deserialize<FoodEnvelope<List<FoodWithNutritionInfoDto>>>(stringResult, jsonSerializerOptions) ?? throw new Exception($"All foods didn't return records as expected");
        foodJson.Food.Count.Should().BeGreaterThan(100, "we are expecting a lot of foods to be returned");
        foreach (var food in foodJson.Food)
        {
            food.Amount.Should().BeGreaterThan(0);
            food.FoodId.Should().BeGreaterThan(0);

            food.Calories.Should().BeGreaterThanOrEqualTo(0, "Some foods like salt, baking powder etc have no calories.");
            food.Protein.Should().BeGreaterThanOrEqualTo(0);
            food.Fat.Should().BeGreaterThanOrEqualTo(0);
            food.Carbohydrates.Should().BeGreaterThanOrEqualTo(0);
            i++;
        }
        testOutputHelper.WriteLine($">>>> {i} <<<<<<<< NUMBER OF FOODS TESTED");
    }

}
