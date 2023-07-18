namespace FoodDiary.Core.Models;

public record DiaryEnvelope<T>(T Diary);

public record DiaryResponse(
    int DiaryId,
    DateTime DiaryDate,
    List<MealResponse> Meals,
    decimal CaloriesInDiary
);

public record DiariesResponse(IEnumerable<DiaryResponse> Diaries);
