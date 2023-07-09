import { GET } from './config'

export type DiaryWithMeals = {
  diary: {
    diaries: {
      diaryId: number
      diaryDate: string
      caloriesInDiary: number
      meals: {
        mealId: number
        mealName: string
        caloriesInMeal: number
        foodInMealResponse: {
          foodId: number
          foodName: string
          consumedAmount: number
          fat: number
          protein: number
          carbohydrates: number
          calories: number
        }[]
      }[]
    }[]
  }
}

export const getDiary = (signal: AbortSignal) =>
  GET<DiaryWithMeals>(`/diary/get`, signal)
