import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { BrowserRouter, Route, Routes } from 'react-router'
import Header from './components/header/Header.tsx'
import 'bootstrap/dist/css/bootstrap.css';
import Login from './pages/Register/Register.tsx'


createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <Header/>
      <Routes>
        <Route path='' element={<App/>}/>
        <Route path='login' element={<Login/>}/>
      </Routes>
    </BrowserRouter>
  </StrictMode>,
)
