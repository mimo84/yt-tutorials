import { useState, useEffect } from 'react'
import {
  DiaryWithMeals,
  deleteMealFromDiary,
  getDiary,
} from '../../../api/diary'
import Loading from '../../../components/Loading/Loading'
import React from 'react'
import { useTranslation } from 'react-i18next'

const ShowDiary = () => {
  const [loading, setLoading] = useState(false)
  const { t } = useTranslation()
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
        const diaryDate = new Date(d.diaryDate)
        return (
          <div className="mb-6 mt-6 " key={d.diaryId}>
            <div className="bg-indigo-100 py-3.5 text-center text-sm font-semibold ">
              {t('show-diary.title', {
                day: diaryDate,
                kcal: Math.round(d.caloriesInDiary),
                formatParams: {
                  day: {
                    weekday: 'long',
                    year: 'numeric',
                    month: 'long',
                    day: 'numeric',
                  },
                },
              })}
            </div>

            {d.meals.map((m) => {
              return (
                <React.Fragment key={m.mealId}>
                  <div key={m.mealId} className="flex bg-slate-200">
                    <div className="w-full sm:w-1/3">
                      {t('show-diary.meal-and-calories', {
                        mealName: m.mealName,
                        calories: m.caloriesInMeal,
                        count: m.caloriesInMeal,
                        formatParams: {
                          calories: { maximumFractionDigits: 2 },
                        },
                      })}
                    </div>

                    <div className="hidden sm:inline-flex">
                      {t('show-diary.heading-consumed')}
                    </div>
                    <div className="hidden sm:inline-flex">
                      {t('show-diary.heading-carbs')}
                    </div>
                    <div className="hidden sm:inline-flex">
                      {t('show-diary.heading-proteins')}
                    </div>
                    <div className="hidden sm:inline-flex">
                      {t('show-diary.heading-fats')}
                    </div>
                    <div className="hidden sm:inline-flex">
                      {t('show-diary.heading-calories')}
                    </div>
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
