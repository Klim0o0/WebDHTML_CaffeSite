import React, {useEffect, useState} from "react";
import {EmptyFunction, Post} from "../Utils/Utils";
import {Redirect} from "react-router-dom";
import Loading from "./Loading";
import "./LoginRegForm.css";

export default function RegistrationForm(props) {
    const [email, setEmail] = useState("");
    const [password, setPass] = useState("");
    const [phone, setPhone] = useState("");
    const [item, setItem] = useState(null);
    const [loaded, setLoaded] = useState(false)
    const [started, setStarted] = useState(false)

    useEffect(x => {
        if (item) {
            props.setIsA(item[0])
        }
    }, [item])

    useEffect(x => {
        if (loaded) {
            setStarted(false)
        }
    }, [loaded])
    useEffect(x => {
        if (started) {
            setLoaded(false)
        }
    }, [started])

    const reg = event => {
        event.preventDefault();
        Post('/api/Account/Register', setItem, setLoaded, EmptyFunction, JSON.stringify({
            Email: email,
            PhoneNumber: phone,
            Password: password
        }))
        setStarted(true)
    }

    return (<form onSubmit={reg}>
        <div className="regLog">
            <div><input type="text" name="email" placeholder="Email" value={email}
                        onChange={event => setEmail(event.target.value)}/></div>
            <div><input type="text" name="phone" placeholder="Номер телефона" value={phone}
                        onChange={event => setPhone(event.target.value)}/></div>
            <div><input type="password" name="password" placeholder="Пароль" value={password}
                        onChange={event => setPass(event.target.value)}/></div>
            <div>
                <button>Зарегистрироваться</button>
            </div>
        </div>
        {started ? <Loading/> : loaded ? <p>{item[1]}</p> : ""}
    </form>)

}