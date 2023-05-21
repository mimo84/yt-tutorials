using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using static E2e.ConfigureWebApplicationFactory;

namespace E2e;

public class DemoE2ETest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private readonly CustomWebApplicationFactory<Program>
        _factory;
    public DemoE2ETest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
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
