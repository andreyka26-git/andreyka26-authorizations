import { BrowserRouter, Routes, Route } from "react-router-dom";
import './App.css';

//pages
import HomePage from "./pages/Home"
import LoginPage from "./pages/Login"
import CallbackPage from "./pages/Callback"

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/login" element={<LoginPage />} />
        <Route path="/callback" element={<CallbackPage />} />
      </Routes>
    </BrowserRouter>
  );
}


