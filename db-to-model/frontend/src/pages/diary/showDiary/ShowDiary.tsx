import { useState, useEffect } from 'react'
import {
  DiaryWithMeals,
  deleteMealFromDiary,
  getDiary,
} from '../../../api/diary'
import Loading from '../../../components/Loading/Loading'
import React from 'react'

const ShowDiary = () => {
  const [loading, setLoading] = useState(false)
  const [diary, setDiary] = useState<DiaryWithMeals>({ diary: { diaries: [] } })

  const handleDeleteFood = (foodId: number) => {
    deleteMealFromDiary(foodId)
      .then((r) => {
        console.log(r)
      })
      .catch((e) => console.log(e))
  }

  useEffect(() => {
    const initDiaries = async () => {
      setLoading(true)
      try {
        const fetchedDiary = await getDiary()
        if (!fetchedDiary) {
          throw new Error('Could not fetch diaries')
        }
        setDiary(fetchedDiary.data)
        setLoading(false)
      } catch {
        /* empty */
      }
    }

    void initDiaries()

    return () => {}
  }, [])

  if (loading) {
    return <Loading />
  }

  const {
    diary: { diaries },
  } = diary
  return (
    <>
      Your diaries:
      {diaries.map((d) => {
        const diaryDate = new Date(d.diaryDate).toLocaleDateString()
        return (
          <div className="mb-6 mt-6 " key={d.diaryId}>
            <div className="bg-indigo-100 py-3.5 text-center text-sm font-semibold ">
              Diary of {diaryDate} - {Math.round(d.caloriesInDiary)}KCal
            </div>

            {d.meals.map((m) => {
              return (
                <React.Fragment key={m.mealId}>
                  <div key={m.mealId} className="flex bg-slate-200">
                    <div className="w-full sm:w-1/3">
                      {m.mealName} - {m.caloriesInMeal}
                    </div>
                    <div className="invisible sm:visible">Consumed</div>
                    <div className="invisible sm:visible">Carbohydrates</div>
                    <div className="invisible sm:visible">Protein</div>
                    <div className="invisible sm:visible">Fat</div>
                    <div className="invisible sm:visible">Calories</div>
                  </div>

                  {m.foodInMealResponse.map((f) => {
                    return (
                      <div
                        className="flex flex-wrap border-b-4 border-slate-200 py-4 pl-4 pr-3 sm:pl-6 lg:pl-8"
                        key={f.foodMealId}
                      >
                        <div className="w-full text-sm font-medium text-gray-900 ">
                          {f.foodName}
                        </div>
                        <div className="mb-4 flex w-full items-center justify-between">
                          <div className="text-sm font-light text-gray-800">
                            {f.consumedAmount}g
                          </div>
                          <div className="hidden px-3 sm:inline-flex ">
                            {f.carbohydrates}
                          </div>
                          <div className="hidden px-3 sm:inline-flex ">
                            {f.protein}
                          </div>
                          <div className="hidden px-3 sm:inline-flex ">
                            {f.fat}
                          </div>
                          <div className="hidden px-3 sm:inline-flex ">
                            {f.calories}
                          </div>
                          <div className="">
                            <button
                              type="button"
                              className="rounded bg-slate-200 px-2 py-1 text-xs font-semibold text-slate-600 shadow-sm hover:bg-slate-100"
                              onClick={() => handleDeleteFood(f.foodMealId)}
                            >
                              Remove
                            </button>
                          </div>
                        </div>
                      </div>
                    )
                  })}
                </React.Fragment>
              )
            })}
          </div>
        )
      })}
    </>
  )
}

export default ShowDiary
