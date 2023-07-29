import ReactDOM from 'react-dom/client'
import { App } from './app.tsx'
import './index.css'
import React from 'react'
import './utlis/i18n.ts'
const rootNode = document.getElementById('app') as HTMLElement

ReactDOM.createRoot(rootNode).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)
