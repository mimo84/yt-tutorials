import { useEffect, useState } from 'react'
import { FoodDto, getFoods } from '../../../api/food'
import configuredAxios from '../../../api/config'
import Loading from '../../../components/Loading/Loading'
import fat from '../../../images/icons/fat.svg'
import carb from '../../../images/icons/carb.svg'
import protein from '../../../images/icons/protein.svg'
import { PlusIcon } from '@heroicons/react/24/outline'

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
  console.log(searchResults)
  return (
    <>
      <div>
        <label
          htmlFor="food_search"
          className="block text-sm font-medium leading-6 text-slate-900"
        >
          Look up food
        </label>
        <div className="mb-4 mt-2">
          <input
            type="text"
            name="food_search"
            value={foodName}
            onChange={handleOnChange}
            id="food_search"
            className="block w-full rounded-md border-0 py-1.5 text-slate-900 shadow-sm ring-1 ring-inset ring-slate-300 placeholder:text-slate-400 focus:ring-2 focus:ring-inset focus:ring-slate-600 sm:text-sm sm:leading-6"
            placeholder="Bread"
          />
        </div>
      </div>
      <>
        {searchResults === undefined ? (
          <Loading />
        ) : (
          <ul
            role="list"
            className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3"
          >
            {searchResults.food.map((searchResult) => (
              <li
                key={searchResult.foodId}
                className="col-span-1 divide-y divide-gray-200 rounded-lg bg-white shadow"
              >
                <div className="flex w-full items-center justify-between space-x-6 p-6">
                  <div className="wrap flex-1">
                    <div className="flex items-center space-x-3">
                      <h3 className="text-sm font-medium text-gray-900">
                        {searchResult.foodName}
                      </h3>
                    </div>
                    <div className="flex items-center justify-between space-x-3">
                      <p className="mt-1 truncate text-sm text-gray-500">
                        {searchResult.calories}KCal/{searchResult.amount}g
                      </p>
                      <div className="inline-flex flex-shrink-0 items-center rounded-full bg-green-50 px-1.5 py-0.5 text-xs font-medium text-green-700 ring-1 ring-inset ring-green-600/20">
                        {searchResult.protein}g
                        <img src={protein} className="ml-1 h-5 w-5" />
                        {searchResult.carbohydrates}g
                        <img src={carb} className="ml-1 h-5 w-5" />
                        {searchResult.fat}g
                        <img src={fat} className="ml-1 h-5 w-5" />
                      </div>
                    </div>
                  </div>
                </div>
                <div>
                  <form onSubmit={handleSubmit} className="flex">
                    <div className="flex w-0 flex-1">
                      <input
                        type="text"
                        defaultValue={searchResult.amount}
                        className="relative  w-0 flex-1 rounded-bl-lg border border-transparent py-4 text-sm font-semibold text-gray-900"
                        name="amount"
                        id="amount"
                      />
                      <input
                        type="hidden"
                        name="foodId"
                        value={searchResult.foodId}
                      />
                    </div>
                    <button
                      type="submit"
                      className="-ml-px flex w-0 flex-1 bg-zinc-100"
                    >
                      <span className="relative inline-flex w-0 flex-1 items-center justify-center gap-x-3 rounded-br-lg border border-transparent py-4 text-sm font-semibold text-gray-900">
                        <PlusIcon
                          className="h-5 w-5 text-gray-400"
                          aria-hidden="true"
                        />
                        Add to diary
                      </span>
                    </button>
                  </form>
                </div>
              </li>
            ))}
          </ul>
        )}
      </>
    </>
  )
}

export default AddFood
