import React, {useEffect, useState} from "react";
import {BrowserRouter, Route, Routes, Redirect, Link, NavLink} from "react-router-dom"
import Home from "./Pages/Home";
import User from "./Pages/User";
import LoginLogout from "./Forms/LoginLogout";
import Menu from "./Pages/Menu";
import NotFound from "./Pages/NotFound";
import Booking from "./Pages/Booking";
import './App.css'
import {Get} from "./Utils/Utils";

function App() {
    const [IsA, setIsA] = useState(false)
    const [loaded, setLoaded] = useState(false)
    useEffect(() => Get("/api/Account/IsAuth", setIsA, setLoaded))
    return (
        <div><BrowserRouter>
            <ul className="topnav">
                <li><NavLink to="/">Главная</NavLink></li>
                <li><NavLink to="/user">Пользователь</NavLink></li>
                <li><NavLink to="/menu">Меню</NavLink></li>
                <li><NavLink to="/booking">Бронирование</NavLink></li>
                <li className="right"><LoginLogout IsA={IsA} setIsA={setIsA} loaded={loaded} setLoaded={setLoaded}/>
                </li>
            </ul>
            <div className="content">
                <Routes>
                    <Route path="/user"
                           element={<User IsA={IsA} setIsA={setIsA} loaded={loaded} setLoaded={setLoaded}/>}/>
                    <Route path="/menu" element={<Menu/>}/>
                    <Route path="/booking" element={<Booking IsA={IsA}/>}/>
                    <Route path="/" element={<Home/>}/>
                    <Route path="*" element={<NotFound/>}/>
                </Routes>
            </div>
        </BrowserRouter></div>);
}


export default App;
