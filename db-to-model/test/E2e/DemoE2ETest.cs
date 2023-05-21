using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace E2e;

public class DemoE2ETest
{
    private HttpClient _httpClient;
    public DemoE2ETest()
    {
        var webAppFactory = new WebApplicationFactory<Program>();
        _httpClient = webAppFactory.CreateDefaultClient();
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
        var payload = @"{
            ""date"": ""2023-05-21T06:18:20.273Z"",
            ""mealEntries"": [
                {
                        ""name"": ""testing"",
                ""foodEntries"": [
                    {
                            ""foodId"": 1340,
                    ""foodAmountId"": 1340,
                    ""consumedAmount"": 180
                    }
                ]
                }
            ]
        }";
        HttpContent c = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/Diary", c);
        var stringResult = await response.Content.ReadAsStringAsync();

        stringResult.Should().Be("true");
    }
}
