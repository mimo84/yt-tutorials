import configuredAxios from './config'

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
          foodMealId: number
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

export const getDiary = () => configuredAxios.get<DiaryWithMeals>(`/diary/get`)

export const deleteMealFromDiary = (foodMealId: number) =>
  configuredAxios.delete(`/diary/foodmeal/${foodMealId}`)
