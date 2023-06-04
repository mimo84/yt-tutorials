import { GET } from "./config";

export const getDiary = (signal: AbortSignal) => GET(`/diary/get`, signal);
