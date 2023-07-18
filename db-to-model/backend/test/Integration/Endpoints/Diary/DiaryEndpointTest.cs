using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using FoodDiary.Data.Contexts;
using Integration.Helpers;
using Integration.Helpers.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static Integration.ConfigureWebApplicationFactory;

namespace Integration.Endpoints;

public class DiaryEndpointTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient authHttpClient;
    private readonly CustomWebApplicationFactory<Program> factory;
    private readonly FoodDiaryDbContext dbContext;

    public DiaryEndpointTest(CustomWebApplicationFactory<Program> _factory)
    {
        factory = _factory;
        var scope = _factory.Services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        dbContext = scopedServices.GetRequiredService<FoodDiaryDbContext>();
        // Before any tests are run, we clean up the database state.
        // We don't do the cleanup at the end so that if a test fails we
        // can check what happened.
        DatabaseUtility.RestoreDatabase(dbContext);

        authHttpClient = AuthClientHelper.GetAuthClient(_factory);
    }

    [Fact]
    public async void TestMe()
    {
        var diaryDate = new DateTime(2023, 5, 23);
        var isoString = diaryDate.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
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

        var response = await authHttpClient.PostAsync("/Diary", c);
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

    [Fact]
    public async void Diary_MealsWithTheSameNameShouldBeInsertedOnlyOnce()
    {
        var diaryDate = new DateTime(2023, 4, 22);
        var isoString = diaryDate.ToString("s", System.Globalization.CultureInfo.InvariantCulture);
        var mealName = "dinner";
        var payload1 = $@"{{
            ""date"": ""{isoString}"",
            ""mealEntries"": [
                {{
                    ""name"": ""{mealName}"",
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
        var payload2 = $@"{{
            ""date"": ""{isoString}"",
            ""mealEntries"": [
                {{
                    ""name"": ""{mealName}"",
                    ""foodEntries"": [
                        {{
                            ""foodId"": 1211,
                            ""foodAmountId"": 1211,
                            ""consumedAmount"": 130
                        }}
                    ]
                }}
            ]
        }}";
        HttpContent c1 = new StringContent(payload1, Encoding.UTF8, "application/json");
        HttpContent c2 = new StringContent(payload2, Encoding.UTF8, "application/json");

        var response1 = await authHttpClient.PostAsync("/Diary", c1);
        var response2 = await authHttpClient.PostAsync("/Diary", c2);

        response1.StatusCode.Should().Be(HttpStatusCode.OK);
        response2.StatusCode.Should().Be(HttpStatusCode.OK);

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
        diary.Meals.First().Name.Should().Be(mealName);
    }
}
