import { useState, useEffect } from 'react'
import { DiaryWithMeals, getDiary } from '../../../api/diary'
import Loading from '../../../components/Loading/Loading'

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
        return (
          <table className={'mt-6'} key={d.diaryId}>
            <thead>
              <tr>
                <td colSpan={6} className={'bg-green-100'}>
                  Diary of {d.diaryDate} - {Math.round(d.caloriesInDiary)}
                </td>
              </tr>
            </thead>
            <tbody>
              {d.meals.map((m) => {
                return (
                  <>
                    <tr key={m.mealId}>
                      <td colSpan={6} className={'bg-slate-200'}>
                        {m.mealName} - {m.caloriesInMeal}
                      </td>
                    </tr>

                    {m.foodInMealResponse.map((f) => {
                      return (
                        <tr key={f.foodId}>
                          <td className={'border-r-4 border-green-500'}>
                            {f.foodName}
                          </td>
                          <td className={'border-r-4 border-green-500'}>
                            {f.consumedAmount}
                          </td>
                          <td className={'border-r-4 border-green-500'}>
                            {f.carbohydrates}
                          </td>
                          <td className={'border-r-4 border-green-500'}>
                            {f.protein}
                          </td>
                          <td className={'border-r-4 border-green-500'}>
                            {f.fat}
                          </td>
                          <td className={'border-r-4 border-green-500'}>
                            {f.calories}
                          </td>
                        </tr>
                      )
                    })}
                  </>
                )
              })}
            </tbody>
          </table>
        )
      })}
    </>
  )
}

export default ShowDiary
