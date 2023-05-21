using System.Net;
using System.Text;
using FluentAssertions;
using FoodDiary.Data.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using static E2e.ConfigureWebApplicationFactory;

namespace E2e;

public class DemoE2ETest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly CustomWebApplicationFactory<Program> factory;
    private readonly FoodDiaryDbContext dbContext;
    private readonly ITestOutputHelper testOutputHelper;


    public DemoE2ETest(CustomWebApplicationFactory<Program> _factory, ITestOutputHelper _testOutputHelper)
    {
        factory = _factory;
        testOutputHelper = _testOutputHelper;

        var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        dbContext = scopedServices.GetRequiredService<FoodDiaryDbContext>();

        _httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async void SwaggerTest()
    {
        var response = await _httpClient.GetAsync("/swagger/index.html");
        var stringResult = await response.Content.ReadAsStringAsync();

        stringResult.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async void TestMe()
    {
        var diaryDate = new DateOnly(2023, 5, 21);
        var diaryDateWithTime = diaryDate.ToDateTime(new TimeOnly(0, 0, 0), DateTimeKind.Utc);
        var isoString = diaryDateWithTime.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
        var payload = $@"{{
            ""date"": ""{isoString}"",
            ""mealEntries"": [
                {{
                    ""name"": ""testing"",
                    ""foodEntries"": [
                        {{
                            ""foodId"": 1340,
                            ""foodAmountId"": 1340,
                            ""consumedAmount"": 180
                        }}
                    ]
                }}
            ]
        }}";
        HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/Diary", c);
        var stringResult = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Using `SingleOrDefault` because we want the system to fail if more than one element is present
        var diary = await dbContext.Diaries.Where(d => d.Date == diaryDate).Include(d => d.Meals).SingleOrDefaultAsync();

        // If we don't find a diary, then we should throw an exception, also avoids a warning below
        if (diary == null)
        {
            Assert.Fail("Diary not found in the database");
            throw new Exception("Diary was not found in the database");
        }

        diary.Date.Should().Be(diaryDate);
        diary.DiaryId.Should().NotBe(null);
        diary.Meals.Count.Should().Be(1);

        stringResult.Should().Be("true");
    }
}
