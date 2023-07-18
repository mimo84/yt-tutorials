using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using FoodDiary.Core.Dto;

namespace FoodDiary.Data.Services;

public class CsvImporter
{
    public CsvImporter() { }

    public List<FoodWithAmountDto> ReadCsv(string filePath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HeaderValidated = null,
            IgnoreBlankLines = true
        };

        // Read the CSV file using CsvHelper
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);
        var actualData = csv.GetRecords<FoodCsvFormat>().ToList();

        List<FoodWithAmountDto> foodWithAmountDtos = actualData
            .Select(
                data =>
                    new FoodWithAmountDto(
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
                            AmountName: $"{data.Amount} - {data.FoodName}"
                        )
                    )
            )
            .ToList();
        return foodWithAmountDtos;
    }
}
