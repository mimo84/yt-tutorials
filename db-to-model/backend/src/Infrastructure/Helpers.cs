namespace FoodDiary.Infrastructure;

public static class Helpers
{
    public static decimal CalculateCalories(decimal Protein, decimal Carbohydrates, decimal Fat)
    {
        return 4 * Protein + 8 * Fat + 4 * Carbohydrates;
    }
}
