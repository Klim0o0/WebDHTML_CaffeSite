import React, {useEffect, useState} from "react";
import {EmptyFunction, Post, PostJsonResponse} from "../Utils/Utils";
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
        PostJsonResponse('/api/Account/Register', setItem, setLoaded, EmptyFunction, JSON.stringify({
            Email: email,
            Password: password
        }))
        setStarted(true)
    }
    if (loaded && item[0]) {
        return (<div>{item[1].map(x => <p>{x}</p>)}</div>)
    }
    return (<form onSubmit={reg}>
        <div className="regLog">
            <div><input type="text" name="email" placeholder="Email" value={email}
                        onChange={event => setEmail(event.target.value)}/></div>
            <div><input type="password" name="password" placeholder="Пароль" value={password}
                        onChange={event => setPass(event.target.value)}/></div>
            <div>
                <button>Зарегистрироваться</button>
            </div>
        </div>
        {started ? <Loading/> : loaded ? <p>{item[1].map(x => <p>{x}</p>)}</p> : ""}
    </form>)

}