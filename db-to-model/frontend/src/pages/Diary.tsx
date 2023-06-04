import { useEffect, useState } from "preact/hooks";
import { DiaryWithMeals, getDiary } from "../api/diary";

export function Diary() {
  const [loading, setLoading] = useState(false);
  const [diary, setDiary] = useState<DiaryWithMeals>({ diaries: [] });

  useEffect(() => {
    const controller = new AbortController();
    const { signal } = controller;

    const initArticles = async () => {
      setLoading(true);
      try {
        const fetchedDiary = await getDiary(signal);
        if (!fetchedDiary) {
          throw new Error("Could not fetch diaries");
        }
        setDiary(fetchedDiary);
        setLoading(false);
      } catch {}
    };

    initArticles();

    return () => {
      controller.abort();
    };
  }, []);

  if (loading) {
    return <h1>Loading...</h1>;
  }

  const { diaries } = diary;

  return (
    <>
      {diaries.map((d) => {
        return (
          <div key={d.diaryId}>
            <h2>Diary of {d.diaryDate}</h2>
            {d.meals.map((m) => {
              return <h3 key={m.mealId}>{m.mealName}</h3>;
            })}
            <hr />
          </div>
        );
      })}
    </>
  );
}
