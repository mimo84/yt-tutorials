using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Services;
using FoodDiary.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Data.Services;

public class FoodRepository : IFoodRepository
{
    private readonly FoodDiaryDbContext dbContext;
    public FoodRepository(FoodDiaryDbContext _dbContext)
    {
        dbContext = _dbContext;
    }

    public async Task<List<Food>> FindFood(string search, CancellationToken cancellationToken)
    {
        IQueryable<Food> query = dbContext.Foods;
        var searchTerms = search.Split(' ').ToList();
        foreach (var term in searchTerms)
        {
            query = query.Where(f => EF.Functions.ILike(f.Name, '%' + term + '%'));
        }

        var results = await query.Take(10).ToListAsync(cancellationToken);
        return results;
    }

    public async Task AddFoodWithAmountsAsync(FoodWithAmountDto foodAmountDto, CancellationToken cancellationToken)
    {
        Food food = new()
        {
            Name = foodAmountDto.Name
        };

        var foodAmount = new FoodAmount()
        {
            Amount = foodAmountDto.FoodAmount.Amount,
            Protein = foodAmountDto.FoodAmount.Protein,
            Fat = foodAmountDto.FoodAmount.Fat,
            Carbohydrates = foodAmountDto.FoodAmount.Carbohydrates,
            Fiber = foodAmountDto.FoodAmount.Fiber,
            Alcohol = foodAmountDto.FoodAmount.Alcohol,
            Sugar = foodAmountDto.FoodAmount.Sugar,
            SaturatedFats = foodAmountDto.FoodAmount.SaturatedFats,
            Sodium = foodAmountDto.FoodAmount.Sodium,
            Cholesterol = foodAmountDto.FoodAmount.Cholesterol,
            Potassium = foodAmountDto.FoodAmount.Potassium,
            Iron = foodAmountDto.FoodAmount.Iron,
            Calcium = foodAmountDto.FoodAmount.Calcium,
            Source = foodAmountDto.FoodAmount.Source,
            AmountName = foodAmountDto.FoodAmount.AmountName,
            Food = food,
        };
        await dbContext.FoodAmounts.AddAsync(foodAmount, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
