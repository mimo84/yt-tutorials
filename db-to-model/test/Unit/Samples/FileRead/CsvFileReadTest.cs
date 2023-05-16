using System.Globalization;
using System.Reflection;
using CsvHelper;
using CsvHelper.Configuration;
using FluentAssertions;
using FoodDiary.Core.Dto;
using Xunit;
using Xunit.Abstractions;

namespace Unit.Samples;

public class FileTest
{
    private readonly ITestOutputHelper testOutputHelper;
    public FileTest(ITestOutputHelper _testOutputHelper)
    {
        testOutputHelper = _testOutputHelper;
    }

    [Fact]
    public void FileReadTest()
    {
        var codeBaseAbsolutePath = new Uri(Assembly.GetExecutingAssembly().Location).AbsolutePath;
        var codeBasePath = Uri.UnescapeDataString(codeBaseAbsolutePath);
        var dirPath = Path.GetDirectoryName(codeBasePath);
        var filePath = Path.Combine(dirPath, "TestData", "file input.csv");

        var testFileShould = File.Exists(filePath);
        testFileShould.Should().BeTrue();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            IgnoreBlankLines = true
        };

        // Read the CSV file using CsvHelper
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);
        var actualData = csv.GetRecords<FoodCsvFormat>().ToList();
        foreach (var data in actualData)
        {
            FoodWithAmountDto foodWithAmountDto = new FoodWithAmountDto(
              Name: data.FoodName,
              FoodAmount: new FoodAmountDto(
                    Amount: data.Amount,
                    Protein: data.Protein,
                    Fat: data.Fat,
                    Carbohydrates: data.Carbs,
                    Fiber: data.Fiber,
                    Alcohol: data.Alcohol ?? 0,
                    Sugar: data.Sugar ?? 0,
                    SaturatedFats: data.SaturatedFats ?? 0,
                    Sodium: data.Sodium ?? 0,
                    Cholesterol: data.Cholesterol ?? 0,
                    Potassium: data.Potassium ?? 0,
                    Iron: data.Iron ?? 0,
                    Calcium: data.Calcium ?? 0,
                    Source: data.Source,
                    AmountName: $"{data.Amount} - ${data.FoodName}"
              )
            );

            foodWithAmountDto.Name.Should().NotBeEmpty();
            foodWithAmountDto.FoodAmount.Protein.Should().BeGreaterThanOrEqualTo(0);
            foodWithAmountDto.FoodAmount.Fat.Should().BeGreaterThanOrEqualTo(0);
            foodWithAmountDto.FoodAmount.Carbohydrates.Should().BeGreaterThanOrEqualTo(0);
            foodWithAmountDto.FoodAmount.Fiber.Should().BeGreaterThanOrEqualTo(0);
            foodWithAmountDto.FoodAmount.Alcohol.Should().BeGreaterThanOrEqualTo(0);
            foodWithAmountDto.FoodAmount.Sugar.Should().BeGreaterThanOrEqualTo(0);
            foodWithAmountDto.FoodAmount.SaturatedFats.Should().BeGreaterThanOrEqualTo(0);
            foodWithAmountDto.FoodAmount.Sodium.Should().BeGreaterThanOrEqualTo(0);
            foodWithAmountDto.FoodAmount.Cholesterol.Should().BeGreaterThanOrEqualTo(0);
            foodWithAmountDto.FoodAmount.Potassium.Should().BeGreaterThanOrEqualTo(0);
            foodWithAmountDto.FoodAmount.Iron.Should().BeGreaterThanOrEqualTo(0);
            foodWithAmountDto.FoodAmount.Calcium.Should().BeGreaterThanOrEqualTo(0);
            foodWithAmountDto.FoodAmount.Source.Should().NotBe("CSV NOT PRESENT");
        }
    }
}
