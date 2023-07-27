import { useEffect, useState } from 'react'
import { FoodDto, getFoods } from '../../../api/food'
import configuredAxios from '../../../api/config'

const AddFood = () => {
  const [foodName, setFoodName] = useState('')
  const [searchResults, setSearchResults] = useState<FoodDto>()
  const handleOnChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFoodName(e.target.value)
  }

  const diaryDate = new Date(new Date().setHours(0, 0, 0, 0))
  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()

    const formData = new FormData(event.currentTarget)
    const foodId = formData.get('foodId') as string
    const amount = formData.get('amount') as string

    configuredAxios
      .post('/diary', {
        date: diaryDate.toISOString(),
        mealEntries: [
          {
            name: 'Lunch',
            foodEntries: [
              {
                foodId: foodId,
                foodAmountId: foodId,
                consumedAmount: amount,
              },
            ],
          },
        ],
      })
      .then((r) => {
        console.log(r)
      })
      .catch((x) => {
        console.log(x)
      })
  }

  useEffect(() => {
    if (foodName.length > 2) {
      getFoods(foodName)
        .then((v) => {
          setSearchResults(v.data)
        })
        .catch((e) => {
          console.log(e)
        })
    }
  }, [foodName])

  return (
    <>
      <div>Add Food Page</div>
      <input
        type="text"
        placeholder="Ricotta"
        value={foodName}
        onChange={handleOnChange}
      />

      <div className="min-w-full divide-y divide-gray-300">
        <div className="flex">
          <div className="w-1/2 py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-slate-900 sm:pl-6 lg:pl-8">
            Food Name
          </div>
          <div className="w-2/12 px-3 py-3.5 text-left text-sm font-semibold text-slate-900">
            Amount
          </div>
          <div className="w-2/12 px-3 py-3.5 text-left text-sm font-semibold text-slate-900">
            Calories
          </div>
          <div className="w-2/12 whitespace-nowrap py-4 pl-3 pr-4 text-right text-sm font-medium sm:pr-6 lg:pr-8">
            Add
          </div>
        </div>

        <div className="divide-y divide-gray-200 bg-white">
          {searchResults?.food.map((searchResult) => (
            <form
              className="flex"
              key={searchResult.foodId}
              onSubmit={handleSubmit}
            >
              <div className="w-1/2 whitespace-nowrap py-4 pl-4 pr-3 text-sm text-gray-900 sm:pl-6 lg:pl-8">
                {searchResult.foodName}
              </div>

              <div className="w-2/12 whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                <input
                  type="number"
                  name="amount"
                  className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                  defaultValue={searchResult.amount}
                />
                <input
                  type="hidden"
                  name="foodId"
                  value={searchResult.foodId}
                />
              </div>

              <div className="w-2/12 whitespace-nowrap px-3 py-4 text-sm text-gray-500">
                {searchResult.calories}
              </div>

              <div className="w-2/12 whitespace-nowrap py-4 pl-3 pr-4 text-right text-sm font-medium sm:pr-6 lg:pr-8">
                <button
                  type="submit"
                  className="text-indigo-600 hover:text-indigo-900"
                >
                  Add<span className="sr-only">, {searchResult.foodId}</span>
                </button>
              </div>
            </form>
          ))}
        </div>
      </div>
    </>
  )
}

export default AddFood
