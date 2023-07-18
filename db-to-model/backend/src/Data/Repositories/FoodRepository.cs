using FoodDiary.Core.Dto;
using FoodDiary.Core.Entities;
using FoodDiary.Core.Extensions;
using FoodDiary.Core.Mappers;
using FoodDiary.Core.Repositories;
using FoodDiary.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FoodDiary.Data.Repositories;

public class FoodRepository : IFoodRepository
{
    private readonly FoodDiaryDbContext dbContext;
    public FoodRepository(FoodDiaryDbContext _dbContext)
    {
        dbContext = _dbContext;
    }

    public async Task<List<FoodWithNutritionInfoDto>> GetAllFoods(CancellationToken cancellationToken)
    {
        var results = await dbContext.Foods.Include(f => f.FoodAmounts).ToListAsync(cancellationToken);
        var resultsDto = results.Select(f => f.AsDto()).ToList();
        return resultsDto;
    }

    public async Task<List<FoodWithNutritionInfoDto>> FindFood(string search, CancellationToken cancellationToken)
    {
        IQueryable<Food> query = dbContext.Foods.Include(f => f.FoodAmounts);
        var searchTerms = search.Split(' ').ToList();
        foreach (var eachTerm in searchTerms)
        {
            query = query.Where(f => EF.Functions.Like(f.Name.ToLower(), '%' + eachTerm.ToLower() + '%'));
        }

        var results = await query.Take(10).ToListAsync(cancellationToken);
        return results.Select(f => f.AsDto()).ToList();
    }

    public async Task AddFoodWithAmountsAsync(FoodWithAmountDto foodAmountDto, CancellationToken cancellationToken)
    {
        var alreadyThere = await dbContext.Foods.Where(f => f.Name == foodAmountDto.Name).AnyAsync(cancellationToken);
        if (alreadyThere)
        {
            return;
        }
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
        await dbContext.AddAsync(food, cancellationToken);
        await dbContext.AddAsync(foodAmount, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
