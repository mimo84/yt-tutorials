import { GET } from "./config";

export type DiaryWithMeals = {
  diaries: {
    diaryId: number;
    diaryDate: string;
    meals: {
      mealId: number;
      mealName: string;
    }[];
  }[];
};

export const getDiary = (signal: AbortSignal) =>
  GET<DiaryWithMeals>(`/diary/get`, signal);
