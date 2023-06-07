using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Net;
using System.Text.Json;
using CsvHelper.Configuration;
using FluentAssertions;
using FoodDiary.Api.Models;
using FoodDiary.Core.Entities;
using FoodDiary.Data.Contexts;
using Integration.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
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
}