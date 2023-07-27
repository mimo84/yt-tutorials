import { useState, useEffect } from 'react'
import { DiaryWithMeals, getDiary } from '../../../api/diary'
import Loading from '../../../components/Loading/Loading'
import React from 'react'

const ShowDiary = () => {
  const [loading, setLoading] = useState(false)
  const [diary, setDiary] = useState<DiaryWithMeals>({ diary: { diaries: [] } })

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
          <div
            className="mt-6 drop-shadow-sm hover:drop-shadow-xl"
            key={d.diaryId}
          >
            <div className="bg-indigo-100 py-3.5 text-center text-sm font-semibold ">
              Diary of {diaryDate} - {Math.round(d.caloriesInDiary)}KCal
            </div>

            {d.meals.map((m) => {
              return (
                <React.Fragment key={m.mealId}>
                  <div key={m.mealId} className="flex bg-slate-200">
                    <div className="w-1/3 py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-slate-900 sm:pl-6 lg:pl-8">
                      {m.mealName} - {m.caloriesInMeal}
                    </div>
                    <div className="w-[13%] py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-slate-900 sm:pl-6 lg:pl-8">
                      Consumed
                    </div>
                    <div className="w-[13%] py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-slate-900 sm:pl-6 lg:pl-8">
                      Carbohydrates
                    </div>
                    <div className="w-[13%] py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-slate-900 sm:pl-6 lg:pl-8">
                      Protein
                    </div>
                    <div className="w-[13%] py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-slate-900 sm:pl-6 lg:pl-8">
                      Fat
                    </div>
                    <div className="w-[13%] py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-slate-900 sm:pl-6 lg:pl-8">
                      Calories
                    </div>
                  </div>

                  {m.foodInMealResponse.map((f) => {
                    return (
                      <div className="flex" key={f.foodId}>
                        <div className="w-1/3 py-3.5 pl-4 pr-3 text-left text-sm font-semibold text-slate-900 sm:pl-6 lg:pl-8">
                          {f.foodName}
                        </div>
                        <div className="w-[13%] py-3.5 pl-4 pr-3 text-left text-sm text-slate-900 sm:pl-6 lg:pl-8">
                          {f.consumedAmount}
                        </div>
                        <div className="w-[13%] py-3.5 pl-4 pr-3 text-left text-sm text-slate-900 sm:pl-6 lg:pl-8">
                          {f.carbohydrates}
                        </div>
                        <div className="w-[13%] py-3.5 pl-4 pr-3 text-left text-sm text-slate-900 sm:pl-6 lg:pl-8">
                          {f.protein}
                        </div>
                        <div className="w-[13%] py-3.5 pl-4 pr-3 text-left text-sm text-slate-900 sm:pl-6 lg:pl-8">
                          {f.fat}
                        </div>
                        <div className="w-[13%] py-3.5 pl-4 pr-3 text-left text-sm text-slate-900 sm:pl-6 lg:pl-8">
                          {f.calories}
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
