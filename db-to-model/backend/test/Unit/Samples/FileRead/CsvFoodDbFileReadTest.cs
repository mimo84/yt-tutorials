using System.Reflection;
using FluentAssertions;
using FoodDiary.Core.Dto;
using FoodDiary.Data.Services;
using Xunit;
using Xunit.Abstractions;

namespace Unit.Samples;

public class CsvFoodDbFileReadTest
{
    private readonly ITestOutputHelper testOutputHelper;

    public CsvFoodDbFileReadTest(ITestOutputHelper _testOutputHelper)
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

        var csvImporter = new CsvImporter();
        List<FoodWithAmountDto> foods = csvImporter.ReadCsv(filePath);
        foreach (var data in foods)
        {
            data.Name.Should().NotBeEmpty();
            data.FoodAmount.Protein.Should().BeGreaterThanOrEqualTo(0);
            data.FoodAmount.Fat.Should().BeGreaterThanOrEqualTo(0);
            data.FoodAmount.Carbohydrates.Should().BeGreaterThanOrEqualTo(0);
            data.FoodAmount.Fiber.Should().BeGreaterThanOrEqualTo(0);
            data.FoodAmount.Alcohol.Should().BeGreaterThanOrEqualTo(0);
            data.FoodAmount.Sugar.Should().BeGreaterThanOrEqualTo(0);
            data.FoodAmount.SaturatedFats.Should().BeGreaterThanOrEqualTo(0);
            data.FoodAmount.Sodium.Should().BeGreaterThanOrEqualTo(0);
            data.FoodAmount.Cholesterol.Should().BeGreaterThanOrEqualTo(0);
            data.FoodAmount.Potassium.Should().BeGreaterThanOrEqualTo(0);
            data.FoodAmount.Iron.Should().BeGreaterThanOrEqualTo(0);
            data.FoodAmount.Calcium.Should().BeGreaterThanOrEqualTo(0);
            data.FoodAmount.Source.Should().NotBe("CSV NOT PRESENT");
        }
    }
}
