import { useEffect, useState } from 'react'
import { DiaryWithMeals, getDiary } from '../../api/diary'
import PageBody from '../../components/Layout/PageBody'
import PageHeading from '../../components/Layout/PageHeading'
import Loading from '../../components/Loading/Loading'
import { PlusIcon } from '@heroicons/react/24/outline'
import Button from '../../components/applicationUi/Button/Button'

export function Diary() {
  const [loading, setLoading] = useState(false)
  const [diary, setDiary] = useState<DiaryWithMeals>({ diary: { diaries: [] } })

  useEffect(() => {
    const controller = new AbortController()
    const { signal } = controller

    const initDiaries = async () => {
      setLoading(true)
      try {
        const fetchedDiary = await getDiary(signal)
        if (!fetchedDiary) {
          throw new Error('Could not fetch diaries')
        }
        setDiary(fetchedDiary)
        setLoading(false)
      } catch {
        /* empty */
      }
    }

    void initDiaries()

    return () => {
      controller.abort()
    }
  }, [])

  if (loading) {
    return <Loading />
  }

  const {
    diary: { diaries },
  } = diary

  return (
    <>
      <PageHeading>Diary</PageHeading>
      <PageBody>
        <Button>
          <>
            Add Food
            <PlusIcon className="-mr-0.5 h-5 w-5" aria-hidden="true" />
          </>
        </Button>
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
      </PageBody>
    </>
  )
}
