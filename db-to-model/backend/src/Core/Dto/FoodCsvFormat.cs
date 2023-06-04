namespace FoodDiary.Core.Dto;

public record FoodCsvFormat(
  string FoodName,

  decimal Amount,

  decimal Protein,

  decimal Fat,

  decimal Carbs,

  decimal Fiber,

  decimal? Alcohol,

  decimal? Sugar,

  decimal? SaturatedFats,

  decimal? Sodium,

  decimal? Cholesterol,

  decimal? Potassium,

  decimal? Iron,

  decimal? Calcium,

  string Source
)
{
    public decimal? Alcohol { get; init; } = Alcohol ?? 0;
    public decimal? Sugar { get; init; } = Sugar ?? 0;
    public decimal? SaturatedFats { get; init; } = SaturatedFats ?? 0;
    public decimal? Sodium { get; init; } = Sodium ?? 0;
    public decimal? Cholesterol { get; init; } = Cholesterol ?? 0;
    public decimal? Potassium { get; init; } = Potassium ?? 0;
    public decimal? Iron { get; init; } = Iron ?? 0;
    public decimal? Calcium { get; init; } = Calcium ?? 0M;
    public string Source { get; init; } = Source ?? "CSV NOT PRESENT";
};