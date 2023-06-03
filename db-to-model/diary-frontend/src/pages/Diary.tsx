import { useEffect, useState } from "preact/hooks";
import { getDiary } from "../api/diary";

export function Diary() {
  const [loading, setLoading] = useState(false);
  const [diary, setDiary] = useState([]);

  useEffect(() => {
    const controller = new AbortController();
    const { signal } = controller;

    const initArticles = async () => {
      setLoading(true);
      try {
        const fetchedDiary = await getDiary(signal);
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

  return (
    <>
      <h1>Diary</h1>
      <pre>
        <code>{JSON.stringify(diary, null, 2)}</code>
      </pre>
    </>
  );
}
