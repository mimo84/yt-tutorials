namespace FoodDiary.Api.Models;

public record DiaryEnvelope<T>(T Diary);

public record DiaryResponse(
  int DiaryId,
  DateOnly DiaryDate,
  List<MealResponse> Meals,
  decimal CaloriesInDiary
);

public record DiariesResponse(IEnumerable<DiaryResponse> Diaries);