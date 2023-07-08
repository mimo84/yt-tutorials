import About from "./pages/About";
import { Diary } from "./pages/Diary";
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import Home from "./pages/Home";
export function App() {
  return (
    <BrowserRouter>
      <Link className="mr-1" to="/">Home</Link>
      <Link className="mr-1" to="/about">About</Link>
      <Link className="mr-1" to="/diary">Diary</Link>
      <Routes>
        <Route path="/" element={<Home />}/>
        <Route path="/about" element={<About />}/>
        <Route path="/diary" element={<Diary />}/>
      </Routes>
    </BrowserRouter>
  )
}