using FoodDiary.Core.Entities;
using FoodDiary.Core.Models;

namespace FoodDiary.Core.Mappers;

public static class DiariesMapper
{
    public static DiaryResponse MapFromDiaryEntity(Diary diary)
    {
        var meals = diary.Meals.Select(m => MealsMapper.MapFromMealEntity(m)).ToList();
        var caloriesInDiary = meals.Sum(m => m.CaloriesInMeal);

        var result = new DiaryResponse(
            DiaryId: diary.DiaryId,
            DiaryDate: diary.Date,
            Meals: meals,
            CaloriesInDiary: caloriesInDiary
        );
        return result;
    }

    public static DiariesResponse MapFromDiariesEntity(List<Diary> diaries)
    {
        var response = diaries.Select(MapFromDiaryEntity).ToList();
        return new DiariesResponse(Diaries: response);
    }
}
