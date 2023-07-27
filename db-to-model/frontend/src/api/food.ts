import configuredAxios from './config'

export interface FoodDto {
  food: [
    {
      foodId: number
      foodName: string
      amount: number
      protein: number
      fat: number
      carbohydrates: number
      calories: number
    },
  ]
}
export const getFoods = (name: string) =>
  configuredAxios.get<FoodDto>(`/food/find`, { params: { Name: name } })
