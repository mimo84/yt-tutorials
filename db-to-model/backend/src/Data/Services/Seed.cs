using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Data.Services;

public class Seed
{
    public static async Task SeedData(FoodDiaryDbContext dbContext, List<FoodWithAmountDto> foodWithAmountDtos)
    {
        var anyFoods = await dbContext.Foods.AnyAsync();
        if (anyFoods)
        {
            Console.WriteLine("Database seems to be already seeded. Seeding will not proceed.");
            return;
        }

        List<Food> foodToInsert = foodWithAmountDtos.Select(x => new Food()
        {
            Name = x.Name,
            FoodAmounts = new List<FoodAmount>()
            {
              new FoodAmount()
              {
                AmountName = x.FoodAmount.AmountName,
                Amount = x.FoodAmount.Amount,
                Protein = x.FoodAmount.Protein,
                Fat = x.FoodAmount.Fat,
                Carbohydrates = x.FoodAmount.Carbohydrates,
                Fiber = x.FoodAmount.Fiber,
                Alcohol = x.FoodAmount.Alcohol,
                Sugar = x.FoodAmount.Sugar,
                SaturatedFats = x.FoodAmount.SaturatedFats,
                Sodium = x.FoodAmount.Sodium,
                Cholesterol = x.FoodAmount.Cholesterol,
                Potassium = x.FoodAmount.Potassium,
                Iron = x.FoodAmount.Iron,
                Calcium = x.FoodAmount.Calcium,
                Source = x.FoodAmount.Source,
              }
            }

        }).ToList();

        dbContext.AddRange(foodToInsert);
        await dbContext.SaveChangesAsync();
    }
}
